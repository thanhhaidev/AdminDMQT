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
    public class CashBillDetailsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        public int SalePrice(int ProductID)
        {
            return db.Products.Find(ProductID).SalePrice;
        }

        // GET: Admin/CashBillDetails
        public PartialViewResult Index()
        {
            if (Session["CashBillDetail"] == null)
                Session["CashBillDetail"] = new List<CashBillDetail>();
            return PartialView(Session["CashBillDetail"]);
        }

        // GET: Admin/CashBillDetails/Create
        public PartialViewResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
            var cashbillDetail = new CashBillDetail();
            cashbillDetail.Quantity = 1;
            cashbillDetail.BillID = 0;
            return PartialView(cashbillDetail);
        }

        // POST: Admin/CashBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(CashBillDetail cashBillDetail)
        {
            if (ModelState.IsValid)
            {
                cashBillDetail.ID = Environment.TickCount;
                cashBillDetail.Product = db.Products.Find(cashBillDetail.ProductID);
                var CTHoaDon = Session["CashBillDetail"] as List<CashBillDetail>;
                if (CTHoaDon == null)
                    CTHoaDon = new List<CashBillDetail>();
                CTHoaDon.Add(cashBillDetail);
                Session["CashBillDetail"] = CTHoaDon;
                return RedirectToAction("Create", "CashBills");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", cashBillDetail.ProductID);
            return View("Create", cashBillDetail);
        }

        // GET: Admin/CashBillDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBillDetail cashBillDetail = db.CashBillDetails.Find(id);
            if (cashBillDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashBillDetail.ProductID);
            return View(cashBillDetail);
        }

        // POST: Admin/CashBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BillID,ProductID,Quantity,SalePrice")] CashBillDetail cashBillDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashBillDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashBillDetail.ProductID);
            return View(cashBillDetail);
        }

        // GET: Admin/CashBillDetails/Delete/5
        public ActionResult Delete(int id)
        {
            var CTHoaDon = Session["CashBillDetail"] as List<CashBillDetail>;
            CTHoaDon = CTHoaDon.Except(CTHoaDon.Where(c => c.ID == id)).ToList();
            Session["CashBillDetail"] = CTHoaDon;
            return RedirectToAction("Create", "CashBills");
        }

        // POST: Admin/CashBillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBillDetail cashBillDetail = db.CashBillDetails.Find(id);
            db.CashBillDetails.Remove(cashBillDetail);
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
