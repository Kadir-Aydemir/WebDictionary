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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace WebDictionary.Controllers
{

    public class MessageController : Controller
    {
        MessageManager mm = new MessageManager(new EfMessageDal());
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());
        MessageValidator validator = new MessageValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());
        public ActionResult Inbox()
        {
            string ReceiverMail = (string)Session["AdminUserName"];
            var list = mm.GetListInbox(ReceiverMail);
            return View(list);
        }

        public ActionResult Sendbox()
        {
            string SenderMail = (string)Session["AdminUserName"];
            var list = mm.GetListSendbox(SenderMail);
            return View(list);
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
        public ActionResult addNewMessage(Message message,DraftMessage draftMessage, FormCollection form)
        {           
            ValidationResult result = validator.Validate(message);
            if (result.IsValid)
            {
                string action = string.Empty;
                string cont = string.Empty;
                string SenderMail = (string)Session["AdminUserName"];
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
                    cont = "DraftMessage";
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
                        cont = "Message";
                    }
                    else
                    {
                        ModelState.AddModelError("ReceiverMail", "The Dictionary does not have an writer with this email!");
                        return View();
                    }                 
                }
                return RedirectToAction(action,cont);
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

        public ActionResult sendboxDetails(int id)
        {
            var sendbox = mm.GetMessage(id);
            return View(sendbox);
        }

        public ActionResult inboxDelete(int id)
        {
            var message = mm.GetMessage(id);
            message.Remove = true;
            mm.UpdateMessage(message);
            return RedirectToAction("Inbox");
        }

        public ActionResult sendboxDelete(int id)
        {
            var message = mm.GetMessage(id);
            message.Remove = true;
            mm.UpdateMessage(message);
            return RedirectToAction("Sendbox");
        }

        public PartialViewResult trashInbox()
        {
            string ReceiverMail = (string)Session["AdminUserName"];
            var inbox = mm.GetListInboxRemoved(ReceiverMail);
            return PartialView(inbox);
        }

        public PartialViewResult trashSendbox()
        {
            string SenderMail = (string)Session["AdminUserName"];
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
            return RedirectToAction("Trash", "Contact");
        }

    }
}