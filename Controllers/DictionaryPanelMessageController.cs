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
    public class DictionaryPanelMessageController : Controller
    {
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator validator = new MessageValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult Index(int id)
        {
            var writer = wm.GetWriter(id);
            ViewBag.mail = writer.WriterMail;
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Message message,string ReceiverId)
        {
            string SenderMail = (string)Session["WriterMail"];

            ValidationResult result = validator.Validate(message);
            if (result.IsValid)
            {
                message.SenderMail = SenderMail;
                message.MessageDate = DateTime.Now;
                mm.AddMessage(message);
                ViewBag.send = "true";
                ViewBag.id = ReceiverId;
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                ViewBag.mail = message.ReceiverMail;
            }
            return View();
        }
    }
}