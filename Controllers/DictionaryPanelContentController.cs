using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelContentController : Controller
    {
        CategoryManager cam = new CategoryManager(new EfCategoryDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        ContentValidator validator = new ContentValidator();
        LikeManager lm = new LikeManager(new EfLikeDal());

        [HttpGet]
        public ActionResult Index(int id, string status)
        {
            var heading = hm.GetHeadingActive(id);
            if (heading != null)
            {
                ViewBag.headingid = heading.HeadingID;               
                var list = cm.GetListByIdOrderasc(id);
                ViewBag.sorted = "Sort by Date";
                if (!string.IsNullOrEmpty(status))
                {
                    if (status == "true")
                    {
                        list = cm.GetListByIdOrderLike(id);
                        ViewBag.sorted = "Sort by Like";
                    }
                }
                return View(list);
            }
            else
            {
                ViewBag.noresult = "true";
                var headingex = hm.GetHeading(id);
                ViewBag.word = headingex.HeadingName;
                return View();
            }
        }

        public ActionResult ContentLink(int id)
        {
            var content = cm.GetContent(id);
            return View(content);
        }

        public PartialViewResult addContentPartial(int id)
        {
            ViewBag.headingid = id;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult addContentPartial(Content content)
        {
            //var id = content.HeadingID;
            ValidationResult result = validator.Validate(content);
            if (result.IsValid)
            {
                string WriterMail = (string)Session["WriterMail"];
                var info = wm.GetWriterIdByMail(WriterMail);
                content.WriterID = info.WriterID;
                content.ContentDate = DateTime.Now;
                cm.AddContent(content);
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                throw new Exception("This field cannot be left blank!");
            }

            return PartialView();
        }

        public ActionResult getInfobyWriter(int id)
        {
            int headingcount = hm.GetListByWriterActive(id).Count();
            int contentcount = cm.GetListByWriterId(id).Count();
            var categorycount = (from h in hm.GetList()
                                 join c in cam.GetList() on h.CategoryID equals c.CategoryID
                                 join ct in cm.GetListAll() on h.HeadingID equals ct.HeadingID
                                 join w in wm.GetList() on ct.WriterID equals w.WriterID
                                 where w.WriterID == id
                                 select c.CategoryID).Distinct().Count();
            var likecount = (from c in cm.GetListAll()
                             join l in lm.GetList() on c.ContentID equals l.ContentID
                             where c.WriterID == id
                             select l.ContentID).Count();

            var dataList = new List<int> { headingcount, contentcount, categorycount , likecount };
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult writerContentsPartial(int id)
        {
            var list = cm.GetListByWriterId(id);
            return PartialView(list);
        }

        public ActionResult SearchHeading(string HeadingName)
        {
            var heading = hm.GetHeadingByName(HeadingName);
            if (heading != null)
            {
                ViewBag.headingid = heading.HeadingID;
                var list = cm.GetListById(heading.HeadingID);
                return View(list);
            }
            else
            {
                ViewBag.noresult = "true";
                ViewBag.word = HeadingName;
                return View();
            }
        }

        [HttpPost]
        public ActionResult addLike(int id)
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            Like like = new Like();
            like.ContentID = id;
            like.WriterID = info.WriterID;
            like.LikeDate = DateTime.Now;
            lm.AddLike(like);
            return Content("Success");
        }

        [HttpPost]
        public ActionResult Dislike(int id)
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            var like = lm.GetLike(id, info.WriterID);
            lm.DeleteLike(like);                    
            return Content("Success");
        }

        public ActionResult GetLikes(int id)
        {
            var list = lm.GetListByContentID(id);
            return PartialView("_LikesModal", list);
        }
    }
}