﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;
using PizzaMargaritta.Models;

namespace PizzaMargaritta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly EFContext _context;

        public OrdersController(EFContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("get")]
        public ContentResult GetOrders()
        {
           var list = _context.Orders.ToList();
            return Content(JsonConvert.SerializeObject(list));
        }

        [HttpGet("getL/{login}:{password}")]
        public ContentResult GetOrdersUser(string login, string password)
        {
            var list = _context.Orders.Where(x => x.User.Login == login && x.User.Password == password);
            return Content(JsonConvert.SerializeObject(list.ToList()));
        }


        [HttpPost("add/{login}:{password}")]
        public ContentResult AddOrder([FromBody] OrderModel model,string login,string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            if(user != null && !user.IsBanned)
            {
                _context.Orders.Add(
                    new Order()
                    {
                        Address = model.Address,
                        UserID = model.UserID,
                        PizzaID = model.PizzaID
                    }
                    );
                _context.SaveChanges();
                return Content("Your order added succesfully");
            }
            return Content("BAN");
        }

    }
}