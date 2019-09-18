using Microsoft.AspNetCore.Mvc;
using storeinventory;

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
  }
}