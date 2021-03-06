﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;
using System.Web.Security;

namespace DienMayQuyetTien.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Session["UserName"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account objUser)
        {
            CheckValidationAccount(objUser);
            if (ModelState.IsValid)
            {
                using (DmQT03Entities db = new DmQT03Entities())
                {
                    var obj = db.Accounts.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["FullName"] = obj.Fullname.ToString();
                        Session["UserName"] = obj.Username.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Incorrect = "Username hoặc Password không chính xác";
                    }
                }
            }
            return View(objUser);
        }

        public void CheckValidationAccount(Account account)
        {
            if (account.Username == null || account.Username.Equals("") || account.Username.StartsWith(" ") || account.Username.EndsWith(" "))
                ModelState.AddModelError("Username", "Username không được bỏ trống hoặc khoảng trống");
            if (account.Password == null || account.Password.Equals("") || account.Password.StartsWith(" ") || account.Password.EndsWith(" "))
                ModelState.AddModelError("Password", "Password không được bỏ trống hoặc khoảng trốnghoặc khoảng trống");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}