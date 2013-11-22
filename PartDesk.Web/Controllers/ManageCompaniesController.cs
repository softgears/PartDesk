using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
        [AuthorizationCheck(Permission.ManageCompanies)]
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
        [AuthorizationCheck(Permission.ManageCompanies)]
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
        [AuthorizationCheck(Permission.ManageCompanies)]
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

        /// <summary>
        /// Сохраняет изменения в существующей компании или создает новую компанию
        /// </summary>
        /// <param name="model">Модель компании</param>
        /// <param name="collection">Коллекция данных форм</param>
        /// <returns></returns>
        [HttpPost]
        [Route("manage/companies/save")]
        [AuthorizationCheck(Permission.ManageCompanies)]
        public ActionResult Save(Company model, FormCollection collection)
        {
            var rep = Locator.GetService<ICompaniesRepository>();

            // Сохраняем загруженный логотип
            var logo = Request.Files["LogoFile"];
            string logoPath = null;
            if (logo != null && logo.ContentType.Contains("image") && logo.ContentLength > 0)
            {
                // Сохраняем файл
                var serverFilename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+Path.GetExtension(logo.FileName);
                var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "Logos");
                var fullPath = Path.Combine(directory, serverFilename);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                // Преобразуем изображение
                var image = new WebImage(logo.InputStream);
                image.Resize(100, 100, true).Save(fullPath);

                logoPath = String.Format("/uploads/logos/{0}", serverFilename);
            }

            if (model.Id <= 0)
            {
                // Создаем компанию
                model.DateCreated = DateTime.Now;
                if (logoPath != null)
                {
                    model.Logo = logoPath;
                }
                rep.Add(model);
                rep.SubmitChanges();

                ShowSuccess("Компания успешно создана");
            }
            else
            {
                // Ищем и редактируем компанию
                var comp = rep.Load(model.Id);
                if (comp == null)
                {
                    ShowError("Компания не найдена");
                    return RedirectToAction("Index");
                }

                // Обновляем
                comp.Name = model.Name;
                comp.LegalType = model.LegalType;
                comp.Email = model.Email;
                comp.Phone = model.Phone;
                comp.City = model.City;
                comp.Address = model.Address;
                comp.ContactPerson = model.ContactPerson;
                comp.INN = model.INN;
                comp.OGRN = model.OGRN;
                comp.KPP = model.KPP;
                comp.OKPO = model.OKPO;
                comp.ContactPhone = model.ContactPhone;
                comp.Fax = model.Fax;
                comp.DirectorFIO = model.DirectorFIO;
                comp.LegalAddress = model.LegalAddress;
                comp.LegalDocument = model.LegalDocument;
                comp.AccountRNumber = model.AccountRNumber;
                comp.AccountKNumber = model.AccountKNumber;
                comp.AccountBankBIK = model.AccountBankBIK;
                comp.AccountBankName = model.AccountBankName;
                comp.PassportNumber = model.PassportNumber;
                comp.PassportIssuedBy = model.PassportIssuedBy;
                comp.PassportIssueDate = model.PassportIssueDate;
                comp.PassportDivisionCode = model.PassportDivisionCode;
                comp.Status = model.Status;
                comp.PersonalDiscount = model.PersonalDiscount;
                if (logoPath != null)
                {
                    comp.Logo = logoPath;
                }
                comp.DateModified = DateTime.Now;

                rep.SubmitChanges();

                ShowSuccess(string.Format("Изменения в компании {0} успешно сохранены", comp.Name));
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Удаляет указанную компанию, если у нее нет пользователей
        /// </summary>
        /// <param name="id">Идентификатор компании</param>
        /// <returns></returns>
        [Route("manage/companies/{id}/delete")]
        [AuthorizationCheck(Permission.ManageCompanies)]
        public ActionResult Delete(long id)
        {
            // репозиторий
            var rep = Locator.GetService<ICompaniesRepository>();
            var comp = rep.Load(id);
            if (comp == null)
            {
                ShowError("Компания не найдена");
                return RedirectToAction("Index");
            }

            if (comp.Users.Count > 0)
            {
                ShowError("Нельзя удалить компанию, в которой созданы пользователи");
                return RedirectToAction("Index");
            }

            rep.Delete(comp);
            rep.SubmitChanges();

            ShowSuccess("Компания успешно удалена");

            return RedirectToAction("Index");
        }

    }
}
