using AS.Commom.Configuration;
using AS.Model;
using AS.Model.AICFrame.Model.Entity;
using AS.OCR.Dapper.Base;
using AS.OCR.IDAO;
using System;
using System.Collections.Generic;
using System.Data;

namespace AS.OCR.Dapper.DAO
{
    public class OrderDAO : Infrastructure<GiftReedemOrder>, IOrderDAO
    {
        public List<OrderModel> GetAllCanRefundOrder()
        {
            var sql = $@"SELECT distinct g.OrderID FROM  GiftReedemOrder  AS g
INNER JOIN  GiftReedemOrderDetail AS god ON g.OrderID=god.GiftReedemOrderID AND god.itemtype=3  
INNER JOIN  Voucher AS v ON v.IsuueTaskID=god.ItemID 
AND v.AddedBy='团购秒杀购买' and v.VoucherStauts in (4,7) and v.ExpiryDate<getdate() 
INNER JOIN Paying AS p ON g.OrderID = p.OrderID
INNER JOIN WxPayInfo AS wpi ON p.PayingID=wpi.PayingId AND wpi.PayStatus=1
and Not exists( select 1 from Voucher as v2 where v2.IsuueTaskID=god.ItemID and v2.BatchNo=g.OrderNo and v2.VoucherStauts=6)";
            return GetListFromSql<OrderModel>(sql);
        }

        /// <summary>
        /// 待支付 超过 15分钟的订单
        /// </summary>
        /// <returns></returns>
        public List<GiftReedemOrder> GetOrderByExpiry()
            => GetListFromSql<GiftReedemOrder>($@"SELECT  * FROM  GiftReedemOrder AS o WHERE  o.Orderstatus=2
                                AND DATEDIFF(mi,o.OrderDate,getdate()) > {ConfigurationUtil.ExpiryTime}");

        /// <summary>
        /// 获取订单的礼品详情
        /// </summary>
        /// <returns></returns>
        public List<OrderItemInfo> GetOrderItemInfo(Guid OrderId, IDbTransaction dbTranscation = null)
        {
            string sql = $@"SELECT o.orderorgid OrgId,god.ItemId ,god.ItemQty,god.unitPrice ItemValue  FROM  GiftReedemOrder AS o 
                            inner join GiftReedemOrderDetail as god on o.orderid = god.giftreedemorderid
                            where  god.itemType=1 and o.OrderId='{OrderId}'";
            return GetListFromSql<OrderItemInfo>(sql, dbTranscation);
        }

    }
}
