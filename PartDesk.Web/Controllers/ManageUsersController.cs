using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Notifications.Templates;
using PartDesk.Domain.Routing;
using PartDesk.Domain.Utils;
using PartDesk.Web.Classes.Security;
using PartDesk.Web.Models.Manage;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления пользователями, зарегистрированными в системе
    /// </summary>
    public class ManageUsersController : BaseController
    {
        /// <summary>
        /// Страница со списком пользователей, зарегистрированных в системе
        /// </summary>
        /// <param name="model">Модель фильтрации</param>
        /// <returns></returns>
        [Route("manage/users/")]
        public ActionResult Index(ManageUsersListModel model)
        {
            // Репозиторий
            var rep = Locator.GetService<IUsersRepository>();

            IEnumerable<User> users = rep.FindAll().OrderByDescending(c => c.DateRegistred);

            if (model.CompanyId.Length > 0)
            {
                users = users.Where(u => model.CompanyId.Contains(u.CompanyId));
            }

            model.Fetched = users.ToList();

            PushNavigationItem("Пользователи","/manage/users");
            PushNavigationItem("Список пользователей","#");

            return View(model);
        }

        /// <summary>
        /// Отображает форму создания нового пользоователя
        /// </summary>
        /// <param name="CompanyId">Идентификатор компании выбранной при создании по умолчанию</param>
        /// <returns></returns>
        [Route("manage/users/add")][AuthorizationCheck()]
        public ActionResult CreateUser(long? CompanyId)
        {
            PushNavigationItem("Пользователи","/manage/users");
            PushNavigationItem("Создание нового пользователя","#");

            ViewBag.companyId = CompanyId.HasValue ? CompanyId.Value : -1;

            return View();
        }

        /// <summary>
        /// Обрабатывает создание нового пользователя через личный кабинет администратора
        /// </summary>
        /// <param name="user">Модель основных данных пользователя</param>
        /// <param name="Password">Пароль</param>
        /// <param name="PasswordConfirm">Подтверждение пароля</param>
        /// <returns></returns>
        [Route("manage/users/create")][HttpPost][AuthorizationCheck]
        public ActionResult Create(User user, string Password, string PasswordConfirm)
        {
            // Реп
            var rep = Locator.GetService<IUsersRepository>();
            if (rep.ExistsUserWithLogin(user.Email))
            {
                ShowError("Такой пользователь уже зарегистрирован");
                return RedirectToAction("Index");
            }

            // Регистрируем
            user.PasswordHash = PasswordUtils.GenerateMD5PasswordHash(Password);
            user.DateRegistred = DateTime.Now;
            
            rep.Add(user);
            rep.SubmitChanges();

            ShowSuccess(string.Format("Успешно зарегистрирован пользователь {0} {1}. На его почту отправлено письмо с информацией о регистрации", user.GetFio(), user.Email));

            // Уведомляем
            var notificationModel = new
            {
                FIO = user.GetFio(),
                Email = user.Email,
                Password = Password
            };

            NotifyEmail(user, "Регистрация в системе PartDesk",
                new ParametrizedFileTemplate(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Mail", "Register.html"),
                    notificationModel).ToString());

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Карточка пользователя с ограниченной инфорацией=
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [AuthorizationCheck()][Route("manage/users/{id}/info")]
        public ActionResult Info(long id)
        {
            var rep = Locator.GetService<IUsersRepository>();
            var user = rep.Load(id);
            if (user == null)
            {
                ShowError("Такой пользователь не найден");
                return RedirectToAction("Index");
            }

            PushNavigationItem("Пользователи","/manage/users");
            PushNavigationItem("Карточка пользователя "+user.GetFio(),"#");

            return View(user);
        }

        /// <summary>
        /// Обновляет некоторые данные о пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="CompanyId">Идентификатор компании</param>
        /// <param name="RoleId">Идентификатор оли</param>
        /// <param name="Status">Статус пользователя</param>
        /// <returns></returns>
        [AuthorizationCheck()][Route("manage/users/update-info")][HttpPost]
        public ActionResult UpdateCard(long id,long CompanyId, long RoleId, short Status)
        {
            var rep = Locator.GetService<IUsersRepository>();
            var compRep = Locator.GetService<ICompaniesRepository>();
            var rolesRep = Locator.GetService<IRolesRepository>();
            var user = rep.Load(id);
            if (user == null)
            {
                ShowError("Такой пользователь не найден");
                return RedirectToAction("Index");
            }

            var comp = compRep.Load(CompanyId);
            var role = rolesRep.Load(RoleId);

            if (CompanyId != user.CompanyId)
            {
                user.Company.Users.Remove(user);
                comp.Users.Add(user);
            }

            if (RoleId != user.RoleId)
            {
                user.Role.Users.Remove(user);
                role.Users.Add(user);
            }
            user.Status = Status;

            user.DateModified = DateTime.Now;
            rep.SubmitChanges();

            ShowSuccess("Изменения успешно сохранены");

            return RedirectToAction("Index");
        }
    }
}
