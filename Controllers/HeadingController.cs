using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class HeadingController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        HeadingValidator validator = new HeadingValidator();

        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        [Authorize]
        public ActionResult Index()
        {
            var list = hm.GetList();
            return View(list);
        }

        [Authorize]
        [HttpGet]
        public ActionResult addHeading()
        {
            List<SelectListItem> categoryList = (from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.ctglist = categoryList;

            List<SelectListItem> writerList = (from x in wm.GetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.WriterName + " " + x.WriterSurname,
                                                   Value = x.WriterID.ToString()
                                               }).ToList();
            ViewBag.wrtlist = writerList;
            return View();
        }

        [HttpPost]
        public ActionResult addHeading(Heading heading)
        {
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                heading.HeadingDate = DateTime.Now;
                hm.AddHeading(heading);
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> categoryList = (from x in cm.GetList()
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryID.ToString()
                                                     }).ToList();
                ViewBag.ctglist = categoryList;

                List<SelectListItem> writerList = (from x in wm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.WriterName + " " + x.WriterSurname,
                                                       Value = x.WriterID.ToString()
                                                   }).ToList();
                ViewBag.wrtlist = writerList;
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult updateHeading(int id)
        {
            List<SelectListItem> categoryList = (from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.ctglist = categoryList;

            var hdg = hm.GetHeading(id);
            return View(hdg);
        }

        [HttpPost]
        public ActionResult updateHeading(Heading heading)
        {
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                hm.UpdateHeading(heading);
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> categoryList = (from x in cm.GetList()
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryID.ToString()
                                                     }).ToList();
                ViewBag.ctglist = categoryList;

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
            var heading = hm.GetHeading(id);
            if (heading.HeadingRemove == true)
            {
                heading.HeadingRemove = false;
            }
            else
            {
                heading.HeadingRemove = true;
            }
            hm.DeleteHeading(heading);
            return RedirectToAction("Index");
        }
    }
}