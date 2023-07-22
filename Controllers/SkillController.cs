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

namespace WebDictionary.Controllers
{
    public class SkillController : Controller
    {
        SkillManager sm = new SkillManager(new EfSkillDal());
        SkillValidator validator = new SkillValidator();
        // GET: Skill
        public ActionResult Index()
        {
            ViewBag.count = sm.GetListActive().Count;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Skill skill, FormCollection form)
        {
            ValidationResult result = validator.Validate(skill);
            if (result.IsValid)
            {
                if (form["btnInsert"] != null)
                {
                    sm.AddSkill(skill);
                    ViewBag.insertresult = "true";
                }
                else if (form["btnUpdate"] != null)
                {
                    sm.UpdateSkill(skill);
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
            ViewBag.count = sm.GetListActive().Count;
            return View();
        }

        public PartialViewResult SkillPartial()
        {
            var list = sm.GetList();
            return PartialView(list);
        }

        [HttpGet]
        public ActionResult updateSkill(int id)
        {
            var skill = sm.GetSkill(id);
            return Json(skill, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var skill = sm.GetSkill(id);
            if (skill.SkillRemove == true)
            {
                skill.SkillRemove = false;
            }
            else
            {
                skill.SkillRemove = true;
            }
            sm.UpdateSkill(skill);
            return RedirectToAction("Index");
        }
    }
}