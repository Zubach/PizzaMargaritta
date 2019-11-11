using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Entities
{
    [Table("BasketPizza")]
    public class BasketPizza
    {
        [Key]
        public int id { get; set; }
        

        [ForeignKey("user")]
        public int User_id { get; set; }

        [ForeignKey("pizza")]
        public int Pizza_id { get; set; }
        [Required]
        public int Count_in { get; set; }

        public virtual User user { get; set; }
        public virtual Pizza pizza { get; set; }
    }
}
