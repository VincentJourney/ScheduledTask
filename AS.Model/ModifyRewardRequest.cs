using System;

namespace AS.Model
{
    public class ModifyRewardRequest
    {
        public Guid cardId { get; set; }
        public Guid companyID { get; set; }
        public Guid orgID { get; set; }
        public string accountType { get; set; }
        public decimal modifyReward { get; set; }
        public string logType { get; set; }
        public Guid storeID { get; set; }
        public Guid transId { get; set; }
        public Guid campaignID { get; set; }
    }
}
