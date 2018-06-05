using Medicine.DAL.EF;
using Medicine.DAL.Entities;
using Medicine.DAL.Interfaces;
using System.Data.Entity;
using System.Collections.Generic;
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

        public string GetId(string email)
        {
            return Database.ClientProfiles.FirstOrDefault(x => x.ApplicationUser.Email == email).Id;
        }
        public void Dispose()
        {
            Database.Dispose();
        }      
    }
}