using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;
using System.Transactions;

namespace DienMayQuyetTien.Controllers
{
    public class HouseController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();
        // GET: House
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult GioiThieu()
        {
            Introduction introduction = db.Introductions.Find(1);
            return View(introduction);
        }


        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LienHe(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        public ActionResult KhuyenMai()
        {
            return View(db.Promotions.ToList());
        }

        public ActionResult TinTuc()
        {
            return View();
        }
    }
}