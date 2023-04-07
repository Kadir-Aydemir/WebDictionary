using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{

    public class MessageController : Controller
    {
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator validator = new MessageValidator();

        [Authorize]
        public ActionResult Inbox()
        {
            var list = mm.GetListInbox();
            return View(list);
        }

        [Authorize]
        public ActionResult Sendbox()
        {
            var list = mm.GetListSendbox();
            return View(list);
        }

        [Authorize]
        [HttpGet]
        public ActionResult addNewMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addNewMessage(Message message)
        {
            ValidationResult result = validator.Validate(message);
            if (result.IsValid)
            {
                message.SenderMail = "admin@gmail.com";
                message.MessageDate = DateTime.Now;
                mm.AddMessage(message);
                return RedirectToAction("Sendbox");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        [Authorize]
        public ActionResult inboxDetails(int id)
        {
            var inbox = mm.GetMessage(id);
            if (inbox.IsRead == false)
            {
                inbox.IsRead = true;
                mm.UpdateMessage(inbox);
            }
            return View(inbox);
        }

        [Authorize]
        public ActionResult sendboxDetails(int id)
        {
            var sendbox = mm.GetMessage(id);
            return View(sendbox);
        }

        [Authorize]
        public ActionResult inboxDelete(int id)
        {
            var message = mm.GetMessage(id);
            message.Remove = true;
            mm.UpdateMessage(message);
            return RedirectToAction("Inbox");
        }

        [Authorize]
        public ActionResult sendboxDelete(int id)
        {
            var message = mm.GetMessage(id);
            message.Remove = true;
            mm.UpdateMessage(message);
            return RedirectToAction("Sendbox");
        }

        [Authorize]
        public PartialViewResult trashInbox()
        {
            var inbox = mm.GetListInboxRemoved();
            return PartialView(inbox);
        }

        [Authorize]
        public PartialViewResult trashSendbox()
        {
            var sendbox = mm.GetListSendboxRemoved();
            return PartialView(sendbox);
        }

        [Authorize]
        public ActionResult trashMessageDetails(int id)
        {
            var message = mm.GetMessage(id);
            return View(message);
        }

        [Authorize]
        public ActionResult trashMessageDelete(int id)
        {
            var message = mm.GetMessage(id);
            mm.DeleteMessage(message);
            return RedirectToAction("Trash", "Contact");
        }

    }
}