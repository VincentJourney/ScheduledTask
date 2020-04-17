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
    public class PayingDAO : Infrastructure<Paying>, IPayingDAO
    {
        public List<Paying> GetByOrderId(Guid OrderiD, IDbTransaction dbTranscation = null) 
            => GetList($"OrderID='{OrderiD}'", dbTranscation);

    }
}
