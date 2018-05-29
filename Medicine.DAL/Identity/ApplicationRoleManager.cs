using Medicine.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Medicine.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>//,int>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole/*,int,IdentityUserRole<int>*/> store):
            base(store)                   
        { }
    }
}
