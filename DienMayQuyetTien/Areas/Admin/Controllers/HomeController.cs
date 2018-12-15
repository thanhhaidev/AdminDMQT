﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;
using System.IO;

namespace DienMayQuyetTien.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                var products = db.Products.Include(p => p.ProductType);
                return View(products.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Admin/Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Home/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeCode");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            CheckValidationProduct(product);
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                product.Avatar = "/Assets/Admin/img/products/" + fileName;
                fileName = Path.Combine(Server.MapPath("/Assets/Admin/img/products/"), fileName);
                product.ImageFile.SaveAs(fileName);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeCode", product.ProductTypeID);
            return View(product);
        }

        private void CheckValidationProduct(Product model)
        {
            if (model.OriginPrice < 0)
                ModelState.AddModelError("OriginPrice", "Giá gốc phải lớn hơn 0!");
            if (model.SalePrice < model.OriginPrice)
                ModelState.AddModelError("SalePrice", "Giá bán phải lớn hơn giá gốc!");
            if (model.InstallmentPrice < model.OriginPrice)
                ModelState.AddModelError("InstallmentPrice", "Giá góp phải lớn hơn giá gốc!");
        }

        // GET: Admin/Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeCode", product.ProductTypeID);
                return View(product);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                product.Avatar = "/Assets/Admin/img/products/" + fileName;
                fileName = Path.Combine(Server.MapPath("/Assets/Admin/img/products/"), fileName);
                product.ImageFile.SaveAs(fileName);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeCode", product.ProductTypeID);
            return View(product);
        }

        // GET: Admin/Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
