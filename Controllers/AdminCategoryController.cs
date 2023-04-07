using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class AdminCategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());

        [Authorize(Roles ="A")]
        public ActionResult Index()
        {
            var ctg = cm.GetList();
            return View(ctg);
        }

        [Authorize]
        public ActionResult addCategory()
        {        
            return View();
        }

        [HttpPost]
        public ActionResult addCategory(Category p)
        {
            CategoryValidator categoryValidator = new CategoryValidator();
            ValidationResult result = categoryValidator.Validate(p);
            if (result.IsValid)
            {
                cm.AddCategory(p);
                return RedirectToAction("Index");
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
        public ActionResult Delete(int id)
        {
            var ctgid = cm.GetCategory(id);
            cm.DeleteCategory(ctgid);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult updateCategory(int id)
        {
            var ctgid = cm.GetCategory(id);
            return View(ctgid);
        }

        [HttpPost]
        public ActionResult updateCategory(Category p)
        {
            CategoryValidator categoryValidator = new CategoryValidator();
            ValidationResult result = categoryValidator.Validate(p);
            if (result.IsValid)
            {
                cm.UpdateCategory(p);
                return RedirectToAction("Index");
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