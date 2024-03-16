using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Model
{
    internal class Classes
    {
        public class Valute
        {
            public string Name { get; set; }
            public long Value { get; set; }
            public string Type { get; set; }
            public DateTime Date { get; set; }
        }
        public class APIValutes
        {
            public double Value { get; set; }
            public string Valute { get; set; }
        }
    }
}
