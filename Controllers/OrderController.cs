using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverbed.Test.FreshBrewApi.Models;
using Riverbed.Test.FreshBrew.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Riverbed.Test.FreshBrewApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderContext context;
        public OrderController(OrderContext context)
        {
            this.context = context;           
        }
        [HttpGet]
        public IEnumerable<OrderItem> GetAll()
        {
            return context.OrderItemDbSet.ToList();
        }

        [HttpGet("{id}", Name = "GetOrderItem")]
        public IActionResult GetById(int id)
        {
            var item = context.OrderItemDbSet.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] OrderItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            context.OrderItemDbSet.Add(item);
            context.SaveChanges();

            return CreatedAtRoute("GetOrderItem", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] OrderItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var itemInDb = context.OrderItemDbSet.FirstOrDefault(t => t.Id == id);
            if (itemInDb == null)
            {
                return NotFound();
            }

            itemInDb.IsReady = item.IsReady;
            itemInDb.Name = item.Name;

            context.OrderItemDbSet.Update(itemInDb);
            context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = context.OrderItemDbSet.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            context.OrderItemDbSet.Remove(item);
            context.SaveChanges();
            return new NoContentResult();
        }
    }
}
