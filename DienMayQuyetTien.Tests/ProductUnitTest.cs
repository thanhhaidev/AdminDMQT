using System;
using System.Linq;
using DienMayQuyetTien.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using DienMayQuyetTien.Areas.Admin.Models;
using System.Collections.Generic;
using Moq;
using System.Web;
using System.Web.Routing;
using System.Transactions;
using DienMayQuyetTien.Areas.Admin.Controllers;

namespace DienMayQuyetTien.Tests
{
    /// <summary>
    /// Summary description for ProductUnitTest
    /// </summary>
    [TestClass]
    public class ProductUnitTest
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new HomeController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Index() as ViewResult;
            var db = new DmQT03Entities();

            //Assert.IsNotNull(result.ViewBag.Message);
            Assert.IsInstanceOfType(result.Model, typeof(List<Product>));
            Assert.AreEqual(db.Products.Count(), (result.Model as List<Product>).Count);

            session.Setup(s => s["UserName"]).Returns(null);
            var redirect = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Login", redirect.RouteValues["action"]);
            Assert.AreEqual("Login", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public void CreateGetTest()
        {
            var controller = new HomeController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData["ProductTypeID"], typeof(SelectList));
        }

        [TestMethod]
        public void CreatePostTest()
        {
            var controller = new HomeController();
            var db = new DmQT03Entities();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(c => c.Request).Returns(request.Object);
            request.Setup(r => r.Files).Returns(files.Object);
            files.Setup(f => f["ImageFile"]).Returns(file.Object);
            file.Setup(f => f.ContentLength).Returns(1);
            file.Setup(c => c.FileName).Returns("image.png");
            context.Setup(c => c.Server.MapPath("/Assets/Admin/img/products/")).Returns("/Assets/Admin/img/products/");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            using (var scope = new TransactionScope())
            {

                var model = new Product();
                model.ProductTypeID = db.ProductTypes.First().ID;
                model.ProductName = "ProductName";
                model.OriginPrice = 123;
                model.SalePrice = 456;
                model.InstallmentPrice = 789;
                model.Quantity = 10;

                var result0 = controller.Create(model) as RedirectToRouteResult;
                //Assert.IsNotNull(result0);
                file.Verify(f => f.SaveAs(It.Is<string>(s => s.StartsWith("/Assets/Admin/img/products/"))));
                Assert.AreEqual("Index", result0.RouteValues["action"]);

                file.Setup(f => f.ContentLength).Returns(0);
                var result1 = controller.Create(model) as ViewResult;
                Assert.IsNotNull(result1);
                Assert.IsInstanceOfType(result1.ViewData["ProductTypeID"], typeof(SelectList));
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            var db = new DmQT03Entities();
            var product = new Product
            {
                ProductName = "ProductName",
                ProductTypeID = db.ProductTypes.First().ID,
                SalePrice = 123,
                OriginPrice = 123,
                InstallmentPrice = 123,
                Quantity = 123,
                Avatar = ""
            };

            var controller = new HomeController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["UserName"]).Returns("abc");
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            using (var scope = new TransactionScope())
            {
                db.Products.Add(product);
                db.SaveChanges();
                var count = db.Products.Count();
                var result2 = controller.DeleteConfirmed(product.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result2);
                Assert.AreEqual(count - 1, db.Products.Count());
            }
        }
    }
}