using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;

namespace PizzaMargaritta.Controllers
{
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
    }
}