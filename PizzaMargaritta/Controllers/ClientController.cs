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


        [HttpGet("{login}:{password}")]
        public ContentResult Login(string login,string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            UserModel userModel;
            if (user != null)
            {
                userModel = new UserModel() { FirstName = user.FirstName, LastName = user.LastName, Login = user.Login, Password = user.Password, Number = user.Number };
                return Content(JsonConvert.SerializeObject(userModel));
            }
            return Content("Login failed");



        }
        [HttpPost("add")]
        public ContentResult AddUser([FromBody] UserModel user)
        {
            if(user != null)
            {
                Entities.User CreateUser = new User() { Login = user.Login, Password=user.Password, FirstName = user.FirstName, LastName = user.LastName, Number = user.Number };
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