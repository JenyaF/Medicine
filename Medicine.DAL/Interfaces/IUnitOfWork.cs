using Medicine.DAL.Identity;
using System;
using System.Threading.Tasks;
using Medicine.DAL.Entities;
namespace Medicine.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IDoctorManager Doctors { get; }
        IPatientManager Patients { get; }
        IRecipeManeger Recipes { get; }
        IMedicamentManeger Medicaments { get; }
       // Task SaveAsync();
        void Save();
    }
}
