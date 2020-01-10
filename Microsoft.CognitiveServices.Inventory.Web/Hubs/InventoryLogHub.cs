using Microsoft.AspNetCore.SignalR;
using Microsoft.CognitiveServices.DeviceBridge.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Hubs
{
    public class InventoryLogHub : Hub
    {
        private readonly IInventory basicInventory;

        public InventoryLogHub(IInventory basicInventory)
        {
            this.basicInventory = basicInventory;
        }

        public IEnumerable<InventoryItem> GetAllItems()
        {
            return this.basicInventory.GetAllItems();
        }

        // some method to monitor a public static data structure.

        public void Reset()
        {
            this.basicInventory.Reset();
        }

        // This receives messages from web clients.
        public async Task SendMessage(string senderId, string message)
        {
            string responseMessage = string.Empty;

            // Message format should be intent,item,quantity. E.g. "make,Latte,2", "shrink,Chocolate Muffin,4".
            string[] parsedMessage = message.Split(',');

            if (parsedMessage.Length >= 3)
            {
                string intent = parsedMessage[0];
                string itemName = parsedMessage[1];
                int quantity = -1;
                if (int.TryParse(parsedMessage[2], out quantity))
                {
                    switch (intent)
                    {
                        case "receive":
                            this.basicInventory.ReceivingItem(itemName, quantity);
                            responseMessage += intent + ", " + itemName + ", " + quantity;
                            break;

                        case "slack":
                            this.basicInventory.SlackItem(itemName, quantity);
                            responseMessage += intent + ", " + itemName + ", " + quantity;
                            break;

                        case "shrink":
                            this.basicInventory.ShrinkItem(itemName, quantity);
                            responseMessage += intent + ", " + itemName + ", " + quantity;
                            break;

                        case "make":
                            this.basicInventory.MakeItem(itemName, quantity);
                            responseMessage += intent + ", " + itemName + ", " + quantity;
                            break;

                        default:
                            responseMessage = "Failed to find intent " + intent;
                            break;
                    }
                }
                else
                {
                    responseMessage = "Failed to parse quantity " + parsedMessage[2];
                }
            }
            else
            {
                responseMessage = "Failed to parse message: " + message;
            }

            if (responseMessage != string.Empty)
            {
                await Clients.All.SendAsync("ReceiveMessage", senderId, responseMessage);
            }
        }
    }
}
