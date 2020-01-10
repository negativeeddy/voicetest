using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Models
{
    // Basic in-memory inventory for demo. Mostly a wrapper for Dictionary.
    public class BasicInventory : IInventory
    {
        private IDictionary<string, InventoryItem> items;

        public BasicInventory()
        {
            this.items = new Dictionary<string, InventoryItem>();
            this.PopulateItems();
        }

        public IEnumerable<InventoryItem> GetAllItems()
        {
            return this.items.Values;
        }

        public bool ContainsItem(string item)
        {
            return this.items.ContainsKey(item);
        }

        public bool TryAddItem(InventoryItem item)
        {
            return this.items.TryAdd(item.Name, item);
        }

        public bool RemoveItem(string itemName)
        {
            return this.items.Remove(itemName);
        }

        public void ReceivingItem(string itemName, int quantityReceived)
        {
            this.items[itemName].QuantityReceived = quantityReceived;
            this.items[itemName].RemainingQuantity += quantityReceived;
        }

        public void SlackItem(string itemName, int quantitySlacked)
        {
            this.items[itemName].RemainingQuantity -= quantitySlacked;
            this.items[itemName].QuantitySlacked += quantitySlacked;
        }

        public void ShrinkItem(string itemName, int quantityShrinked)
        {
            this.items[itemName].RemainingQuantity -= quantityShrinked;
            this.items[itemName].QuantityShrinked += quantityShrinked;
        }

        public void MakeItem(string itemName, int quantityMade)
        {
            this.items[itemName].RemainingQuantity -= quantityMade;
            this.items[itemName].QuantityMade += quantityMade;
        }

        public void Reset()
        {
            this.items.Clear();
            this.PopulateItems();
        }

        private void PopulateItems()
        {
            // Dummy/test data.
            this.TryAddItem(new InventoryItem("Americano", 20));
            this.TryAddItem(new InventoryItem("Cappuccino", 20));
            this.TryAddItem(new InventoryItem("Espresso", 20));
            this.TryAddItem(new InventoryItem("Latte", 20));
            this.TryAddItem(new InventoryItem("Macchiato", 20));

            this.TryAddItem(new InventoryItem("Ninja Cat Cold Brew", 20));
            this.TryAddItem(new InventoryItem("Unicorn Cappucino", 20));
            this.TryAddItem(new InventoryItem("Tyrannosaurus Tea", 20));

            this.TryAddItem(new InventoryItem("Croissant", 20));
            this.TryAddItem(new InventoryItem("Chocolate Muffin", 20));
            this.TryAddItem(new InventoryItem("Danish Pastry", 20));
            this.TryAddItem(new InventoryItem("Fruit Bread", 20));
        }
    }
}
