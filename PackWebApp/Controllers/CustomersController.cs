using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository customersRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customersRepository;
            _logger = logger; 
            _logger.LogInformation("customers controller started");
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            _logger.LogInformation("------> GetAllCustomers()");
            var allCustomersDto = from customer in _customerRepository.GetAll().ToList()
                               select Mapper.Map<CustomerDto>(customer);
            return Ok(allCustomersDto);
        }

        [HttpGet]
        [Route("{id}", Name="getSingleCustomer")]
        public IActionResult GetSingleCustomer(Guid id)
        {
            Customer customerFromRepo = _customerRepository.GetSingle(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            return Ok(customerFromRepo);

        }

        [HttpPost]
        public IActionResult AddCustomer([FromBody]CustomerCreateDto customerCreateDto)
        {
            Customer toAdd = Mapper.Map<Customer>(customerCreateDto);

            _customerRepository.Add(toAdd);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return CreatedAtRoute("GetSingleCustomer", 
                                  new {id = toAdd.Id }, 
                                  Mapper.Map<CustomerDto>(toAdd));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto customerUpdateDto)
        {
            var existingCustomer = _customerRepository.GetSingle(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            Mapper.Map(customerUpdateDto, existingCustomer);

            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));

        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult PartiallyUpdate(Guid id, [FromBody]JsonPatchDocument<CustomerUpdateDto> customerPatchDoc)
        {
            if (customerPatchDoc == null)
            {
                return BadRequest(); 
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerToPatch = Mapper.Map<CustomerUpdateDto>(existingCustomer);
            customerPatchDoc.ApplyTo(customerToPatch);

            Mapper.Map(customerToPatch, existingCustomer);
            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return Ok(Mapper.Map<CustomerDto>(existingCustomer));

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(id);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return NoContent();
        }

        
    }
}