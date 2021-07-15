using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public override string ToString()
            => $"Order With ID :: {Id} And Account {Account}";
       
    }
}
