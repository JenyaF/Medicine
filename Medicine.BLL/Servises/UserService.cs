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

            ApplicationUser user = FindByEmail(userDto.Email);// Database.UserManager.FindByEmail(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                CreateUser(user, userDto.Password);// Database.UserManager.Create(user, userDto.Password);
                AddToRole(user.Id, userDto.Role); // Database.UserManager.AddToRole(user.Id, userDto.Role);
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
            ApplicationUser user = Find(userDto.Email, userDto.Password);//Database.UserManager.Find(userDto.Email, userDto.Password);
            if (user != null)
                claim = CreateIdentify(user, DefaultAuthenticationTypes.ApplicationCookie);//Database.UserManager.CreateIdentity(user,  DefaultAuthenticationTypes.ApplicationCookie);                                         
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
            // await Create(adminDto);
        }
        public void Dispose()
        {
            Database.Dispose();
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
            OperationDetails operationDetails = Create((UserDTO)doctorDTO);//.Result  createAsync
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
        public void Delete(string id, string doctorId)
        {
            Database.Doctors.Delete(id, doctorId);
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
            return new DoctorDTO() { Id = doctor.Id, Email = doctor.ClientProfile.ApplicationUser.Email, Name = doctor.ClientProfile.Name, Surname = doctor.ClientProfile.Surname, Qualification = doctor.Qualification, Role = "doctor", UserName = doctor.ClientProfile.ApplicationUser.UserName,DateOfBirth=doctor.ClientProfile.DateOfBirth };
        }
        public PatientDTO GetPatient(string id)
        {
            Patient patient = Database.Patients.Get(id);
            return new PatientDTO() { Id = patient.Id, Email = patient.ClientProfile.ApplicationUser.Email, Name = patient.ClientProfile.Name, Surname = patient.ClientProfile.Surname, historyOfTreatment = patient.historyOfTreatment, Role = "patient", UserName = patient.ClientProfile.ApplicationUser.UserName, DoctorId = patient.DoctorId,DateOfBirth=patient.ClientProfile.DateOfBirth };

        }
        




        
        /*     public async Task<OperationDetails> CreateAsync(DoctorDTO doctorDTO)
             {
                 OperationDetails operationDetails = CreateAsync((UserDTO)doctorDTO).Result;
                 if (operationDetails.Succedeed)
                 {
                     Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
                     await Database.SaveAsync();
                 }
                 return operationDetails;
             }
         */
         /* public async Task<OperationDetails> CreateDoctorAsync(DoctorDTO doctorDTO)
         {
             OperationDetails operationDetails = await this.CreateAsync(doctorDTO);
             Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
             await Database.SaveAsync();
             return operationDetails;
         }*/
        /*  public async Task<OperationDetails> CreateAsync(UserDTO userDto)
          {
              ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
              if (user == null)
              {
                  user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                  await Database.UserManager.CreateAsync(user, userDto.Password);
                  // добавляем роль
                  await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                  // создаем профиль клиента
                  ClientProfile clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name,DateOfBirth=userDto.DateOfBirth,Surname=userDto.Surname,Role=userDto.Role };
                  Database.ClientManager.Create(clientProfile);
                  await Database.SaveAsync();
                  return new OperationDetails(true, "Регистрация успешно пройдена", "");

              }
              else
              {
                  return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
              }
          }

         public async Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto)
         {
             ClaimsIdentity claim = null;
             // находим пользователя
             ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
             // авторизуем его и возвращаем объект ClaimsIdentity
             if (user != null)
                 claim = await Database.UserManager.CreateIdentityAsync(user,
                                             DefaultAuthenticationTypes.ApplicationCookie);
             return claim;
         }
  */
        /*
        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {            
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
           // await Create(adminDto);
        }
        */

    }
}