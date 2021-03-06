﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace Medicine.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
