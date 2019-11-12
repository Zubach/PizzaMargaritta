using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta
{
    public class Filter
    {
        public string PropertyName { get; set; }

        public List<string> Conditions { get; set; }

        public List<string> Values { get; set; }
    }
}
