﻿using System;
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
        EFContext _context;
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
            return Content(JsonConvert.SerializeObject(list));
        }

        [HttpPost("add")]
        public ContentResult Add([FromBody] PizzaModel model)
        {
            string nameOfImage = string.Empty;
            if (model.Image != null)
            {
                // Шлях до нашої папки з проектом
                string directory = _env.ContentRootPath;
                string path = Path.Combine(directory, "Content", _configuration["ProductImages"]);
                nameOfImage = Path.GetRandomFileName() + ".jpg";
                string pathToFile = Path.Combine(path, nameOfImage);
                
                byte[] imageBytes = Convert.FromBase64String(model.Image);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    var image = Image.FromStream(ms);
                    image.Save(pathToFile, ImageFormat.Jpeg);
                }
            }
            return Content("");
        }

    }
}