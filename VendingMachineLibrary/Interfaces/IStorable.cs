
namespace VendingMachineLibrary.Interfaces
{
    public interface IStorable
    {
        /// <summary>
        /// Returns a string that contains the information about the object that is stored in the inventory
        /// </summary>
        /// <returns>Returns a string that contains the information about the object that is stored in the inventory</returns>
        string ItemInventoryInfo();
    }
}
