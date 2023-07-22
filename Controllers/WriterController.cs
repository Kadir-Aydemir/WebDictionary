using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());
        BlockedManager bm = new BlockedManager(new EfBlockedDal());
        BlockedValidator validator = new BlockedValidator();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Blocked blocked, string dayCount)
        {
            ValidationResult result = validator.Validate(blocked);
            if (result.IsValid)
            {
                if (!string.IsNullOrEmpty(dayCount))
                {
                    blocked.BlockedDate = DateTime.Now;
                    DateTime expire = DateTime.Now.AddDays(int.Parse(dayCount));
                    blocked.ExpireDate = expire;
                    bm.AddBlocked(blocked);
                    ViewBag.insertresult = "true";
                }
                else
                {
                    ModelState.AddModelError("ExpireDate", "You cannot leave this field blank or enter a value less than zero!");
                    ViewBag.alert = "true";
                }
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                ViewBag.alert = "true";
            }

            return View();
        }

        public PartialViewResult WriterPartial()
        {
            var list = wm.GetList();
            return PartialView(list);
        }

        [Authorize(Roles = "A")]
        public ActionResult Delete(int id)
        {
            var writer = wm.GetWriter(id);
            if (writer.WriterRemove == true)
            {
                writer.WriterRemove = false;
            }
            else
            {
                writer.WriterRemove = true;
            }
            wm.UpdateWriter(writer);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        public ActionResult WriterReport()
        {
            var writerlist = wm.GetList();
            return View(writerlist);
        }

        public ActionResult GetImage(int id)
        {
            var writer = wm.GetWriter(id);
            return File(writer.WriterImage, "image/jpeg"); // Görüntüyü byte dizisi olarak döndürme
        }

        [HttpGet]
        public JsonResult Show(int id)
        {
            var writer = wm.GetWriter(id);
            return Json(writer, JsonRequestBehavior.AllowGet);
        }
    }
}