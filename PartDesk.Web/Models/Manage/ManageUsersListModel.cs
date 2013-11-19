// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: ManageUsersListModel.cs
// 
// Created by: ykors_000 at 19.11.2013 13:43
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;

namespace PartDesk.Web.Models.Manage
{
    /// <summary>
    /// Модель списка пользователей
    /// </summary>
    public class ManageUsersListModel: GenericListModel<User>
    {
        /// <summary>
        /// Идентификаторы компаний
        /// </summary>
        public long[] CompanyId { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ManageUsersListModel()
        {
            CompanyId = new long[] {};
        }
    }
}