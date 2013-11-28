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

            var deliveryRep = Locator.GetService<IWarehouseDeliveryPeriodsRepository>();
            var periods = deliveryRep.FindAll().ToList();

            ViewBag.periods = periods;

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

        /// <summary>
        /// Отображает форму добавления нового элемента информации о доставке со складов
        /// </summary>
        /// <returns></returns>
        [Route("manage/settings/periods/add")][AuthorizationCheck(Permission.ManageSettings)]
        public ActionResult AddPeriod()
        {
            PushNavigationItem("Настройки","/manage/settings");
            PushNavigationItem("Добавление периода доставки","#");

            return View("EditPeriod", new WarehouseDeliveryPeriod());
        }

        /// <summary>
        /// Отображает форму редактирования периода
        /// </summary>
        /// <param name="id">Идентификатор периода</param>
        /// <returns></returns>
        [Route("manage/settings/periods/{id}/edit")][AuthorizationCheck(Permission.ManageSettings)]
        public ActionResult EditPeriod(long id)
        {
            var rep = Locator.GetService<IWarehouseDeliveryPeriodsRepository>();
            var period = rep.Load(id);
            if (period == null)
            {
                ShowError("Период не найден");
                return RedirectToAction("Index");
            }

            PushNavigationItem("Настройки", "/manage/settings");
            PushNavigationItem("Редактирование периода доставки", "#");

            return View("EditPeriod", period);
        }

        /// <summary>
        /// Удаляет указанный период доставки с указанного склада
        /// </summary>
        /// <param name="id">Идентификатор периода</param>
        /// <returns></returns>
        [Route("manage/settings/periods/{id}/delete")][AuthorizationCheck(Permission.ManageSettings)]
        public ActionResult DeletePeriod(long id)
        {
            var rep = Locator.GetService<IWarehouseDeliveryPeriodsRepository>();
            var period = rep.Load(id);
            if (period == null)
            {
                ShowError("Период не найден");
                return RedirectToAction("Index");
            }

            rep.Delete(period);
            rep.SubmitChanges();

            ShowSuccess("Период доставки был успешно удален");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создает или обновляет информацию по периоду доставки до указанного склада
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns></returns>
        [Route("manage/settings/periods/save")][AuthorizationCheck(Permission.ManageSettings)][HttpPost]
        public ActionResult SavePeriod(WarehouseDeliveryPeriod model)
        {
            var rep = Locator.GetService<IWarehouseDeliveryPeriodsRepository>();
            if (model.Id <= 0)
            {
                model.DateCreated = DateTime.Now;
                rep.Add(model);
                rep.SubmitChanges();

                ShowSuccess("Информация была успешно создана");
            }
            else
            {
                // Ищем
                var period = rep.Load(model.Id);
                if (period == null)
                {
                    ShowError("Период не найден");
                    return RedirectToAction("Index");
                }

                // Обновляем
                period.WarehouseName = model.WarehouseName;
                period.RowColor = model.RowColor;
                period.Vendor = model.Vendor;
                period.DeliveryDate = model.DeliveryDate;
                period.DateModified = DateTime.Now;
                rep.SubmitChanges();

                ShowSuccess("Информация была успешно изменена");
            }

            return Redirect("/manage/settings/#periods");
        }

    }
}
