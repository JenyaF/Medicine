using Ninject.Modules;
using Medicine.DAL.Repositories;
using Medicine.BLL.Services;
using Medicine.BLL.Interfaces;

namespace Medicine.BLL.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IMedicametService>().To<MedicamentService>().WithConstructorArgument("uow", new EFUnitOfWork("DefaultContext"));
            Bind<IRecipeService>().To<RecipeService>().WithConstructorArgument("uow", new EFUnitOfWork("DefaultContext"));
        }
    }
}