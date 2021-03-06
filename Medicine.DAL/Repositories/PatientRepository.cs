﻿using System.Collections.Generic;
using System.Linq;
using Medicine.DAL.Interfaces;
using Medicine.DAL.Entities;
using Medicine.DAL.EF;
using System.Data.Entity;

namespace Medicine.DAL.Repositories
{
    public class PatientRepository : IPatientManager
    {
        private ApplicationContext db;
        public PatientRepository(ApplicationContext context)
        {
            this.db = context;
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
                db.Recipes.RemoveRange(item.Recipes);
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