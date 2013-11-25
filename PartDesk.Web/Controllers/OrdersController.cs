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

    }
}
