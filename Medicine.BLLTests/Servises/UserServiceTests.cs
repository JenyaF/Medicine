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
            userService.CreateIdentify = (user, cookie) => new ClaimsIdentity();
        }
        [TestMethod]
        public void Authenticate_ExistUser_Test()
        {
            userService.Find = (email, password) => new ApplicationUser();
            var result = userService.Authenticate(new UserDTO() { Password = It.IsAny<string>(), Email = It.IsAny<string>() });
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Authenticate_NoExistUser_Test()
        {
            userService.Find = (email, password) => null;
            var result = userService.Authenticate(new UserDTO() { Password = It.IsAny<string>(), Email = It.IsAny<string>() });
            Assert.IsNull(result);
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
    }
}