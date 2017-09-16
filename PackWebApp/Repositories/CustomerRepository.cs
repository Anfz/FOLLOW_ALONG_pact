using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackWebApp.Entities;
using PackWebApp.QueryParameters;

namespace PackWebApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private PackDbContext _context; 
        public CustomerRepository(PackDbContext context)
        {
            _context = context; 

        }

        public IQueryable<Customer> GetAll(CustomerQueryParametrs customerQueryParametrs)
        {
            IQueryable<Customer> allCustomers = _context.Customers.OrderBy(c => c.Firstname);

            if (customerQueryParametrs.HasQuery)
            {
                allCustomers = allCustomers.Where(c =>
                    (String.Equals(c.Firstname, customerQueryParametrs.Query, StringComparison.InvariantCultureIgnoreCase))
                    || (String.Equals(c.Firstname, customerQueryParametrs.Query, StringComparison.InvariantCultureIgnoreCase)));
            }

            return allCustomers
                .Skip(customerQueryParametrs.PageCount * (customerQueryParametrs.Page - 1))
                .Take(customerQueryParametrs.PageCount);
        }

        public async Task<Customer> GetSingleAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async void AddAsync(Customer item)
        {
            await _context.Customers.AddAsync(item); 
        }

        public async void DeleteAsync(Guid id)
        {
            Customer customer = await GetSingleAsync(id);
            _context.Customers.Remove(customer);
        }

        public void Update(Customer item)
        {
            _context.Customers.Update(item);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0; 
        }
    }
}
