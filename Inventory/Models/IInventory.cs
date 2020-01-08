using System.Collections.Generic;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Models
{
    public interface IInventory
    {
        bool ContainsItem(string item);
        IEnumerable<InventoryItem> GetAllItems();
        void ReceivingItem(string itemName, int quantityReceived);
        bool RemoveItem(string itemName);
        void ShrinkItem(string itemName, int quantityShrinked);
        void SlackItem(string itemName, int quantitySlacked);
        void MakeItem(string itemName, int quantity);
        bool TryAddItem(InventoryItem item);
        void Reset();
    }
}