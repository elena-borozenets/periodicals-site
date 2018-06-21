using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Periodicals.Controllers;
using Moq;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using System.Web.Mvc;
using System.Web;
using Periodicals.Models;
using XAssert = Xunit.Assert;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Periodicals.Test
{
    [TestClass]
    public class EditionControllerTests
    {

        [TestMethod]
        public void Index_ReturnsAViewResult_IsNotNull()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Id=0,
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow
                },
                new Edition()
                {
                    Id=1,
                    Name = "Edition1",
                    Description = "Edition is Edition1",
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
            EditionController controller = new EditionController(mock.Object, mockT.Object, mockU.Object);

            //Act
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void Index_ReturnsAViewResult_WithAListOfEditionModel()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Id=0,
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow
                },
                new Edition()
                {
                    Id=1,
                    Name = "Edition1",
                    Description = "Edition is Edition1",
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
            EditionController controller = new EditionController(mock.Object, mockT.Object, mockU.Object);

            //Act
            var result = controller.Index() as ViewResult;
            //var m = result.Model as IEnumerable<EditionModel>;
            //Assert
            var viewResult = XAssert.IsType<ViewResult>(result);
            var model = XAssert.IsAssignableFrom<IEnumerable<EditionModel>>(
                viewResult.ViewData.Model);
            ////Assert.IsNull(model);
            XAssert.Equal(2, model.Count());

        }


    }
}
