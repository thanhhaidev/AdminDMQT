using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Models;
using System.Transactions;

namespace DienMayQuyetTien.Controllers
{
    public class HouseController : Controller
    {
        private DmQT03EntitiesFrontEnd db = new DmQT03EntitiesFrontEnd();
        // GET: House
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GioiThieu()
        {
            return View();
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
            return View();
        }

        public ActionResult TinTuc()
        {
            return View();
        }
    }
}