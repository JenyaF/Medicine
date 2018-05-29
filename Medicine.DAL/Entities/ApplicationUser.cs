using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Medicine.DAL.Entities
{
    public class ApplicationUser : IdentityUser//<int,IdentityUserLogin<int>,IdentityUserRole<int>,IdentityUserClaim<int>>
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
