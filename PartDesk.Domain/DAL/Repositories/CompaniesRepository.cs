// ============================================================
// 
// 	Asgard
// 	Asgard.Domain 
// 	CompaniesRepository.cs
// 
// 	Created by: ykorshev 
// 	 at 02.08.2013 11:36
// 
// ============================================================

using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL.Repositories
{
    /// <summary>
    /// СУБД реализация репозитория компаний
    /// </summary>
    public class CompaniesRepository: BaseRepository<Company>, ICompaniesRepository
    {
        /// <summary>
        /// Инициализирует новый инстанс абстрактного репозитория для указанного типа
        /// </summary>
        /// <param name="dataContext"></param>
        public CompaniesRepository(PartDeskDataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Загружает указанную сущность по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным идентификатором или Null</returns>
        public override Company Load(long id)
        {
            return Find(c => c.Id == id);
        }
    }
}