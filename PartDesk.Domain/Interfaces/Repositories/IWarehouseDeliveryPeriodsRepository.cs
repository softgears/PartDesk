// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: IWarehouseDeliveryPeriodsRepository.cs
// 
// Created by: ykors_000 at 28.11.2013 13:18
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;

namespace PartDesk.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий данных о доставке до указанных складов
    /// </summary>
    public interface IWarehouseDeliveryPeriodsRepository: IBaseRepository<WarehouseDeliveryPeriod>
    {
         
    }
}