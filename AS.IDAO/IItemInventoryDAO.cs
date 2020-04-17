using AS.IDAO.Base;
using AS.Model;
using AS.Model.AICFrame.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace AS.OCR.IDAO
{
    public interface IItemInventoryDAO : IBaseDAO<ItemInventory>
    {
        int ChangeInventoryBySql(Guid ItemId, Guid OrgId, int Qty, decimal ItemValue, IDbTransaction dbTransaction = null);
    }
}
