using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Models
{
    public class PizzaModel
    {
        
        public int id { get; set; }
        public string Name { get; set; }

        
        public decimal Price { get; set; }

        
        public string Description { get; set; }


        public string Image { get; set; }

        public override bool Equals(object obj)
        {
            var model = obj as PizzaModel;
            return model != null &&
                   Name == model.Name &&
                   Price == model.Price &&
                   Description == model.Description &&
                   Image == model.Image;
        }
    }
}
