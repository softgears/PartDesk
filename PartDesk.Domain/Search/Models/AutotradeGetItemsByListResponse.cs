// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: AutotradeGetItemsByListResponse.cs
// 
// Created by: ykors_000 at 21.11.2013 13:56
// 
// Property of SoftGears
// 
// ========

using System.Collections.Generic;
using Newtonsoft.Json;

namespace PartDesk.Domain.Search.Models
{
    /// <summary>
    /// Модель ответа на запрос о наличии автозапчастей у поставщика Autotrade.su
    /// </summary>
    public class AutotradeGetItemsByListResponse
    {
        /// <summary>
        /// Всего найдено
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// Страница
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// Лимит
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Возвращенные элементы
        /// </summary>
        [JsonProperty("items")]
        public Dictionary<string,AutoTradeGetItemsListItem> Items { get; set; }

        /// <summary>
        /// Элемент в массиве информации о наличии товаров на складе
        /// </summary>
        public class AutoTradeGetItemsListItem
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Артикул
            /// </summary>
            [JsonProperty("article")]
            public string Article { get; set; }

            /// <summary>
            /// Имя
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Запрошенное количество
            /// </summary>
            [JsonProperty("quantity")]
            public string RequestedQuantity { get; set; }

            /// <summary>
            /// Внутренний номер
            /// </summary>
            [JsonProperty("inside_id_in")]
            public string InnerId { get; set; }

            /// <summary>
            /// Цена
            /// </summary>
            [JsonProperty("price")]
            public decimal Price { get; set; }

            /// <summary>
            /// Фото
            /// </summary>
            [JsonProperty("photo")]
            public string Photo { get; set; }

            /// <summary>
            /// Тех. инфо существует
            /// </summary>
            [JsonProperty("techinfo_exists")]
            public string TechInfoExists { get; set; }

            /// <summary>
            /// Кроссы существует
            /// </summary>
            [JsonProperty("cross_exists")]
            public string CrossInfoExists { get; set; }

            /// <summary>
            /// Замены существуют
            /// </summary>
            [JsonProperty("subs_exists")]
            public string SubExists { get; set; }

            /// <summary>
            /// Тип элемента
            /// </summary>
            [JsonProperty("itemtype ")]
            public string ItemType { get; set; }

            /// <summary>
            /// Наличие на складах
            /// </summary>
            [JsonProperty("stocks")]
            public Dictionary<long,AutotradeStockInfo> Stocks { get; set; }

            /// <summary>
            /// Замены на элементы
            /// </summary>
            [JsonProperty("itemSubs")]
            public Dictionary<string,AutoTradeGetItemsListItem> Subs { get; set; }

            /// <summary>
            /// Имя бренда
            /// </summary>
            [JsonProperty("brand")]
            public string Brand { get; set; }
        }

        /// <summary>
        /// Информация о складе
        /// </summary>
        public class AutotradeStockInfo
        {
            /// <summary>
            /// Ид склада
            /// </summary>
            [JsonProperty("id")]
            public long Id { get; set; }

            /// <summary>
            /// Имя склада
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Легенда
            /// </summary>
            [JsonProperty("legend")]
            public string Legend { get; set; }

            /// <summary>
            /// Количество незапакованных остатков
            /// </summary>
            [JsonProperty("quantity_unpacked")]
            public int QuantityUnpacked { get; set; }

            /// <summary>
            /// Количество запакованных остатков
            /// </summary>
            [JsonProperty("quantity_packed")]
            public int QuantityPacked { get; set; }
        }
    }
}