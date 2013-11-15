// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: PartDeskDataContext.cs
// 
// Created by: ykors_000 at 15.11.2013 13:00
// 
// Property of SoftGears
namespace PartDesk.Domain.DAL
{
    /// <summary>
    /// Контекст доступа к данным
    /// </summary>
    public partial class PartDeskDataContext
    {
        /// <summary>
        /// Стандартный конструктор с инекцией строки подключения
        /// </summary>
        public PartDeskDataContext() : base(System.Configuration.ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString)
        {
        }
    }
}