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
    public class PatientRepository : IPatientManager
    {
      //  public string DoctorId{ get; set; }
        private ApplicationContext db;
       // private Doctor doctor;
        public PatientRepository(ApplicationContext context)
        {
            this.db = context;
          //  doctor= db.Doctors.Find(DoctorId);
        }
        public void Create(Patient patient)
        {          
            db.Patients.Add(patient);
        }
        public IEnumerable<Patient> GetAll(string doctorId)
        {
            return db.Patients.Where(x=>x.DoctorId==doctorId).ToList();
        }
        public void Delete(string id)
        {

            var item = db.Patients.Find(id);
            if (item != null)
            {
                db.Patients.Remove(item);
            }
        }
        public void Update(Patient item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Patient Get(string id)
        {
          return db.Patients.Find(id);
        }
    }
}