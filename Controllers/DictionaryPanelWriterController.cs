using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelWriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        CategoryManager cam = new CategoryManager(new EfCategoryDal());
        LikeManager lm = new LikeManager(new EfLikeDal());
        // GET: DictionaryPanelWriter
        public ActionResult Index(int id)
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


            ViewBag.headingCount = headingcount;
            ViewBag.contentCount = contentcount;
            ViewBag.categoryCount = categorycount;
            ViewBag.likeCount = likecount;

            var writer = wm.GetWriter(id);
            return View(writer);
        }

        public PartialViewResult writerCategoriesPartial(int id)
        {
            var categorylist = (from h in hm.GetList()
                                 join c in cam.GetList() on h.CategoryID equals c.CategoryID
                                 join ct in cm.GetListAll() on h.HeadingID equals ct.HeadingID
                                 join w in wm.GetList() on ct.WriterID equals w.WriterID
                                 where w.WriterID == id
                                 select c).Distinct().ToList();
           
            return PartialView(categorylist);
        }
    }
}