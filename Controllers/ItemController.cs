using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storeinventory;
using StoreInventory.Models;

namespace StoreInventory.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ItemController : ControllerBase
  {
    private DatabaseContext context;

    public ItemController(DatabaseContext _context)
    {
      this.context = _context;
    }

    [HttpPost]
    public ActionResult<Item> CreateItem([FromBody]Item item)
    {
      context.Items.Add(item);
      context.SaveChanges();
      return item;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetAllItems()
    {
      var items = context.Items.OrderByDescending(item => item.DateOrdered);
      return items.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult GetOneItem(int id)
    {
      var singleItem = context.Items.FirstOrDefault(item => item.Id == id);
      if (singleItem == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(singleItem);
      }
    }

    [HttpPut("{id}")]
    public ActionResult<Item> UpdateItem([FromBody]Item updatedInfo, int id)
    {
      var updateItem = context.Items.FirstOrDefault(item => item.Id == id);
      if (id != updatedInfo.Id)
      {
        return BadRequest();
      }

      if (updateItem == null)
      {
        return NotFound();
      }
      else
      {
        // updateItem.SKU = updatedInfo.SKU;
        // updateItem.Name = updatedInfo.Name;
        // updateItem.Description = updatedInfo.Description;
        // updateItem.NumberInStock = updatedInfo.NumberInStock;
        // updateItem.Price = updatedInfo.Price;
        // updateItem.DateOrdered = updatedInfo.DateOrdered;
        // ^^^ valid way of doing it, but not scaleable

        // context.Items.Update(updatedInfo);
        // ^^^ Checks EVERY item and its children...can cause performance issues

        context.Entry(updatedInfo).State = EntityState.Modified;
        context.SaveChanges();
        return Ok(updateItem);
        // return Ok(updatedInfo);
      }

    }

    [HttpDelete("{id}")]
    public ActionResult DeleteItem(int id)
    {
      var deletedItem = context.Items.FirstOrDefault(item => item.Id == id);
      if (deletedItem == null)
      {
        return NotFound();
      }
      else
      {
        context.Items.Remove(deletedItem);
        context.SaveChanges();
        return Ok(deletedItem);
      }
    }

    [HttpGet("out-of-stock")]
    public ActionResult GetOOSItems()
    {
      var outOfStockItems = context.Items.Where(i => i.NumberInStock == 0);
      if (outOfStockItems == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(outOfStockItems);
      }
    }

    [HttpGet("sku/{SKU}")]
    public ActionResult GetSKUItem(int sku)
    {
      var SKUItem = context.Items.FirstOrDefault(i => i.SKU == sku);
      if (SKUItem == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(SKUItem);
      }
    }
  }
}