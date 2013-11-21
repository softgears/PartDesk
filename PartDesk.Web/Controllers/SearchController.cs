using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Search;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Routing;
using PartDesk.Web.Classes.Security;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления поиском автозапчастей
    /// </summary>
    public class SearchController : BaseController
    {
        /// <summary>
        /// Отображает страницу с формой поиской автозапчастей
        /// </summary>
        /// <param name="query">запрос поисковый</param>
        /// <returns></returns>
        [Route("search")][AuthorizationCheck(Permission.SearchParts)]
        public ActionResult Index(string query)
        {
            PushNavigationItem("Поиск","/search");
            PushNavigationItem("Форма поиска автозапчастей","#");

            // Отображает вид
            ViewBag.query = query;
            return View();
        }

        /// <summary>
        /// Отображает частичный вид с результатами поиска
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("search/query")][AuthorizationCheck(Permission.SearchParts)][HttpPost]
        public ActionResult Query(string query)
        {
            // Выполняем поиск
            var searcher = Locator.GetService<IPartDeskSearch>();
            var result = searcher.Search(query);

            // Отдаем результат
            return PartialView(result);
        }

    }
}
