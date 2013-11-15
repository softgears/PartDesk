using PartDesk.Domain.Entities;

namespace PartDesk.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий пользователей
    /// </summary>
    public interface IUsersRepository: IBaseRepository<User>
    {
        /// <summary>
        /// Возвращает пользователя с указанной комбинацией логина и пароля
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="passwordHash">Хеш пароля</param>
        /// <returns>Объект пользователя</returns>
        User GetUserByLoginAndPasswordHash(string login, string passwordHash);

        /// <summary>
        /// Проверяет, существует ли в системе пользователь с указанным логином
        /// </summary>
        /// <param name="login">Логин пользователь</param>
        /// <returns>true если существует</returns>
        bool ExistsUserWithLogin(string login);

        /// <summary>
        /// Возвращает пользователя по его логину
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        User FindUserByLogin(string identity);
    }
}