using Ordering.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Data
{
    public class OrderDbContext:DbContext
    {
        //public OrderDbContext() : base("OrderingSystem") { }
        public DbSet<Order> Orders { get; set; }
    }
}
