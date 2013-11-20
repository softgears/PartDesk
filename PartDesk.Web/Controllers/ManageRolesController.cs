using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using PartDesk.Domain.Entities;
using PartDesk.Domain.Interfaces.Repositories;
using PartDesk.Domain.IoC;
using PartDesk.Domain.Routing;
using PartDesk.Web.Classes.Security;

namespace PartDesk.Web.Controllers
{
    /// <summary>
    /// Контроллер управления ролями
    /// </summary>
    public class ManageRolesController : BaseController
    {
        /// <summary>
        /// Отображает список зарегистрированных системных ролей
        /// </summary>
        /// <returns></returns>
        [Route("manage/roles")][AuthorizationCheck(Permission.ManageRoles)]
        public ActionResult Index()
        {
            // Репозиторий
            var rep = Locator.GetService<IRolesRepository>();
            var roles = rep.FindAll().ToList();

            PushNavigationItem("Роли","/manage/roles");
            PushNavigationItem("Список ролей","#");

            return View(roles);
        }

        /// <summary>
        /// Отображает форму создания новой роли
        /// </summary>
        /// <returns></returns>
        [Route("manage/roles/add")]
        [AuthorizationCheck(Permission.ManageRoles)]
        public ActionResult Add()
        {
            PushNavigationItem("Роли", "/manage/roles");
            PushNavigationItem("Новая роль", "#");

            return View("EditRole", new Role());
        }

        /// <summary>
        /// Отображает форму редактирования существующей роли
        /// </summary>
        /// <param name="id">Идентификатор роли</param>
        /// <returns></returns>
        [Route("manage/roles/{id}/edit")]
        [AuthorizationCheck(Permission.ManageRoles)]
        public ActionResult Edit(long id)
        {
            // Репозиторий
            var rep = Locator.GetService<IRolesRepository>();
            var role = rep.Load(id);
            if (role == null)
            {
                ShowError("Такая роль не найдена");
                return RedirectToAction("Index");
            }

            PushNavigationItem("Роли", "/manage/roles");
            PushNavigationItem("Новая роль", "#");

            return View("EditRole", role);
        }

        /// <summary>
        /// Удаляет указанную роль если в ней нет пользователей
        /// </summary>
        /// <param name="id">Идентификатор роли</param>
        /// <returns></returns>
        [Route("manage/roles/{id}/delete")]
        [AuthorizationCheck(Permission.ManageRoles)]
        public ActionResult Delete(long id)
        {
            // Репозиторий
            var rep = Locator.GetService<IRolesRepository>();
            var role = rep.Load(id);
            if (role == null)
            {
                ShowError("Такая роль не найдена");
                return RedirectToAction("Index");
            }

            if (role.Id <= 2)
            {
                ShowError("Нельзя удалить эту роль");
                return RedirectToAction("Index");
            }

            if (role.Users.Count != 0)
            {
                ShowError("Нельзя удалить роль, в которой находятся пользователи");
                return RedirectToAction("Index");
            }

            rep.Delete(role);
            rep.SubmitChanges();

            ShowSuccess("Роль успешно удалена");

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Сохраняет изменения в существующей роли или создает новую роль
        /// </summary>
        /// <param name="model">Модель основных данных роли</param>
        /// <param name="collection">Коллекция</param>
        /// <returns></returns>
        [Route("manage/roles/save")]
        [AuthorizationCheck(Permission.ManageRoles)]
        [HttpPost]
        public ActionResult Save(Role model, FormCollection collection)
        {
            // Репозиторий
            var rep = Locator.GetService<IRolesRepository>();
            Role role;
            if (model.Id <= 0)
            {
                role = model;
                role.DateCreated = DateTime.Now;

                rep.Add(role);
                rep.SubmitChanges();

                ShowSuccess("Роль успешно создана");
            }
            else
            {
                role = rep.Load(model.Id);
                if (role == null)
                {
                    ShowError("Такая роль не найдена");
                    return RedirectToAction("Index");
                }

                role.SystemName = model.SystemName;
                role.DisplayName = model.DisplayName;
                role.DateModified = DateTime.Now;

                rep.SubmitChanges();
                ShowSuccess("Роль успешно отредактирована");
            }

            // Обновляем пермишенны для роли
            role.RolePermissions.Clear();
            foreach (var permId in collection.AllKeys.Where(k => k.StartsWith("permission_")).Select(key => Convert.ToInt64(key.Split('_')[1])))
            {
                role.RolePermissions.Add(new RolePermission()
                {
                    Role = role,
                    PermissionId = permId,
                    DateCreated = DateTime.Now
                });
            }
            rep.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}
