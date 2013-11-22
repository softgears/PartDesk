// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: BergGetStockResponse.cs
// 
// Created by: ykors_000 at 22.11.2013 11:09
// 
// Property of SoftGears
// 
// ========

using Newtonsoft.Json;

namespace PartDesk.Domain.Search.Models
{
    /// <summary>
    /// Ответ на запрос о наличии к поставщику Берг
    /// </summary>
    public class BergGetStockResponse
    {
        /// <summary>
        /// Ресурсы ответа
        /// </summary>
        [JsonProperty("resources")]
        public BergResponseResource[] Resources { get; set; }

        /// <summary>
        /// Ответ на запрос о наличии ресурса
        /// </summary>
        public class BergResponseResource
        {
            /// <summary>
            /// Идентификатор товара внутренний
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Наименование товара
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Артикул товара
            /// </summary>
            [JsonProperty("article")]
            public string Article { get; set; }

            /// <summary>
            /// Бренд товара
            /// </summary>
            [JsonProperty("brand")]
            public BergBrand Brand { get; set; }

            /// <summary>
            /// Предложения по данному товару
            /// </summary>
            [JsonProperty("offers")]
            public BergOffer[] Offers { get; set; }
        }


        /// <summary>
        /// Информация о бренде
        /// </summary>
        public class BergBrand
        {
            /// <summary>
            /// Идентификатор бренда
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Наименование бренда
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        /// <summary>
        /// Предложение
        /// </summary>
        public class BergOffer
        {
            /// <summary>
            /// Цена в валюте договора
            /// </summary>
            [JsonProperty("price")]
            public decimal Price { get; set; }

            /// <summary>
            /// Количество на складе
            /// </summary>
            [JsonProperty("quantity")]
            public int Quantity { get; set; }

            /// <summary>
            /// Склад предложения
            /// </summary>
            [JsonProperty("warehouse")]
            public BergWarehouse Warehouse { get; set; }
        }

        /// <summary>
        /// Склад
        /// </summary>
        public class BergWarehouse
        {
            /// <summary>
            /// Идентификатор бренда
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Наименование бренда
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Тип бренда
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }
        }
    }
}