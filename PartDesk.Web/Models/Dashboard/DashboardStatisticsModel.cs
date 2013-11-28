// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: DashboardStatisticsModel.cs
// 
// Created by: ykors_000 at 28.11.2013 12:17
// 
// Property of SoftGears
// 
// ========
namespace PartDesk.Web.Models.Dashboard
{
    /// <summary>
    /// Раздел статистики на дашбоарде
    /// </summary>
    public class DashboardStatisticsModel
    {
        /// <summary>
        /// Всего пользователей
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// Всего компаний
        /// </summary>
        public int TotalCompanies { get; set; }

        /// <summary>
        /// Всего заказов
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Выполнено заказов
        /// </summary>
        public int CompletedOrders { get; set; }

        /// <summary>
        /// Заказов в работе
        /// </summary>
        public int OrdersInWork { get; set; }

        /// <summary>
        /// Отменено заказов
        /// </summary>
        public int CanceledOrders { get; set; }

        /// <summary>
        /// Общая стоимость всех заказов
        /// </summary>
        public decimal TotalOrdersPrice { get; set; }

        /// <summary>
        /// Общая маржа с выполненных заказов
        /// </summary>
        public decimal CompletedOrdersMargin { get; set; }
    }
}