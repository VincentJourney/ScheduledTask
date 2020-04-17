using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Model
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper.Contrib.Extensions;

    namespace AICFrame.Model.Entity
    {

        [Table("GiftReedemOrder")]
        public class GiftReedemOrder
        {
            [ExplicitKey]
            public Guid OrderID { get; set; }

            public string OrderNo { get; set; }

            public short OrderType { get; set; }

            public Guid OrderCompanyID { get; set; }

            public short OrderOrgLevl { get; set; }

            public Guid OrderOrgID { get; set; }

            public Guid OrderStoreID { get; set; }

            public short OrderChannel { get; set; }

            public DateTime? OrderDate { get; set; }

            public DateTime? OrderValidto { get; set; }

            public Guid CustomerID { get; set; }

            public Guid CardID { get; set; }
            /// <summary>
            /// 订单状态0、未提交,购物车1、 提交,可砍价  2、后台确认  3、已支付 4、已领取 5、已邮寄  6、取消 7、过期
            /// </summary>
            public short Orderstatus { get; set; }

            public Guid RedeemsLogID { get; set; }

            public string AddedBy { get; set; }

            public DateTime? AddedOn { get; set; }

            public string EditedBy { get; set; }

            public DateTime? EditedOn { get; set; }

            public string DeletedBy { get; set; }

            public DateTime? DeletedOn { get; set; }
            /// <summary>
            /// 1礼品 2 礼券
            /// </summary>
            public string RedeemWay { get; set; }

            public Guid RedeemRule { get; set; }

            public string Telephone { get; set; }

            public Guid? MailingAddressId { get; set; }

            public string AddressName { get; set; }

            public string PostAddress { get; set; }
            /// <summary>
            /// 1、积分商城；2、团购活动； 3、秒杀活动；4、砍价活动；5、抽奖
            /// </summary>
            public string SourceOrder { get; set; }
            /// <summary>
            /// 领取方式，1 客服台领取 2、邮寄
            /// </summary>
            public int? ReceiveType { get; set; }
            /// <summary>
            /// 收件人
            /// </summary>
            public string ReceiveBy { get; set; }
            /// <summary>
            /// 领取地点
            /// </summary>
            public string ReceiptAddress { get; set; }
            /// <summary>
            /// 快递公司
            /// </summary>
            public string ExpressDelivery { get; set; }
            /// <summary>
            /// 快递单号
            /// </summary>
            public string ExpressNo { get; set; }
            public string Remarks { get; set; }
            public decimal? TotalAmount { get; set; }
            /// <summary>
            /// 待支付金额
            /// </summary>
            public decimal? TransactionAmt { get; set; }
            /// <summary>
            /// 待支付积分
            /// </summary>
            public decimal? TotalDeductPoints { get; set; }
            /// <summary>
            /// 订单金额
            /// </summary>
            public decimal? OrderAmount { get; set; }
            /// <summary>
            /// 预约领取日期
            /// </summary>
            public DateTime? ReceiptDate { get; set; }
            public int? OrderQuantity { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            /// <summary>
            /// 1、积分兑换；2、原价购买
            /// </summary>
            public string BuyType { get; set; }
            public decimal? Postage { get; set; }
            public decimal? PostagePoints { get; set; }
            public string Address { get; set; }
            public Guid? ExpressCompany { get; set; }
            public string OrderNum { get; set; }

            public DateTime? LastRedeemDate { get; set; }
        }
    }
}