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
    public class WriterPanelMessageController : Controller
    {
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator validator = new MessageValidator();
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());
        DraftMessageValidator dvalidator = new DraftMessageValidator();

        public ActionResult Inbox()
        {
            var list = mm.GetListInbox();
            return View(list);
        }

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

        public ActionResult inboxDelete(int id)
        {
            var message = mm.GetMessage(id);
            message.Remove = true;
            mm.UpdateMessage(message);
            return RedirectToAction("Inbox");
        }

        public ActionResult Sendbox()
        {
            var list = mm.GetListSendbox();
            return View(list);
        }

        public ActionResult sendboxDetails(int id)
        {
            var sendbox = mm.GetMessage(id);
            return View(sendbox);
        }

        public ActionResult sendboxDelete(int id)
        {
            var message = mm.GetMessage(id);
            message.Remove = true;
            mm.UpdateMessage(message);
            return RedirectToAction("Sendbox");
        }

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
                message.SenderMail = "kadiraydemir123@gmail.com";
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

        public ActionResult draftMessage()
        {
            var list = dmm.GetList();
            return View(list);
        }

        public ActionResult draftMessageDelete(int id)
        {
            var draft = dmm.GetDraftMessage(id);
            dmm.DeleteDraftMessage(draft);
            return RedirectToAction("draftMessage");
        }

        [HttpGet]
        public ActionResult updateDraft(int id)
        {
            var draft = dmm.GetDraftMessage(id);
            return View(draft);
        }

        [HttpPost]
        public ActionResult updateDraft(DraftMessage p)
        {
            ValidationResult result = dvalidator.Validate(p);
            if (result.IsValid)
            {
                dmm.UpdateDraftMessage(p);
                return RedirectToAction("draftMessage");
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

        public ActionResult Trash()
        {
            return View();
        }

        public PartialViewResult trashInbox()
        {
            var inbox = mm.GetListInboxRemoved();
            return PartialView(inbox);
        }

        public PartialViewResult trashSendbox()
        {
            var sendbox = mm.GetListSendboxRemoved();
            return PartialView(sendbox);
        }

        public ActionResult trashMessageDetails(int id)
        {
            var message = mm.GetMessage(id);
            return View(message);
        }

        public ActionResult trashMessageDelete(int id)
        {
            var message = mm.GetMessage(id);
            mm.DeleteMessage(message);
            return RedirectToAction("Trash");
        }

        public PartialViewResult leftMenu()
        {
            int inbox = mm.GetListInboxNotRead().Count();
            ViewBag.inbox = inbox;

            int draft = dmm.GetList().Count();
            ViewBag.draft = draft;

            int trash = (mm.GetListSendboxRemoved().Count()) + (mm.GetListInboxRemoved().Count());
            ViewBag.trash = trash;

            return PartialView();
        }
    }
}