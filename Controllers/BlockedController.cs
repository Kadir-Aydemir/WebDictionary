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
    public class BlockedController : Controller
    {
        BlockedManager bm = new BlockedManager(new EfBlockedDal());
        BlockedValidator validator = new BlockedValidator();
        // GET: Blocked
        public ActionResult Index(string status)
        {
            ViewBag.status = status;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Blocked blocked, string dayCount)
        {
            ValidationResult result = validator.Validate(blocked);
            if (result.IsValid)
            {
                if (!string.IsNullOrEmpty(dayCount))
                {
                    var blkinfo = bm.GetBlocked(blocked.BlockID);
                    DateTime expire = blkinfo.BlockedDate.AddDays(int.Parse(dayCount));                   
                    blkinfo.BlockID = blocked.BlockID;
                    blkinfo.ExpireDate = expire;
                    blkinfo.Reason = blocked.Reason;
                    bm.UpdateBlocked(blkinfo);
                    ViewBag.updateresult = "true";
                }
                else
                {
                    ModelState.AddModelError("ExpireDate", "You cannot leave this field blank or enter a value less than 'past days'!");
                    ViewBag.updatealert = "true";
                }
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                ViewBag.updatealert = "true";
            }

            return View();
        }

        [HttpGet]
        public ActionResult updateBlocked(int id)
        {
            var blckd = bm.GetBlocked(id);

            TimeSpan difference = blckd.ExpireDate.Subtract(DateTime.Now);
            int leftdays = (int)difference.TotalDays;
            TimeSpan difference2 = blckd.ExpireDate.Subtract(blckd.BlockedDate);
            int totaldays = (int)difference2.TotalDays;
            int min = totaldays - leftdays;

            List<object> list;
            list = new List<object> {
                blckd.BlockID,
                blckd.WriterID,
                blckd.ExpireDate,
                blckd.Reason,
                blckd.BlockedDate,
                leftdays,
                totaldays,
                min,
            };

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult BlockedPartial(string status)
        {
            var list = bm.GetList();

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "true")
                {
                    list = bm.GetListInactive();
                }
                else if (status == "false")
                {
                    list = bm.GetListActive();
                }
            }

            return PartialView(list);
        }

        [Authorize(Roles = "A")]
        public ActionResult Delete(int id)
        {
            var block = bm.GetBlocked(id);
            if (block.Situation == true)
            {
                block.Situation = false;
            }
            else
            {
                block.Situation = true;
            }
            bm.UpdateBlocked(block);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        public ActionResult BlockedReport()
        {
            var list = bm.GetList();
            return View(list);
        }

        public ActionResult CheckExpire()
        {
            bm.CheckExpireDateForEnd();
            return RedirectToAction("Index");
        }
    }
}