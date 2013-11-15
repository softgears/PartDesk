using System.Linq;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL.Repositories
{
    /// <summary>
    /// СУБД реализация репозитория пользователей
    /// </summary>
    public class UsersRepository: BaseRepository<User>, IUsersRepository
    {
        /// <summary>
        /// СУБД реализация репозитория пользователей
        /// </summary>
        /// <param name="dataContext"></param>
        public UsersRepository(PartDeskDataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Загружает указанную сущность по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным идентификатором или Null</returns>
        public override User Load(long id)
        {
            return Find(u => u.Id == id);
        }

        /// <summary>
        /// Возвращает пользователя с указанной комбинацией логина и пароля
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="passwordHash">Хеш пароля</param>
        /// <returns>Объект пользователя</returns>
        public User GetUserByLoginAndPasswordHash(string login, string passwordHash)
        {
            return Find(u => u.Email.ToLower() == login.ToLower() && u.PasswordHash == passwordHash);
        }

        /// <summary>
        /// Проверяет, существует ли в системе пользователь с указанным логином
        /// </summary>
        /// <param name="login">Логин пользователь</param>
        /// <returns>true если существует</returns>
        public bool ExistsUserWithLogin(string login)
        {
            return Find(s => s.Email.ToLower() == login.ToLower()) != null;
        }

        /// <summary>
        /// Возвращает пользователя по его логину
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public User FindUserByLogin(string identity)
        {
            return Find(s => s.Email.ToLower() == identity.ToLower());
        }

        /// <summary>
        /// Удаляет сущность из репозитория
        /// </summary>
        /// <param name="entity">Сущность для удаления</param>
        public override void Delete(User entity)
        {
            base.Delete(entity);
        }
    }
}