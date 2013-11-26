// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: MXGroupSearcher.cs
// 
// Created by: ykors_000 at 21.11.2013 16:30
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PartDesk.Domain.Search.Vendors
{
    /// <summary>
    /// Поисковик по каталогу поставщика MXGroup
    /// </summary>
    public class MXGroupSearcher: IVendorSearcher
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
                var login = settingsRep.GetValue<string>("api_mxgroup_login");
                var password = settingsRep.GetValue<string>("api_mxgroup_password");
                var availableWarehouses = settingsRep.GetValue<string>("api_mxgroup_warehouse_names");

                var result = new List<SearchResultItem>();

                // Выполняем поиск по артикулу
                var url = String.Format("http://zakaz.mxgroup.ru/mxapi/?m={0}&login={1}&password={2}&zapros={3}&out=json", "search", login, password,HttpUtility.UrlEncode(article));
                var client = new WebClient()
                {
                    Encoding = new UTF8Encoding()
                };
                var response = client.DownloadString(url);

                if (!response.Contains("error"))
                {
                    var info = JsonConvert.DeserializeObject<MXGroupSearchResponse>(response,new JsonSerializerSettings()
                    {
                        Culture = CultureInfo.GetCultureInfo("ru-RU")
                    });

                    if (info.Result.Length > 0)
                    {
                        foreach (var res in info.Result)
                        {
                            result.Add(new SearchResultItem()
                            {
                                Article = res.Article,
                                VendorId = res.Id,
                                Brand = res.Brand,
                                Name = res.Name,
                                Quantity = res.Count.Contains("Out") ? 0 : Convert.ToInt32(res.Count),
                                Vendor = PartVendor.MXGroup,
                                VendorPrice = res.DiscountPrice != null ? Convert.ToDecimal(res.DiscountPrice.Replace('.',','),new CultureInfo("ru-RU")) : new decimal?(),
                                Warehouse = res.StoreName,
                                WarehouseId = res.StoreId
                            });
                        }
                        
                    }
                }

                // Фильтруем по именам складов
                if (!String.IsNullOrEmpty(availableWarehouses))
                {
                    result = result.Where(i => !String.IsNullOrEmpty(i.Warehouse) && availableWarehouses.ToLower().Contains(i.Warehouse.ToLower())).ToList();
                }
                

                return result;
            }
        }
    }
}