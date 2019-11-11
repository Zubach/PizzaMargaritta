using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Models
{

    public class BasketPizzaModel
    {
  
        public string Name { get; set; }
   
        public decimal Price { get; set; }
      
        public string Description { get; set; }
       
        public string Image { get; set; }

        public int Count_in { get; set; }

        public int Pizza_id { get; set; }


    }
}
