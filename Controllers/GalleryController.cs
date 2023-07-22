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
using System.Web.UI.WebControls;

namespace WebDictionary.Controllers
{
    public class GalleryController : Controller
    {
        ImageFileManager im = new ImageFileManager(new EfImageFileDal());
        ImageFileValidator validator = new ImageFileValidator();

        public ActionResult Index()
        {
            ViewBag.count = im.GetListActive().Count;
            return View();
        }

        [HttpPost]
        public ActionResult Index(ImageFile image, FormCollection form, HttpPostedFileBase imageFile)
        {
            ValidationResult result = validator.Validate(image);
            if (result.IsValid)
            {
                if (form["btnInsert"] != null)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        byte[] imageData = new byte[imageFile.ContentLength];
                        imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                        image.ImagePath = imageData;
                        image.ImageRemove = true;
                        im.AddImageFile(image);
                        ViewBag.insertresult = "true";
                    }
                    else
                    {
                        ModelState.AddModelError("ImagePath", "You cannot leave this field blank!");
                        ViewBag.alert = "true";
                    }
                }
                else if (form["btnUpdate"] != null)
                {
                    var info = im.GetImageFile(image.ImageId);
                    info.ImageName = image.ImageName;

                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        byte[] imageData = new byte[imageFile.ContentLength];
                        imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                        image.ImagePath = imageData;
                        info.ImagePath = image.ImagePath;
                    }

                    im.UpdateImageFile(info);
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
            ViewBag.count = im.GetListActive().Count;
            return View();
        }

        public PartialViewResult ImagePartial()
        {
            var list = im.GetList();
            return PartialView(list);
        }


        [HttpGet]
        public ActionResult updateImage(int id)
        {
            var image = im.GetImageFile(id);
            return Json(image, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var image = im.GetImageFile(id);
            if (image.ImageRemove == true)
            {
                image.ImageRemove = false;
            }
            else
            {
                image.ImageRemove = true;
            }
            im.UpdateImageFile(image);
            return RedirectToAction("Index");
        }

        public ActionResult GetImage(int id)
        {
            var image = im.GetImageFile(id);
            return File(image.ImagePath, "image/jpeg"); // Görüntüyü byte dizisi olarak döndürme
        }
    }
}