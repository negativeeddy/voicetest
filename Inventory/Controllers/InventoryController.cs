using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.DeviceBridge.Web.Models;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryManager _inventoryManager;

        public InventoryController(IInventoryManager inventoryManager)
        {
            this._inventoryManager = inventoryManager;            
        } 
        
        [HttpPost("UpdateInventory")]
        public async Task<JsonResult> UpdateInventory(string action, string product, int quantity)
        {
            Trace.WriteLine($"Update Inventory called: {action} {quantity} {product}");
            await this._inventoryManager.UpdateInventory(action, product, quantity);
            return new JsonResult(new { action, product, quantity });
        }       
    }
}
