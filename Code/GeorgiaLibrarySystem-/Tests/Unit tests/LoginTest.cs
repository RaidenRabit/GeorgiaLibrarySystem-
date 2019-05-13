using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Extras.Moq;
using GtlService.DataAccess;
using GtlService.Model;
using Moq;
using System.Data.Entity;
using GtlService.DataManagement;

namespace Tests.Unit_tests
{
    public class LoginTest
    {
        [Test]
        public void Login_CorrectEmailAndPassword_True()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                mock.Mock<GTLEntities>()
                    .Setup(x => x.People.Any())
                    .Returns(true);

                //Act
                var cls = mock.Create<LoginDA>();
                var expected = true;

                var actual = cls.Login(1, "test");

                //Assert
                Assert.IsTrue(expected == actual);
            }
        }
    }
}

/*
            var mockSet = new Mock<DbSet<Blog>>();

            var mockContext = new Mock<BloggingContext>();
            mockContext.Setup(m => m.Blogs).Returns(mockSet.Object);

            var service = new BlogService(mockContext.Object);
            service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

            mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            
             //using (var mock = AutoMock.GetLoose())
            //{
                //Arrange
                var mock = new Mock<GTLEntities>();

                mock.Setup(x => x.People.Any(y => y.SSN == It.IsAny<int>() && y.Password.Equals(It.IsAny<string>())))
                    .Returns(true);

                //Act
                var repo = new LoginDA(mock.Object);
                var result = repo.Login(1, "test");

                //Assert
                Assert.IsTrue(result);
            //}

            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                mock.Mock<GTLEntities>()
                    .Setup(x => x.People.Any(y => y.SSN == It.IsAny<int>() && y.Password.Equals(It.IsAny<string>())))
                    .Returns(true);

                //Act
                var cls = mock.Create<LoginDA>();
                var expected = true;

                var actual = cls.Login(1, "test");

                //Assert
                Assert.IsTrue(expected == actual);
            }
            */