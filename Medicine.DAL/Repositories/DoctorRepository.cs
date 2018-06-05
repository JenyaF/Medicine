using System.Collections.Generic;
using System.Linq;
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
            db.SaveChanges();
        }

        public IEnumerable<Doctor> GetAll()
        {           
            return db.Doctors.ToList();
        }

        public void Delete(string removedDoctorId,string replasingDoctorId)
        {
            var newDoctor= db.Doctors.Find(replasingDoctorId);
            var item = db.Doctors.Find( removedDoctorId);
            if (item != null&&newDoctor!=null)
            {
                if (item.Patients.Count != 0)
                    foreach (var obj in item.Patients)
                        newDoctor.Patients.Add(obj);
                db.Doctors.Remove(item);
            }
            db.SaveChanges();
        }

        public void Update(Doctor item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Doctor Get(string id)
        {
          return  db.Doctors.Find(id);
        }
    }
}
