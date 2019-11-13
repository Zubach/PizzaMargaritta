using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaMargaritta.Entities;
using PizzaMargaritta.Models;

namespace PizzaMargaritta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketPizzasController : ControllerBase
    {
        private readonly EFContext _context;
        public BasketPizzasController(EFContext context)
        {
            _context = context;
        }

        [HttpGet("get/{userid}")]
        public ContentResult GetAll(int userid)
        {
            List<BasketPizzaModel> bpm = new List<BasketPizzaModel>();
            var models = _context.pizzas_in_basket
                .Include(t=>t.pizza)
                .Where(t => t.User_id == userid)
                .Select(t => new BasketPizzaModel()
                {
                    Count_in = t.Count_in,
                    Description = t.pizza.Description,
                    Image = t.pizza.Image,
                    Name = t.pizza.Name,
                    Price = t.pizza.Price,
                    Pizza_id = t.pizza.ID
                }).ToList();

            if (models.Count > 0)
            {
                return Content(JsonConvert.SerializeObject(models));
            }
            return Content("BAN");
        }
    
        

        [HttpGet("getl/{login}:{password}")]
        public ContentResult getId(string login,string password)
        {
           
            int i = 1;
            foreach (var p in _context.Users)
            {
                if (p.Login == login && p.Password == password)
                {

                    return Content(i.ToString());
                }
                i++;
            }
            return Content("Not Found");
        }

        [HttpPost("post/{user_id}")]
        public ContentResult Add([FromBody]BasketPizzaModel model, int user_id)
        {
            BasketPizza Bp = new BasketPizza()
            {

                User_id = user_id,
                Count_in = model.Count_in,
                Pizza_id = model.Pizza_id,
             
                
               
               
            };
            _context.pizzas_in_basket.Add(Bp);
            _context.SaveChanges();
            return Content("Added");
        }

        [HttpPut("edit/{user_id}")]
        public ContentResult Edit([FromBody]BasketPizzaModel model, int user_id) 
        {


            var result = _context.Pizzas.FirstOrDefault(x => x.Name == model.Name);


            foreach(var p in _context.pizzas_in_basket.ToList())
            {
                if(p.Pizza_id == result.ID && p.User_id == user_id)
                {
                    p.Count_in = model.Count_in;
                    _context.SaveChanges();
                    return Content("Yup");

                }
            }
            
            return Content("Nope");

        }

        [HttpDelete("DEL/{user_id}")]
        public ContentResult Delete([FromBody]BasketPizzaModel model, int user_id)
        {
            BasketPizza bp = null;
            foreach(var p in _context.pizzas_in_basket)
            {
                if(p.User_id == user_id && p.Pizza_id == model.Pizza_id)
                {
                    bp = p;
                    break;
               
                }
            }
            if(bp != null)
            {
                _context.pizzas_in_basket.Remove(bp);
                _context.SaveChanges();
                return Content("succsesfuly");
            }
            return Content("BAN");
        }

    }
}