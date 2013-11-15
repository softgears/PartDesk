// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: IRolesRepository.cs
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
    /// Абстрактный репозиторий ролей
    /// </summary>
    public interface IRolesRepository: IBaseRepository<Role>
    {
         
    }
}