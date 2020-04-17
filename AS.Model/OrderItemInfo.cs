using System;

namespace AS.Model
{
    public class OrderItemInfo
    {
        public Guid OrgId { get; set; }
        public Guid ItemId { get; set; }
        public int ItemQty { get; set; }
        public decimal ItemValue { get; set; }
    }
}
