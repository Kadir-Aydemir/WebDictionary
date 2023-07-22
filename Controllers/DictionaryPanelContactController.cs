using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelContactController : Controller
    {
        ContactManager cm = new ContactManager(new EfContactDal());
        ContactValidator validator = new ContactValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());
        // GET: DictionaryPanelContact
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (User.Identity.IsAuthenticated)
            {
                string usermail = User.Identity.Name;
                var writer = wm.GetWriterIdByMail(usermail);
                contact.Name = writer.WriterName + " " + writer.WriterSurname;
                contact.Mail = usermail;
            }
            contact.ContactDate = DateTime.Now;

            ValidationResult result = validator.Validate(contact);
            if (result.IsValid)
            {
                cm.AddContact(contact);
                ViewBag.send = "true";
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }
    }
}