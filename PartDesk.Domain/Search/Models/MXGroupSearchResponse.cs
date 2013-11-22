// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: MXGroupSearchResponse.cs
// 
// Created by: ykors_000 at 21.11.2013 16:37
// 
// Property of SoftGears
// 
// ========

using Newtonsoft.Json;

namespace PartDesk.Domain.Search.Models
{
    /// <summary>
    /// Класс для десериализации ответа от поставщика MXGroup
    /// </summary>
    public class MXGroupSearchResponse
    {
        /// <summary>
        /// Запрос
        /// </summary>
        [JsonProperty("zapros")]
        public string Request { get; set; }

        /// <summary>
        /// Результаты поиска
        /// </summary>
        [JsonProperty("result")]
        public MXGroupSearchResultItem[] Result { get; set; }

        /// <summary>
        /// Результат подбора
        /// </summary>
        public class MXGroupSearchResultItem
        {
            /// <summary>
            /// Артикул
            /// </summary>
            [JsonProperty("articul")]
            public string Article { get; set; }

            /// <summary>
            /// Код
            /// </summary>
            [JsonProperty("code")]
            public string Code { get; set; }

            /// <summary>
            /// Наименование товара
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Цена
            /// </summary>
            [JsonProperty("price")]
            public string Price { get; set; }

            /// <summary>
            /// Цена со скидкой
            /// </summary>
            [JsonProperty("discountprice")]
            public string DiscountPrice { get; set; }

            /// <summary>
            /// Остаток
            /// </summary>
            [JsonProperty("count")]
            public string Count { get; set; }

            /// <summary>
            /// Идентификатор в 1с
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Имя бренда
            /// </summary>
            [JsonProperty("brand")]
            public string Brand { get; set; }
        }

        
    }
}