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
using PartDesk.Domain.Entities;
using PartDesk.Domain.Enums;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;

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
        public int Quantity { get; set; }

        /// <summary>
        /// Цена на запчасть у поставщика
        /// </summary>
        public decimal? VendorPrice { get; set; }

        /// <summary>
        /// Бренд
        /// </summary>
        public string Brand { get; set; }

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

        /// <summary>
        /// Наименование склада
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Возвращает true если бренд является оригиналом
        /// </summary>
        public bool IsOriginal
        {
            get
            {
                return "Toyota,Nissan,Mazda,Subaru,Suzuki,Honda,Mitsubishi".ToLower().Contains(Brand.ToLower());
            }
        }

        /// <summary>
        /// Получает цену на позицию с учетом накрутки системы, а так же персональной скидки компании
        /// </summary>
        /// <param name="company">Компания</param>
        /// <param name="companyMargin">Отобразить цену с учетом накрутки компании</param>
        /// <returns></returns>
        public decimal GetPrice(Company company, bool companyMargin = false)
        {
            var margin = CalculateMargin();

            var price = (VendorPrice.HasValue ? VendorPrice.Value : 0) + margin;

            if (companyMargin)
            {
                price -= price*company.PriceMargin/100;
            }

            return price;
        }

        /// <summary>
        /// Вычисляет накрутку системы на указанную позицию товара
        /// </summary>
        /// <returns></returns>
        public decimal CalculateMargin()
        {
            // Получаем процент
            var rep = Locator.GetService<ISettingsRepository>();
            string vendor;
            switch (Vendor)
            {
                case PartVendor.Autotrade:
                    vendor = "autotrade";
                    break;
                case PartVendor.BERG:
                    vendor = "berg";
                    break;
                case PartVendor.MXGroup:
                    vendor = "mxgroup";
                    break;
                case PartVendor.GKAutomechanics:
                    vendor = "gkauto";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Извлекаем данные
            var margin100 = rep.GetValue<decimal>(String.Format("margin_{0}_100", vendor));
            var margin300 = rep.GetValue<decimal>(String.Format("margin_{0}_300", vendor));
            var margin500 = rep.GetValue<decimal>(String.Format("margin_{0}_500", vendor));
            var margin700 = rep.GetValue<decimal>(String.Format("margin_{0}_700", vendor));
            var margin1000 = rep.GetValue<decimal>(String.Format("margin_{0}_1000", vendor));
            var margin2000 = rep.GetValue<decimal>(String.Format("margin_{0}_2000", vendor));
            var margin3000 = rep.GetValue<decimal>(String.Format("margin_{0}_3000", vendor));
            var margin4000 = rep.GetValue<decimal>(String.Format("margin_{0}_4000", vendor));
            var marginOther = rep.GetValue<decimal>(String.Format("margin_{0}_other", vendor));

            // Базовая цена
            var basePrice = VendorPrice.HasValue ? VendorPrice.Value : 0;

            decimal margin = 0;
            if (basePrice <= 100)
            {
                margin = basePrice*margin100/100;
            }
            else if (basePrice <= 300)
            {
                margin = basePrice*margin300/100;
            }
            else if (basePrice <= 500)
            {
                margin = basePrice*margin500/100;
            }
            else if (basePrice <= 700)
            {
                margin = basePrice*margin700/100;
            }
            else if (basePrice <= 1000)
            {
                margin = basePrice*margin1000/100;
            }
            else if (basePrice <= 2000)
            {
                margin = basePrice*margin2000/100;
            }
            else if (basePrice <= 3000)
            {
                margin = basePrice*margin3000/100;
            }
            else if (basePrice <= 4000)
            {
                margin = basePrice*margin4000/100;
            }
            else
            {
                margin = basePrice*marginOther/100;
            }
            return margin;
        }
    }
}