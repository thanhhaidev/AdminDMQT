using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;

namespace DienMayQuyetTien.Areas.Admin.Controllers
{
    public class IntroductionsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: Admin/Introductions
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                Introduction introduction = db.Introductions.Find(1);
                return View(introduction);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Introduction introduction)
        {
            if (ModelState.IsValid)
            {
                introduction.ID = 1;
                db.Entry(introduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(introduction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
