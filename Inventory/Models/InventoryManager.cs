using Microsoft.AspNetCore.SignalR;
using Microsoft.CognitiveServices.DeviceBridge.Web.Hubs;
using System.Threading.Tasks;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Models
{
    public class InventoryManager : IInventoryManager
    {
        private readonly IHubContext<InventoryLogHub> inventoryLogHubContext;
        private readonly IInventory basicInventory;

        public InventoryManager(IHubContext<InventoryLogHub> inventoryLogHubContext, IInventory basicInventory)
        {
            this.inventoryLogHubContext = inventoryLogHubContext;
            this.basicInventory = basicInventory;
        }

        public async Task UpdateInventory(string action, string item, int quantity)
        {
            string logMessage = $"{action} {quantity} {item}";

            if (this.basicInventory.ContainsItem(item))
            {
                switch(action.ToLowerInvariant())
                {
                    case "make":
                        this.basicInventory.MakeItem(item, quantity);
                        break;
                    case "slack":
                        this.basicInventory.SlackItem(item, quantity);
                        break;
                    case "shrink":
                        this.basicInventory.ShrinkItem(item, quantity);
                        break;
                    case "receive":
                        this.basicInventory.ReceivingItem(item, quantity);
                        break;
                }

                await this.inventoryLogHubContext.Clients.All.SendAsync("ReceiveMessage", action, logMessage);
            }
            else
            {
                logMessage = $"Failed to {logMessage}, {item} not found.";
                await this.inventoryLogHubContext.Clients.All.SendAsync("ReceiveMessage", string.Empty, logMessage);
            }
        }
    }
}
