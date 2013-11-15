// ============================================================
// 
// 	Asgard
// 	Asgard.Domain 
// 	PermissionsRepository.cs
// 
// 	Created by: ykorshev 
// 	 at 03.08.2013 10:33
// 
// ============================================================

using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL.Repositories
{
    /// <summary>
    /// СУБД реализация репозитория пермишеннов
    /// </summary>
    public class PermissionsRepository: BaseRepository<Permission>, IPermissionsRepository
    {
        /// <summary>
        /// Инициализирует новый инстанс абстрактного репозитория для указанного типа
        /// </summary>
        /// <param name="dataContext"></param>
        public PermissionsRepository(PartDeskDataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Загружает указанную сущность по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным идентификатором или Null</returns>
        public override Permission Load(long id)
        {
            return Find(p => p.Id == id);
        }
    }
}