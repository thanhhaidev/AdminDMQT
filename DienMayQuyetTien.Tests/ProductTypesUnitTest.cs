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

            //Assert.IsNotNull(result.ViewBag.Message);
            Assert.IsInstanceOfType(result.Model, typeof(List<ProductType>));
            Assert.AreEqual(db.Products.Count(), (result.Model as List<ProductType>).Count);

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

	 [TestMethod]
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
    }
}
