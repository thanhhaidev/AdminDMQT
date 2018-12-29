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
    public class InstallmentBillsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        public int setSessionTaken(int taken)
        {
            Session["Taken"] = taken;
            return (int)Session["Taken"];
        }

        // GET: Admin/InstallmentBills
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Message = TempData["message"];
                return View(db.InstallmentBills.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Admin/InstallmentBills/Details/5
        public ActionResult Cancel()
        {
            if (Session["IBillDetail"] != null || Session["IBillDetail"] != null)
            {
                Session["IBill"] = null;
                Session["IBillDetail"] = null;
                Session["Taken"] = null;
                Session["total"] = null;
                return RedirectToAction("Index", "InstallmentBills");
            }
            return RedirectToAction("Index", "InstallmentBills");
        }

        // GET: Admin/InstallmentBills/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode");
                return View(Session["IBill"]);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/InstallmentBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstallmentBill installmentBill)
        {
            if (ModelState.IsValid)
            {
                Session["IBill"] = installmentBill;
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
            return View(installmentBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2()
        {
            using (var scope = new TransactionScope())
                try
                {
                    var iBill = Session["IBill"] as InstallmentBill;
                    var CTHoaDonTG = Session["IBillDetail"] as List<InstallmentBillDetail>;
                    iBill.Date = DateTime.Now;
                    iBill.GrandTotal = (int)Session["total"];
                    iBill.Taken = (int)Session["Taken"];
                    iBill.Remain = ((int)Session["total"] - (int)Session["Taken"]);

                    db.InstallmentBills.Add(iBill);
                    db.SaveChanges();

                    foreach (var chiTiet in CTHoaDonTG)
                    {
                        chiTiet.BillID = iBill.ID;
                        chiTiet.Product = null;
                        db.InstallmentBillDetails.Add(chiTiet);
                    }
                    db.SaveChanges();
                    scope.Complete();

                    Session["IBill"] = null;
                    Session["IBillDetail"] = null;
                    Session["total"] = null;
                    Session["Taken"] = null;
                    TempData["message"] = "Tạo hóa đơn thành công.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Create");
        }


        // GET: Admin/InstallmentBills/Edit/5
        public ActionResult Edit(int id)
        {
            if(Session["UserName"] != null)
            {
                InstallmentBill installmentBill = db.InstallmentBills.Find(id);
                Session["IBill"] = installmentBill;
                ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
                return View(installmentBill);
            } else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/InstallmentBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstallmentBill installmentBill)
        {
            if (ModelState.IsValid)
            {
                installmentBill.Date = DateTime.Now;
                db.Entry(installmentBill).State = EntityState.Modified;
                Session["IBill"] = installmentBill;
                db.SaveChanges();
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
            return View(installmentBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2()
        {
            using (var scope = new TransactionScope())
                try
                {
                    var iBill = Session["IBill"] as InstallmentBill;
                    var CTHoaDonTG = Session["IBillDetail"] as List<InstallmentBillDetail>;
                    iBill.Date = DateTime.Now;
                    iBill.GrandTotal = (int)Session["total"];
                    iBill.Taken = (int)Session["Taken"];
                    iBill.Remain = ((int)Session["total"] - (int)Session["Taken"]);

                    db.Entry(iBill).State = EntityState.Modified;
                    db.SaveChanges();

                    foreach (var chiTiet in CTHoaDonTG)
                    {
                        chiTiet.BillID = iBill.ID;
                        chiTiet.Product = null;
                        db.InstallmentBillDetails.Add(chiTiet);
                    }
                    db.SaveChanges();
                    scope.Complete();

                    Session["IBill"] = null;
                    Session["IBillDetail"] = null;
                    Session["total"] = null;
                    Session["Taken"] = null;
                    TempData["message"] = "Chỉnh sửa hóa đơn thành công.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Edit");
        }

        // GET: Admin/InstallmentBills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            if (installmentBill == null)
            {
                return HttpNotFound();
            }
            return View(installmentBill);
        }

        // POST: Admin/InstallmentBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            db.InstallmentBills.Remove(installmentBill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult Print(int id)
        {
            var installment = db.InstallmentBills.FirstOrDefault(o => o.ID == id);
            if (installment != null)
            {
                ReceiptIBModel rp = new ReceiptIBModel();
                rp.BillCode = installment.BillCode;
                rp.CustomerID = installment.CustomerID;
                rp.Date = installment.Date;
                rp.Shipper = installment.Shipper;
                rp.Note = installment.Note;
                rp.Method = installment.Method;
                rp.Period = installment.Period;
                rp.GrandTotal = installment.GrandTotal;
                rp.Taken = installment.Taken;
                rp.Remain = installment.Remain;
                rp.installmentBillDetail = installment.InstallmentBillDetails.ToList();
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
