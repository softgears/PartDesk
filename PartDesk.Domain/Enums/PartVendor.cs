// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: PartVendor.cs
// 
// Created by: ykors_000 at 21.11.2013 12:05
// 
// Property of SoftGears
// 
// ========
namespace PartDesk.Domain.Enums
{
    /// <summary>
    /// Поставщики
    /// </summary>
    public enum PartVendor: short
    {
        /// <summary>
        /// Autotrader.su
        /// </summary>
        [EnumDescription("Autotrade.su")]
        Autotrade = 1,

        /// <summary>
        /// Берг
        /// </summary>
        [EnumDescription("Берг")]
        BERG = 2,

        /// <summary>
        /// MXGroup.ru
        /// </summary>
        [EnumDescription("MXGroup")]
        MXGroup = 3,

        /// <summary>
        /// ГК Автомеханика
        /// </summary>
        [EnumDescription("ГК Автомеханика")]
        GKAutomechanics = 4
    }
}