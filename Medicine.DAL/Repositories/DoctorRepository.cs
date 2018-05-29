using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.DAL.Interfaces;
using Medicine.DAL.Entities;
using Medicine.DAL.EF;
using System.Data.Entity;

namespace Medicine.DAL.Repositories
{
   public class DoctorRepository :IDoctorManager
    {
        private ApplicationContext db;
        public DoctorRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public void Create(Doctor doctor)
        {
            db.Doctors.Add(doctor);
        }

        public IEnumerable<Doctor> GetAll()
        {           
            return db.Doctors.ToList();
        }

        public void Delete(string id,string doctorId)
        {
            var newDoctor= db.Doctors.Find(doctorId);
            var item = db.Doctors.Find(id);
            if (item != null&&newDoctor!=null)
            {
                if (item.Patients.Count != 0)
                    foreach (var obj in item.Patients)
                        newDoctor.Patients.Add(obj);
                db.Doctors.Remove(item);
            }
        }

        public void Update(Doctor item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Doctor Get(string id)
        {
          return  db.Doctors.Find(id);
        }
    }
}
