// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: OrdersRepository.cs
// 
// Created by: ykors_000 at 15.11.2013 14:02
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL.Repositories
{
    /// <summary>
    /// СУБД реализация репозитория заказов
    /// </summary>
    public class OrdersRepository: BaseRepository<Order>, IOrdersRepository
    {
        /// <summary>
        /// Инициализирует новый инстанс абстрактного репозитория для указанного типа
        /// </summary>
        /// <param name="dataContext"></param>
        public OrdersRepository(PartDeskDataContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Загружает указанную сущность по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным идентификатором или Null</returns>
        public override Order Load(long id)
        {
            return Find(o => o.Id == id);
        }
    }
}