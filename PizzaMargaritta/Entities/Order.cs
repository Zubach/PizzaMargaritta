using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaMargaritta.Entities
{
    [Table("tblOrders")]
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Address { get; set; }

        [ForeignKey("Pizza")]
        public int PizzaID { get; set; }

        public Pizza Pizza { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}