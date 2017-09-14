using System;
using System.Linq;
using PackWebApp.Entities;

namespace PackWebApp.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAll();
        Customer GetSingle(Guid id);
        void Add(Customer item);
        void Delete(Guid id);
        void Update(Customer item);
        bool Save();
    }
}