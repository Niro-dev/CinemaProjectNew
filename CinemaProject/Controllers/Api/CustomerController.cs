﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using CinemaProject.Dtos;
using CinemaProject.Models;

namespace CinemaProject.Controllers.Api
{
    public class CustomerController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult GetCustomers()
        {
            return Ok(_context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>));
        }

        public IHttpActionResult GetCustomer(string id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerUserId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Customer,CustomerDto>(customer));
        }

        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.CustomerUserId;

            return Created(new Uri(Request.RequestUri + "/" + customer.CustomerUserId), customerDto);
        }

        [HttpPut]
        public IHttpActionResult UpdateCustomer(string id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.CustomerUserId == id);

            if (customerInDb == null)
                return NotFound();
            Mapper.Map(customerDto, customerInDb);
            
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteCustomer(string id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.CustomerUserId == id);

            if (customerInDb == null)
                return NotFound();

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
            return Ok();
        }
    }
}
