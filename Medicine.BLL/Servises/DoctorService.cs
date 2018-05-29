using Medicine.BLL.DTO;
using Medicine.BLL.Infrastructure;
using Medicine.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Medicine.BLL.Interfaces;
using Medicine.DAL.Interfaces;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;

namespace Medicine.BLL.Services
{
    class DoctorService : IDoctorService
    {
        public IUserService userService { get; set; }
        public async Task<OperationDetails> CreateAsync(DoctorDTO doctorDTO)
        {
            OperationDetails operationDetails = userService.CreateAsync(doctorDTO).Result;
            if (operationDetails.Succedeed)
            {
                userService.Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
                await userService.Database.SaveAsync();
            }
            return operationDetails;
        }

        public void Create(DoctorDTO doctorDTO)
        {
            OperationDetails operationDetails = userService.CreateAsync(doctorDTO).Result;
            if (operationDetails.Succedeed)
            {
                userService.Database.Doctors.Create(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
                userService.Database.SaveAsync().GetAwaiter();
            }
        }

        public void Update(DoctorDTO doctorDTO)
        {
            userService.Database.Doctors.Update(new Doctor() { Id = doctorDTO.Id, Qualification = doctorDTO.Qualification });
        }

       public void Delete(string id, string doctorId)
        {
            userService.Database.Doctors.Delete(id, doctorId);
        }

        public IEnumerable<DoctorDTO> GetAll()
        {
             return userService.Database.Doctors.GetAll().
                Join(userService.Database.ClientManager.GetAll(),
                        x => x.Id,
                        y => y.Id,
                        (x, y) => new DoctorDTO() { Id = x.Id, Name = y.Name, Surname = y.Surname, Qualification = x.Qualification }).ToList();          
        }

        
    }
}