using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;

namespace WebDictionary.Controllers
{
    public class ChartController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());

        // GET: Chart
        public ActionResult CategoryHeading()
        {
            var rs = hm.ListCategoryHeading();
            return View(rs);
        }

        public ActionResult CategoryHeadingChart()
        {
            var result = hm.ListCategoryHeading();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HeadingContent()
        {
            var rs = cm.ListHeadingContent();
            return View(rs);
        }

        public ActionResult HeadingContentChart()
        {
            var result = cm.ListHeadingContent();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WriterContent()
        {
            var rs = cm.ListWriterContent();
            return View(rs);
        }

        public ActionResult WriterContentChart()
        {
            var result = cm.ListWriterContent();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}