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
using PartDesk.Web.Models.Manage;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления заказами
    /// </summary>
    public class ManageOrdersController : BaseController
    {
        /// <summary>
        /// Отображает список заказов исходя из указанных условий фильтрации
        /// </summary>
        /// <returns></returns>
        [Route("manage/orders")][AuthorizationCheck(Permission.ManageOrders)]
        public ActionResult Index(ManageOrdersListModel model)
        {
            // Репозиторий
            var rep = Locator.GetService<IOrdersRepository>();

            // Все заказы
            var orders = rep.Search(o => o.Status > 0);

            // Фильтруем
            if (model.Statuses.Length > 0)
            {
                orders = orders.Where(o => model.Statuses.Contains((OrderStatus)o.Status));
            }
            if (model.CompaniesId.Length > 0)
            {
                orders = orders.Where(o => model.CompaniesId.Contains(o.CompanyId));
            }
            if (model.UsersId.Length > 0)
            {
                orders = orders.Where(o => model.UsersId.Contains(o.AuthorId));
            }
            if (model.ManagersId.Length > 0)
            {
                orders = orders.Where(o => model.ManagersId.Contains(o.ManagerId));
            }

            // Выбираем
            model.Fetched = orders.OrderByDescending(o => o.LastUpdate).ToList();

            PushNavigationItem("Управление заказами","/manage/orders");
            PushNavigationItem("Список заказов","#");

            return View(model);
        }

        /// <summary>
        /// Отображает информацию о заказе
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/{id}/info")]
        public ActionResult Info(long id)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var order = rep.Load(id);
            if (order == null)
            {
                ShowError("Такой заказ не найден");
                return RedirectToAction("Index");
            }

            PushNavigationItem("Управление заказами", "/manage/orders");
            PushNavigationItem("Список заказов", "#");

            return View(order);
        }

        /// <summary>
        /// Обрабатывает изменение цены поставщика на позицию в заказе
        /// </summary>
        /// <param name="id">Идентификатор позиции</param>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="value">Новое значение</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/change-vendor-price")]
        public ActionResult ChangeVendorPrice(long id, long orderId, decimal value)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var order = rep.Load(orderId);
            if (order == null)
            {
                return Json(new {success = false, msg = "Заказ не найден"});
            }

            // Ищем позицию
            var item = order.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (item == null)
            {
                return Json(new {success = false, msg = "Позиция не найдена"});
            }

            // Меняем и пересчитываем
            item.VendorPrice = value;
            item.Price = item.VendorPrice.Value + item.Margin;
            item.DateModified = DateTime.Now;
            rep.SubmitChanges();

            // Отдаем результат
            return Json(new {success = true});
        }

        /// <summary>
        /// Обрабатывает изменение цены накрутки на позицию
        /// </summary>
        /// <param name="id">Идентификатор позиции</param>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="value">Новое значение</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/change-margin")]
        public ActionResult ChangeMargin(long id, long orderId, decimal value)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var order = rep.Load(orderId);
            if (order == null)
            {
                return Json(new {success = false, msg = "Заказ не найден"});
            }

            // Ищем позицию
            var item = order.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (item == null)
            {
                return Json(new {success = false, msg = "Позиция не найдена"});
            }

            // Меняем и пересчитываем
            item.Margin = value;
            item.Price = item.VendorPrice.Value + item.Margin;
            item.DateModified = DateTime.Now;
            rep.SubmitChanges();

            // Отдаем результат
            return Json(new {success = true});
        }

        /// <summary>
        /// Обрабатывает изменение количества позиций
        /// </summary>
        /// <param name="id">Идентификатор позиции</param>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="value">Новое значение</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/change-quantity")]
        public ActionResult ChangeQuantity(long id, long orderId, int value)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var order = rep.Load(orderId);
            if (order == null)
            {
                return Json(new {success = false, msg = "Заказ не найден"});
            }

            // Ищем позицию
            var item = order.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (item == null)
            {
                return Json(new {success = false, msg = "Позиция не найдена"});
            }

            // Меняем и пересчитываем
            item.Quantity = value;
            item.DateModified = DateTime.Now;
            rep.SubmitChanges();

            // Отдаем результат
            return Json(new {success = true});
        }

        /// <summary>
        /// Обрабатывает удаление указанной позиции из заказа
        /// </summary>
        /// <param name="id">Идентификатор позиции</param>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/position/{id}/delete")]
        public ActionResult DeletePosition(long id, long orderId)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var order = rep.Load(orderId);
            if (order == null)
            {
                ShowError("Такой заказ не найден");
                return RedirectToAction("Index");
            }

            // Ищем позицию
            var item = order.OrderItems.FirstOrDefault(oi => oi.Id == id);
            if (item == null)
            {
                ShowError("Такая позиция не найдена");
                return RedirectToAction("Info",new {id = orderId});
            }

            // Удаляем
            order.OrderItems.Remove(item);
            rep.SubmitChanges();
            ShowSuccess("Позиция успешно удалена");

            return RedirectToAction("Info", new {id = orderId});
        }

        /// <summary>
        /// Обрабатывает изменение статуса заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="status">Новый статус</param>
        /// <param name="comments">Комментарии к заказу</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/change-status")]
        public ActionResult ChangeStatus(long id, short status, string comments)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var order = rep.Load(id);
            if (order == null)
            {
                ShowError("Такой заказ не найден");
                return RedirectToAction("Index");
            }

            // Изменяем
            order.Status = status;
            order.OrderStatusChangements.Add(new OrderStatusChangement()
            {
                User = CurrentUser,
                Status = status,
                Comments = comments,
                DateCreated = DateTime.Now,
                Order = order
            });
            rep.SubmitChanges();

            // Уведомляем
            ShowSuccess("Статус заказа был успешно изменен");

            return Redirect(string.Format("/manage/orders/{0}/info#history", id));
        }

        /// <summary>
        /// Обрабатывает изменение статуса заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="managerId">Идентификатор менеджера</param>
        /// <returns></returns>
        [AuthorizationCheck(Permission.ManageOrders)][Route("manage/orders/change-manager")]
        public ActionResult ChangeManager(long id, long managerId)
        {
            // Ищем заказ
            var rep = Locator.GetService<IOrdersRepository>();
            var usersRep = Locator.GetService<IUsersRepository>();
            var order = rep.Load(id);
            if (order == null)
            {
                ShowError("Такой заказ не найден");
                return RedirectToAction("Index");
            }

            // Ищем пользователя
            var manager = usersRep.Load(managerId);
            if (manager == null)
            {
                ShowError("Такой менеджер не найден");
                return RedirectToAction("Index");
            }

            // Изменяем
            if (manager.Id != order.ManagerId)
            {
                order.Manager.ManagedOrders.Remove(order);
                manager.ManagedOrders.Add(order);

                order.OrderStatusChangements.Add(new OrderStatusChangement()
                {
                    User = CurrentUser,
                    Status = order.Status,
                    Comments = string.Format("Новый менеджер {0} назначен на заказ", manager.Id),
                    DateCreated = DateTime.Now,
                    Order = order
                });

                rep.SubmitChanges();
                
                // Уведомляем
                ShowSuccess("Менеджер заказа был успешно изменен");
            }

            return Redirect(string.Format("/manage/orders/{0}/info#history", id));
        }

    }
}
