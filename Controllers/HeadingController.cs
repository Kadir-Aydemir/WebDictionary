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
using System.Web.UI.WebControls;

namespace WebDictionary.Controllers
{
    public class HeadingController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        HeadingValidator validator = new HeadingValidator();

        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        MessageManager mm = new MessageManager(new EfMessageDal());

        public ActionResult Index()
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
        public ActionResult Index(Heading heading, FormCollection form, HttpPostedFileBase imageFile)
        {
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                if (form["btnInsert"] != null)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        byte[] imageData = new byte[imageFile.ContentLength];
                        imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                        heading.HeadingImage = imageData;
                    }

                    heading.HeadingDate = DateTime.Now;
                    hm.AddHeading(heading);
                    ViewBag.insertresult = "true";
                }
                else if (form["btnUpdate"] != null)
                {
                    var info = hm.GetHeading(heading.HeadingID);
                    info.HeadingID = heading.HeadingID;
                    info.HeadingName = heading.HeadingName;
                    info.CategoryID = heading.CategoryID;
                    info.WriterID = heading.WriterID;
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        byte[] imageData = new byte[imageFile.ContentLength];
                        imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                        heading.HeadingImage = imageData;
                        info.HeadingImage = heading.HeadingImage;
                    }
                    hm.UpdateHeading(info);
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

        public PartialViewResult HeadingPartial()
        {
            var list = hm.GetList();
            return PartialView(list);
        }

        [HttpGet]
        public ActionResult updateHeading(int id)
        {
            var hdg = hm.GetHeading(id);
            List<object> list = new List<object> {
                hdg.HeadingID,
                hdg.HeadingName,
                hdg.CategoryID,
                hdg.WriterID,
                hdg.HeadingImage,
            };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImage(int id)
        {
            var heading = hm.GetHeading(id);
            return File(heading.HeadingImage, "image/jpeg"); // Görüntüyü byte dizisi olarak döndürme
        }

        public ActionResult Delete(int id, string reason)
        {
            var heading = hm.GetHeading(id);

            Message message = new Message();
            message.SenderMail = User.Identity.Name;
            message.ReceiverMail = heading.Writer.WriterMail;
            message.Subject = "Your heading";
            message.MessageContent = "Your heading '" + heading.HeadingName + "' has been removed for '" + reason + "'. Please do not neglect the Dictionary rules while creating a heading. If this happens again, you may be blocked for a while. As the Dictionary team, we wish you a good day.";
            message.MessageDate = DateTime.Now;
            mm.AddMessage(message);

            hm.DeleteHeading(heading);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
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
            hm.UpdateHeading(heading);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        public ActionResult HeadingReport()
        {
            var list = hm.GetList();
            return View(list);
        }

        public ActionResult HeadingByCategory(int id)
        {
            var list = hm.GetListByCategory(id);
            var category = cm.GetCategory(id);
            ViewBag.category = category.CategoryName;
            return View(list);
        }

        public ActionResult HeadingByWriter(int id)
        {
            var list = hm.GetListByWriter(id);
            var writer = wm.GetWriter(id);
            ViewBag.writer = writer.WriterName + " " + writer.WriterSurname;
            ViewBag.writerid = writer.WriterID;
            return View(list);
        }
    }
}