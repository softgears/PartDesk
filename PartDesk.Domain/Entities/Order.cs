// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: Order.cs
// 
// Created by: ykors_000 at 18.11.2013 16:35
// 
// Property of SoftGears
// 
// ========

using System;
using System.Linq;

namespace PartDesk.Domain.Entities
{
    /// <summary>
    /// Заказ в системе
    /// </summary>
    public partial class Order
    {
        /// <summary>
        /// Дата и  время последнего изменения по заказу
        /// </summary>
        public DateTime? LastUpdate
        {
            get {
                return OrderStatusChangements.Count == 0 ? DateCreated : OrderStatusChangements.OrderByDescending(d => d.DateCreated).First().DateCreated;
            }
        }

        /// <summary>
        /// Возвращает статус заказа
        /// </summary>
        /// <returns></returns>
        public string GetStatus()
        {
            
        }
    }
}