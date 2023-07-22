using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebDictionary.Controllers
{
    public class AuthorizationController : Controller
    {
        AdminManager am = new AdminManager(new EfAdminDal());
        AdminValidator validator = new AdminValidator();

        public ActionResult Index()
        {
            ViewData["AdminRole"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Admin" },
                new SelectListItem { Value = "E", Text = "Editor" }
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Index(Admin admin, FormCollection form)
        {
            ValidationResult result = validator.Validate(admin);
            if (result.IsValid)
            {
                if (form["btnInsert"] != null)
                {
                    var existingUser = am.IsMailExist(admin.AdminUserName);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("AdminUserName", "An account already exists with this email address!");
                        ViewBag.alert = "true";
                    }
                    else
                    {
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(admin.AdminPassword);
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                            admin.AdminPassword = hashedPassword;
                        }
                        am.AddAdmin(admin);
                        ViewBag.insertresult = "true";
                    }
                }
                else if (form["btnUpdatemail"] != null || form["btnUpdatepassword"] != null || form["btnUpdaterole"] != null)
                {
                    var info = am.GetAdmin(admin.AdminId);
                    info.AdminId = admin.AdminId;
                    if (info.AdminPassword != admin.AdminPassword)
                    {
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(admin.AdminPassword);
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                            admin.AdminPassword = hashedPassword;
                        }
                    }
                    info.AdminPassword = admin.AdminPassword;
                    info.AdminRole = admin.AdminRole;
                    if (info.AdminUserName != admin.AdminUserName)
                    {
                        var existingUser = am.IsMailExist(admin.AdminUserName);
                        if (existingUser != null)
                        {
                            ModelState.AddModelError("AdminUserName", "An account already exists with this email address!");
                            ViewBag.updatemailalert = "true";
                        }
                        else
                        {
                            if (info.AdminUserName == User.Identity.Name)
                            {
                                info.AdminUserName = admin.AdminUserName;
                                am.UpdateAdmin(info);
                                FormsAuthentication.SignOut();
                                Session.Abandon();
                                return RedirectToAction("Index", "Login");
                            }
                            else
                            {
                                info.AdminUserName = admin.AdminUserName;
                                am.UpdateAdmin(info);
                                ViewBag.updateresult = "true";
                            }
                        }
                    }
                    else
                    {
                        info.AdminUserName = admin.AdminUserName;
                        am.UpdateAdmin(info);
                        ViewBag.updateresult = "true";
                    }
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
                else if (form["btnUpdatemail"] != null)
                {
                    ViewBag.updatemailalert = "true";
                }
                else if (form["btnUpdatepassword"] != null)
                {
                    ViewBag.updatepasswordalert = "true";
                }
                else if (form["btnUpdaterole"] != null)
                {
                    ViewBag.updaterolealert = "true";
                }
            }

            ViewData["AdminRole"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Admin" },
                new SelectListItem { Value = "E", Text = "Editor" }
            }, "Value", "Text");

            return View();
        }

        public PartialViewResult AdminPartial()
        {
            var admin = am.GetAdminForRole(User.Identity.Name);
            ViewBag.userId = admin.AdminId;
            var list = am.GetList();
            return PartialView(list);
        }

        [HttpGet]
        public ActionResult updateAdmin(int id)
        {
            ViewData["AdminRole"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Admin" },
                new SelectListItem { Value = "E", Text = "Editor" }
            }, "Value", "Text");
            var admin = am.GetAdmin(id);
            return Json(admin, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var admin = am.GetAdmin(id);
            if (admin.AdminStatus == true)
            {
                admin.AdminStatus = false;
            }
            else
            {
                admin.AdminStatus = true;
            }
            am.UpdateAdmin(admin);
            return RedirectToAction("Index");
        }
    }
}