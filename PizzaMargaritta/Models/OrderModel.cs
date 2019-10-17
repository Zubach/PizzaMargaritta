using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Models
{
    public class OrderModel
    {
        public string Address { get; set; }
        public int PizzaID { get; set; }
        public int UserID { get; set; }

    }
}
