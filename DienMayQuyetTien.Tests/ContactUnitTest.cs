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
    public class ContactUnitTest
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new ContactsController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Index() as ViewResult;
            var db = new DmQT03Entities();

            Assert.IsInstanceOfType(result.Model, typeof(List<Contact>));
            Assert.AreEqual(db.ProductTypes.Count(), (result.Model as List<Contact>).Count);

            session.Setup(s => s["UserName"]).Returns(null);
            var redirect = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Login", redirect.RouteValues["action"]);
            Assert.AreEqual("Login", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public void CreateGetTest()
        {
            var controller = new ContactsController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePostTest()
        {
            var controller = new ContactsController();
            var db = new DmQT03Entities();
            var context = new Mock<HttpContextBase>();

            using (var scope = new TransactionScope())
            {

                var model = new Contact();
                model.Title = "Title";
                model.Email = "test@gmail.com";
                model.Fullname = "Nguyen Van A";
                model.Phone = 013456789;
                model.Comment = "Hello";
                var result0 = controller.Create(model) as RedirectToRouteResult;
                Assert.IsNotNull(result0);
            }
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
        [TestMethod]
        public void EditPostTest()
        {
            var controller = new ContactsController();
            var db = new DmQT03Entities();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();

            var model = db.Contacts.AsNoTracking().First();

            using (var scope = new TransactionScope())
            {
                model.Title = "Title";
                model.Email = "test@gmail.com";
                model.Fullname = "Nguyen Van A";
                model.Phone = 013456789;
                model.Comment = "Hello";

                var result = controller.Edit(model) as RedirectToRouteResult;

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            var db = new DmQT03Entities();
            var contact = new Contact
            {
                Title = "Title",
                Email = "test@gmail.com",
                Fullname = "Nguyen Van A",
                Phone = 013456789,
                Comment = "Hello"
            };

            var controller = new ContactsController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["UserName"]).Returns("abc");
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            using (var scope = new TransactionScope())
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                var count = db.Contacts.Count();
                var result2 = controller.DeleteConfirmed(contact.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result2);
                Assert.AreEqual(count - 1, db.Contacts.Count());
            }
        }
    }
}
