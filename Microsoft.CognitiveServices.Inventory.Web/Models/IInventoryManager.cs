using System.Threading.Tasks;

namespace Microsoft.CognitiveServices.DeviceBridge.Web.Models
{
    public interface IInventoryManager
    {      
        Task UpdateInventory(string action, string itemName, int quantity);
    }
}