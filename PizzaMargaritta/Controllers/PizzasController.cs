using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;

namespace PizzaMargaritta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        EFContext _context;
        public PizzasController(EFContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ContentResult GetAll()
        {
            var list = _context.Pizzas.ToList();
            return Content(JsonConvert.SerializeObject(list));
        }
    }
}