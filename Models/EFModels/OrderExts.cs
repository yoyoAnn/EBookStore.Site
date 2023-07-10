using EBookStore.Site.Models.ViewModels;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.EFModels
{
    public static class OrderExts
    {
        public static Order ToEntity(this OrderVm order)
        {
            return new Order
            {
                Id = order.Id,
                UserId = order.UserId,
                ReceiverName = order.ReceiverName,
                ReceiverAddress = order.ReceiverAddress,
                ReceiverPhone = order.ReceiverPhone,
                TaxIdNum = order.TaxIdNum,
                VehicleNum = order.VehicleNum,
                Remark = order.Remark,
                OrderTime = order.OrderTime,
                OrderStatusId = order.OrderStatusId,
                TotalAmount = order.TotalAmount,
                ShippingNumber = order.ShippingNumber,
                ShippingTime = order.ShippingTime,
                ShippingFee = order.ShippingFee,
                ShippingStatusId = order.ShippingStatusId,
                TotalPayment = order.TotalPayment
            };
        }
    }
}