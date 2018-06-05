using Medicine.BLL.DTO;
using Medicine.BLL.Infrastructure;
using Medicine.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Medicine.BLL.Interfaces;
using Medicine.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System;
using AutoMapper;

namespace Medicine.BLL.Services
{
    public class UserService : IUserService
    {
        public Func<string, ApplicationUser> FindByEmail { get; set; }
        public Func<ApplicationUser, string, IdentityResult> CreateUser { get; set; }
        public Func<string, string,IdentityResult>AddToRole { get; set; }
        public Func<string,string, ApplicationUser> Find { get; set; }
        public Func<ApplicationUser,string,ClaimsIdentity> CreateIdentify { get; set; }
        public IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
            FindByEmail=(email)=> Database.UserManager.FindByEmail(email);
            CreateUser=(user,userPassword)=> Database.UserManager.Create(user, userPassword);
            AddToRole=(userId,role)=> Database.UserManager.AddToRole(userId, role);
            Find = (email, password) => Database.UserManager.Find(email, password);
            CreateIdentify=(user,cookie) => Database.UserManager.CreateIdentity(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
        }

        public IEnumerable<DoctorDTO> GetDoctors()
        {
            return Database.Doctors.GetAll().Select(d => new DoctorDTO() { Name = d.ClientProfile.Name, Qualification = d.Qualification, Surname = d.ClientProfile.Surname, Email = d.ClientProfile.ApplicationUser.Email });
        }
        private OperationDetails Create(UserDTO userDto)
        {

            ApplicationUser user = FindByEmail(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                CreateUser(user, userDto.Password);
                AddToRole(user.Id, userDto.Role); 
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name, DateOfBirth = userDto.DateOfBirth, Surname = userDto.Surname, Role = userDto.Role };
                Database.ClientManager.Create(clientProfile);
                Database.Save();
                userDto.Id = user.Id;
                return new OperationDetails(true, "Registration is successful", "");
            }
            else
            {
                return new OperationDetails(false, "User with such login already exists", "Email");
            }
        }
        
        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = Find(userDto.Email, userDto.Password);
            if (user != null)
                claim = CreateIdentify(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public void SetInitialData(List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = Database.RoleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    Database.RoleManager.Create(role);
                }
            }
        }

        public string GetRole(string email)
        {
            return Database.ClientManager.GetRole(email);
        }

        public string GetId(string email)
        {
            return Database.ClientManager.GetId(email);
        }

        public OperationDetails Create(DoctorDTO doctorDTO)
        {
            OperationDetails operationDetails = Create((UserDTO)doctorDTO);
            if (operationDetails.Succedeed)
            {
                Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
                Database.Save();
            }
            return operationDetails;
        }

        public void Update(DoctorDTO doctorDTO)
        {
            Database.ClientManager.Update(new ClientProfile() { Id = doctorDTO.Id, DateOfBirth = doctorDTO.DateOfBirth, Name = doctorDTO.Name, Role = "doctor", Surname = doctorDTO.Surname });
            Database.Doctors.Update(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
            Database.Save();
        }

        //id-removed doctor id,doctorid-doctor id for replase
        public void Delete(string RemovedDoctorId, string replasingDoctorId)
        {
            Database.Doctors.Delete(RemovedDoctorId, replasingDoctorId);
            Database.Save();
        }

        public IEnumerable<DoctorDTO> GetAll()
        {
            return Database.Doctors.GetAll().
               Join(Database.ClientManager.GetAll(),
                       x => x.Id,
                       y => y.Id,
                       (x, y) => new DoctorDTO() { Id = x.Id, Name = y.Name, Surname = y.Surname, Qualification = x.Qualification,DateOfBirth=y.DateOfBirth,Email=y.ApplicationUser.Email ,Role="doctor"}).ToList();
        }

        public OperationDetails Create(PatientDTO patientDTO)
        {
            OperationDetails operationDetails = Create((UserDTO)patientDTO);
            if (operationDetails.Succedeed)
            {
                Database.Patients.Create(new Patient() { Id = patientDTO.Id, DoctorId = patientDTO.DoctorId, historyOfTreatment = patientDTO.historyOfTreatment });
                Database.Save();
            }
            return operationDetails;
        }

        public void Update(PatientDTO patientDTO)
        {
            Database.ClientManager.Update(new ClientProfile() { Id = patientDTO.Id, DateOfBirth = patientDTO.DateOfBirth, Name = patientDTO.Name, Role = "patient", Surname = patientDTO.Surname });
            Database.Patients.Update(new Patient() { Id = patientDTO.Id, DoctorId = patientDTO.DoctorId, historyOfTreatment = patientDTO.historyOfTreatment });
            Database.Save();
        }

        public void Delete(string patientId)
        {
            Database.Patients.Delete(patientId);
            Database.Save();
        }

        public IEnumerable<PatientDTO> GetAll(string doctorId)
        {
            return Database.Patients.GetAll(doctorId).
               Join(Database.ClientManager.GetAll(),
                       x => x.Id,
                       y => y.Id,
                       (x, y) => new PatientDTO() { Id = x.Id, Name = y.Name, Surname = y.Surname, Email = x.ClientProfile.ApplicationUser.Email, historyOfTreatment = x.historyOfTreatment, DoctorId = x.DoctorId,DateOfBirth=x.ClientProfile.DateOfBirth,Role="patient",UserName=y.ApplicationUser.UserName }).ToList();
        }

        public DoctorDTO GetDoctor(string id)
        {
            Doctor doctor = Database.Doctors.Get(id);
            return new DoctorDTO() {Id = doctor.Id, Email = doctor.ClientProfile.ApplicationUser.Email, Name = doctor.ClientProfile.Name, Surname = doctor.ClientProfile.Surname, Qualification = doctor.Qualification, Role = "doctor", UserName = doctor.ClientProfile.ApplicationUser.UserName,DateOfBirth=doctor.ClientProfile.DateOfBirth };
        }

        public PatientDTO GetPatient(string id)
        {
            Patient patient = Database.Patients.Get(id);
            return new PatientDTO() { Id = patient.Id, Email = patient.ClientProfile.ApplicationUser.Email, Name = patient.ClientProfile.Name, Surname = patient.ClientProfile.Surname, historyOfTreatment = patient.historyOfTreatment, Role = "patient", UserName = patient.ClientProfile.ApplicationUser.UserName, DoctorId = patient.DoctorId,DateOfBirth=patient.ClientProfile.DateOfBirth };

        }
        
        public void Dispose()
        {
            Database.Dispose();
        }       
      
    }
}