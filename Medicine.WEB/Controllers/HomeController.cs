using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Medicine.WEB.Models;
using Medicine.BLL.DTO;
using System.Security.Claims;
using Medicine.BLL.Interfaces;
using Medicine.BLL.Infrastructure;
using AutoMapper;
using System.Linq;

namespace Medicine.WEB.Controllers
{
    public class HomeController : Controller
    {

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Index()
        {
            
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View(new LoginModel() { Email = "email1@ukr.net", Password = "qwerty654321" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                   // string role = claim.RoleClaimType;
                
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    //HttpContext.User.IsInRole("admin")
                    string role = UserService.GetRole(model.Email);
                    switch (role)
                    {
                        case "admin":
                            {
                                return RedirectToAction("GetListOfDoctors");
                            }
                        case "doctor":
                            {
                                return RedirectToAction("GetListOfPatients");
                            }
                        case "patient":
                            {
                                return RedirectToAction("GetListOfMedicaments");
                            }
                        default:
                            {

                                return RedirectToAction("Login");
                            }
                    }
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

       
        public ActionResult GetListOfDoctors()
        {
              var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO, DoctorView>()).CreateMapper();
              var doctors = mapper.Map<IEnumerable<DoctorDTO>, List<DoctorView>>(UserService.GetAll());
            return View(doctors);
        }
        public ActionResult GetListOfPatients()
        {
            var doctorEmail = HttpContext.User.Identity.Name;
            string doctorId = UserService.GetDoctorId(doctorEmail);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO, DoctorView>()).CreateMapper();
            var patients = mapper.Map<IEnumerable<PatientDTO>, List<PatientView>>(UserService.GetAll(doctorId));
            return View(patients);
        }
        public ActionResult RegisterDoctor()
        {
            return View(new DoctorView() { Surname = "Smith", Name = "Ann", Email = "asdfg@gmail.com", Password = "qwerty654321", Qualification = "low",DateOfBirth="01.02.1970" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterDoctor(DoctorView model)
        {
          //  await SetInitialDataAsync();          
            if (ModelState.IsValid)
            {
                DoctorDTO doctorDTO = new DoctorDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Surname=model.Surname,
                    Role = "doctor",
                    Qualification=model.Qualification
                };
               // OperationDetails operationDetails = await UserService.CreateDoctorA(doctorDTO);
                UserService.CreateAsync(doctorDTO).GetAwaiter();
              /*  if (operationDetails.Succedeed)
                    return View("SuccessRegister");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);*/
                    return RedirectToAction("Login", "Home");
            }
            return View(model);
        }
        public ActionResult UpdateDoctor(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO, DoctorView>()).CreateMapper();
            var doctor = mapper.Map<DoctorDTO, DoctorView>(UserService.GetDoctor(id));
            return View(doctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDoctor(DoctorView model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap< DoctorView,DoctorDTO>()).CreateMapper();
            var doctor = mapper.Map< DoctorView,DoctorDTO>(model);
            UserService.Update(doctor);
            return RedirectToAction("GetListOfDoctors");
        }

        public ActionResult DeleteDoctorView(string id)
        {
            ViewBag.id = id;
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO,DoctorView >()).CreateMapper();
            var item = mapper.Map<IEnumerable< DoctorDTO>,IEnumerable <DoctorView>>(UserService.GetAll().Where(x=>x.Id!=id));
            return View(item);
        }
       
        public ActionResult DeleteDoctor(string id,string newId)
        {
            UserService.Delete(id,newId);
            return RedirectToAction("GetListOfDoctors");
        }
        public ActionResult CreatePatient()
        {
            return View(new PatientView() { Surname = "Smith", Name = "Ann", Email = "asdfgp@gmail.com", Password = "qwerty654321", historyOfTreatment="some diagnosis" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatient(PatientView model)
        {
            string doctorId = UserService.GetDoctorId(HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
               var patientDTO=new PatientDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Surname = model.Surname,
                    Role = "patient",
                   DoctorId=doctorId,
                   historyOfTreatment=model.historyOfTreatment,
                   
                };
                UserService.CreateAsync(patientDTO).GetAwaiter();
                return RedirectToAction("GetListOfPatients", "Home");
            }
            return View(model);
        }

        public ActionResult UpdatePatient(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PatientDTO, PatientView>()).CreateMapper();
            var patient = mapper.Map<PatientDTO, PatientView>(UserService.GetPatient(id));
            return View(patient);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePatient(PatientView model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PatientView, PatientDTO>()).CreateMapper();
            var patient = mapper.Map<PatientView, PatientDTO>(model);
            UserService.Update(patient);
            return RedirectToAction("GetListOfPatients");
        }

        public ActionResult DeletePatient(string id)
        {
            UserService.Delete(id);
            return RedirectToAction("GetListOfPatients");
        }











        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "somemail@mail.ru",
                Password = "ad46D_ewr3",
                Name = "Семен Семенович Горбунков",
                Role = "admin",
            }, new List<string> { "user", "admin","doctor","patient" });

        }
    }
}
/*
 *        private void SetInitialData()
{
    UserService.SetInitialData(new UserDTO
    {
        Email = "a@mail.ru",
        UserName = "a@mail.ru",
        Password = "1",
        Name = "Jek",
        Role = "admin",
    }, new List<string> { "user", "admin", "doctor", "patient" });
}
 * public ActionResult Login(LoginModel model)
  {
      // SetInitialData();
      if (ModelState.IsValid)
      {
          UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
          ClaimsIdentity claim =  UserService.Authenticate(userDto);
          if (claim == null)
          {
              ModelState.AddModelError("", "Неверный логин или пароль.");
          }
          else
          {
              AuthenticationManager.SignOut();
              AuthenticationManager.SignIn(new AuthenticationProperties
              {
                  IsPersistent = true
              }, claim);
              return RedirectToAction("Index", "Home");
          }
      }
      return View(model);
  }

public ActionResult Logout()
{
    AuthenticationManager.SignOut();
    return RedirectToAction("Index", "Home");
}

public ActionResult Register()
{
    return View();
}
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Register(RegisterModel model)
{
   // SetInitialData();
    if (ModelState.IsValid)
    {
        UserDTO userDto = new UserDTO
        {
            Email = model.Email,
            Password = model.Password,
            Name = model.Name,
            Role = "user"
        };
        OperationDetails operationDetails = UserService.Create(userDto);
        if (operationDetails.Succedeed)
            return View("SuccessRegister");
        else
            ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
    }
    return View(model);
}

}
}*/
