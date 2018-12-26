using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;
using System.Transactions;

namespace DienMayQuyetTien.Areas.Admin.Controllers
{
    public class CashBillsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: Admin/CashBills
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View(db.CashBills.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Admin/CashBills/Details/5
        public ActionResult Cancel()
        {
            if(Session["CashBill"] != null || Session["CashBillDetail"] != null)
            {
                Session["CashBill"] = null;
                Session["CashBillDetail"] = null;
                return RedirectToAction("Index", "CashBills");
            }
            return RedirectToAction("Index", "CashBills");
        }

        // GET: Admin/CashBills/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                return View(Session["CashBill"]);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/CashBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CashBill cashBill)
        {
            checkValidator(cashBill);
            if (ModelState.IsValid)
            {
                Session["CashBill"] = cashBill;
            }
            
            return View(cashBill);
        }

        private void checkValidator(CashBill cashBill)
        {
            if (cashBill.CustomerName == null || cashBill.CustomerName.Equals(""))
                ModelState.AddModelError("CustomerName", "Tên khách hàng không được bỏ trống");
            if (cashBill.Address == null || cashBill.Address.Equals(""))
                ModelState.AddModelError("Address", "Địa chỉ không được bỏ trống");
            if (cashBill.PhoneNumber == null || cashBill.PhoneNumber.Equals(""))
                ModelState.AddModelError("PhoneNumber", "Số điện thoại không được bỏ trống");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2()
        {
            using (var scope = new TransactionScope())
                try
                {
                    var cashBill = Session["CashBill"] as CashBill;
                    var CTHoaDon = Session["CashBillDetail"] as List<CashBillDetail>;
                    cashBill.Date = DateTime.Now;
                    cashBill.GrandTotal = (int)Session["total"];

                    db.CashBills.Add(cashBill);
                    db.SaveChanges();

                    foreach (var chiTiet in CTHoaDon)
                    {
                        chiTiet.BillID = cashBill.ID;
                        chiTiet.Product = null;
                        db.CashBillDetails.Add(chiTiet);
                    }
                    db.SaveChanges();
                    scope.Complete();

                    Session["CashBill"] = null;
                    Session["CashBillDetail"] = null;
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Create");
        }

        // GET: Admin/CashBills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashBill = db.CashBills.Find(id);
            if (cashBill == null)
            {
                return HttpNotFound();
            }
            return View(cashBill);
        }

        // POST: Admin/CashBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BillCode,CustomerName,PhoneNumber,Address,Date,Shipper,Note,GrandTotal")] CashBill cashBill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashBill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cashBill);
        }

        // GET: Admin/CashBills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashBill = db.CashBills.Find(id);
            if (cashBill == null)
            {
                return HttpNotFound();
            }
            return View(cashBill);
        }

        // POST: Admin/CashBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBill cashBill = db.CashBills.Find(id);
            db.CashBills.Remove(cashBill);
            db.SaveChanges();
            return RedirectToAction("Index");
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
