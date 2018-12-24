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

        public void DeleteTest()
        {
            ProductAdminController target = new ProductAdminController();
            //int id = 50;

            var db = new DIENMAYQUYETTIENEntities();

            using (var scope = new TransactionScope())
            {
                var product = new Product
                {
                    ProductCode = "Code",
                    ProductName = "ProductName",
                    ProductTypeID = db.ProductTypes.First().ID,
                    SalePrice = 123,
                    OriginPrice = 123,
                    InstallmentPrice = 123,
                    Quantity = 123,
                    Avatar = ""
                };
                db.Products.Add(product);
                db.SaveChanges();
                // test view delete
                var result1 = target.Delete(product.ID) as ViewResult;
                Assert.IsNotNull(result1);
                Assert.AreEqual(product.ID, (result1.Model as Product).ID);

                // test delete post
                var count = db.Products.Count();
                var result2 = target.De(product.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result2);
                Assert.AreEqual(count - 1, db.Products.Count());
            }
        }
    }
}