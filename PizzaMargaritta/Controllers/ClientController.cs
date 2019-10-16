using System;
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
    public class ClientController : Controller
    {
        private readonly EFContext _context;

        public ClientController(EFContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        

        [HttpPost("add")]
        public ContentResult AddUser([FromBody] UserModel user)
        {
            if(user != null)
            {
                Entities.User CreateUser = new User() { FirstName = user.FirstName, LastName = user.LastName, Number = user.Number };
                var findedUser= _context.Users.Contains(CreateUser);

                if(!findedUser)
                {
                    _context.Users.Add(CreateUser);
                    _context.SaveChanges();
                    return Content("User added");
                }
                else
                {
                    return Content("Phone number is busy");
                }
            }
            return Content("Kozak poplakav");
        }

        [HttpGet("users")]
        public ContentResult GetUsers()
        {
            var list = _context.Users.ToList();
            return Content(JsonConvert.SerializeObject(list));
        }
    }
}