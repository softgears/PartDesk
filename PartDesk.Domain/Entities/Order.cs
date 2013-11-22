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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PartDesk.Domain.Enums;

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
            return ((OrderStatus) Status).GetEnumMemberName();
        }

        /// <summary>
        /// Возвращает количество уникальных наименований в заказе
        /// </summary>
        /// <returns></returns>
        public int GetTypesCount()
        {
            return OrderItems.Count;
        }

        /// <summary>
        /// Возвращает общее количество элементов в заказе
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            return OrderItems.Sum(oi => oi.Quantity);
        }

        /// <summary>
        /// Возвращает общую стоимость заказа
        /// </summary>
        /// <returns></returns>
        public string GetTotalPrice()
        {
            return String.Format("{0:c}", OrderItems.Sum(oi => oi.Margin * oi.Quantity));
        }

        /// <summary>
        /// Возвращает все элементы заказа
        /// </summary>
        /// <returns></returns>
        public IList<OrderItem> GetItems()
        {
            return OrderItems.ToList();
        }
    }
}