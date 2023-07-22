using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace WebDictionary.Controllers
{
    public class WriterPanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        HeadingValidator validator = new HeadingValidator();
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator wvalidator = new WriterValidator();

        MessageManager mm = new MessageManager(new EfMessageDal());
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());

        [HttpGet]
        public ActionResult WriterProfile(bool newMember = false)
        {
            string WriterMail = (string)Session["WriterMail"];
            var writer = wm.GetWriterIdByMail(WriterMail);
            if (writer.WriterRemove == true)
            {
                writer.WriterRemove = false;
                wm.UpdateWriter(writer);
                ViewBag.comeback = "true";
            }
            if (newMember == true)
            {
                ViewBag.newMember = "true";
            }
            return View(writer);
        }

        [HttpPost]
        public ActionResult WriterProfile(Writer writer, string NewWriterPassword, string validatePass, string croppedImageUrl, FormCollection form)
        {
            if (form["btnUpdate"] != null)
            {
                ValidationResult results = wvalidator.Validate(writer);
                if (results.IsValid)
                {
                    var firstWriter = wm.GetWriter(writer.WriterID);

                    firstWriter.WriterID = writer.WriterID;
                    firstWriter.WriterName = writer.WriterName;
                    firstWriter.WriterSurname = writer.WriterSurname;
                    firstWriter.WriterTitle = writer.WriterTitle;
                    firstWriter.WriterAbout = writer.WriterAbout;

                    if (croppedImageUrl != null && croppedImageUrl.Length > 0)
                    {
                        if (croppedImageUrl == "default")
                        {
                            firstWriter.WriterImage = null;
                        }
                        else
                        {
                            byte[] imageBytes = Convert.FromBase64String(croppedImageUrl);
                            firstWriter.WriterImage = imageBytes;
                        }
                    }

                    if (NewWriterPassword != "")
                    {
                        if (NewWriterPassword.Length < 6)
                        {
                            TempData["validate"] = "Password cannot be shorter than 6 characters!";
                            return View(writer);
                        }
                        if (NewWriterPassword.Length > 50)
                        {
                            TempData["validate"] = "Password cannot exceed 50 characters!";
                            return View(writer);
                        }
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(NewWriterPassword);
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                            firstWriter.WriterPassword = hashedPassword;
                        }
                    }

                    if (firstWriter.WriterMail != writer.WriterMail)
                    {
                        var existingUser = wm.GetWriterIdByMail(writer.WriterMail);
                        if (existingUser != null)
                        {
                            ModelState.AddModelError("WriterMail", "This account is already in use by someone else!");
                        }
                        else
                        {
                            var recordsToReceiver = mm.GetListReceiver(firstWriter.WriterMail);
                            var recordsToSender = mm.GetListSender(firstWriter.WriterMail);
                            var recordsTodraftReceiver = dmm.GetListReceiver(firstWriter.WriterMail);
                            var recordsTodraftSender = dmm.GetListSender(firstWriter.WriterMail);
                            foreach (var record in recordsToReceiver)
                            {
                                record.ReceiverMail = writer.WriterMail;
                                mm.UpdateMessage(record);
                            }
                            foreach (var record in recordsToSender)
                            {
                                record.SenderMail = writer.WriterMail;
                                mm.UpdateMessage(record);
                            }
                            foreach (var record in recordsTodraftReceiver)
                            {
                                record.DraftReceiverMail = writer.WriterMail;
                                dmm.UpdateDraftMessage(record);
                            }
                            foreach (var record in recordsTodraftSender)
                            {
                                record.DraftSenderMail = writer.WriterMail;
                                dmm.UpdateDraftMessage(record);
                            }

                            firstWriter.WriterMail = writer.WriterMail;
                            wm.UpdateWriter(firstWriter);
                            FormsAuthentication.SignOut();
                            Session.Abandon();
                            return RedirectToAction("WriterLogin", "Login");
                        }
                    }
                    else
                    {
                        firstWriter.WriterMail = writer.WriterMail;
                        wm.UpdateWriter(firstWriter);
                        ViewBag.updateresult = "true";
                    }

                    return View(firstWriter);
                }
                else
                {
                    foreach (var item in results.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    return View(writer);
                }
            }
            else if (form["btnDelete"] != null)
            {
                var firstWriter = wm.GetWriter(writer.WriterID);

                if (validatePass != "")
                {
                    string hashedPassword;
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(validatePass);
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                        hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                    }

                    if (hashedPassword != firstWriter.WriterPassword)
                    {
                        ViewBag.validate = "true";
                        return View(firstWriter);
                    }

                    firstWriter.WriterRemove = true;
                    wm.UpdateWriter(firstWriter);
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    return RedirectToAction("weAreSorry", "DictionaryPanel");
                }
                else
                {
                    ViewBag.pass = "true";
                    return View(firstWriter);
                }
            }
            return View();
        }

        public ActionResult MyHeading()
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            int id = info.WriterID;
            var headings = hm.GetListByWriter(id);
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
        public ActionResult addHeading(Heading heading, HttpPostedFileBase imageFile)
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
        public ActionResult updateHeading(Heading heading, HttpPostedFileBase imageFile)
        {
            var info = hm.GetHeading(heading.HeadingID);
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
                    info.HeadingID = heading.HeadingID;
                    info.HeadingName = heading.HeadingName;
                    info.CategoryID = heading.CategoryID;
                    info.HeadingImage = heading.HeadingImage;
                    hm.UpdateHeading(info);
                }
                else
                {
                    if (heading.HeadingName == info.HeadingName)
                    {
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            byte[] imageData = new byte[imageFile.ContentLength];
                            imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                            heading.HeadingImage = imageData;
                        }
                        info.HeadingID = heading.HeadingID;
                        info.CategoryID = heading.CategoryID;
                        info.HeadingImage = heading.HeadingImage;
                        hm.UpdateHeading(info);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Heading already exists. Please look at the previously opened headings and write your thoughts there without opening the same or similar headings." }, JsonRequestBehavior.AllowGet);
                    }                 
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

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

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
            hm.UpdateHeading(heading);
            return RedirectToAction("MyHeading");
        }

        public ActionResult AllHeading()
        {
            var list = hm.GetListActive();
            return View(list);
        }


    }
}