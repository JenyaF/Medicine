using Medicine.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Medicine.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>//,int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser/*,int*/> store)
                : base(store)
        {
        }
    }
}
