using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        AdminManager am = new AdminManager(new EfAdminDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        BlockedManager bm = new BlockedManager(new EfBlockedDal());
        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string AdminUserName, string AdminPassword)
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "6LfotoglAAAAAPIBvMHQ48rtJF5x-IfSwXHdzWUs";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            string act = string.Empty;
            string cont = string.Empty;
            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Please verify security.";
                act = "Index";
                cont = "Login";
            }
            else
            {
                string hashedPassword;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(AdminPassword);
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                    hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                }

                var admin = am.ControlAdmin(AdminUserName, hashedPassword);
                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.AdminUserName, false);
                    Session["AdminUserName"] = admin.AdminUserName;
                    act = "Index";
                    cont = "AdminCategory";
                }
                else
                {
                    TempData["validate"] = "Please make sure your username or password is correct!";
                    act = "Index";
                    cont = "Login";
                }
            }
            return RedirectToAction(act, cont);
        }

        [HttpGet]
        public ActionResult WriterLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult WriterLogin(string WriterMail, string WriterPassword, bool? RememberMe)
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "6LfotoglAAAAAPIBvMHQ48rtJF5x-IfSwXHdzWUs";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            string act = string.Empty;
            string cont = string.Empty;
            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Please verify security.";
                act = "WriterLogin";
                cont = "Login";
            }
            else
            {
                var blockControl = bm.GetBlockedByWriterMail(WriterMail);
                if (blockControl == null)
                {
                    string hashedPassword;
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(WriterPassword);
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                        hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                    }
                    var writer = wm.ControlWriter(WriterMail, hashedPassword);
                    if (writer != null)
                    {
                        bool remember = RememberMe ?? false;

                        if (remember)
                        {
                            FormsAuthentication.SetAuthCookie(writer.WriterMail, false);
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(writer.WriterMail, true);
                        }
                        Session["WriterMail"] = writer.WriterMail;

                        act = "WriterProfile";
                        cont = "WriterPanel";
                    }
                    else
                    {
                        TempData["validate"] = "Please make sure your username or password is correct!";
                        act = "WriterLogin";
                        cont = "Login";
                    }
                }
                else
                {
                    TimeSpan difference = blockControl.ExpireDate.Subtract(blockControl.BlockedDate);
                    int days = (int)difference.TotalDays;

                    TempData["validate"] = "Unfortunately, you have been blocked for " + days.ToString() + " due to " + blockControl.Reason + ". You can log in again on " + blockControl.ExpireDate.ToString("dd MMM yyyy");
                    act = "WriterLogin";
                    cont = "Login";
                }
            }
            return RedirectToAction(act, cont);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LogOut", "DictionaryPanel");
        }

        public ActionResult LogOutAdmin()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}