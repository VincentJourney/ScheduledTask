using AS.IDAO.Base;
using AS.Model;
using AS.Model.AICFrame.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace AS.OCR.IDAO
{
    public interface IPayingDAO : IBaseDAO<Paying>
    {
        List<Paying> GetByOrderId(Guid OrderiD, IDbTransaction dbTranscation = null);
    }
}
