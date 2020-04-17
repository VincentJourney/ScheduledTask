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

        [Table("Paying")]
        public class Paying
        {
            [ExplicitKey]
            public Guid PayingID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Guid OrderID { set; get; }

            /// <summary>
            /// 
            /// </summary>
            public DateTime PayTime { set; get; }

            /// <summary>
            /// 
            /// </summary>
            public decimal OrderAmount { set; get; }

            /// <summary>
            /// 1、微信 2、积分 3、礼券
            /// </summary>
            public string PayType { set; get; }

            /// <summary>
            /// 
            /// </summary>
            public decimal PayAmount { set; get; }

            /// <summary>
            /// 礼券支付：传入礼券id。积分支付：传入积分值
            /// </summary>
            public string PayAccount { set; get; }

            /// <summary>
            /// 
            /// </summary>
            public string Remark { set; get; }
        }
    }
}