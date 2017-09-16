using System;
using System.Linq;
using PackWebApp.Entities;
using PackWebApp.QueryParameters;

namespace PackWebApp.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAll(CustomerQueryParametrs customerQueryParametrs);
        Customer GetSingle(Guid id);
        void Add(Customer item);
        void Delete(Guid id);
        void Update(Customer item);
        bool Save();
        int Count();
    }
}