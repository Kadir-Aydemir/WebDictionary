using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Message = EntityLayer.Concrete.Message;

namespace WebDictionary.Controllers
{
    public class WriterPanelMessageController : Controller
    {
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator validator = new MessageValidator();
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());
        DraftMessageValidator dvalidator = new DraftMessageValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult Inbox()
        {
            string ReceiverMail = (string)Session["WriterMail"];
            var list = mm.GetListInbox(ReceiverMail);
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
            string SenderMail = (string)Session["WriterMail"];
            var list = mm.GetListSendbox(SenderMail);
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
        public ActionResult addNewMessage(string SenderMail)
        {
            if (SenderMail != null)
            {
                ViewBag.mail = SenderMail;
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addNewMessage(Message message, DraftMessage draftMessage, FormCollection form)
        {
            ValidationResult result = validator.Validate(message);
            if (result.IsValid)
            {
                string action = string.Empty;
                string SenderMail = (string)Session["WriterMail"];
                if (form["btnDraft"] != null)
                {
                    string ReceiverMail = form["ReceiverMail"];
                    string Subject = form["Subject"];
                    string MessageContent = form["MessageContent"];

                    draftMessage.DraftReceiverMail = ReceiverMail;
                    draftMessage.DraftSenderMail = SenderMail;
                    draftMessage.DraftSubject = Subject;
                    draftMessage.DraftMessageContent = MessageContent;
                    draftMessage.DraftMessageDate = DateTime.Now;
                    dmm.AddDraftMessage(draftMessage);
                    action = "draftMessage";
                }
                else if (form["btnMessage"] != null)
                {
                    var receiver = wm.GetWriterByMailControl(message.ReceiverMail);
                    if (receiver != null)
                    {
                        message.SenderMail = SenderMail;
                        message.MessageDate = DateTime.Now;
                        mm.AddMessage(message);

                        action = "Sendbox";
                    }
                    else
                    {
                        ModelState.AddModelError("ReceiverMail", "The Dictionary does not have an writer with this email!");
                        return View();
                    }
                }
                return RedirectToAction(action);
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
            string SenderMail = (string)Session["WriterMail"];
            var list = dmm.GetList(SenderMail);
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
        [ValidateInput(false)]
        public ActionResult updateDraft(DraftMessage draftMessage, Message message, FormCollection form)
        {
            ValidationResult result = dvalidator.Validate(draftMessage);
            if (result.IsValid)
            {
                string action = string.Empty;

                if (form["btnDraft"] != null)
                {
                    dmm.UpdateDraftMessage(draftMessage);
                    action = "draftMessage";
                }
                else if (form["btnMessage"] != null)
                {
                    var receiver = wm.GetWriterByMailControl(draftMessage.DraftReceiverMail);
                    if (receiver != null)
                    {
                        DateTime Date = DateTime.Now;

                        message.ReceiverMail = draftMessage.DraftReceiverMail;
                        message.SenderMail = draftMessage.DraftSenderMail;
                        message.Subject = draftMessage.DraftSubject;
                        message.MessageContent = draftMessage.DraftMessageContent;
                        message.MessageDate = Date;
                        mm.AddMessage(message);

                        dmm.DeleteDraftMessage(draftMessage);

                        action = "Sendbox";
                    }
                    else
                    {
                        ModelState.AddModelError("DraftReceiverMail", "The Dictionary does not have an writer with this email!");
                        return View();
                    }
                }
                return RedirectToAction(action);
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
            string ReceiverMail = (string)Session["WriterMail"];
            var inbox = mm.GetListInboxRemoved(ReceiverMail);
            return PartialView(inbox);
        }

        public PartialViewResult trashSendbox()
        {
            string SenderMail = (string)Session["WriterMail"];
            var sendbox = mm.GetListSendboxRemoved(SenderMail);
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
            string ReceiverMail = (string)Session["WriterMail"];
            int inbox = mm.GetListInboxNotRead(ReceiverMail).Count();
            ViewBag.inbox = inbox;

            int draft = dmm.GetList(ReceiverMail).Count();
            ViewBag.draft = draft;

            int trash = (mm.GetListSendboxRemoved(ReceiverMail).Count()) + (mm.GetListInboxRemoved(ReceiverMail).Count());
            ViewBag.trash = trash;

            return PartialView();
        }
    }
}