using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Enums;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Routing;
using PartDesk.Web.Classes.Security;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления настройками системы
    /// </summary>
    public class ManageSettingsController : BaseController
    {
        /// <summary>
        /// Отображает список настроек системы
        /// </summary>
        /// <returns></returns>
        [Route("manage/settings")][AuthorizationCheck(Permission.ManageSettings)]
        public ActionResult Index()
        {
            var rep = Locator.GetService<ISettingsRepository>();
            var settings = rep.FindAll().ToList();

            PushNavigationItem("Настройки","/manage/settings");
            PushNavigationItem("Список настроек системы","#");

            return View(settings);
        }

        /// <summary>
        /// Обрабатывает сохранение значений настроек
        /// </summary>
        /// <param name="collection">Коллекция данных форм</param>
        /// <returns></returns>
        [Route("manage/settings/save")][AuthorizationCheck(Permission.ManageSettings)][HttpPost]
        public ActionResult Save(FormCollection collection)
        {
            var rep = Locator.GetService<ISettingsRepository>();
            var settings = rep.FindAll().ToList();

            foreach (var setting in settings)
            {
                switch ((SettingDataType)setting.DataType)
                {
                    case SettingDataType.String:
                    case SettingDataType.Digits:
                        setting.Value = collection[setting.Key];
                        break;
                    case SettingDataType.Boolean:
                        setting.Value = collection.AllKeys.Contains(setting.Key) ? "true" : "false";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                setting.DateModified = DateTime.Now;
            }
            rep.SubmitChanges();

            ShowSuccess("Настройки успешно сохранены");

            return RedirectToAction("Index");
        }

    }
}
