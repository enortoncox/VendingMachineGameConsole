using VendingMachineLibrary.Enums;
using VendingMachineLibrary.Interfaces;

namespace VendingMachineLibrary.Entities
{
    public class Food : Consumable, IStorable
    {
        public Food(string name, double price, int rec, ItemType itemType, int quantity)
        {
            Name = name;
            Price = price;
            Recovery = rec;
            ItemType = itemType;
            Quantity = quantity;
        }

        /// <summary>
        /// Returns a string that contains the statement for when a Food object is consumed
        /// </summary>
        /// <returns>Returns a string that contains the statement for when a Food object is consumed</returns>
        public override string Consume()
        {
            return $"You ate the {Name}, it helped lessen your hunger.";
        }

        /// <summary>
        /// Returns a string that contains the Name, ItemType and Recovery information of the Food object being stored
        /// </summary>
        /// <returns>Returns a string that contains the Name, ItemType and Recovery information of the Food object being stored</returns>
        public string ItemInventoryInfo()
        {
            return $"{this.Name} - ({ItemType} : {Recovery}% Hunger)";
        }
    }
}
