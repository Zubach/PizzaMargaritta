using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaMargarittaUI.Models
{
    public class OrderModel
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public int PizzaID { get; set; }
        public int UserID { get; set; }

    }
}
