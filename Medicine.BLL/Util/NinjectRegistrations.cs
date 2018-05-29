using Ninject.Modules;
using Medicine.DAL.Interfaces;
using Medicine.DAL.Repositories;
using Medicine.BLL.Services;
using Medicine.BLL.Interfaces;

namespace Medicine.BLL.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {

            // Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument();
            //  Bind<IDoctorService>().To<DoctorService>();
            // Bind<IPatientService>().To<PatientService>();
            Bind<IMedicametService>().To<MedicamentService>().WithConstructorArgument("uow", new EFUnitOfWork("DefaultContext"));
            Bind<IRecipeService>().To<RecipeService>().WithConstructorArgument("uow", new EFUnitOfWork("DefaultContext"));
        }

    }

}