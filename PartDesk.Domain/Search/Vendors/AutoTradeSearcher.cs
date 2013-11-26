// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: AutoTradeSearcher.cs
// 
// Created by: ykors_000 at 21.11.2013 12:50
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using PartDesk.Domain.Enums;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.Interfaces.Search;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Search.Models;
using PartDesk.Domain.Utils;

namespace PartDesk.Domain.Search.Vendors
{
    /// <summary>
    /// Поисковик по API сайта AutoTrade.su
    /// </summary>
    public class AutoTradeSearcher: IVendorSearcher
    {
        /// <summary>
        /// Выполняет поиск по указанному поставщике
        /// </summary>
        /// <param name="article">Артикул детали</param>
        /// <returns></returns>
        public IList<SearchResultItem> Search(string article)
        {
            using (var scope = Locator.BeginNestedHttpRequestScope())
            {
                // Инициаилизируем
                var settingsRep = Locator.GetService<ISettingsRepository>();
                var apiKey = settingsRep.GetValue<string>("api_autotrade_key");

                // Подгатавливаем объект к запросу
                var requestModel = new AutotradeRequestModel(apiKey, "getItemsByList",
                    new { items = new Dictionary<string, string> { { article, "10" } } });

                var response = ExecuteRequest<AutotradeGetItemsByListResponse>(requestModel);

                var result = new List<SearchResultItem>();

                // Анализируем что там пришло
                if (response != null && response.Items.Count > 0)
                {
                    foreach (var item in response.Items.Values)
                    {
                        // Добавляем основной элемент
                        result.Add(new SearchResultItem()
                        {
                            Article = item.Article,
                            VendorId = item.InnerId,
                            Brand = item.Brand,
                            Quantity = item.Stocks.Values.Max(v => v.QuantityPacked + v.QuantityUnpacked),
                            Name = item.Name,
                            Vendor = PartVendor.Autotrade,
                            VendorPrice = item.Price
                        });

                        // Добавляем кроссы и замены
                        if (item.Subs != null && item.Subs.Count > 0)
                        {
                            foreach (var sub in item.Subs.Values)
                            {
                                result.Add(new SearchResultItem()
                                {
                                    Article = sub.Article,
                                    VendorId = sub.InnerId,
                                    Quantity = sub.Stocks.Values.Max(v => v.QuantityPacked + v.QuantityUnpacked),
                                    Name = sub.Name,
                                    Vendor = PartVendor.Autotrade,
                                    VendorPrice = sub.Price,
                                    Brand = sub.Brand
                                });
                            }
                        }
                    }
                }

                return result;   
            }
        }

        /// <summary>
        /// Выполняет запрос к API магазина Autotrade.su и возвращает сериализованный объект
        /// </summary>
        /// <typeparam name="T">Тип результата</typeparam>
        /// <param name="requestModel">Модель данных запроса</param>
        /// <returns></returns>
        private T ExecuteRequest<T>(AutotradeRequestModel requestModel)
        {
            // Сериализуем
            var requestData = JsonConvert.SerializeObject(requestModel);

            // Подгатавливаем объект
            var client = new WebClient();
            const string url = "http://api2.autotrade.su/?json";

            var response = Encoding.UTF8.GetString(client.UploadValues(url,new NameValueCollection(){{"data",requestData}}));

            // Десериализуем данные
            return JsonConvert.DeserializeObject<T>(response, new JsonSerializerSettings()
            {
                Culture = CultureInfo.GetCultureInfo("ru-RU")
            });
        }
    }
}