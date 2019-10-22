using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Models
{
    public class UserModel
    {

        public string Login { get; set; }



        public bool IsBanned { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Number { get; set; }
    }
}
