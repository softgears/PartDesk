using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Notifications;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Routing;
using PartDesk.Domain.Utils;
using PartDesk.Web.Classes.Security;
using PartDesk.Web.Models.Account;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления личным кабинетом и профилем пользователя
    /// </summary>
    public class AccountController : BaseController
    {
        #region Misc

        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        public IUsersRepository UsersRepository { get; set; }

        /// <summary>
        /// Конструктор с инъекцией параметра
        /// </summary>
        public AccountController()
            : base()
        {
            UsersRepository = Locator.GetService<IUsersRepository>();
        }

        #endregion

        #region Авторизация

        /// <summary>
        /// Проверяет наличие зарегистрированного логина в системе
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <returns></returns>
        [Route("account/check-login")]
        public ActionResult CheckLogin(string Email)
        {
            var exists = UsersRepository.FindUserByLogin(Email) != null;
            if (exists)
            {
                return Content("Пользователь с таким email уже зарегистрирован");
            }
            return Content("true");
        }

        /// <summary>
        /// Отображает страницу входа в личный кабинет
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (IsAuthentificated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        /// <summary>
        /// Обрабатывает авторизацию на сайте
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (IsAuthentificated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // Валидируем что пользоатель есть
            var rep = Locator.GetService<IUsersRepository>();
            var user = rep.GetUserByLoginAndPasswordHash(model.Email,
                                                            PasswordUtils.GenerateMD5PasswordHash(model.Password));
            if (user == null)
            {
                Locator.GetService<IUINotificationManager>().Error("Такой пользователь не найден");
                return View(model);
            }

            // Авторизуем
            AuthorizeUser(user, model.RememberMe);
            user.DateModified = DateTime.Now;
            rep.SubmitChanges();

            // Идем в личный кабинет
            return RedirectToAction("Index","Dashboard");
        }

        /// <summary>
        /// Обрабатыает выход пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            if (IsAuthentificated)
            {
                CloseAuthorization();
            }

            return RedirectToAction("Login");
        }

        #endregion

        #region Profile

        /// <summary>
        /// Отображает форму редактирования профайла текущего пользователя
        /// </summary>
        /// <returns></returns>
        [AuthorizationCheck()]
        public new ActionResult Profile()
        {
            PushNavigationItem("Профиль","/account/profile");

            return View();
        }

        /// <summary>
        /// Обновляет личные данные профиля
        /// </summary>
        /// <param name="model">Модель данных</param>
        /// <returns></returns>
        [AuthorizationCheck][HttpPost][Route("account/profile/update")]
        public ActionResult UpdateProfile(User model)
        {
            CurrentUser.FirstName = model.FirstName;
            CurrentUser.LastName = model.LastName;
            CurrentUser.SurName = model.SurName;
            CurrentUser.Phone = model.Phone;
            CurrentUser.DateModified = model.DateModified;
            Locator.GetService<IUsersRepository>().SubmitChanges();

            ShowSuccess("Профиль был успешно сохранен");

            return RedirectToAction("Profile");
        }

        #endregion
    }
}
