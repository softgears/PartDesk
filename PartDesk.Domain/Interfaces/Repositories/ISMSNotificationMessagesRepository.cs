using System.Collections.Generic;
using PartDesk.Domain.Entities;

namespace PartDesk.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий SMS нотификаций
    /// </summary>
    public interface ISMSNotificationMessagesRepository: IBaseRepository<SMSNotificationMessage>
    {
        /// <summary>
        /// Получает очередь неотправленных смс сообщений
        /// </summary>
        /// <returns></returns>
        IEnumerable<SMSNotificationMessage> GetEnqueuedMessages();
    }
}