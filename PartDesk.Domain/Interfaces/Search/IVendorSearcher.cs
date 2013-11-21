// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: IVendorSearcher.cs
// 
// Created by: ykors_000 at 21.11.2013 12:32
// 
// Property of SoftGears
// 
// ========

using System.Collections.Generic;
using PartDesk.Domain.Search;

namespace PartDesk.Domain.Interfaces.Search
{
    /// <summary>
    /// Абстрактный поисковик по данным поставщика запчастей
    /// </summary>
    public interface IVendorSearcher
    {
        /// <summary>
        /// Выполняет поиск по указанному поставщике
        /// </summary>
        /// <param name="article">Артикул детали</param>
        /// <returns></returns>
        IList<SearchResultItem> Search(string article);
    }
}