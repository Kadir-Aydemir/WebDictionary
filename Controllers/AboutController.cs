using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace WebDictionary.Controllers
{
    public class AboutController : Controller
    {
        AboutManager am = new AboutManager(new EfAboutDal());
        AboutValidator validator = new AboutValidator();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(About about, FormCollection form)
        {
            ValidationResult result = validator.Validate(about);
            if (result.IsValid)
            {
                if (form["btnInsert"] != null)
                {
                    am.AddAbout(about);
                    ViewBag.insertresult = "true";
                }
                else if (form["btnUpdate"] != null)
                {
                    am.UpdateAbout(about);
                    ViewBag.updateresult = "true";
                }
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                if (form["btnInsert"] != null)
                {
                    ViewBag.alert = "true";
                }
                else if (form["btnUpdate"] != null)
                {
                    ViewBag.updatealert = "true";
                }            
            }
            return View();
        }

        public PartialViewResult AboutPartial()
        {
            var list = am.GetList();
            return PartialView(list);
        }

        public ActionResult Delete(int id)
        {
            var find = am.GetAbout(id);
            if (find.AboutRemove == true)
            {
                find.AboutRemove = false;
            }
            else
            {
                find.AboutRemove = true;
            }
            am.UpdateAbout(find);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult updateAbout(int id)
        {
            var about = am.GetAbout(id);
            return Json(about, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Show(int id)
        {
            var about = am.GetAbout(id);
            return Json(about, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "A")]
        public ActionResult AboutReport()
        {
            var list = am.GetList();
            return View(list);
        }
    }
}