﻿using System;
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
                Session["CashBill"] = null;
                Session["CashBillDetail"] = null;
                Session["total"] = null;
                ViewBag.Message = TempData["message"];
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
                Session["total"] = null;
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
            if (cashBill.CustomerName == null || cashBill.CustomerName.Equals(" ") || cashBill.CustomerName.StartsWith(" "))
                ModelState.AddModelError("CustomerName", "Tên khách hàng không được bỏ trống");
            if (cashBill.Address == null || cashBill.Address.Equals(" ") || cashBill.Address.StartsWith(" "))
                ModelState.AddModelError("Address", "Địa chỉ không được bỏ trống");
            if (cashBill.PhoneNumber == null || cashBill.PhoneNumber.Equals(" ") || cashBill.PhoneNumber.StartsWith(" "))
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
                    Session["total"] = null;
                    TempData["message"] = "Tạo hóa đơn thành công.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Create");
        }

        // GET: Admin/CashBills/Edit/5
        public ActionResult Edit(int id)
        {
            if(Session["UserName"] != null)
            {
                CashBill cashBill = db.CashBills.Find(id);
                Session["CashBill"] = cashBill;
                return View(cashBill);
            } else {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/CashBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CashBill cashBill)
        {
            checkValidator(cashBill);
            if (ModelState.IsValid)
            {
                cashBill.Date = DateTime.Now;
                db.Entry(cashBill).State = EntityState.Modified;
                Session["CashBill"] = cashBill;
                db.SaveChanges();
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2()
        {
            using (var scope = new TransactionScope())
                try
                {
                    var cashBill = Session["CashBill"] as CashBill;
                    var CTHoaDon = Session["CashBillDetail"] as List<CashBillDetail>;
                    cashBill.Date = DateTime.Now;
                    cashBill.GrandTotal = (int)Session["total"];

                    db.Entry(cashBill).State = EntityState.Modified;
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
                    Session["total"] = null;
                    TempData["message"] = "Chỉnh sửa hóa đơn thành công.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Edit");
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
            if (db.CashBillDetails.Where(c => c.BillID == id).ToList().Count > 0 || db.InstallmentBillDetails.Where(i => i.BillID == id).ToList().Count > 0)
            {
                TempData["message"] = "Không thể xóa hóa đơn có chứa sản phẩm.";
                return RedirectToAction("Index");
            }
            else
            {
                CashBill cashBill = db.CashBills.Find(id);
                db.CashBills.Remove(cashBill);
                db.SaveChanges();
                TempData["message"] = "Xóa hóa đơn thành công.";
                return RedirectToAction("Index");
            }

        }

        public PartialViewResult Print(int id)
        {
            var cashbill = db.CashBills.FirstOrDefault(c => c.ID == id);
            if (cashbill != null)
            {
                ReceiptCBModel rp = new ReceiptCBModel();
                rp.Address = cashbill.Address;
                rp.BillCode = cashbill.BillCode;
                rp.CustomerName = cashbill.CustomerName;
                rp.Date = cashbill.Date;
                rp.cashBillDetail = cashbill.CashBillDetails.ToList();
                rp.Shipper = cashbill.Shipper;
                rp.PhoneNumber = cashbill.PhoneNumber;
                rp.GrandTotal = cashbill.GrandTotal;
                rp.Note = cashbill.Note;
                return PartialView(rp);
            }
            else
            {
                return PartialView();
            }
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
