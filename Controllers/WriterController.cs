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
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator validator = new WriterValidator();

        [Authorize]
        public ActionResult Index()
        {
            var writerlist = wm.GetList();
            return View(writerlist);
        }

        [Authorize]
        [HttpGet]
        public ActionResult addWriter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addWriter(Writer writer)
        {
            ValidationResult results = validator.Validate(writer);
            if (results.IsValid)
            {
                wm.AddWriter(writer);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult updateWriter(int id)
        {
            var writer = wm.GetWriter(id);
            return View(writer);
        }

        [HttpPost]
        public ActionResult updateWriter(Writer writer)
        {           
            ValidationResult results = validator.Validate(writer);
            if (results.IsValid)
            {
                wm.UpdateWriter(writer);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }
    }
}