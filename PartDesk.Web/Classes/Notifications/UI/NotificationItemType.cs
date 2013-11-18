using PartDesk.Domain.Enums;

namespace PartDesk.Web.Classes.Notifications.UI
{
    /// <summary>
    /// ��� �������� �����������
    /// </summary>
    public enum NotificationItemType
    {
        /// <summary>
        /// ��������� �� ������
        /// </summary>
        [EnumDescription("success-notification")]
        Success,

        /// <summary>
        /// ��������� �� ������
        /// </summary>
        [EnumDescription("error-notification")]
        Error,

        /// <summary>
        /// ��������������
        /// </summary>
        [EnumDescription("warning-notification")]
        Warning
    }
}