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
        public IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

       /* public async Task<OperationDetails> CreateDoctorAsync(DoctorDTO doctorDTO)
        {
            OperationDetails operationDetails = await this.CreateAsync(doctorDTO);
            Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
            await Database.SaveAsync();
            return operationDetails;
        }*/
        public IEnumerable<DoctorDTO> GetDoctors()
        {          
            return Database.Doctors.GetAll().Select(d => new DoctorDTO() { Name = d.ClientProfile.Name, Qualification = d.Qualification, Surname = d.ClientProfile.Surname, Email = d.ClientProfile.ApplicationUser.Email });
        }

        public async Task<OperationDetails> CreateAsync(UserDTO userDto)
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

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
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

        public void Dispose()
        {
            Database.Dispose();
        }

        public string GetRole(string email)
        {
            return Database.ClientManager.GetRole(email);
        }
        public string GetDoctorId(string email)
        {
            return Database.ClientManager.GetDoctorId(email);
        }
        public async Task<OperationDetails> CreateAsync(DoctorDTO doctorDTO)
        {
            OperationDetails operationDetails = CreateAsync((UserDTO)doctorDTO).Result;
            if (operationDetails.Succedeed)
            {
                Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
                await Database.SaveAsync();
            }
            return operationDetails;
        }
    
        public void Create(DoctorDTO doctorDTO)
        {
            OperationDetails operationDetails = CreateAsync((UserDTO)doctorDTO).Result;
            if (operationDetails.Succedeed)
            {
                Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
                Database.SaveAsync().GetAwaiter();
            }
        }

        public void Update(DoctorDTO doctorDTO)
        {
            Database.ClientManager.Update(new ClientProfile() { Id = doctorDTO.Id, DateOfBirth = doctorDTO.DateOfBirth, Name = doctorDTO.Name, Role = "doctor", Surname = doctorDTO.Surname });
            Database.Doctors.Update(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
            Database.SaveAsync().GetAwaiter();
        }

        public void Delete(string id, string doctorId)
        {
            Database.Doctors.Delete(id, doctorId);
            Database.SaveAsync().GetAwaiter();
        }

        public IEnumerable<DoctorDTO> GetAll()
        {
            return Database.Doctors.GetAll().
               Join(Database.ClientManager.GetAll(),
                       x => x.Id,
                       y => y.Id,
                       (x, y) => new DoctorDTO() { Id = x.Id, Name = y.Name, Surname = y.Surname, Qualification = x.Qualification }).ToList();
        }

        public void Create(PatientDTO patientDTO)
        {
            OperationDetails operationDetails = CreateAsync((UserDTO)patientDTO).Result;
            if (operationDetails.Succedeed)
            {
                Database.Patients.Create(new Patient() { Id = patientDTO.Id,DoctorId=patientDTO.DoctorId,historyOfTreatment=patientDTO.historyOfTreatment});
                Database.SaveAsync().GetAwaiter();
            }
        }

        public void Update(PatientDTO patientDTO)
        {
            Database.ClientManager.Update(new ClientProfile() { Id = patientDTO.Id, DateOfBirth = patientDTO.DateOfBirth, Name = patientDTO.Name, Role = "patient", Surname = patientDTO.Surname });
            Database.Patients.Update(new Patient() { Id = patientDTO.Id, DoctorId = patientDTO.DoctorId, historyOfTreatment = patientDTO.historyOfTreatment }); 
            Database.SaveAsync().GetAwaiter();
        }

        public void Delete(string patientId)
        {
            Database.Patients.Delete(patientId);
            Database.SaveAsync().GetAwaiter();
        }

        public IEnumerable<PatientDTO> GetAll(string doctorId)
        {
            return Database.Patients.GetAll(doctorId).
               Join(Database.ClientManager.GetAll(),
                       x => x.Id,
                       y => y.Id,
                       (x, y) => new PatientDTO() { Id = x.Id, Name = y.Name, Surname = y.Surname, Email=x.ClientProfile.ApplicationUser.Email,historyOfTreatment=x.historyOfTreatment,DoctorId=x.DoctorId }).ToList();
        }
        public DoctorDTO GetDoctor(string id)
        {
             Doctor doctor= Database.Doctors.Get(id);
            return new DoctorDTO() { Id = doctor.Id, Email = doctor.ClientProfile.ApplicationUser.Email, Name = doctor.ClientProfile.Name, Surname = doctor.ClientProfile.Surname, Qualification = doctor.Qualification, Role = "doctor", UserName = doctor.ClientProfile.ApplicationUser.UserName };
        }
        public PatientDTO GetPatient(string id)
        {
            Patient patient= Database.Patients.Get(id);
            return new PatientDTO() { Id = patient.Id, Email = patient.ClientProfile.ApplicationUser.Email, Name = patient.ClientProfile.Name, Surname = patient.ClientProfile.Surname, Password = patient.ClientProfile.ApplicationUser.PasswordHash, historyOfTreatment = patient.historyOfTreatment, Role = "patient", UserName = patient.ClientProfile.ApplicationUser.UserName,DoctorId=patient.DoctorId };

        }
    }

    /* public OperationDetails Create(UserDTO userDto)
     {
         ApplicationUser user = Database.UserManager.FindByEmail(userDto.Email);
         if (user == null)
         {
             user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
             var result =  Database.UserManager.Create(user, userDto.Password);
             if (result.Errors.Count() > 0)
                 return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
             // добавляем роль
             Database.UserManager.AddToRole(user.Id, userDto.Role);
             // создаем профиль клиента
             ClientProfile clientProfile = new ClientProfile { Id = user.Id, Name = userDto.Name };
             Database.ClientManager.Create(clientProfile);
             Database.Save();
             return new OperationDetails(true, "Регистрация успешно пройдена", "");
         }
         else
         {
             return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
         }
     }

     public ClaimsIdentity Authenticate(UserDTO userDto)
     {
         ClaimsIdentity claim = null;
         // находим пользователя
         ApplicationUser user = Database.UserManager.Find(userDto.Email, userDto.Password);
         // авторизуем его и возвращаем объект ClaimsIdentity
         if (user != null)
             claim = Database.UserManager.CreateIdentity(user,
                                         DefaultAuthenticationTypes.ApplicationCookie);
         return claim;
     }

     // начальная инициализация бд
     public void SetInitialData(UserDTO adminDto, List<string> roles)
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
         Create(adminDto);
     }

     public void Dispose()
     {
         Database.Dispose();
     }
 }*/
}