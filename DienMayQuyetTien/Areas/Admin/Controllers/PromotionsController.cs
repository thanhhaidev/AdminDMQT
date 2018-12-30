using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;
using System.IO;
using System.Text;
using System.Transactions;

namespace DienMayQuyetTien.Areas.Admin.Controllers
{
    public class PromotionsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: Admin/Promotions
        public ActionResult Index()
        {
            
            if (Session["UserName"] != null)
            {
                ViewBag.Message = TempData["message"];
                return View(db.Promotions.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Admin/Promotions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // GET: Admin/Promotions/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.PromotionID = new SelectList(db.Promotions, "ID", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/Promotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        

        public ActionResult Create(Promotion promotion)
        {
            //CheckValidationPromotion(promotion);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    if (Request.Files["ImageFile"] != null && Request.Files["ImageFile"].ContentLength < 2097152)
                    {
                        //string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                        string extension = Path.GetExtension(Request.Files["ImageFile"].FileName);
                        string fileName = RandomString(5, true) + DateTime.Now.ToString("yymmssfff") + extension;
                        promotion.Image = "/Assets/Admin/img/promotions/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/Assets/Admin/img/promotions/"), fileName);
                        Request.Files["ImageFile"].SaveAs(fileName);
                        db.Promotions.Add(promotion);
                        db.SaveChanges();
                        scope.Complete();
                        TempData["message"] = "Tạo khuyễn mãi thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError("Image", "Chưa có hình khuyến mãi hoặc hình ảnh lớn hơn 2MB!");
                }
            }
            ViewBag.PromotionID = new SelectList(db.Promotions, "ID", "Name", promotion.ID);
            return View(promotion);
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
                return sb.ToString().ToLower();
            return sb.ToString();

        }

        //private void CheckValidationPromotion(Promotion model)
        //{
        //    if (model.Name == null || model.Name.Equals("") || model.Name.StartsWith(" ") || model.Name.EndsWith(" "))
        //        ModelState.AddModelError("Name", "Name không được bỏ trống hoặc khoảng trống hoặc khoảng trống");

        //}

        // GET: Admin/Promotions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                Promotion promotion = db.Promotions.Find(id);
                ViewBag.Promotion = db.Promotions.OrderByDescending(x => x.ID).ToList();
                return View(promotion);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Admin/Promotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        

        public ActionResult Edit(Promotion promotion)
        {
            //CheckValidationPromotion(promotion);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    if (Request.Files["ImageFile"] != null && Request.Files["ImageFile"].ContentLength < 2097152)
                    {
                        var fileNameOld = Server.MapPath(promotion.Image);
                        if (System.IO.File.Exists(fileNameOld))
                        {
                            System.IO.File.Delete(fileNameOld);
                        }
                        //string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                        string extension = Path.GetExtension(Request.Files["ImageFile"].FileName);
                        string fileName = RandomString(5, true) + DateTime.Now.ToString("yymmssfff") + extension;
                        promotion.Image = "/Assets/Admin/img/promotions/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/Assets/Admin/img/promotions/"), fileName);
                        Request.Files["ImageFile"].SaveAs(fileName);
                    }
                    db.Entry(promotion).State = EntityState.Modified;
                    db.SaveChanges();
                    scope.Complete();
                    TempData["message"] = "Chỉnh sửa sản phẩm thành công.";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Promotion = new SelectList(db.Promotions, "ID", "Name", promotion.ID);
            return View(promotion);
        }

        // GET: Admin/Promotions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // POST: Admin/Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);

                

                db.SaveChanges();
                TempData["message"] = "Xóa sản phẩm thành công.";
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
