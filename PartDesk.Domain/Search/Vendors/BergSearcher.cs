// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: BergSearcher.cs
// 
// Created by: ykors_000 at 22.11.2013 11:00
// 
// Property of SoftGears
// 
// ========

using System;
using System.Collections.Generic;
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

namespace PartDesk.Domain.Search.Vendors
{
    /// <summary>
    /// Поисковик по поставщику Berg
    /// </summary>
    public class BergSearcher: IVendorSearcher
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
                var apiKey = settingsRep.GetValue<string>("api_berg_key");

                var result = new List<SearchResultItem>();

                var url =
                    String.Format(
                        "http://api.berg.ru/ordering/get_stock.json?items[0][resource_article]={0}&analogs=1&key={1}",article,apiKey);

                // Клиент
                var request = HttpWebRequest.CreateHttp(url);
                request.AllowAutoRedirect = true;

                try
                {
                    var responseObj = request.GetResponse();

                    ProcessReponse(result,responseObj);
                }
                catch (WebException e)
                {
                    ProcessReponse(result,e.Response);
                }

                return result;
            }
        }

        public void ProcessReponse(List<SearchResultItem> list, WebResponse resp)
        {
            var response = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            var info = JsonConvert.DeserializeObject<BergGetStockResponse>(response);

            if (info != null && info.Resources.Length > 0)
            {
                foreach (var resource in info.Resources.Where(r => r.Offers != null))
                {
                    list.Add(new SearchResultItem()
                    {
                        Article = resource.Article,
                        Name = resource.Name,
                        Vendor = PartVendor.BERG,
                        VendorPrice = resource.Offers.Min(r => r.Price),
                        Quantity = resource.Offers.Sum(r => r.Quantity),
                        VendorId = resource.Id,
                        Brand = resource.Brand.Name
                    });
                }
            }
        }
    }
}