using System.Collections.Generic;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;

namespace PartDesk.Domain.DAL.Repositories
{
    /// <summary>
    /// СУБД реализация репозитория СМС сообщений
    /// </summary>
    public class SMSNotificationMessagesRepository: BaseRepository<SMSNotificationMessage>, ISMSNotificationMessagesRepository
    {
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="dataContext">Контекст доступа к данным</param>
        public SMSNotificationMessagesRepository(PartDeskDataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Загружает указанную сущность по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным идентификатором или Null</returns>
        public override SMSNotificationMessage Load(long id)
        {
            return Find(m => m.Id == id);
        }

        /// <summary>
        /// Получает очередь неотправленных смс сообщений
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SMSNotificationMessage> GetEnqueuedMessages()
        {
            return Search(m => m.Sended == false);
        }
    }
}