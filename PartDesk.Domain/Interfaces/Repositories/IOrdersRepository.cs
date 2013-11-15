// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: IOrdersRepository.cs
// 
// Created by: ykors_000 at 15.11.2013 13:39
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;

namespace PartDesk.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий заказов
    /// </summary>
    public interface IOrdersRepository: IBaseRepository<Order>
    {
         
    }
}