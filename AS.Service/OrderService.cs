using AS.Commom.Configuration;
using AS.Commom.Log;
using AS.Model;
using AS.OCR.Commom.Http;
using AS.OCR.Dapper.DAO;
using AS.OCR.IDAO;
using Newtonsoft.Json;
using System;
using System.Linq;
using AICFrame.Common.Redis;
using System.Transactions;
using AS.Model.AICFrame.Model.Entity;
using AS.Dapper.Base;
using AS.OCR.Dapper.Base;
using System.Collections.Generic;

namespace AS.Service
{
    public class OrderService : ServerBase
    {
        private readonly IOrderDAO orderDAO = new OrderDAO();
        private readonly IItemInventoryDAO itemInventoryDAO = new ItemInventoryDAO();
        private readonly IPayingDAO payingDAO = new PayingDAO();
        private readonly IVoucherDAO voucherDAO = new VoucherDAO();
        private readonly IVoucherLogDAO voucherLogDAO = new VoucherLogDAO();

        public void OrderRurn()
        {
            Console.WriteLine($"========{DateTime.Now.ToString()}：执行订单退货========");
            try
            {
                var OrderIdList = orderDAO.GetAllCanRefundOrder();
                foreach (var item in OrderIdList)
                {
                    Log.Info($@"【执行订单退货】 : 订单Id ：【{item.OrderId}】==");
                    Console.WriteLine($@"【执行订单退货】 : 订单Id ：【{item.OrderId}】==");
                    RequestModel<VoucherReturnByOrderIdRequest> req = new RequestModel<VoucherReturnByOrderIdRequest>();
                    req.Shared = new Shared();
                    req.Data = new VoucherReturnByOrderIdRequest { OrderId = item.OrderId };
                    var r = PostCRM<bool?>("api/reward/OrderReturn", req);
                    if ((r == null) || (r.Result == null))
                    {
                        Log.Error($"【api/reward/OrderReturn 异常】 【ErrorMessage】：返回值为空 ", null);
                        Console.WriteLine($"【api/reward/OrderReturn 异常】 【ErrorMessage】：返回值为空 ");
                        continue;
                    }
                    if (r.Result.HasError)
                    {
                        Log.Error($"【api/reward/OrderReturn 异常】 【ErrorMessage】：{r.Result.ErrorMessage} ", null);
                        Console.WriteLine($"【api / reward / OrderReturn 异常】 【ErrorMessage】：{ r.Result.ErrorMessage}");
                        continue;
                    }
                    if (r.Data == null || !r.Data.Value)
                    {
                        Log.Error($"【api/reward/OrderReturn 异常】 【ErrorMessage】：{r.Result.ErrorMessage} ", null);
                        Console.WriteLine($"【api/reward/OrderReturn 异常】 【ErrorMessage】：{r.Result.ErrorMessage} ");
                    }
                    else
                    {
                        Log.Info($@"【订单退货成功】 : 订单Id ：【{item.OrderId}】==");
                        Console.WriteLine($@"【订单退货成功】 : 订单Id ：【{item.OrderId}】==");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("【OrderRurn 异常】", ex);
            }
        }


        public void OrderCancel()
        {
            Console.WriteLine($"========{DateTime.Now.ToString()}：逾期未支付订单自动关闭启动========");
            var OrderList = orderDAO.GetOrderByExpiry();
            foreach (var x in OrderList)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}：逾期未支付订单自动关闭 订单号为：{x.OrderID}");
                Log.Info($"{DateTime.Now.ToString()}：逾期未支付订单自动关闭 订单号为：{x.OrderID}");


                using (DapperTransaction trans = new DapperTransaction(DapperHelper.dbConnection))
                {
                    try
                    {
                        #region 修改订单状态
                        x.Orderstatus = 6;
                        orderDAO.Update(x, trans._dbTransaction);
                        #endregion

                        #region 修改库存
                        var ItemInfoList = orderDAO.GetOrderItemInfo(x.OrderID, trans._dbTransaction);
                        if (ItemInfoList.Count() > 0)
                        {
                            var rh = GetRedisHelper();
                            foreach (var item in ItemInfoList)
                            {
                                var invRedisKey = $"{ConfigurationUtil.RedisKey_ItemInventotyQty}_{item.ItemId}_{item.OrgId}";
                                rh.StringIncrement(invRedisKey, item.ItemQty);
                                var result = itemInventoryDAO.ChangeInventoryBySql(item.ItemId, item.OrgId, item.ItemQty, item.ItemValue, trans._dbTransaction);
                                if (result <= 0)
                                    rh.StringDecrement(invRedisKey, item.ItemQty);
                            }
                        }
                        #endregion

                        #region 恢复积分,礼券
                        var payingList = payingDAO.GetByOrderId(x.OrderID, trans._dbTransaction);
                        foreach (var p in payingList)
                        {
                            #region 积分
                            if (p.PayType == "2")
                            {
                                decimal DeductPoints = 0;
                                if (!decimal.TryParse(p.PayAccount, out DeductPoints))
                                    throw new Exception($"Paying 积分转换值错误 ");

                                DeductPoints = Math.Abs(DeductPoints);
                                if (DeductPoints > 0)
                                {
                                    var req = new RequestModel<ModifyRewardRequest>
                                    {
                                        Shared = new Shared(),
                                        Data = new ModifyRewardRequest
                                        {
                                            cardId = x.CardID,
                                            companyID = x.OrderCompanyID,
                                            orgID = x.OrderOrgID,
                                            accountType = "1",
                                            modifyReward = DeductPoints,
                                            logType = "14",
                                            storeID = Guid.Empty,
                                            transId = x.OrderID,
                                            campaignID = Guid.Empty
                                        }
                                    };
                                    var r = PostCRM<bool>("api/master/ModifyReward", req);
                                    if ((r == null) || (r.Result == null))
                                        throw new Exception($"OrderID:{p.OrderID} 【api/master/ModifyReward 异常】 【ErrorMessage】：返回值为空 ");

                                    if (r.Result.HasError)
                                        throw new Exception($"OrderID:{p.OrderID}【api/master/ModifyReward 异常】 【ErrorMessage】：{r.Result.ErrorMessage} ");

                                    if (!r.Data)
                                        throw new Exception($"OrderID:{p.OrderID}【api/master/ModifyReward 异常】 积分失败 ");
                                }

                            }
                            #endregion

                            #region 礼券
                            if (p.PayType == "3")
                            {
                                var VoucherID = Guid.Parse(p.PayAccount);
                                var voucher = voucherDAO.Get(VoucherID, trans._dbTransaction);
                                if (voucher == null)
                                    throw new Exception($"OrderID:{p.OrderID} 查不到礼券");

                                //if (!(voucher.CardID == card.CardID)) BError("礼券id错误", "4");
                                if (voucher.VoucherStauts == 8)
                                {
                                    if (voucher.ExpiryDate <= DateTime.Now) voucher.VoucherStauts = 7;
                                    else
                                        voucher.VoucherStauts = 4; //可用礼券
                                    voucherDAO.Update(voucher, trans._dbTransaction);

                                    VoucherLog logModel = new VoucherLog();
                                    logModel.VoucherLogID = Guid.NewGuid();
                                    logModel.VoucherID = VoucherID;
                                    logModel.UseValue = voucher.Value;
                                    logModel.RelatedOrgID = voucher.CurrentOrgID;
                                    logModel.StoreID = voucher.CurrentStoreID;
                                    logModel.Operator = "取消订单";
                                    logModel.OperateOn = DateTime.Now;
                                    logModel.AddedBy = "订单取消计划任务";
                                    logModel.AddedOn = DateTime.Now;
                                    logModel.LogType = (short)voucher.VoucherStauts;
                                    logModel.CompanyID = Guid.Empty;
                                    voucherLogDAO.Add(logModel, trans._dbTransaction);
                                }
                            }
                            #endregion

                        }
                        #endregion

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("【OrderCancel 异常】", ex);
                        trans.RollBack();
                    }
                }

            }

        }

        private static string Token;

        private string GetToken()
        {
            var url = $"{ConfigurationUtil.CRMUrl}api/platform/token?appid={ConfigurationUtil.CRMTokenAppid}&secret={ConfigurationUtil.CRMTokenSecret}";
            var res = HttpHelper.HttpGet(url);
            ResponseModel<string> r = null;
            try
            {
                r = JsonConvert.DeserializeObject<ResponseModel<string>>(res);
            }
            catch { r = null; }
            if (r != null) if (r.Result != null) if (r.Result.HasError) throw new Exception(r.Result.ErrorMessage);
            return res;
        }

        public ResponseModel<Response> PostCRM<Response>(string url, object model)
        {
            if (string.IsNullOrWhiteSpace(Token))
                Token = GetToken();
            string res = Post(url, JsonConvert.SerializeObject(model));
            var r = JsonConvert.DeserializeObject<ResponseModel<Response>>(res);
            if (r.Result != null) if (r.Result.HasError) if (r.Result.ErrorCode == "401") //Invalid Token or expired.
                    {
                        Token = GetToken();
                        res = Post(url, JsonConvert.SerializeObject(model));
                        r = JsonConvert.DeserializeObject<ResponseModel<Response>>(res);
                    }
            return r;
        }

        /// <summary>
        /// 调用中台接口
        /// </summary>
        /// <param name="route">接口路径</param>
        /// <param name="postData">传入参数</param>
        /// <returns></returns>
        private string Post(string route, string postData)
        {
            string posturl = ConfigurationUtil.CRMUrl + "/" + route + "?token=" + Token;
            var result = HttpHelper.HttpPost(posturl, postData);
            return result;
        }
    }

    public class VoucherReturnByOrderIdRequest
    {
        public Guid OrderId { get; set; }
    }
}
