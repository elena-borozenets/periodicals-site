using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periodicals.Controllers;
using Moq;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Core.Identity;
using Periodicals.Core.Interfaces;
using System.Web.Mvc;

namespace Periodicals.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow
                }
            });
            var mockT = new Mock<IRepository<Topic>>();
            mockT.Setup(a => a.List()).Returns(new List<Topic>()
            {
                new Topic()
                {
                    TopicName = "Topic"
                }
            });
            var mockU = new Mock<IUserRepository>();
            HomeController controller = new HomeController(mock.Object, mockT.Object, mockU.Object);

            //Act
            var result = controller.Index();
            //Assert
            //(result as ViewResult).
        }
    }
}
