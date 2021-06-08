
using Riverbed.Test.FreshBrew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrewApi.Services
{
    public interface IOrderItem
    {
        IEnumerable<OrderItem> GetAll();
        OrderItem Get(int id);
        OrderItem Add(OrderItem newOrderItem);
        OrderItem Update(OrderItem newOrderItem);
        bool Delete(int id);
    }
}
