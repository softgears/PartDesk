// 
// 
// Solution: PartDesk
// Project: PartDesk.Web
// File: IoCConfig.cs
// 
// Created by: ykors_000 at 15.11.2013 14:13
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.DAL;
using PartDesk.Domain.Interfaces.Notifications;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Utils;
using PartDesk.Web.Classes;

namespace PartDesk.Web
{
    /// <summary>
    /// Конфигуратор IoC контейнера
    /// </summary>
    public static class IoCConfig
    {
        /// <summary>
        /// Инициализирует IoC контейнер
        /// </summary>
        public static void Init()
        {
            Locator.Init(new DataAccessLayer(), new DomainLayer(), new WebLayer());
            Locator.GetService<IMailNotificationManager>().Init();
            Locator.GetService<ISMSNotificationManager>().Init();
        }
    }
}