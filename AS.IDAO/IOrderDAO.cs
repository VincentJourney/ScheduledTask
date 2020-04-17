using AS.IDAO.Base;
using AS.Model;
using AS.Model.AICFrame.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace AS.OCR.IDAO
{
    public interface IOrderDAO : IBaseDAO<GiftReedemOrder>
    {
        List<OrderModel> GetAllCanRefundOrder();
        List<GiftReedemOrder> GetOrderByExpiry();
        List<OrderItemInfo> GetOrderItemInfo(Guid OrderId, IDbTransaction dbTranscation = null);
    }
}
