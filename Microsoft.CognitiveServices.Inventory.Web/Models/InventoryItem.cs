using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Models
{
    public class InventoryItem
    {
        public readonly string Name;

        public int TotalQuantity { get { return RemainingQuantity + QuantityMade + QuantityShrinked + QuantitySlacked; } }
        public int RemainingQuantity { get; set; }
        public int QuantityShrinked { get; set; }
        public int QuantitySlacked { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityMade { get; set; }

        public InventoryItem(string name, int startingQuantity)
        {
            this.Name = name;
            this.RemainingQuantity = startingQuantity;
            this.QuantityShrinked = 0;
            this.QuantitySlacked = 0;
            this.QuantityReceived = 0;
            this.QuantityMade = 0;
        }       
    }
}
