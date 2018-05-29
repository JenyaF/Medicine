using Medicine.DAL.EF;
using Medicine.DAL.Entities;
using Medicine.DAL.Interfaces;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Medicine.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Database { get; set; }
        public ClientManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }
        public void Update(ClientProfile item)
        {
            Database.Entry(item).State = EntityState.Modified;
            Database.SaveChanges();
        }
        public IEnumerable<ClientProfile> GetAll()
        {
            return Database.ClientProfiles.ToList();
        }
         public string GetRole(string email)
         {
            return Database.ClientProfiles.FirstOrDefault(x => x.ApplicationUser.Email == email).Role;
         }
        public string GetDoctorId(string email)
        {
            return Database.ClientProfiles.FirstOrDefault(x => x.ApplicationUser.Email == email).Id;
        }
        public void Dispose()
        {
            Database.Dispose();
        }

        /*  public void Delete(string id)
        {
            var item = Database.ClientProfiles.Find(id);
            if (item != null) Database.ClientProfiles.Remove(item);
            Database.SaveChanges();
        }*/
    }
}