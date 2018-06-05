using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Medicine.WEB.Models;
using Medicine.BLL.DTO;
using System.Security.Claims;
using Medicine.BLL.Interfaces;
using AutoMapper;
using System.Linq;
using Medicine.BLL.Infrastructure;

namespace Medicine.WEB.Controllers
{
    public class HomeController : Controller
    {

        private IUserService UserService { get => HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        private IAuthenticationManager AuthenticationManager { get => HttpContext.GetOwinContext().Authentication; }

        public ActionResult Index()
        {

            UserService.SetInitialData(new List<string>() { "admin", "doctor", "patient" });
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View(new LoginModel() { Email = "email@ukr.net", Password = "qwerty654321" });
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ViewBag.Message = "Incorrect login or password.";
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    string role = UserService.GetRole(model.Email);
                    switch (role)
                    {
                        case "admin":
                            {
                                return RedirectToAction("GetListOfDoctors");
                            }
                        case "doctor":
                            {
                                return RedirectToAction($"GetListOfPatients/{UserService.GetId(userDto.Email)}");
                            }
                        case "patient":
                            {
                                return RedirectToAction($"GetListOfRecipes/{UserService.GetId(userDto.Email)}", "Recipe");
                            }
                        default:
                            {
                                break;
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

        [Authorize(Roles = "admin")]
        public ActionResult GetListOfDoctors()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO, DoctorView>()).CreateMapper();
            var doctors = mapper.Map<IEnumerable<DoctorDTO>, List<DoctorView>>(UserService.GetAll());
            return View(doctors);
        }

        [Authorize(Roles = "admin, doctor")]
        public ActionResult GetListOfPatients(string doctorId)
        {
            if (doctorId == null)
            {
                var doctorEmail = HttpContext.User.Identity.Name;
                doctorId = UserService.GetId(doctorEmail);
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PatientDTO, PatientView>()).CreateMapper();
            var patients = mapper.Map<IEnumerable<PatientDTO>, List<PatientView>>(UserService.GetAll(doctorId));
            ViewBag.doctorId = doctorId;
            return View(patients);
        }
        [Authorize(Roles = "admin")]
        public ActionResult RegisterDoctor()
        {
            return View(new DoctorView() { Surname = "Smith", Name = "Ann", Email = "asdfg@gmail.com", Password = "qwerty654321", Qualification = "low", DateOfBirth = "2000-09-28" });
        }

        [HttpPost]
        public ActionResult RegisterDoctor(DoctorView model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorView, DoctorDTO>()).CreateMapper();
                var doctorDTO = mapper.Map<DoctorView, DoctorDTO>(model);
                doctorDTO.Role = "doctor";
                OperationDetails operationDetails = UserService.Create(doctorDTO);
                if (operationDetails.Succedeed)
                {
                    return RedirectToAction("GetListOfDoctors");
                }

                else
                    ViewBag.Message = "Already exist!";
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult UpdateDoctor(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO, DoctorView>()).CreateMapper();
            var doctor = mapper.Map<DoctorDTO, DoctorView>(UserService.GetDoctor(id));
            doctor.Password = "qwerty654321";
            return View(doctor);
        }
        [HttpPost]
        public ActionResult UpdateDoctor(DoctorView model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorView, DoctorDTO>()).CreateMapper();
                var doctor = mapper.Map<DoctorView, DoctorDTO>(model);
                UserService.Update(doctor);
                return RedirectToAction("GetListOfDoctors");
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteDoctorView(string id)
        {
            ViewBag.id = id;
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DoctorDTO, DoctorView>()).CreateMapper();
            var item = mapper.Map<IEnumerable<DoctorDTO>, IEnumerable<DoctorView>>(UserService.GetAll().Where(x => x.Id != id));
            return View(item);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteDoctor(string id, string newId)
        {
            UserService.Delete(id, newId);
            return RedirectToAction("GetListOfDoctors");
        }

        [Authorize(Roles = "admin, doctor")]
        public ActionResult CreatePatient(string doctorId)
        {
            return View(new PatientView() { DoctorId = doctorId ?? UserService.GetId(HttpContext.User.Identity.Name), Surname = "Smith", Name = "Ann", Email = "asdfgp@gmail.com", Password = "qwerty654321", historyOfTreatment = "some diagnosis", DateOfBirth = "1998-10-20" });
        }

        [HttpPost]
        public ActionResult CreatePatient(PatientView model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PatientView, PatientDTO>()).CreateMapper();
                var patientDTO = mapper.Map<PatientView, PatientDTO>(model);
                patientDTO.Role = "patient";
                OperationDetails operationDetails = UserService.Create(patientDTO);
                if (operationDetails.Succedeed)
                    return RedirectToAction($"GetListOfPatients", new { doctorId = patientDTO.DoctorId });
                else ViewBag.Message = "Already exist";
            }
            return View(model);
        }

        [Authorize(Roles = "admin, doctor")]
        public ActionResult UpdatePatient(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PatientDTO, PatientView>()).CreateMapper();
            var patient = mapper.Map<PatientDTO, PatientView>(UserService.GetPatient(id));
            patient.Password = "qwerty654321";
            return View(patient);
        }

        [HttpPost]
        public ActionResult UpdatePatient(PatientView model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PatientView, PatientDTO>()).CreateMapper();
                var patient = mapper.Map<PatientView, PatientDTO>(model);
                UserService.Update(patient);
                return RedirectToAction($"GetListOfPatients", new { doctorId = model.DoctorId });
            }
            return View(model.Id);
        }

        [Authorize(Roles = "admin, doctor")]
        public ActionResult DeletePatient(string id)
        {
            string doctorid = UserService.GetPatient(id).DoctorId;
            UserService.Delete(id);
            return RedirectToAction($"GetListOfPatients", new { doctorId = doctorid });
        }
    }
}

