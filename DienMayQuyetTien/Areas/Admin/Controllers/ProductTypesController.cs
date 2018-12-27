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
    public class ProductTypesController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: Admin/ProductTypes
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.Message = TempData["message"];
                return View(db.ProductTypes.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Admin/ProductTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // GET: Admin/ProductTypes/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/ProductTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductType productType)
        {
            checkValidatorProductType(productType);
            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(productType);
                db.SaveChanges();
                TempData["message"] = "Tạo loại sản phẩm thành công.";
                return RedirectToAction("Index");
            }

            return View(productType);
        }

        public void checkValidatorProductType(ProductType pt)
        {
            if (pt.ProductTypeCode.Length > 3)
                ModelState.AddModelError("ProductTypeCode", "ProductTypeCode phải nhỏ hơn 3 kí tự!");
            if (pt.ProductTypeName.Length > 100)
                ModelState.AddModelError("ProductTypeName", "ProductTypeName phải nhỏ hơn 100!");
        }

        // GET: Admin/ProductTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductType productType = db.ProductTypes.Find(id);
                if (productType == null)
                {
                    return HttpNotFound();
                }
                return View(productType);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/ProductTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductType productType)
        {
            checkValidatorProductType(productType);
            if (ModelState.IsValid)
            {
                db.Entry(productType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Chỉnh sửa loại sản phẩm thành công.";
                return RedirectToAction("Index");
            }
            return View(productType);
        }

        // GET: Admin/ProductTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // POST: Admin/ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductType productType = db.ProductTypes.Find(id);
            db.ProductTypes.Remove(productType);
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
