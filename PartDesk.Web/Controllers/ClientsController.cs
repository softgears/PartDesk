using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Routing;
using PartDesk.Web.Classes.Security;
using PartDesk.Web.Models.Manage;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления клиентами компании
    /// </summary>
    public class ClientsController : BaseController
    {
        /// <summary>
        /// Список клиентов компании
        /// </summary>
        /// <returns></returns>
        [Route("clients")][AuthorizationCheck()]
        public ActionResult Index(GenericListModel<Client> model)
        {
            PushNavigationItem("Клиенты","/clients");
            PushNavigationItem("Список клиентов компании","#");

            model.Fetched = CurrentUser.Company.Clients.OrderByDescending(c => c.DateCreated).ToList();
            
            return View(model);
        }

        /// <summary>
        /// Отображает форму создания нового клиента
        /// </summary>
        /// <returns></returns>
        [Route("clients/add")][AuthorizationCheck]
        public ActionResult Create()
        {
            PushNavigationItem("Клиенты", "/clients");
            PushNavigationItem("Создание нового клиента", "#");

            return View();
        }

        /// <summary>
        /// Отображает форму редактирования существующего клиента и просмотра списка заказов
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns></returns>
        [Route("clients/{id}/edit")][AuthorizationCheck()]
        public ActionResult Edit(long id)
        {
            // Ищем
            var client = CurrentUser.Company.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                ShowError("Такой клиент не найден");
                return RedirectToAction("Index");
            }

            // Навигация
            PushNavigationItem("Клиенты", "/clients");
            PushNavigationItem("Редактирование клиента "+client.ToString(), "#");

            // Вид
            return View(client);
        }

        /// <summary>
        /// Обработка создания или редактирования клиента
        /// </summary>
        /// <param name="model">Модель данных</param>
        /// <returns></returns>
        [Route("clients/save")][AuthorizationCheck()][HttpPost]
        public ActionResult Save(Client model)
        {
            if (model.Id <= 0)
            {
                model.DateCreated = DateTime.Now;
                CurrentUser.Company.Clients.Add(model);
                Locator.GetService<ICompaniesRepository>().SubmitChanges();

                ShowSuccess("Клиент успешно добавлен");

                return RedirectToAction("Index");
            }
            else
            {
                var client = CurrentUser.Company.Clients.FirstOrDefault(c => c.Id == model.Id);
                if (client == null)
                {
                    ShowError("Такой клиент не найден");
                    return RedirectToAction("Index");
                }

                client.FirstName = model.FirstName;
                client.SurName = model.SurName;
                client.LastName = model.LastName;
                client.Phone = model.Phone;
                client.Email = model.Email;
                client.Address = model.Address;
                client.DateModified = DateTime.Now;

                Locator.GetService<ICompaniesRepository>().SubmitChanges();

                ShowSuccess("Клиент успешно отредактирован");

                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Удаляет указанного клиента если у него нет активных заказов
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns></returns>
        [Route("clients/{id}/delete")][AuthorizationCheck()]
        public ActionResult Delete(long id)
        {
            var client = CurrentUser.Company.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                ShowError("Такой клиент не найден");
                return RedirectToAction("Index");
            }

            if (client.Orders.Count > 0)
            {
                ShowError("Нельзя удалить клиента с заказами");
                return RedirectToAction("Index");
            }

            CurrentUser.Company.Clients.Remove(client);
            Locator.GetService<ICompaniesRepository>().SubmitChanges();

            ShowSuccess("Клиент успешно удален");

            return RedirectToAction("Index");

        }
    }
}
