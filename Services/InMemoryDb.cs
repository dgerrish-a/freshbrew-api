using Riverbed.Test.FreshBrew.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrewApi.Services
{
    public class InMemoryDb: IOrderItem
    {
        List<OrderItem> orderList;
        public InMemoryDb()
        {
            orderList = new List<OrderItem>
            {
                new OrderItem{Id=0,Name="Iced Espresso Latte",IsReady=false, DelayByInSeconds=0},
                new OrderItem{Id=1,Name="Night Owl Brew",IsReady=false,DelayByInSeconds=0},
                new OrderItem{Id=2,Name="Caramel Iced Coffee",IsReady=false,DelayByInSeconds=0}
            };
        }

        public OrderItem Add(OrderItem newOrderItem)
        {
            Log.Information("InMemoryDb:Add");
            newOrderItem.Id = orderList.Max(r => r.Id) + 1;
            orderList.Add(newOrderItem);
            if (newOrderItem.DelayByInSeconds > 0)
            {
                var milliSec = 1000 * newOrderItem.DelayByInSeconds;
                System.Threading.Thread.Sleep(milliSec);
            }
            return newOrderItem;
        }

        public bool Delete(int id)
        {
            Log.Information("InMemoryDb:Delete");
            var itemInDb = orderList.FirstOrDefault(t => t.Id == id);
            if (itemInDb == null)
                return false;
            orderList.Remove(itemInDb);
            return true;
        }

        public OrderItem Get(int id)
        {
            Log.Information("InMemoryDb:GetById");
            return orderList.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<OrderItem> GetAll()
        {
            Log.Information("InMemoryDb:GetAll");
            return orderList.OrderBy(t => t.Name);
        }

        public OrderItem Update(OrderItem newOrderItem)
        {
            Log.Information("InMemoryDb:Update");
            var itemInDb = orderList.FirstOrDefault(t => t.Id == newOrderItem.Id);
            if (itemInDb == null)
                return null;
            
            itemInDb.Name = newOrderItem.Name;
            itemInDb.IsReady = newOrderItem.IsReady;
            itemInDb.DelayByInSeconds = newOrderItem.DelayByInSeconds;
            if (newOrderItem.DelayByInSeconds > 0)
            {
                var milliSec = 1000 * newOrderItem.DelayByInSeconds;
                System.Threading.Thread.Sleep(milliSec);
            }
            return itemInDb;
        }
    }
}
