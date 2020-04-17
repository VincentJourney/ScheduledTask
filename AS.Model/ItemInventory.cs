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

        [Table("ItemInventory")]
        public class ItemInventory
        {
            [ExplicitKey]
            public Guid InventoryID { get; set; }

            public Guid ItemID { get; set; }

            public Guid CompanyID { get; set; }

            public short InventoryLevel { get; set; }

            public Guid OrgID { get; set; }

            public Guid? StoreID { get; set; }

            public int? InventoryQuantity { get; set; }

            public decimal? InvenroryValue { get; set; }

            public int? FreezeQuantity { get; set; }

            public decimal? FreezeValue { get; set; }
        }
    }
}