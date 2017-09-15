using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackWebApp.Dtos;
using PackWebApp.Entities;
using PackWebApp.Repositories;

namespace PackWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customersRepository)
        {
            _customerRepository = customersRepository;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var allCustomersDto = from customer in _customerRepository.GetAll().ToList()
                               select Mapper.Map<CustomerDto>(customer);
            return Ok(allCustomersDto);
        }

        
    }
}