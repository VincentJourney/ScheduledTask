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

        [Table("Voucher")]
        public class Voucher
        {
            [ExplicitKey]
            public Guid VoucherID { get; set; }

            public Guid CurrentOrgID { get; set; }

            public Guid CurrentStoreID { get; set; }

            public string VoucherCode { get; set; }

            public Guid VoucherTypeID { get; set; }

            public Guid AccountID { get; set; }

            public Guid CardID { get; set; }

            public string HoldBy { get; set; }

            public string HolderContacts { get; set; }

            public DateTime? IssueDate { get; set; }

            public DateTime? EffectiveDate { get; set; }

            public DateTime? ExpiryDate { get; set; }

            public Guid IsuueTaskID { get; set; }

            public Guid VoidTaskID { get; set; }

            public string VoidBy { get; set; }

            public DateTime? VoidOn { get; set; }

            public int? IsTransferalbe { get; set; }

            public int? VoucherStauts { get; set; }

            public Guid? TraceId { get; set; }

            public string VerfiyCode { get; set; }

            public string AddedBy { get; set; }

            public DateTime? AddedOn { get; set; }

            public string EditedBy { get; set; }

            public DateTime? EditedOn { get; set; }

            public string DeletedBy { get; set; }

            public DateTime? DeletedOn { get; set; }

            public Guid? CampaignID { get; set; }

            public decimal? Value { get; set; }
            public string Remarks { get; set; }
            public int? VoucherCount { get; set; }
            public string BatchNo { get; set; }
            public string sourcetype { get; set; }

            public string activitytype { get; set; }
            public DateTime? WriteOffTime { get; set; }
        }
    }
}