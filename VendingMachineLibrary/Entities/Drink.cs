using VendingMachineLibrary.Enums;
using VendingMachineLibrary.Interfaces;

namespace VendingMachineLibrary.Entities
{
    public class Drink : Consumable, IStorable
    {
        //The Flavour of the Drink
        public Flavour Flavour { get; set; }

        public Drink(string name, double price, int rec, ItemType itemType, int quantity, Flavour flavour)
        {
            Name = name;
            Price = price;
            Recovery = rec;
            ItemType = itemType;
            Quantity = quantity;
            Flavour = flavour;
        }

        /// <summary>
        /// Returns a string that contains the statement for when a Drink object is consumed
        /// </summary>
        /// <returns>Returns a string that contains the statement for when a Drink object is consumed</returns>
        public override string Consume()
        {
            return $"You drank the {Name}, it had a nice {Flavour} flavour.";       
        }

        /// <summary>
        /// Returns a string that contains the Name, ItemType and Recovery information of the Drink object being stored
        /// </summary>
        /// <returns>Returns a string that contains the Name, ItemType and Recovery information of the Drink object being stored</returns>
        public string ItemInventoryInfo()
        {
            return $"{this.Name} - ({ItemType} : {Recovery}% Thirst)";
        }
    }
}
