// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: OrderStatus.cs
// 
// Created by: ykors_000 at 18.11.2013 16:42
// 
// Property of SoftGears
// 
// ========
namespace PartDesk.Domain.Enums
{
    /// <summary>
    /// Статус заказа
    /// </summary>
    public enum OrderStatus: short
    {
        /// <summary>
        /// Создается
        /// </summary>
        [EnumDescription("В стадии создании")]
        Initial = 0,

        /// <summary>
        /// Не подтвержден
        /// </summary>
        [EnumDescription("Не подтвержден")]
        Unverified = 1,

        /// <summary>
        /// Подтвержден
        /// </summary>
        [EnumDescription("Подтвержден")]
        Verified = 2,

        /// <summary>
        /// В стадии обработки
        /// </summary>
        [EnumDescription("В стадии обработки")]
        InProgress = 3,

        [EnumDescription("Ожидание доставки")]
        Logistic = 4,

        [EnumDescription("Выполнен")]
        Completed = 5,

        [EnumDescription("Отменен")]
        Canceled = 6


    }
}