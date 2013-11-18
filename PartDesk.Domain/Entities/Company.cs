// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: Company.cs
// 
// Created by: ykors_000 at 18.11.2013 14:56
// 
// Property of SoftGears
// 
// ========

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PartDesk.Domain.Entities
{
    /// <summary>
    /// Компания, зарегистрированная в системе
    /// </summary>
    public partial class Company
    {
        /// <summary>
        /// Возвращает количество заказов, оформленное компанией
        /// </summary>
        /// <returns></returns>
        public int GetOrdersCount()
        {
            return Users.Sum(u => u.GetOrdersCount());
        }

        /// <summary>
        /// Возвращает количество денег, которые должен уплатить или уплатил поставщик за все заказы
        /// </summary>
        /// <returns></returns>
        public decimal GetIncomeAmount()
        {
            return Users.Sum(u => u.GetOrdersIncomeAmount());
        }

        /// <summary>
        /// Возвращает коллекцию пользователей компании
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUsers()
        {
            return Users.ToList();
        }

        /// <summary>
        /// Возвращает список заказов, которые получили сотрудники компаний
        /// </summary>
        /// <returns></returns>
        public IList<Order> GetOrders()
        {
            return Orders.Where(o => o.Status > 0).OrderByDescending(o => o.LastUpdate).ToList();
        }
    }
}