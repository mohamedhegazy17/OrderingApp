using Ordering.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Service
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        void AddOrder(Order order);
    }
}
