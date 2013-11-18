using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            return View(new DashboardModel());
        }
    }
}
