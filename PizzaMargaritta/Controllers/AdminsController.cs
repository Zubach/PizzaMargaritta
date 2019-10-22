using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;
using PizzaMargaritta.Models;

namespace PizzaMargaritta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {

        private readonly EFContext _context;
        public AdminsController(EFContext context)
        {
            _context = context;
        }

        [HttpGet("{login}:{password}")]
        public ContentResult Login(string login,string password)
        {
            var admin = _context.Admins.FirstOrDefault(x=> x.Login == login && x.Password == password);
            if (admin != null)
            {
                AdminModel model = new AdminModel()
                {
                    Login = admin.Login,
                    Password = admin.Password
                };
                return Content(JsonConvert.SerializeObject(model));
            }
            return Content("BAN");
        }
    }
}