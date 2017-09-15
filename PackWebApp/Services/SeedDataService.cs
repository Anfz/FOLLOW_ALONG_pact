using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PackWebApp.Entities;
using SQLitePCL;

namespace PackWebApp.Services
{
    public class SeedDataService
    {
        private readonly PackDbContext _context;

        public SeedDataService(PackDbContext context)
        {
            _context = context; 
        }

        public async Task EnsureSeedData()
        {
            _context.Database.EnsureCreated(); 
            _context.Customers.RemoveRange(_context.Customers);
            _context.SaveChanges();

            Customer customer = new Customer();
            customer.Firstname = "Chris";
            customer.Lastname = "Beaver";
            customer.Age = 30;
            customer.Id = Guid.NewGuid();

            _context.Add(customer);

            await _context.SaveChangesAsync();
        }
    }
}
