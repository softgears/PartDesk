// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: DataAccessLayer.cs
// 
// Created by: ykors_000 at 15.11.2013 13:55
// 
// Property of SoftGears
// 
// ========

using Autofac;
using Autofac.Integration.Mvc;
using PartDesk.Domain.DAL.Repositories;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL
{
    /// <summary>
    /// Модуль регистрации зависимостей DAL слоя
    /// </summary>
    public class DataAccessLayer: Autofac.Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        ///             registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PartDeskDataContext>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<CompaniesRepository>().As<ICompaniesRepository>();
            builder.RegisterType<UsersRepository>().As<IUsersRepository>();
            builder.RegisterType<PermissionsRepository>().As<IPermissionsRepository>();
            builder.RegisterType<RolesRepository>().As<IRolesRepository>();
            builder.RegisterType<OrdersRepository>().As<IOrdersRepository>();
            builder.RegisterType<MailNotificationMessagesRepository>().As<IMailNotificationMessagesRepository>();
            builder.RegisterType<SMSNotificationMessagesRepository>().As<ISMSNotificationMessagesRepository>();
            builder.RegisterType<SettignsRepository>().As<ISettingsRepository>().InstancePerHttpRequest();
            builder.RegisterType<WarehouseDeliveryPeriodsRepository>().As<IWarehouseDeliveryPeriodsRepository>();
        }
    }
}