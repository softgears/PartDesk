// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: AutotradeRequestModel.cs
// 
// Created by: ykors_000 at 21.11.2013 13:41
// 
// Property of SoftGears
// 
// ========

using Newtonsoft.Json;

namespace PartDesk.Domain.Search.Models
{
    /// <summary>
    /// Модель запроса, используемая для вызовов методом API Autotrade.su
    /// </summary>
    public class AutotradeRequestModel
    {
        /// <summary>
        /// API ключ
        /// </summary>
        [JsonProperty("auth_key")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Метод
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Объект параметров
        /// </summary>
        [JsonProperty("params")]
        public object Params { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// КОличество выводимых элементов
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AutotradeRequestModel(string apiKey, string method, object @params, int page = 1, int limit = 100)
        {
            ApiKey = apiKey;
            Method = method;
            Params = @params;
            Page = page;
            Limit = limit;
        }
    }
}