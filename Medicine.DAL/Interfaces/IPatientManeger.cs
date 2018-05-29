using Medicine.DAL.Entities;
using System.Collections.Generic;

namespace Medicine.DAL.Interfaces
{
    public interface IPatientManager 
    {
       // string DoctorId { get; set; }
        IEnumerable<Patient> GetAll(string DoctorId);
        void Create(Patient item);
        void Update(Patient item);
        void Delete(string id);
        Patient Get(string id);
    }
}