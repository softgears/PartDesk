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
    /// Контроллер управления компаниями в системе
    /// </summary>
    public class ManageCompaniesController : BaseController
    {
        /// <summary>
        /// Отображает список зарегистрированных компаний в системе
        /// </summary>
        /// <returns></returns>
        [AuthorizationCheck()]
        [Route("manage/companies")]
        public ActionResult Index()
        {
            // Репозиторий
            var rep = Locator.GetService<ICompaniesRepository>();
            var comps = rep.FindAll().OrderBy(c => c.Name).ToList();

            // Навигационная цепочка
            PushNavigationItem("Компании","/manage/companies");
            PushNavigationItem("Список компаний","#");

            return View(new GenericListModel<Company>()
            {
                Fetched = comps
            });
        }

        /// <summary>
        /// Отображает форму создания компании
        /// </summary>
        /// <returns></returns>
        [Route("manage/companies/add")]
        [AuthorizationCheck()]
        public ActionResult Add()
        {
            // Навигационная цепочка
            PushNavigationItem("Компании", "/manage/companies");
            PushNavigationItem("Новая компания", "#");

            return View();
        }

        /// <summary>
        /// Отображает форму редактирования указанной компании
        /// </summary>
        /// <param name="id">Идентификатор компании</param>
        /// <returns></returns>
        [Route("manage/companies/{id}/edit")]
        [AuthorizationCheck()]
        public ActionResult Edit(long id)
        {
            // Репозиторий
            var rep = Locator.GetService<ICompaniesRepository>();
            var comp = rep.Load(id);
            if (comp == null)
            {
                ShowError("Компания не найдена");
                return RedirectToAction("Index");
            }

            // Отображаем
            PushNavigationItem("Компании", "/manage/companies");
            PushNavigationItem("Редактировании компании "+comp.Name, "#");

            // Отображаем вид
            return View(comp);
        }

    }
}
