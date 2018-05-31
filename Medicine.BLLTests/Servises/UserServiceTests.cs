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
        private static UserService userService;
        private static Mock<IUnitOfWork> databaseMock;
        [ClassInitialize]
         public static void ClassInitialize(TestContext context)
        {
            databaseMock = new Mock<IUnitOfWork>();
            databaseMock.Setup(x => x.ClientManager.Create(It.IsAny<ClientProfile>()));
            userService = new UserService(databaseMock.Object);
            userService.CreateUser = (user, userPassword) => new IdentityResult();
            userService.AddToRole = (userId, role) => new IdentityResult();
        }
        [TestMethod]
        public void AuthenticateTest()
        {           
            Assert.Fail();
        }

        [TestMethod]
        public void Create_No_Exist_Doctor_Test()
        {
            userService.FindByEmail = (email) => null;
            databaseMock.Setup(x => x.Doctors.Create(It.IsAny<Doctor>()));

            var result = userService.Create(new DoctorDTO());

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void Create_Exist_Doctor_Test()
        {

            userService.FindByEmail = (email) => new ApplicationUser();
            databaseMock.Setup(x => x.Doctors.Create(It.IsAny<Doctor>()));

            var result = userService.Create(new DoctorDTO());

            Assert.IsFalse(result.Succedeed);
        }
        [TestMethod]
        public void Create_Exist_Patient_Test()
        {

            userService.FindByEmail = (email) => new ApplicationUser();
            databaseMock.Setup(x => x.Patients.Create(It.IsAny<Patient>()));

            var result = userService.Create(new PatientDTO());

            Assert.IsFalse(result.Succedeed);
        }
        [TestMethod]
        public void Create_No_Exist_Patient_Test()
        {
            userService.FindByEmail = (email) => null;
            databaseMock.Setup(x => x.Patients.Create(It.IsAny<Patient>()));

            var result = userService.Create(new PatientDTO());

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