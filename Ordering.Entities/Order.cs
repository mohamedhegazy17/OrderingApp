﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Entities
{
    [DataContract]
    public class Order
    {
        [DataMember]

        public int Id { get; set; }
        [DataMember]

        public string Account { get; set; }
        [DataMember]

        public decimal Price { get; set; }
        [DataMember]

        public int Quantity { get; set; }
    }
}
