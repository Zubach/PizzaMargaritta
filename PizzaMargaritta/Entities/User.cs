using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Entities
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]

        public string Number { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            return user != null &&
                   Login == user.Login || Number == user.Number;
        }
    }
}
