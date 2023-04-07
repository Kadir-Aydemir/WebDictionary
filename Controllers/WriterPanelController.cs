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
    public class WriterPanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        HeadingValidator validator = new HeadingValidator();
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult WriterProfile()
        {
            return View();
        }

        public ActionResult MyHeading()
        {
            var headings = hm.GetListByWriter();
            return View(headings);
        }

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
            return View();
        }

        [HttpPost]
        public ActionResult addHeading(Heading heading)
        {
            heading.HeadingDate = DateTime.Now;
            heading.WriterID = 1;
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                hm.AddHeading(heading);
                return RedirectToAction("MyHeading");
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
                return RedirectToAction("MyHeading");
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

        public ActionResult Delete(int id)
        {
            var heading = hm.GetHeading(id);
            heading.HeadingRemove = true;
            hm.DeleteHeading(heading);
            return RedirectToAction("MyHeading");
        }
    }
}