// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: OrdersListModel.cs
// 
// Created by: ykors_000 at 25.11.2013 15:54
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;
using PartDesk.Web.Models.Manage;

namespace PartDesk.Web.Models.Orders
{
    /// <summary>
    /// Модель списка заказов
    /// </summary>
    public class OrdersListModel: GenericListModel<Order>
    {
        /// <summary>
        /// Заголовок списка заказов
        /// </summary>
        public string Title { get; set; }
    }
}