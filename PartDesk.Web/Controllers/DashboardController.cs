using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.DAL.Repositories;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Enums;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Routing;
using PartDesk.Web.Classes.Security;
using PartDesk.Web.Models.Dashboard;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Котроллер управления дашбоардом
    /// </summary>
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Отображает дашбоард компании в системе
        /// </summary>
        /// <returns></returns>
        [Route("dashboard")][AuthorizationCheck()]
        public ActionResult Index()
        {
            // Отображаем
            PushNavigationItem("Дашбоард","/dashboard");

            // Репозитории
            var ordersRep = Locator.GetService<IOrdersRepository>();
            var usersRep = Locator.GetService<IUsersRepository>();
            var compRep = Locator.GetService<ICompaniesRepository>();

            // Выборка данных
            List<Order> userOrders = new List<Order>();
            List<Order> companyOrders = new List<Order>();
            List<Order> newOrders = new List<Order>();
            List<Order> assignedOrders = new List<Order>();
            var statsModel = new DashboardStatisticsModel();
            if (CurrentUser.CreatedOrders.Count(o => o.Status > 0) > 0)
            {
                userOrders = CurrentUser.CreatedOrders.Where(o => o.Status > 0).OrderByDescending(o => o.LastUpdate).Take(10).ToList();
            }
            if (CurrentUser.Company.Orders.Count > 0)
            {
                companyOrders =
                    CurrentUser.Company.Orders.Where(o => o.Status > 0)
                        .OrderByDescending(o => o.LastUpdate)
                        .Take(10)
                        .ToList();
            }
            if (CurrentUser.HasPermission(Permission.ManageOrders))
            {
                newOrders = ordersRep.Search(o => o.Status > 0 && o.Status <= 4).Take(10).ToList();
            }
            if (CurrentUser.HasPermission(Permission.ManageOrders) && CurrentUser.ManagedOrders.Count(o => o.Status <= 4) > 0)
            {
                assignedOrders =
                    CurrentUser.ManagedOrders.Where(o => o.Status <= 4).OrderByDescending(o => o.DateCreated).Take(10).ToList();
            }
            if (CurrentUser.HasAdministrativePermission())
            {
                statsModel = new DashboardStatisticsModel()
                {
                    TotalUsers = usersRep.FindAll().Count(),
                    TotalCompanies = compRep.FindAll().Count(),
                    TotalOrders = ordersRep.FindAll().Count(o => o.Status > 0),
                    CompletedOrders = ordersRep.Search(o => o.Status == (short) OrderStatus.Completed).Count(),
                    OrdersInWork = ordersRep.Search(o => o.Status > 0 && o.Status <= 4).Count(),
                    CanceledOrders = ordersRep.Search(o => o.Status == (short) OrderStatus.Canceled).Count(),
                    TotalOrdersPrice =
                        ordersRep.Search(o => o.Status > 0 && o.Status != (short) OrderStatus.Canceled)
                            .Sum(o => o.OrderItems.Sum(oi => oi.Price*oi.Quantity)),
                    CompletedOrdersMargin =
                        ordersRep.Search(o => o.Status == (short) OrderStatus.Completed)
                            .Sum(o => o.OrderItems.Sum(oi => oi.Margin*oi.Quantity))
                };
            }

            return View(new DashboardModel()
            {
                MyOrders = userOrders,
                CompanyOrders = companyOrders,
                NewOrders = newOrders,
                AssignedOrders = assignedOrders,
                Statistics = statsModel
            });
        }
    }
}
