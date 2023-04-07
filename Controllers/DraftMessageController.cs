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
    public class DraftMessageController : Controller
    {
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());
        DraftMessageValidator dvalidator = new DraftMessageValidator();

        [Authorize]
        public ActionResult draftMessage()
        {
            var list = dmm.GetList();
            return View(list);
        }

        [Authorize]
        public ActionResult draftMessageDelete(int id)
        {
            var draft = dmm.GetDraftMessage(id);
            dmm.DeleteDraftMessage(draft);
            return RedirectToAction("draftMessage");
        }

        [Authorize]
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
    }
}