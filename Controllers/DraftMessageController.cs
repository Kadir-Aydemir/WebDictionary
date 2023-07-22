using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class DraftMessageController : Controller
    {
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());
        MessageManager mm = new MessageManager(new EfMessageDal());
        DraftMessageValidator dvalidator = new DraftMessageValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult draftMessage()
        {
            string SenderMail = (string)Session["AdminUserName"];
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
                string cont = string.Empty;
                if (form["btnDraft"] != null)
                {
                    dmm.UpdateDraftMessage(draftMessage);
                    action = "draftMessage";
                    cont = "DraftMessage";
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
                        cont = "Message";
                    }
                    else
                    {
                        ModelState.AddModelError("DraftReceiverMail", "The Dictionary does not have an writer with this email!");
                        return View();
                    }
                }
                return RedirectToAction(action, cont);
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
    }
}