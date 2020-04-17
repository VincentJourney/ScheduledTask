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
    public class ItemInventoryDAO : Infrastructure<ItemInventory>, IItemInventoryDAO
    {

        public int ChangeInventoryBySql(Guid ItemId, Guid OrgId, int Qty, decimal ItemValue, IDbTransaction dbTransaction = null)
        {
            var sql = $@"Update ItemInventory set InventoryQuantity=InventoryQuantity+'{Qty}'
,FreezeQuantity=FreezeQuantity-'{Qty}'
,InvenroryValue=InvenroryValue+'{Qty * ItemValue}'
,FreezeValue=FreezeValue-'{Qty * ItemValue}'
where ItemId='{ItemId}' and OrgId='{OrgId}'";
            return Execute(sql, dbTransaction);
        }
    }
}
