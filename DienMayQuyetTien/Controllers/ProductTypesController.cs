using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;

namespace DienMayQuyetTien.Controllers
{
    public class ProductTypesController : Controller
    {
        private DmQT03Entities db = new DmQT03Entities();

        // GET: ProductTypes
        public PartialViewResult Index()
        {
            return PartialView(db.ProductTypes.ToList());
        }

        public PartialViewResult Index1()
        {
            return PartialView(db.ProductTypes.ToList());
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
