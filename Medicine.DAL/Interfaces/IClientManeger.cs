using  Medicine.DAL.Entities;
using System.Collections.Generic;
using System;
namespace Medicine.DAL.Interfaces
{
    public interface IClientManager : IDisposable 
    {
        void Create(ClientProfile item);
        void Update(ClientProfile item);
        IEnumerable<ClientProfile> GetAll();
        string GetRole(string email);
        string GetId(string email);
    }
}