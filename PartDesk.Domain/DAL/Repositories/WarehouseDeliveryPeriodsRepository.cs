// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: WarehouseDeliveryPeriodsRepository.cs
// 
// Created by: ykors_000 at 28.11.2013 13:19
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL.Repositories
{
    /// <summary>
    /// СУБД реализация репозитория данных о доставке до указанных складов
    /// </summary>
    public class WarehouseDeliveryPeriodsRepository: BaseRepository<WarehouseDeliveryPeriod>, IWarehouseDeliveryPeriodsRepository
    {
        /// <summary>
        /// Инициализирует новый инстанс абстрактного репозитория для указанного типа
        /// </summary>
        /// <param name="dataContext"></param>
        public WarehouseDeliveryPeriodsRepository(PartDeskDataContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Загружает указанную сущность по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным идентификатором или Null</returns>
        public override WarehouseDeliveryPeriod Load(long id)
        {
            return Find(d => d.Id == id);
        }
    }
}