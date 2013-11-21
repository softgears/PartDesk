// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: IPartDeskSearch.cs
// 
// Created by: ykors_000 at 21.11.2013 12:35
// 
// Property of SoftGears
// 
// ========

using System.Collections.Generic;
using PartDesk.Domain.Search;

namespace PartDesk.Domain.Interfaces.Search
{
    /// <summary>
    /// Класс, выполняющий поиск запчастей по поставщикам
    /// </summary>
    public interface IPartDeskSearch
    {
        /// <summary>
        /// Выполняет поиск 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        IList<SearchResultItem> Search(string article);
    }
}