using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Services;

namespace Periodicals.Test
{
    [TestClass]
    public class EditionServicesTests
    {
        [TestMethod]
        public void GetEditionsByLanguageMethod_ReturnsListCounts2()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "ru"
                },
                new Edition()
                {
                    Name = "Edition1",
                    Description = "Edition is Edition1",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng"
                }
            });
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.GetEditionsByLanguage("eng");
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        public void GetEditionsByLanguageMethod_ReturnsEmptyList()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>());
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.GetEditionsByLanguage("eng");
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        public void GetEditionsByLanguageMethod_ByUA_ReturnsEmptyList()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "ru"
                },
                new Edition()
                {
                    Name = "Edition1",
                    Description = "Edition is Edition1",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng"
                }
            });
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.GetEditionsByLanguage("ua");
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void SortByNameTest_ReturnsSortedList()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng"
                },
                new Edition()
                {
                    Name = "Edition1",
                    Description = "Edition is Edition1",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng"
                }
            });
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.SortByName(true);
            //Assert
            Assert.AreEqual("Edition",result[0].Name );
        }

        [TestMethod]
        public void SortByNameTest_ReturnsEmptyList()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>());
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.SortByName(true);
            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SortByPriceTest_ReturnsSortedList()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng",
                    Price = 3
                },
                new Edition()
                {
                    Name = "Edition1",
                    Description = "Edition is Edition1",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng",
                    Price = 5
                }
            });
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.SortByPrice(true);
            //Assert
            Assert.AreEqual(3, result[0].Price);
        }

        [TestMethod]
        public void SearchByName_Ed_Returns_ListOfTwoElements()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng",
                    Price = 3
                },
                new Edition()
                {
                    Name = "Edition1",
                    Description = "Edition is Edition1",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng",
                    Price = 5
                }
            });
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.SearchByName("ed");
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void SearchByName_K_Returns_ListOfZeroElements()
        {
            //Arrange
            var mock = new Mock<IRepository<Edition>>();
            mock.Setup(a => a.List()).Returns(new List<Edition>()
            {
                new Edition()
                {
                    Name = "Edition",
                    Description = "Edition is Edition",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng",
                    Price = 3
                },
                new Edition()
                {
                    Name = "Edition1",
                    Description = "Edition is Edition1",
                    DateNextPublication = DateTime.UtcNow,
                    Language = "eng",
                    Price = 5
                }
            });
            EditionServices service = new EditionServices(mock.Object);

            //Act
            var result = service.SearchByName("k");
            //Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
