// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: OrderConfirmationModel.cs
// 
// Created by: ykors_000 at 25.11.2013 15:13
// 
// Property of SoftGears
// 
// ========
namespace PartDesk.Web.Models.Orders
{
    /// <summary>
    /// Модель подтверждения
    /// </summary>
    public class OrderConfirmationModel
    {
        /// <summary>
        /// Адрес доставки
        /// </summary>
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// Комментарии к заказу
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Адрес клиента
        /// </summary>
        public string Address { get; set; }
    }
}