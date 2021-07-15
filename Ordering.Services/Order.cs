using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Services
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public decimal Price { get; set; }
        public int Countity { get; set; }
    }
}
