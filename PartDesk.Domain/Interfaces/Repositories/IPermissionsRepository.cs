// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: IPermissionsRepository.cs
// 
// Created by: ykors_000 at 15.11.2013 13:38
// 
// Property of SoftGears
// 
// ========

using PartDesk.Domain.Entities;

namespace PartDesk.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий пермишеннов
    /// </summary>
    public interface IPermissionsRepository: IBaseRepository<Permission>
    {
         
    }
}