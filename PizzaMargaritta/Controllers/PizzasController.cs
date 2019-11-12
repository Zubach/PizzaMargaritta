using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;
using PizzaMargaritta.Models;

namespace PizzaMargaritta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly EFContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        public PizzasController(EFContext context, IConfiguration configuration, IHostingEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public ContentResult GetAll()
        {
            
            var list = _context.Pizzas.ToList();
            List<PizzaModel> ModelList = new List<PizzaModel>();
            foreach (var item in list)
            {
                ModelList.Add(
                    new PizzaModel()
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Price = item.Price,
                        id = item.ID,
                    }
                    );
            }
            return Content(JsonConvert.SerializeObject(ModelList));
        }

        private List<PizzaModel> FinishFilter(Filter filter,List<PizzaModel> list)
        {
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
                                    if (filter.Values[0] == prop.GetValue(item))
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
                            list.Remove(new PizzaModel() {
                                Name = item.Name,
                                Description = item.Description,
                                Image = item.Image,
                                Price = item.Price
                            });

                        }
                        break;
                    }
                }
            }
            return list;
        }

        [HttpPut("filtered/get")]
        public ContentResult FilteredGet([FromBody] MultyFilter filter)
        {

            
            var pizzas = _context.Pizzas.ToList();
            List<PizzaModel> filteredList = new List<PizzaModel>();
            var firstFilter = filter.Filters.First();
            foreach (var item in pizzas)
            {
                var properties = item.GetType().GetProperties();
                foreach(var prop in properties)
                {
                    if(prop.Name == firstFilter.PropertyName)
                    {
                        if(firstFilter.Conditions[0] == ">" || firstFilter.Conditions[0] == ">=" || firstFilter.Conditions[0] == "<" || firstFilter.Conditions[0] == "<=")
                        {
                            if(firstFilter.Conditions.Count > 1)
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
                            if(firstFilter.Conditions[0] == "==")
                            {
                                if(prop.GetValue(item) is string)
                                {
                                    if (firstFilter.Values[0] == (string)prop.GetValue(item))
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
                                    if(decimal.Parse(firstFilter.Values[0]) == (decimal)prop.GetValue(item))
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
            if(filter.Filters.Count > 1)
            {
                for (int i = 1; i < filter.Filters.Count; i++)
                {
                    filteredList = FinishFilter(filter.Filters[i], filteredList);
                }
            }
            return Content(JsonConvert.SerializeObject(filteredList));
        }

        [HttpPut("edit/{name}/{login}:{password}")]
        public ContentResult Edit([FromBody] PizzaModel model,string name,string login,string password)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (admin != null)
            {
                var pizza = _context.Pizzas.FirstOrDefault(x => x.Name == name);
                if(pizza != null)
                {
                    pizza.Name = model.Name;
                    pizza.Price = model.Price;
                    pizza.Description = model.Description;
                    _context.SaveChanges();
                    return Content("Edited succesfully");
                }
            }
            return Content("BAN");
        }

        [HttpPost("add/{login}:{password}")]
        public ContentResult Add([FromBody] PizzaModel model,string login,string password)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (admin != null)
            {
                string nameOfImage = string.Empty;
                if (model.Image != null)
                {
                    // Шлях до нашої папки з проектом
                    string directory = _env.ContentRootPath;
                    string path = Path.Combine(directory, "Content", _configuration["PizzaImages"]);
                    // directory + "\\Content" + "\\" + _configuration["PizzaImages"];
                    nameOfImage = Path.GetRandomFileName() + ".jpg";
                    string pathToFile = Path.Combine(path, nameOfImage);

                    byte[] imageBytes = Convert.FromBase64String(model.Image);
                    using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        var image = Image.FromStream(ms);
                        image.Save(pathToFile, ImageFormat.Jpeg);
                    }
                }
                Pizza pizza = new Pizza()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    Image = nameOfImage
                };
                _context.Pizzas.Add(pizza);
                _context.SaveChanges();
                return Content("Added succesfully");
            }
            return Content("BAN");
        }

        [HttpDelete("delete/{name}/{login}:{password}")]
        public ContentResult Delete(string name,string login,string password)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Login == login && x.Password == password);
            if(admin != null)
            {
                var pizza = _context.Pizzas.FirstOrDefault(x => x.Name == name);
                
                string directory = _env.ContentRootPath;
                string path = Path.Combine(directory, "Content", _configuration["PizzaImages"]);

                
                string pathToFile = Path.Combine(path, pizza.Image);
                System.IO.File.Delete(pathToFile);

                _context.Pizzas.Remove(pizza);
                _context.SaveChanges();
                return Content("Deleted succesfully");
            }
            return Content("BAN");
        }

    }
}