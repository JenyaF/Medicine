using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Medicine.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Medicine.DAL.Identity;
using Medicine.DAL.Repositories;
using System;

namespace Medicine.DAL.EF
{
    public class StoreDbInitializer  : DropCreateDatabaseAlways<ApplicationContext>
    {
        ApplicationRoleManager roleManager;
        ApplicationUserManager userManager;
        ApplicationContext db;
        protected override void Seed(ApplicationContext db)
        {
            this.db = db;
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            AddMedicament();
            var roles =new List<string>() { "doctor", "user", "admin", "patient" };
            foreach (var roleName in roles)
            {
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    roleManager.Create(role);
                }
            }
            var user = new ApplicationUser { Email = $"email@ukr.net", UserName = $"email@ukr.net" };
            userManager.Create(user, "qwerty654321");
            userManager.AddToRole(user.Id, "admin");
            var clientProfile = new ClientProfile { Id = user.Id, Name = "name", Surname = "surname", DateOfBirth = "01.02.1970", Role = "admin" };
            db.ClientProfiles.Add(clientProfile);
            db.SaveChanges();
            for ( int i = 1; i < 5; i++)
            {
                AddDoctor(i);
            }           
        }

        void AddDoctor(int i)
        {           
            var user = new ApplicationUser { Email = $"email{i}@ukr.net", UserName = $"email{i}@ukr.net" };
            userManager.Create(user, "qwerty654321");
            userManager.AddToRole(user.Id, "doctor");
            var clientProfile = new ClientProfile { Id = user.Id, Name = $"name{i}",Surname=$"surname{i}" ,DateOfBirth="1980-06-03",Role="doctor"};
            db.ClientProfiles.Add(clientProfile);
            var doctor = new Doctor() { Qualification = "Higth",Id=user.Id };
            db.Doctors.Add(doctor);
            db.SaveChanges();
            for(var j = 0; j < i; j++)
            {
                AddPatient(user.Id,i, j);
            }
        }
        void AddPatient(string doctorId,int i,int j)
        {
            var doctor = db.Doctors.Find(doctorId);
            var user = new ApplicationUser { Email = $"email{i}{j}@ukr.net", UserName = $"email{i}{j}@ukr.net" };
            userManager.Create(user, "qwerty654321");
            userManager.AddToRole(user.Id, "patient");
            var clientProfile = new ClientProfile { Id = user.Id, Name = $"name{i}{j}", Surname = $"surname{i}{j}", DateOfBirth = "1970-01-02", Role = "patient" };
            db.ClientProfiles.Add(clientProfile);
            var patient = new Patient() { DoctorId = doctorId, historyOfTreatment = "some diagnosis", Id = user.Id };
            db.Patients.Add(patient);
            AddRecipes(patient.Id);
            db.SaveChanges();
        }
        void AddMedicament()
        {
            for(int i = 0; i < 10; i++)
            {
                db.Medicaments.Add(new Medicament() { Name = $"medicament{i}" });
            }
            db.SaveChanges();
        }
        void AddRecipes(string patientid)
        {
            for(var i = 7; i > 0; i--)
            {             
            db.Recipes.Add(new Recipe() { Volume = i - 0.1, AmountPerDay = i, FinishDate ="2018-10-01", StartDate = "2018-06-30",MedicamentId=i,PatientId=patientid });
            }
        }
    }   
}
