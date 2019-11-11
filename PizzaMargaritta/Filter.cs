using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta
{
    public class Filter
    {
        public string PropertyNames { get; set; }

        public string[] Conditions { get; set; }

        public string[] Values { get; set; }
    }
}
