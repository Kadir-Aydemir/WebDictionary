using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebDictionary.Controllers
{
    public class AdminCategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        CategoryValidator categoryValidator = new CategoryValidator();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Category category, FormCollection form)
        {
            ValidationResult result = categoryValidator.Validate(category);
            if (result.IsValid)
            {
                if (form["btnInsert"] != null)
                {                  
                    cm.AddCategory(category);
                    ViewBag.insertresult = "true";
                }
                else if (form["btnUpdate"] != null)
                {                   
                    cm.UpdateCategory(category);
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

        public PartialViewResult CategoryPartial()
        {
            var list = cm.GetListAll();
            return PartialView(list);
        }

        public ActionResult Delete(int id)
        {
            var ctgid = cm.GetCategory(id);
            if (ctgid.CategoryStatus == true)
            {
                ctgid.CategoryStatus = false;
            }
            else
            {
                ctgid.CategoryStatus = true;
            }
            cm.UpdateCategory(ctgid);
            return RedirectToAction("Index");          
        }

        [HttpGet]
        public ActionResult updateCategory(int id)
        {
            var ctgid = cm.GetCategory(id);
            return Json(ctgid, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles ="A")]
        public ActionResult CategoryReport()
        {
            var list = cm.GetListAll();
            return View(list);
        }
    }
}