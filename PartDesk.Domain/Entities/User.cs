﻿// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: User.cs
// 
// Created by: ykors_000 at 18.11.2013 11:40
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartDesk.Domain.Utils;

namespace PartDesk.Domain.Entities
{
    /// <summary>
    /// Авторизованный пользователь системы
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Проверяет, есть ли у пользователя указанное разрешение
        /// </summary>
        /// <param name="requiredPermission"></param>
        /// <returns></returns>
        public bool HasPermission(long requiredPermission)
        {
            return Role.RolePermissions.Any(rp => rp.PermissionId == requiredPermission);
        }

        /// <summary>
        /// Возвращает количество заказов
        /// </summary>
        /// <returns></returns>
        public int GetOrdersCount()
        {
            return 0;
        }

        /// <summary>
        /// Возвращает урл аватарки
        /// </summary>
        /// <returns></returns>
        public string GetAvatar()
        {
            return String.Format("http://www.gravatar.com/avatar/{0}?d=monsterid",
                                 PasswordUtils.GenerateMD5PasswordHash(Email.Trim().ToLower()).ToLower());
        }

        /// <summary>
        /// Возвращает фио пользователя
        /// </summary>
        /// <returns></returns>
        public string GetFio()
        {
            return String.Format("{0} {1} {2}", LastName, FirstName, SurName).Trim();
        }

        /// <summary>
        /// Возвращает ФИО в сокращенном формате
        /// </summary>
        /// <returns></returns>
        public string GetFioSmall()
        {
            var sb = new StringBuilder();
            sb.Append(LastName);
            if (!String.IsNullOrEmpty(FirstName))
                sb.AppendFormat(" {0}.", FirstName.First());
            if (!String.IsNullOrEmpty(SurName))
                sb.AppendFormat(" {0}.", SurName.First());
            return sb.ToString();
        }

        /// <summary>
        /// Возвращает фио в международном формате
        /// </summary>
        /// <returns></returns>
        public string GetFioInternation()
        {
            var sb = new StringBuilder();
            sb.Append(FirstName);
            if (!String.IsNullOrEmpty(SurName))
                sb.AppendFormat(" {0}", SurName.First());
            if (!String.IsNullOrEmpty(LastName))
                sb.AppendFormat(" {0}", LastName);
            return sb.ToString().Transliterate();
        }

        /// <summary>
        /// Проверяет есть ли у пользователя административные разрешения
        /// </summary>
        /// <returns></returns>
        public bool HasAdministrativePermission()
        {
            return Role.RolePermissions.Any(p => p.Permission.PermissionGroup == "Администрирование");
        }

        /// <summary>
        /// Возвращает сумму, которую пользователь должен уплатить за все свои заказы
        /// </summary>
        /// <returns></returns>
        public decimal GetOrdersIncomeAmount()
        {
            return 0;
        }

        /// <summary>
        /// Получает список всех заказов у пользователя
        /// </summary>
        /// <returns></returns>
        public IList<Order> GetOrders()
        {
            return CreatedOrders.Where(o => o.Status > 0).OrderByDescending(d => d.LastUpdate).ToList();
        }

        /// <summary>
        /// Получает или создает активный неподтвержденный заказ
        /// </summary>
        /// <returns></returns>
        public Order GetOrCreateCurrentOrder()
        {
            // Перебираем список заказов и ищем неподтвержденный
            var activeOrder = CreatedOrders.FirstOrDefault(o => o.Status == 0);
            if (activeOrder == null)
            {
                activeOrder = new Order()
                {
                    Author = this,
                    ManagerId = -1,
                    DateCreated = DateTime.Now,
                    CompanyId = this.CompanyId,
                    ClientId = -1,
                    Status = 0
                };
                CreatedOrders.Add(activeOrder);
            }

            return activeOrder;
        }
    }
}