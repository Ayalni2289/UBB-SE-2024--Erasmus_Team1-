using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class TicketType
    {
        public string Type { get; set; }
        public decimal Price { get; set; }

        public TicketType(string type, decimal price)
        {
            Type = type;
            Price = price;
        }

        public string DisplayInfo
        {
            get
            {
                return $"{Type} - ${Price}";
            }
        }
    }
}
