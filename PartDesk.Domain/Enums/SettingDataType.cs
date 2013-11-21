// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: SettingDataType.cs
// 
// Created by: ykors_000 at 21.11.2013 11:18
// 
// Property of SoftGears
// 
// ========
namespace PartDesk.Domain.Enums
{
    /// <summary>
    /// Тип поля настройки
    /// </summary>
    public enum SettingDataType: short
    {
        /// <summary>
        /// Строка
        /// </summary>
        [EnumDescription("Строковое")]
        String = 0,

        /// <summary>
        /// Число
        /// </summary>
        [EnumDescription("Число")]
        Digits = 1,

        /// <summary>
        /// Флаг
        /// </summary>
        [EnumDescription("Флаг")]
        Boolean = 2
    }
}