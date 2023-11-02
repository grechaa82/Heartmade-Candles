﻿using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class OrderDetailMapping
{
    public static Basket MapToOrderDetail(OrderDetailCollection orderDetailCollection)
    {
        return new Basket
        {
            Id = orderDetailCollection.Id,
            Items = OrderDetailItemMapping.MapToOrderDetailItem(orderDetailCollection.Items),
            // TotalConfigurationString = orderDetailCollection.TotalConfigurationString
        };
    }

    public static OrderDetailCollection MapToOrderDetailCollection(Basket orderDetail)
    {
        return new OrderDetailCollection
        {
            Items = OrderDetailItemMapping.MapToOrderDetailItemCollection(orderDetail.Items),
            TotalPrice = orderDetail.TotalPrice,
            TotalQuantity = orderDetail.TotalQuantity,
            TotalConfigurationString = orderDetail.TotalConfigurationString
        };
    }
}

