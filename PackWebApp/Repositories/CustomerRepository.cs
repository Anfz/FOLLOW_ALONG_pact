using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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

        public Customer GetSingle(Guid id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Customer item)
        {
            _context.Customers.Add(item); 
        }

        public void Delete(Guid id)
        {
            Customer customer = GetSingle(id);
            _context.Customers.Remove(customer);
        }

        public void Update(Customer item)
        {
            _context.Customers.Update(item);
        }

        public int Count()
        {
            return _context.Customers.Count();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0; 
        }
    }
}
