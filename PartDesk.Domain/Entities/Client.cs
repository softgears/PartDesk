// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: Client.cs
// 
// Created by: ykors_000 at 19.11.2013 11:10
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PartDesk.Domain.Utils;

namespace PartDesk.Domain.Entities
{
    /// <summary>
    /// Клиент, заказывающий запчасть
    /// </summary>
    public partial class Client
    {
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2} ({3})", LastName, FirstName, SurName,Phone.FormatPhoneNumber());
        }

        /// <summary>
        /// Количество заказов, произведенное данным клиентом
        /// </summary>
        /// <returns></returns>
        public int GetOrdersCount()
        {
            return Orders.Count;
        }

        /// <summary>
        /// Список заказов по указанному клиенту
        /// </summary>
        /// <returns></returns>
        public IList<Order> GetOrders()
        {
            return Orders.OrderByDescending(o => o.LastUpdate).ToList();
        }
    }
}