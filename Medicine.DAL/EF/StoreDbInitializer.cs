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
    public class StoreDbInitializer  : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        ApplicationRoleManager roleManager;
        ApplicationUserManager userManager;
        ClientManager clientManager;
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
            /*  
                 db.ClientProfiles.AddRange(new List<ClientProfile>(){new ClientProfile() { Name = "Jon",Surname="Swith",DateOfBirth="01.02.1998",ApplicationUser=new ApplicationUser(){Email="ssa@ukr.net" } },
                                                                      new ClientProfile() { Name = "Jey",Surname="Swith",DateOfBirth="07.07.1990",ApplicationUser=new ApplicationUser(){Email="sa@ukr.net" } },
                                                                      new ClientProfile() { Name = "Jim",Surname="Broun",DateOfBirth="09.02.1978",ApplicationUser=new ApplicationUser(){Email="ssag@ukr.net" } },
                                                                      new ClientProfile() { Name = "Bill",Surname="Broun",DateOfBirth="09.02.1978",ApplicationUser=new ApplicationUser(){Email="ssag1@ukr.net" } },
                                                                      new ClientProfile() { Name = "Li",Surname="Broun",DateOfBirth="09.02.1978",ApplicationUser=new ApplicationUser(){Email="ssag2@ukr.net" } },
                                                                      new ClientProfile() { Name = "Jek",Surname="Broun",DateOfBirth="09.02.1978",ApplicationUser=new ApplicationUser(){Email="ssag3@ukr.net" } }
                 });
                 db.Doctors.AddRange(new List<Doctor>() { new Doctor() { Id=1,Qualification="Higth"},
                                                          new Doctor() { Id=2,Qualification="Low"}
                 });
                 db.Patients.AddRange(new List<Patient>() { new Patient() { Id = "3", DoctorId = "1",historyOfTreatment="Bad diagnosis" },
                                                            new Patient() { Id = "4", DoctorId = 1, historyOfTreatment="Bad diagnosis" },
                                                            new Patient() { Id = 5, DoctorId = 1, historyOfTreatment="Alive" },
                                                            new Patient() { Id = 6, DoctorId = 2, historyOfTreatment="Bad diagnosis" },

                 });
                 db.Patients.AddRange(new List<Patient>() { new Patient() { Id = 3, DoctorId = 1,historyOfTreatment="Bad diagnosis" },
                                                            new Patient() { Id = 4, DoctorId = 1, historyOfTreatment="Bad diagnosis" },
                                                            new Patient() { Id = 5, DoctorId = 1, historyOfTreatment="Alive" },
                                                            new Patient() { Id = 6, DoctorId = 2, historyOfTreatment="Bad diagnosis" },

                 });

                 db.Recipes.AddRange(new List<Recipe>() { new Recipe() { Id = 3, AmountPerDay = 2, MedicamentId = 1, Volume = 1 },
                                                          new Recipe() { Id = 3, AmountPerDay = 5, MedicamentId = 2, Volume = 1 },
                                                          new Recipe() { Id = 3, AmountPerDay = 3, MedicamentId = 3, Volume = 1 },
                                                          new Recipe() { Id = 3, AmountPerDay = 2, MedicamentId = 4, Volume = 2 },
                                                          new Recipe() { Id = 3, AmountPerDay = 1, MedicamentId = 5, Volume = 1 },
                 });

              db.Medicaments.AddRange(new List<Medicament>(){ new Medicament() { Name="med1"},
                                                                 new Medicament() { Name="med2"},
                                                                 new Medicament() { Name="med3"},
                                                                 new Medicament() { Name="med4"},
                                                                 new Medicament() { Name="med5"}
                 });
              db.SaveChanges();*/
        }
        void AddDoctor(int i)
        {           
            var user = new ApplicationUser { Email = $"email{i}@ukr.net", UserName = $"email{i}@ukr.net" };
            userManager.Create(user, "qwerty654321");
            userManager.AddToRole(user.Id, "doctor");
            var clientProfile = new ClientProfile { Id = user.Id, Name = $"name{i}",Surname=$"surname{i}" ,DateOfBirth="01.02.1970",Role="doctor"};
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
            var clientProfile = new ClientProfile { Id = user.Id, Name = $"name{i}{j}", Surname = $"surname{i}{j}", DateOfBirth = "01.02.1970", Role = "patient" };
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
                db.Recipes.Add(new Recipe() { Volume = i - 0.1, AmountPerDay = i, FinishDate = DateTime.Now.AddMonths(i).ToShortDateString(), StartDate = DateTime.Now.ToShortDateString(),MedicamentId=i,PatientId=patientid });
            }
        }
    }   
}
