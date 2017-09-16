using System;
using System.Linq;
using System.Threading.Tasks;
using PackWebApp.Entities;
using PackWebApp.QueryParameters;

namespace PackWebApp.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAll(CustomerQueryParametrs customerQueryParametrs);
        Task<Customer> GetSingleAsync(Guid id);
        void AddAsync(Customer item);
        void DeleteAsync(Guid id);
        void Update(Customer item);
        Task<bool> SaveAsync();
        Task<int> CountAsync();
    }
}