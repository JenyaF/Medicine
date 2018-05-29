using Medicine.DAL.Entities;
using System.Collections.Generic;

namespace Medicine.DAL.Interfaces
{
    public interface IDoctorManager 
    {
         IEnumerable<Doctor> GetAll();
        void Create(Doctor item);
        void Update(Doctor item);
        void Delete(string id, string doctorId);
        Doctor Get(string id);
    }
}
