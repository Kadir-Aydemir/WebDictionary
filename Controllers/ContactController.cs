using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class ContactController : Controller
    {
        ContactManager cm = new ContactManager(new EfContactDal());
        ContactValidator validator = new ContactValidator();
        MessageManager mm = new MessageManager(new EfMessageDal());
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());

        public ActionResult Index()
        {
            var list = cm.GetList();
            return View(list);
        }

        public ActionResult Details(int id)
        {
            var contact = cm.GetContact(id);
            if (contact.IsRead == false)
            {
                contact.IsRead = true;
                cm.UpdateContact(contact);
            }
            return View(contact);
        }

        public ActionResult Delete(int id)
        {
            var contact = cm.GetContact(id);
            contact.Remove = true;
            cm.UpdateContact(contact);
            return RedirectToAction("Index");
        }

        public PartialViewResult leftMenu()
        {
            int contact = cm.GetListNotRead().Count();
            ViewBag.contact = contact;

            string ReceiverMail = (string)Session["AdminUserName"];
            int inbox = mm.GetListInboxNotRead(ReceiverMail).Count();
            ViewBag.inbox = inbox;

            int draft = dmm.GetList(ReceiverMail).Count();
            ViewBag.draft = draft;

            int trash = (mm.GetListSendboxRemoved(ReceiverMail).Count()) + (mm.GetListInboxRemoved(ReceiverMail).Count()) + (cm.GetListRemoved().Count());
            ViewBag.trash = trash;

            return PartialView();
        }

        public ActionResult Trash()
        {
            return View();
        }

        public PartialViewResult trashContact()
        {
            var contact = cm.GetListRemoved();
            return PartialView(contact);
        }

        public ActionResult trashContactDelete(int id)
        {
            var contact = cm.GetContact(id);
            cm.DeleteContact(contact);
            return RedirectToAction("Trash");
        }

        public ActionResult trashContactDetails(int id)
        {
            var contact = cm.GetContact(id);
            return View(contact);
        }

    }
}