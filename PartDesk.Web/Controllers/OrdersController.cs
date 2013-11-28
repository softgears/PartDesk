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
using PartDesk.Web.Models.Orders;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер заказов пользователя
    /// </summary>
    public class OrdersController : BaseController
    {
        /// <summary>
        /// Отображает список позиций в текущем заказе пользователя
        /// </summary>
        /// <returns></returns>
        [Route("orders/current")][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult Current()
        {
            PushNavigationItem("Заказы","/orders/active");
            PushNavigationItem("Формирование заказа","#");

            return View(CurrentUser.GetOrCreateCurrentOrder());
        }

        /// <summary>
        /// Добавляет позицию в текущий заказ пользователя
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [Route("orders/add-position")][HttpPost][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult AddPosition(OrderItem position)
        {
            var order = CurrentUser.GetOrCreateCurrentOrder();

            // Добавляем позицию в заказ
            var idParts = position.UniqueId.Split('-');
            switch (idParts[0])
            {
                case "AT":
                    position.Vendor = (short) PartVendor.Autotrade;
                    break;
                case "BERG":
                    position.Vendor = (short) PartVendor.BERG;
                    break;
                case "MX":
                    position.Vendor = (short) PartVendor.MXGroup;
                    break;
                case "GKA":
                    position.Vendor = (short) PartVendor.GKAutomechanics;
                    break;
            }
            position.DateCreated = DateTime.Now;
            order.OrderItems.Add(position);
            Locator.GetService<IOrdersRepository>().SubmitChanges();

            return PartialView(order);
        }

        /// <summary>
        /// Удаляет указанную позицию из заказа
        /// </summary>
        /// <param name="id">Идентификатор позиции в заказе</param>
        /// <returns></returns>
        [Route("orders/position/{id}/delete")][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult DeletePosition(long id)
        {
            var order = CurrentUser.GetOrCreateCurrentOrder();
            var item = order.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (item == null)
            {
                ShowError("Позиция не найдена");
                return RedirectToAction("Current");
            }

            order.OrderItems.Remove(item);
            Locator.GetService<IOrdersRepository>().SubmitChanges();
            ShowSuccess("Позиция была успешно удалена");

            return RedirectToAction("Current");
        }

        /// <summary>
        /// Обрабатывает изменение количества наименований в позиции
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Route("orders/change-quantity")][AuthorizationCheck(Permission.MakeOrder)][HttpPost]
        public ActionResult ChangeQuantity(long id, int quantity)
        {
            var order = CurrentUser.GetOrCreateCurrentOrder();
            var item = order.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (item == null)
            {
                return Json(new {success = false, msg = "Позиция не найдена"});
            }

            item.Quantity = quantity;
            item.DateModified = DateTime.Now;
            Locator.GetService<IOrdersRepository>().SubmitChanges();

            return Json(new {success = true});
        }

        /// <summary>
        /// Отображает форму оформления текущего заказа
        /// </summary>
        /// <returns></returns>
        [Route("orders/confirm")][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult Confirm()
        {
            PushNavigationItem("Заказы", "/orders/active");
            PushNavigationItem("Оформление заказа", "#");

            return View(CurrentUser.GetOrCreateCurrentOrder());
        }

        /// <summary>
        /// Подтверждает заказ и присваивает его менеджеру
        /// </summary>
        /// <param name="model">Модель оформления заказа</param>
        /// <returns></returns>
        [HttpPost][Route("orders/do-confirm")][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult DoConfirm(OrderConfirmationModel model)
        {
            var usersRep = Locator.GetService<IUsersRepository>();

            // Ищем менеджера под заказ
            var manager =
                usersRep.Search(u => u.RoleId == 4)
                    .OrderBy(u => u.ManagedOrders.Count(o => o.Status >= 1 && o.Status <= 4))
                    .FirstOrDefault();

            if (manager == null)
            {
                ShowError("В системе не зарегистрировано менеджеров");
                return RedirectToAction("Confirm");
            }

            // Валидируем
            var order = CurrentUser.GetOrCreateCurrentOrder();
            if (order.GetTotalCount() <= 0)
            {
                ShowError("Заказ не может быть пустым");
                return RedirectToAction("Current");
            }

            // Присваиваем клиента
            Client client;
            if (model.ClientId == -1)
            {
                client = new Client()
                {
                    Address = model.Address,
                    Company = CurrentUser.Company,
                    DateCreated = DateTime.Now,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    SurName = model.SurName
                };
                CurrentUser.Company.Clients.Add(client);
                usersRep.SubmitChanges();
            }
            else
            {
                client = CurrentUser.Company.Clients.FirstOrDefault(c => c.Id == model.ClientId);
                if (client == null)
                {
                    ShowError("Такой клиент не найден");
                    return RedirectToAction("Confirm");
                }
            }

            // Устанавливаем заказ
            order.Description = model.Description;
            order.DeliveryAddress = model.DeliveryAddress;
            client.Orders.Add(order);
            order.Status = (short) OrderStatus.Unverified;
            order.DateCreated = DateTime.Now;
            order.Manager = manager;
            manager.ManagedOrders.Add(order);

            // Добавляем этап жизни в заказ
            order.OrderStatusChangements.Add(new OrderStatusChangement()
            {
                AuthorId = CurrentUser.Id,
                Order = order,
                Status = (short)OrderStatus.Unverified,
                Comments = string.Format("Менеджер {0} назначен на заказ", manager.GetFio()),
                DateCreated = DateTime.Now
            });

            usersRep.SubmitChanges();

            PushNavigationItem("Заказы", "/orders/current");
            PushNavigationItem("Заказ подтвержден", "#");

            return View(order);
        }

        /// <summary>
        /// Отображает список активных заказов у компании
        /// </summary>
        /// <returns></returns>
        [Route("orders/active")][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult Active()
        {
            // Реп
            var ordersRep = Locator.GetService<IOrdersRepository>();

            // Сортируем
            var orders = ordersRep.Search(o => o.CompanyId == CurrentUser.CompanyId && o.Status >= 1 && o.Status <= 4).ToList();

            PushNavigationItem("Заказы", "/orders/active");
            PushNavigationItem("Активные заказы", "#");

            return View("OrdersList", new OrdersListModel()
            {
                Title = "Активные заказы",
                Fetched = orders.ToList()
            });
        }

        /// <summary>
        /// Отображает список завершенных заказов
        /// </summary>
        /// <returns></returns>
        [Route("orders/archive")]
        [AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult Completed()
        {
            // Реп
            var ordersRep = Locator.GetService<IOrdersRepository>();

            // Сортируем
            var orders = ordersRep.Search(o => o.CompanyId == CurrentUser.CompanyId && o.Status > 4).ToList();

            PushNavigationItem("Заказы", "/orders/active");
            PushNavigationItem("Завершенные заказы", "#");

            return View("OrdersList", new OrdersListModel()
            {
                Title = "Завершенные заказы",
                Fetched = orders.ToList()
            });
        }

        /// <summary>
        /// Отображает окно с информацией об указанном заказе
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns></returns>
        [Route("orders/{id}/info")][AuthorizationCheck(Permission.MakeOrder)]
        public ActionResult Info(long id)
        {
            // Ищем
            var order = CurrentUser.Company.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                ShowError("Такой заказ не найден");
                return RedirectToAction("Active");
            }

            // Отображаем информацию о заказе
            PushNavigationItem("Заказы", "/orders/active");
            PushNavigationItem("Заказ №"+order.Id, "#");

            return View(order);
        }

    }
}
