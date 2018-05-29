using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.DTO;
using Medicine.BLL.Interfaces;
using Medicine.BLL.Infrastructure;
using Medicine.DAL.Entities;

namespace Medicine.BLL.Services
{
    public class PatientService//:IPatientService
    {
       /* public IUserService userService { get; set; }
        public string DoctorId { get; set; }

        public IEnumerable<PatientDTO> GetAll()
        {

            return userService.Database.Patients.GetAll().
                Join(userService.Database.ClientManager.GetAll(),
                        x => x.Id,
                        y => y.Id,
                        (x, y) => new PatientDTO() { Id = x.Id, Name = y.Name, Surname = y.Surname, historyOfTreatment=x.historyOfTreatment}).
                 ToList();
        }
        public void Create(PatientDTO item)
        {
            OperationDetails operationDetails = userService.CreateAsync(item).Result;
            if (operationDetails.Succedeed)
            {
                userService.Database.Patients.DoctorId = this.DoctorId;
                userService.Database.Patients.Create(new Patient() { Id = item.Id, historyOfTreatment=item.historyOfTreatment,DoctorId=this.DoctorId});
                userService.Database.SaveAsync().GetAwaiter();
            }
        }
        public  void Update(PatientDTO item)
        {
            userService.Database.Patients.Update(new Patient() { Id = item.Id,historyOfTreatment=item.historyOfTreatment,DoctorId=item.DoctorId});
        }
        public  void Delete(string id)
        {
            userService.Database.Patients.Delete(id);
        }*/
    }
}
