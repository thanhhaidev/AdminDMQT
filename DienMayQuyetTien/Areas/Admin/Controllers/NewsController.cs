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
using System.IO;
using System.Text;

namespace DienMayQuyetTien.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: Admin/News
        public ActionResult Index()
        {

            if (Session["UserName"] != null)
            {
                ViewBag.Message = TempData["message"];
                return View(db.News.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
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

        // POST: Admin/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( News news)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    if (Request.Files["ImageFile"] != null && Request.Files["ImageFile"].ContentLength < 2097152)
                    {
                        string extension = Path.GetExtension(Request.Files["ImageFile"].FileName);
                        string fileName = RandomString(5, true) + DateTime.Now.ToString("yymmssfff") + extension;
                        news.Image = "/Assets/Admin/img/news/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/Assets/Admin/img/news/"), fileName);
                        Request.Files["ImageFile"].SaveAs(fileName);
                        db.News.Add(news);
                        db.SaveChanges();
                        scope.Complete();
                        TempData["message"] = "Tạo Tin Tức thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError("Image", "Chưa có hình tin tức hoặc hình ảnh lớn hơn 2MB!");
                }
            }
            return View(news);
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

        // GET: Admin/News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                News news = db.News.Find(id);
                return View(news);
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


        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    if (Request.Files["ImageFile"] != null && Request.Files["ImageFile"].ContentLength > 0)
                    {
                        var fileNameOld = Server.MapPath(news.Image);
                        if (System.IO.File.Exists(fileNameOld))
                        {
                            System.IO.File.Delete(fileNameOld);
                        }
                        //string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                        string extension = Path.GetExtension(Request.Files["ImageFile"].FileName);
                        string fileName = RandomString(5, true) + DateTime.Now.ToString("yymmssfff") + extension;
                        news.Image = "/Assets/Admin/img/news/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/Assets/Admin/img/news/"), fileName);
                        Request.Files["ImageFile"].SaveAs(fileName);
                    }
                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                    scope.Complete();
                    TempData["message"] = "Chỉnh sửa tin tức thành công.";
                    return RedirectToAction("Index");
                }
            }
            return View(news);
        }
        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
