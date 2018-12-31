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
    [TestClass]
    public class ProductTypesUnitTest
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new ProductTypesController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Index() as ViewResult;
            var db = new DmQT03Entities();
<<<<<<< HEAD
            
            Assert.IsInstanceOfType(result.Model, typeof(List<ProductType>));
            Assert.AreEqual(db.ProductTypes.Count(), (result.Model as List<ProductType>).Count);
=======

            //Assert.IsNotNull(result.ViewBag.Message);
            Assert.IsInstanceOfType(result.Model, typeof(List<ProductType>));
            Assert.AreEqual(db.Products.Count(), (result.Model as List<ProductType>).Count);
>>>>>>> 2fb75e2e61c4d872c5b746bf1b7ad06b7c016987

            session.Setup(s => s["UserName"]).Returns(null);
            var redirect = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Login", redirect.RouteValues["action"]);
            Assert.AreEqual("Login", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public void CreateGetTest()
        {
            var controller = new ProductTypesController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }
<<<<<<< HEAD

        [TestMethod]
=======
	
	[TestMethod]
>>>>>>> 2fb75e2e61c4d872c5b746bf1b7ad06b7c016987
        public void CreatePostTest()
        {
            var controller = new ProductTypesController();
            var db = new DmQT03Entities();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();

            using (var scope = new TransactionScope())
            {

                var model = new ProductType();
                model.ProductTypeName = "ProductTypeName";
                model.ProductTypeCode = "TES";
                var result0 = controller.Create(model) as RedirectToRouteResult;
                Assert.IsNotNull(result0);
            }
        }

<<<<<<< HEAD
        [TestMethod]
=======
	 [TestMethod]
>>>>>>> 2fb75e2e61c4d872c5b746bf1b7ad06b7c016987
        public void EditGetTest()
        {
            var db = new DmQT03Entities();
            var controller = new ProductTypesController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var productTypesID = db.ProductTypes.First().ID;

            var result = controller.Edit(productTypesID) as ViewResult;

            Assert.IsNotNull(result);
        }
<<<<<<< HEAD
        [TestMethod]
=======
   [TestMethod]
>>>>>>> 2fb75e2e61c4d872c5b746bf1b7ad06b7c016987
        public void EditPostTest()
        {
            var controller = new ProductTypesController();
            var db = new DmQT03Entities();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            
            var model = db.ProductTypes.AsNoTracking().First();

            using (var scope = new TransactionScope())
            {
                model.ProductTypeCode = "TES";
                model.ProductTypeName = "TEST";
                
                var result = controller.Edit(model) as RedirectToRouteResult;

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            var db = new DmQT03Entities();
            var productType = new ProductType
            {
                ProductTypeName = "ProductName",
                ProductTypeCode = "123",
            };

            var controller = new ProductTypesController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["UserName"]).Returns("abc");
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            using (var scope = new TransactionScope())
            {
                db.ProductTypes.Add(productType);
                db.SaveChanges();
                var count = db.ProductTypes.Count();
                var result2 = controller.DeleteConfirmed(productType.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result2);
                Assert.AreEqual(count - 1, db.ProductTypes.Count());
            }
        }
    }
}