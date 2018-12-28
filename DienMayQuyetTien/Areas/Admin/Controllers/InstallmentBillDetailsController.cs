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
    public class InstallmentBillDetailsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        public int InstallmentPrice(int ProductID)
        {
            return db.Products.Find(ProductID).InstallmentPrice;
        }

        // GET: Admin/InstallmentBillDetails
        public PartialViewResult Index()
        {
            if (Session["IBillDetail"] == null)
                Session["IBillDetail"] = new List<InstallmentBillDetail>();
            return PartialView(Session["IBillDetail"]);
        }

        // GET: Admin/InstallmentBillDetails/Create
        public PartialViewResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
            var ibillDetail = new InstallmentBillDetail();
            ibillDetail.Quantity = 1;
            ibillDetail.BillID = 0;
            return PartialView(ibillDetail);
        }

        // GET: Admin/CashBillDetails/Create3
        public PartialViewResult Create3()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
            var ibillDetail = new InstallmentBillDetail();
            ibillDetail.Quantity = 1;
            ibillDetail.BillID = 0;
            return PartialView(ibillDetail);
        }

        // POST: Admin/InstallmentBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(InstallmentBillDetail iBillDetail)
        {
            if (ModelState.IsValid)
            {
                iBillDetail.ID = Environment.TickCount;
                iBillDetail.Product = db.Products.Find(iBillDetail.ProductID);
                var CTHoaDonTG = Session["IBillDetail"] as List<InstallmentBillDetail>;
                if (CTHoaDonTG == null)
                    CTHoaDonTG = new List<InstallmentBillDetail>();
                CTHoaDonTG.Add(iBillDetail);
                Session["IBillDetail"] = CTHoaDonTG;
                return RedirectToAction("Create", "InstallmentBills");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", iBillDetail.ProductID);
            return View("Create", iBillDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(InstallmentBillDetail iBillDetail)
        {
            if (ModelState.IsValid)
            {
                iBillDetail.ID = Environment.TickCount;
                iBillDetail.Product = db.Products.Find(iBillDetail.ProductID);
                var CTHoaDonTG = Session["IBillDetail"] as List<InstallmentBillDetail>;
                if (CTHoaDonTG == null)
                    CTHoaDonTG = new List<InstallmentBillDetail>();
                CTHoaDonTG.Add(iBillDetail);
                Session["IBillDetail"] = CTHoaDonTG;
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", iBillDetail.ProductID);
            return View("Create", iBillDetail);
        }

        // GET: Admin/InstallmentBillDetails/Edit/5
        public PartialViewResult Edit(int id)
        {
            List<InstallmentBillDetail> ibDetails = db.InstallmentBillDetails.Where(c => c.BillID == id).ToList();
            if (Session["IBillDetail"] == null)
                Session["IBillDetail"] = new List<CashBillDetail>();
            ViewBag.ibDetails = ibDetails;
            ViewBag.IBillDetail = Session["IBillDetail"];
            return PartialView();
        }

        // POST: Admin/InstallmentBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstallmentBillDetail installmentBillDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(installmentBillDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.InstallmentBills, "ID", "BillCode", installmentBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", installmentBillDetail.ProductID);
            return View(installmentBillDetail);
        }

        // GET: Admin/InstallmentBillDetails/Delete/5
        public ActionResult Delete(int id)
        {
            var CTHoaDonTG = Session["IBillDetail"] as List<InstallmentBillDetail>;
            CTHoaDonTG = CTHoaDonTG.Except(CTHoaDonTG.Where(c => c.ID == id)).ToList();
            Session["IBillDetail"] = CTHoaDonTG;
            return RedirectToAction("Create", "InstallmentBills");
        }

        public ActionResult Delete1(int id, int BillID)
        {
            InstallmentBillDetail iBillDetail = db.InstallmentBillDetails.Find(id);
            db.InstallmentBillDetails.Remove(iBillDetail);
            db.SaveChanges();
            return RedirectToAction("Edit/" + BillID, "InstallmentBills");
        }

        public ActionResult Delete2(int id)
        {
            var CTHoaDonTG = Session["IBillDetail"] as List<InstallmentBillDetail>;
            CTHoaDonTG = CTHoaDonTG.Except(CTHoaDonTG.Where(c => c.ID == id)).ToList();
            Session["IBillDetail"] = CTHoaDonTG;
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        // POST: Admin/InstallmentBillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            db.InstallmentBillDetails.Remove(installmentBillDetail);
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
