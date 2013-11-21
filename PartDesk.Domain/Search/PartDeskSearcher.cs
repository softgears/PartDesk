// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: PartDeskSearcher.cs
// 
// Created by: ykors_000 at 21.11.2013 12:34
// 
// Property of SoftGears
// 
// ========

using System.Collections.Generic;
using System.Linq;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.Interfaces.Search;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Search.Vendors;

namespace PartDesk.Domain.Search
{
    /// <summary>
    /// Класс выполняющий поиск запчастей по всем поставщика
    /// </summary>
    public class PartDeskSearcher: IPartDeskSearch
    {
        /// <summary>
        /// Выполняет поиск 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public IList<SearchResultItem> Search(string article)
        {
            // Список всех поставщиков
            var vendors = new List<IVendorSearcher>();
            
            // Формируем список поставщиков исходя из настроек
            var settingsRep = Locator.GetService<ISettingsRepository>();
            if (settingsRep.GetValue<bool>("search_autotrader"))
            {
                vendors.Add(new AutoTradeSearcher());
            }

            // Выбираем данные параллельно
            var fetched = vendors.AsParallel().Select(v => v.Search(article));

            // Формируем результат
            var result = new List<SearchResultItem>();
            foreach (var fetch in fetched)
            {
                result.AddRange(fetch);
            }
            return result;
        }
    }
}