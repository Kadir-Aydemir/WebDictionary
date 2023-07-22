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
using System.Web.UI.WebControls;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelHeadingController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        HeadingValidator validator = new HeadingValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());

        public ActionResult Index(int id)
        {
            ViewBag.categoryid = id;
            var list = hm.GetListByCategory(id);
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(Heading heading, HttpPostedFileBase imageFile)
        {
            heading.HeadingDate = DateTime.Now;
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            heading.WriterID = info.WriterID;
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                var headingExist = hm.GetHeadingByName(heading.HeadingName);
                if (headingExist == null)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        byte[] imageData = new byte[imageFile.ContentLength];
                        imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                        heading.HeadingImage = imageData;
                    }

                    hm.AddHeading(heading);
                }
                else
                {
                    return Json(new { success = false, message = "Heading already exists. Please look at the previously opened headings and write your thoughts there without opening the same or similar headings." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                throw new Exception("This field cannot be left blank!");
            }

            return Json(new { success = true, heading = heading }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult writerHeadingsPartial(int id)
        {
            var list = hm.GetListByWriterActive(id);
            return PartialView(list);
        }
        
        public PartialViewResult categoryHeadingsPartial(int id)
        {
            ViewBag.categoryid = id;
            return PartialView();
        }
       
    }
}