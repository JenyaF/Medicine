using Medicine.DAL.EF;
using Medicine.DAL.Entities;
using Medicine.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Medicine.DAL.Identity;

namespace Medicine.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;
        private DoctorRepository doctorRepository;
        private PatientRepository patientRepository;
        private MedicamentRepository medicamentRepository;
        private RecipeRepository recipeRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }

        public ApplicationUserManager UserManager  {  get => userManager?? new ApplicationUserManager(new UserStore<ApplicationUser>(db));  }        
        public IClientManager ClientManager { get => clientManager ?? new ClientManager(db); }
        public ApplicationRoleManager RoleManager { get => roleManager ?? new ApplicationRoleManager(new RoleStore<ApplicationRole>(db)); }

        public IDoctorManager Doctors { get => doctorRepository ?? new DoctorRepository(db); }
        public IPatientManager Patients { get => patientRepository ?? new PatientRepository(db); }
        public IMedicamentManeger Medicaments { get => medicamentRepository ?? new MedicamentRepository(db); }
        public IRecipeManeger Recipes { get => recipeRepository ?? new RecipeRepository(db); }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
