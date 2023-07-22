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
using System.Web.Services.Description;

namespace WebDictionary.Controllers
{
    public class WriterPanelContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        ContentValidator validator = new ContentValidator();
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());

        public ActionResult MyContent()
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            int id = info.WriterID;
            var list = cm.GetListByWriterIdAll(id);
            return View(list);
        }

        [HttpGet]
        public ActionResult addContent(int id)
        {
            ViewBag.id = id;
            var head = hm.GetHeading(id);
            ViewBag.heading = head.HeadingName;
            return View();
        }

        [HttpPost]
        public ActionResult addContent(Content content)
        {
            ValidationResult result = validator.Validate(content);
            if (result.IsValid)
            {
                string WriterMail = (string)Session["WriterMail"];
                var info = wm.GetWriterIdByMail(WriterMail);
                int id = info.WriterID;
                content.WriterID = id;
                content.ContentDate = DateTime.Now;
                cm.AddContent(content);
                return RedirectToAction("MyContent");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                ViewBag.id = content.HeadingID;
                var head = hm.GetHeading(content.HeadingID);
                ViewBag.heading = head.HeadingName;
                return View();
            }
        }

        [HttpGet]
        public ActionResult updateContent(int id)
        {
            var content = cm.GetContent(id);
            return View(content);
        }

        [HttpPost]
        public ActionResult updateContent(Content content)
        {
            var firstcontent = cm.GetContent(content.ContentID);
            ValidationResult result = validator.Validate(content);
            if (result.IsValid)
            {               
                firstcontent.ContentID = content.ContentID;
                firstcontent.ContentCaption = content.ContentCaption;
                cm.UpdateContent(firstcontent);
                return RedirectToAction("MyContent");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(firstcontent);
            }
        }

        public ActionResult Delete(int id)
        {
            var content = cm.GetContent(id);
            if (content.ContentRemove == true)
            {
                content.ContentRemove = false;
            }
            else
            {
                content.ContentRemove = true;
            }
            cm.UpdateContent(content);
            return RedirectToAction("MyContent");
        }
    }
}