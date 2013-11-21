// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: SearchResultItem.cs
// 
// Created by: ykors_000 at 21.11.2013 12:16
// 
// Property of SoftGears
// 
// ========

using System;
using System.Text;
using PartDesk.Domain.Enums;

namespace PartDesk.Domain.Search
{
    /// <summary>
    /// Результат поиска запчасти
    /// </summary>
    public class SearchResultItem
    {
        /// <summary>
        /// Артикул запчасти
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Наименование запчасти у поставщика
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Поставщик, у которого найдена данная запчасть
        /// </summary>
        public PartVendor Vendor { get; set; }

        /// <summary>
        /// Внутренний идентификатор запчасти у поставщика
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Количество остатков на складе поставщика
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Цена на запчасть у поставщика
        /// </summary>
        public decimal? VendorPrice { get; set; }

        /// <summary>
        /// Запчасть является кроссом т.е. аналогом от оригинала
        /// </summary>
        public bool IsCross { get; set; }

        /// <summary>
        /// Абсолютный идентификатор запчасти в системе
        /// </summary>
        public string Id
        {
            get
            {
                var sb = new StringBuilder();
                string partVendor;
                switch (Vendor)
                {
                    case PartVendor.Autotrade:
                        partVendor = "AT";
                        break;
                    case PartVendor.BERG:
                        partVendor = "BERG";
                        break;
                    case PartVendor.MXGroup:
                        partVendor = "MX";
                        break;
                    case PartVendor.GKAutomechanics:
                        partVendor = "GKA";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                sb.AppendFormat("{0}-{1}", partVendor, Article);
                if (!String.IsNullOrEmpty(VendorId))
                {
                    sb.AppendFormat("-{0}", VendorId);
                }
                return sb.ToString();
            }
        }
    }
}