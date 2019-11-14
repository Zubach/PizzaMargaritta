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

        [HttpPut("edit/{userlogin}/{login}:{password}")]
        public ContentResult Put([FromBody] UserForAdminModel model,string userlogin,string login,string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == userlogin);
            if(user != null)
            {
                var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
                if (admin != null)
                {
                    user.Login = model.Login;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Number = model.Number;
                    user.IsBanned = model.IsBanned;
                    _context.SaveChanges();
                    return Content("Edited succesfully");
                }
                return Content("BAN");
            }
            return Content("BAN");
        }

        private List<UserForAdminModel> FinishFilter(Filter filter, List<UserForAdminModel> list)
        {
            List<UserForAdminModel> result = list.GetRange(0, list.Count);
            foreach (var item in list)
            {
                var properties = item.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.Name == filter.PropertyName)
                    {
                        bool IsTrue = false;
                        if (filter.Conditions[0] == ">" || filter.Conditions[0] == ">=" || filter.Conditions[0] == "<" || filter.Conditions[0] == "<=")
                        {
                            if (filter.Conditions.Count > 1)
                            {

                                switch (filter.Conditions[0])
                                {
                                    case ">":
                                        switch (filter.Conditions[1])
                                        {
                                            case "<":
                                                if ((decimal)prop.GetValue(item) > decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) < decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                            case "<=":
                                                if ((decimal)prop.GetValue(item) > decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) <= decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                        }

                                        break;
                                    case ">=":
                                        switch (filter.Conditions[1])
                                        {
                                            case "<":
                                                if ((decimal)prop.GetValue(item) >= decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) < decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                            case "<=":
                                                if ((decimal)prop.GetValue(item) >= decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) <= decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                        }
                                        break;
                                    case "<":
                                        switch (filter.Conditions[1])
                                        {
                                            case ">":
                                                if ((decimal)prop.GetValue(item) < decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) > decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                            case ">=":
                                                if ((decimal)prop.GetValue(item) < decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) >= decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                        }
                                        break;
                                    case "<=":
                                        switch (filter.Conditions[1])
                                        {
                                            case ">":
                                                if ((decimal)prop.GetValue(item) <= decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) > decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                            case ">=":
                                                if ((decimal)prop.GetValue(item) <= decimal.Parse(filter.Values[0]) && (decimal)prop.GetValue(item) >= decimal.Parse(filter.Values[1]))
                                                    IsTrue = true;
                                                break;
                                        }
                                        break;

                                }

                            }
                            else
                            {
                                switch (filter.Conditions[0])
                                {
                                    case ">":
                                        if ((decimal)prop.GetValue(item) > decimal.Parse(filter.Values[0]))
                                            IsTrue = true;
                                        break;
                                    case ">=":
                                        if ((decimal)prop.GetValue(item) >= decimal.Parse(filter.Values[0]))
                                            IsTrue = true;
                                        break;
                                    case "<":
                                        if ((decimal)prop.GetValue(item) < decimal.Parse(filter.Values[0]))
                                            IsTrue = true;
                                        break;
                                    case "<=":
                                        if ((decimal)prop.GetValue(item) <= decimal.Parse(filter.Values[0]))
                                            IsTrue = true;
                                        break;

                                }
                            }

                        }
                        else
                        {
                            if (filter.Conditions[0] == "==")
                            {
                                if (prop.GetValue(item) is string)
                                {
                                    if (((string)prop.GetValue(item)).Contains(filter.Values[0]))
                                        IsTrue = true;
                                }
                                else
                                {
                                    if (decimal.Parse(filter.Values[0]) == (decimal)prop.GetValue(item))
                                        IsTrue = true;

                                }
                            }
                        }
                        if (!IsTrue)
                        {
                            result.Remove(new UserForAdminModel()
                            {
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                IsBanned = item.IsBanned,
                                Login = item.Login,
                                Number = item.Number
                            });

                        }
                        break;
                    }
                }
            }
            return result;
        }

        [HttpPut("filtered/get/{login}:{password}")]
        public ContentResult FilteredGet(string login,string password)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
            if(admin != null)
            {
                var pizzas = _context.Pizzas.ToList();
                List<PizzaModel> filteredList = new List<PizzaModel>();
                var firstFilter = filter.Filters.First();
                foreach (var item in pizzas)
                {
                    var properties = item.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        if (prop.Name == firstFilter.PropertyName)
                        {
                            if (firstFilter.Conditions[0] == ">" || firstFilter.Conditions[0] == ">=" || firstFilter.Conditions[0] == "<" || firstFilter.Conditions[0] == "<=")
                            {
                                if (firstFilter.Conditions.Count > 1)
                                {

                                    switch (firstFilter.Conditions[0])
                                    {
                                        case ">":
                                            switch (firstFilter.Conditions[1])
                                            {
                                                case "<":
                                                    if ((decimal)prop.GetValue(item) > decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) < decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                                case "<=":
                                                    if ((decimal)prop.GetValue(item) > decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) <= decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                            }

                                            break;
                                        case ">=":
                                            switch (firstFilter.Conditions[1])
                                            {
                                                case "<":
                                                    if ((decimal)prop.GetValue(item) >= decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) < decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                                case "<=":
                                                    if ((decimal)prop.GetValue(item) >= decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) <= decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                            }
                                            break;
                                        case "<":
                                            switch (firstFilter.Conditions[1])
                                            {
                                                case ">":
                                                    if ((decimal)prop.GetValue(item) < decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) > decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                                case ">=":
                                                    if ((decimal)prop.GetValue(item) < decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) >= decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                            }
                                            break;
                                        case "<=":
                                            switch (firstFilter.Conditions[1])
                                            {
                                                case ">":
                                                    if ((decimal)prop.GetValue(item) <= decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) > decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                                case ">=":
                                                    if ((decimal)prop.GetValue(item) <= decimal.Parse(firstFilter.Values[0]) && (decimal)prop.GetValue(item) >= decimal.Parse(firstFilter.Values[1]))
                                                        filteredList.Add(new PizzaModel()
                                                        {
                                                            Name = item.Name,
                                                            Description = item.Description,
                                                            Image = item.Image,
                                                            Price = item.Price

                                                        });
                                                    break;
                                            }
                                            break;

                                    }

                                }
                                else
                                {
                                    switch (firstFilter.Conditions[0])
                                    {
                                        case ">":
                                            if ((decimal)prop.GetValue(item) > decimal.Parse(firstFilter.Values[0]))
                                                filteredList.Add(new PizzaModel()
                                                {
                                                    Name = item.Name,
                                                    Description = item.Description,
                                                    Image = item.Image,
                                                    Price = item.Price

                                                });
                                            break;
                                        case ">=":
                                            if ((decimal)prop.GetValue(item) >= decimal.Parse(firstFilter.Values[0]))
                                                filteredList.Add(new PizzaModel()
                                                {
                                                    Name = item.Name,
                                                    Description = item.Description,
                                                    Image = item.Image,
                                                    Price = item.Price

                                                });
                                            break;
                                        case "<":
                                            if ((decimal)prop.GetValue(item) < decimal.Parse(firstFilter.Values[0]))
                                                filteredList.Add(new PizzaModel()
                                                {
                                                    Name = item.Name,
                                                    Description = item.Description,
                                                    Image = item.Image,
                                                    Price = item.Price

                                                });
                                            break;
                                        case "<=":
                                            if ((decimal)prop.GetValue(item) <= decimal.Parse(firstFilter.Values[0]))
                                                filteredList.Add(new PizzaModel()
                                                {
                                                    Name = item.Name,
                                                    Description = item.Description,
                                                    Image = item.Image,
                                                    Price = item.Price

                                                });
                                            break;

                                    }
                                }

                            }
                            else
                            {
                                if (firstFilter.Conditions[0] == "==")
                                {
                                    if (prop.GetValue(item) is string)
                                    {
                                        if (((string)prop.GetValue(item)).Contains(firstFilter.Values[0]))
                                            filteredList.Add(new PizzaModel()
                                            {
                                                Name = item.Name,
                                                Description = item.Description,
                                                Image = item.Image,
                                                Price = item.Price
                                            });
                                    }
                                    else
                                    {
                                        if (decimal.Parse(firstFilter.Values[0]) == (decimal)prop.GetValue(item))
                                            filteredList.Add(new PizzaModel()
                                            {
                                                Name = item.Name,
                                                Description = item.Description,
                                                Image = item.Image,
                                                Price = item.Price
                                            });

                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                if (filter.Filters.Count > 1)
                {
                    for (int i = 1; i < filter.Filters.Count; i++)
                    {
                        filteredList = FinishFilter(filter.Filters[i], filteredList);
                    }
                }
            }
            return Content("BAN");
        }

        [HttpPut("{userlogin}/ban/{login}:{password}")]
        public ContentResult Ban( string userlogin, string login, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == userlogin);
            if(user != null)
            {
                var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
                if (admin != null)
                {
                    user.IsBanned = true;
                    _context.SaveChanges();
                    return Content("Banned succesfully");
                }
                return Content("BAN");
            }
            return Content("BAN");
        }

        [HttpGet("get/{login}:{password}")]
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


        [HttpGet("login/{login}:{password}")]
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