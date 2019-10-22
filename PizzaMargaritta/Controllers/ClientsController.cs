using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;
using PizzaMargaritta.Models;

namespace PizzaMargaritta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly EFContext _context;

        public ClientsController(EFContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPut("{userlogin}/{login}:{password}")]
        public ContentResult Ban( string userlogin, string login, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == userlogin);
            if(user != null)
            {
                user.IsBanned = true;
                return Content("Banned succesfully");
            }
            return Content("BAN");
        }

        [HttpGet("{login}:{password}")]
        public ContentResult Get(string login,string password)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
            if(admin != null)
            {
                List<UserForAdminModel> users = new List<UserForAdminModel>();
                foreach (var item in _context.Users.ToList())
                {
                    users.Add(
                        new UserForAdminModel()
                        {
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Login = item.Login,
                            Number = item.Number,
                            IsBanned = item.IsBanned
                        });
                }

                return Content(JsonConvert.SerializeObject(users));
            }
            return Content("Ti ne admin");
        }

        [HttpGet("{login}:{password}")]
        public ContentResult Login(string login,string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            UserModel userModel;
            if (user != null)
                
            {
                if (!user.IsBanned)
                {
                    userModel = new UserModel() { FirstName = user.FirstName, LastName = user.LastName, Login = user.Login, Password = user.Password, Number = user.Number };

                    return Content(JsonConvert.SerializeObject(userModel));
                }
                else
                {
                    return Content("BAN");
                }
            }
            return Content("Login failed");
 


        }

        [HttpPost("add")]
        public ContentResult AddUser([FromBody] UserModel user)
        {
            if(user != null)
            {
                Entities.User CreateUser = new User() { Login = user.Login, Password=user.Password, FirstName = user.FirstName, LastName = user.LastName, Number = user.Number,IsBanned =false};
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

        
    }
}