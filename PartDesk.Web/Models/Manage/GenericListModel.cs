// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: GenericListModel.cs
// 
// Created by: ykors_000 at 18.11.2013 14:37
// 
// Property of SoftGears
// 
// ========

using System.Collections;
using System.Collections.Generic;

namespace PartDesk.Web.Models.Manage
{
    /// <summary>
    /// Базовый класс для построения списков с фильтрацией
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericListModel<T> where T: class
    {
        /// <summary>
        /// Всего элементов
        /// </summary>
        public int TotalItems { get; set; }
        
        /// <summary>
        /// Элементов на странице
        /// </summary>
        public int PerPage { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Массив выбранных элементов
        /// </summary>
        public IList<T> Fetched { get; set; }

        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string SearchTerm { get; set; }
    }
}