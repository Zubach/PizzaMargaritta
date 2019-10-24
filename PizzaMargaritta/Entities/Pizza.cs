using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaMargaritta.Entities
{
    [Table("tblPizzas")]
    public class Pizza
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]

        public string Image { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
       
    }
}