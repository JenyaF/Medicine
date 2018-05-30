using Microsoft.VisualStudio.TestTools.UnitTesting;
using Medicine.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Medicine.BLL.Interfaces;
using Medicine.DAL.Interfaces;
using Medicine.BLL.DTO;
using Medicine.BLL.Infrastructure;
using Medicine.DAL.Entities;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Medicine.DAL.Identity;


namespace Medicine.BLL.Services.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService userService;

        [TestMethod]
        public void AuthenticateTest()
        {
            Mock<IUnitOfWork> databaseMock = new Mock<IUnitOfWork>();
         //   databaseMock.Setup(x => x.UserManager.Find                //only async!
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateDoctorTest()
        {
            Mock<IUnitOfWork> databaseMock = new Mock<IUnitOfWork>();
            //   databaseMock.

            // databaseMock.Setup(x => x.Doctors.GetAll()).Returns(new List<Doctor>());

            databaseMock.Setup(x => x.UserManager.FindByEmail(It.IsAny<string>())).Returns<ApplicationUser>(null);
            //  databaseMock.Setup(x => x.Save());

            userService = new UserService(databaseMock.Object);
            var result=  userService.Create(new DoctorDTO() { Surname = "Smith", Name = "Ann", Email = "asdfg@gmail.com", Password = "qwerty654321", Qualification = "low", DateOfBirth = "01.02.1970", Role = "doctor", UserName = "asdfg@gmail.com" });
            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateDoctorTest()
        {
            Mock<IUnitOfWork> databaseMock = new Mock<IUnitOfWork>();
            databaseMock.SetReturnsDefault(new ApplicationUser());
            databaseMock.Setup(x => x.Doctors.Create(It.IsAny<Doctor>()));

            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDoctorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPatientTest()
        {
            Assert.Fail();
        }
    }
}