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

        [Table("VoucherLog")]
        public class VoucherLog
        {
            [ExplicitKey]
            public Guid VoucherLogID { get; set; }

            public Guid CompanyID { get; set; }

            public short LogType { get; set; }

            public Guid VoucherID { get; set; }

            public Guid RelatedBillID { get; set; }

            public Guid RelatedTxnCode { get; set; }

            public Guid RelatedOrgID { get; set; }

            public Guid StoreID { get; set; }

            public decimal? UseValue { get; set; }

            public string Operator { get; set; }

            public DateTime? OperateOn { get; set; }

            public string AddedBy { get; set; }

            public DateTime? AddedOn { get; set; }

            public string EditedBy { get; set; }

            public DateTime? EditedOn { get; set; }

            public Guid? SettlementID { get; set; }

            public decimal? SharePercent { get; set; }

            public Guid? ShareCostID { get; set; }
        }
    }
}