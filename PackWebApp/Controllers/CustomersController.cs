using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PackWebApp.Dtos;
using PackWebApp.Entities;
using PackWebApp.QueryParameters;
using PackWebApp.Repositories;

namespace PackWebApp.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
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
        [ProducesResponseType(typeof(List<Customer>), 200)]
        public IActionResult GetAllCustomers(CustomerQueryParametrs customerQueryParametrs)
        {
            _logger.LogInformation("------> GetAllCustomers()");

            var allCustomersDto = from customer in _customerRepository.GetAll(customerQueryParametrs).ToList()
                               select Mapper.Map<CustomerDto>(customer);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new {totalCount = _customerRepository.CountAsync().Result}));
            return Ok(allCustomersDto);
        }

        [HttpGet]
        [Route("{id}", Name="getSingleCustomer")]
        public IActionResult GetSingleCustomer(Guid id)
        {
            Customer customerFromRepo = _customerRepository.GetSingleAsync(id).Result;

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            return Ok(customerFromRepo);

        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(typeof(CustomerDto), 400)]
        public IActionResult AddCustomer([FromBody]CustomerCreateDto customerCreateDto)
        {

            if (customerCreateDto == null)
            {
                return BadRequest("Please supplier customer details");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer toAdd = Mapper.Map<Customer>(customerCreateDto);

            _customerRepository.AddAsync(toAdd);

            bool result = _customerRepository.SaveAsync().Result;

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

            if (customerUpdateDto == null)
            {
                return BadRequest(); 
            }

            var existingCustomer = _customerRepository.GetSingleAsync(id).Result;
            if (existingCustomer == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Mapper.Map(customerUpdateDto, existingCustomer);


            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.SaveAsync().Result;

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

            var existingCustomer = _customerRepository.GetSingleAsync(id).Result;

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerToPatch = Mapper.Map<CustomerUpdateDto>(existingCustomer);
            customerPatchDoc.ApplyTo(customerToPatch, ModelState);

            TryValidateModel(customerToPatch);

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            Mapper.Map(customerToPatch, existingCustomer);
            _customerRepository.Update(existingCustomer);

            bool result = _customerRepository.SaveAsync().Result;

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
            var existingCustomer = _customerRepository.GetSingleAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            _customerRepository.DeleteAsync(id);

            bool result = _customerRepository.SaveAsync().Result;

            if (!result)
            {
                return new StatusCodeResult(500);
            }

            return NoContent();
        }

        
    }
}