// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: DashboardModel.cs
// 
// Created by: ykors_000 at 18.11.2013 12:17
// 
// Property of SoftGears
// 
// ========

using System.Collections.Generic;
using PartDesk.Domain.Entities;

namespace PartDesk.Web.Models.Dashboard
{
    /// <summary>
    /// Модель данных используемая для отображения информации на дашбоарде
    /// </summary>
    public class DashboardModel
    {
        /// <summary>
        /// Мои заказы
        /// </summary>
        public List<Order> MyOrders { get; set; }

        /// <summary>
        /// Заказы моей компании
        /// </summary>
        public List<Order> CompanyOrders { get; set; }

        /// <summary>
        /// Новые заказы
        /// </summary>
        public List<Order> NewOrders { get; set; }

        /// <summary>
        /// Назначенные на меня заказы
        /// </summary>
        public List<Order> AssignedOrders { get; set; }

        /// <summary>
        /// Статистика
        /// </summary>
        public DashboardStatisticsModel Statistics { get; set; }
    }
}