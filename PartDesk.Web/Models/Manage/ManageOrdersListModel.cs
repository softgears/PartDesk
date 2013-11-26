// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: ManageOrdersListModel.cs
// 
// Created by: ykors_000 at 26.11.2013 10:35
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;
using PartDesk.Domain.Enums;

namespace PartDesk.Web.Models.Manage
{
    /// <summary>
    /// Модель управления списком заказов
    /// </summary>
    public class ManageOrdersListModel: GenericListModel<Order>
    {
        /// <summary>
        /// Статусы заказов
        /// </summary>
        public OrderStatus[] Statuses { get; set; }

        /// <summary>
        /// Идентификаторы компаний
        /// </summary>
        public long[] CompaniesId { get; set; }

        /// <summary>
        /// Идентификаторы пользователей
        /// </summary>
        public long[] ManagersId { get; set; }

        /// <summary>
        /// Идентификаторы пользователей
        /// </summary>
        public long[] UsersId { get; set; }

        public ManageOrdersListModel()
        {
            Statuses = new OrderStatus[]{};
            CompaniesId = new long[]{};
            ManagersId = new long[]{};
            UsersId = new long[]{};
        }
    }
}