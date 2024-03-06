using VendingMachineLibrary.Enums;

namespace VendingMachineLibrary.Entities
{
    public abstract class Consumable
    {
        //The Name of the Consumable
        public string Name { get; set; }

        //The Price of the Consumable for the PurchaseMenu
        public double Price { get; set; }

        //The Amount of stat recovery the Consumable gives
        public int Recovery { get; set; }

        //The ItemType of the Consumable
        public ItemType ItemType { get; set; }

        //The Quantity remaining of the Consumable. If it is 0 then it is Sold Out
        public int Quantity { get; set; }

        /// <summary>
        /// Returns a string that contains the statement for when a Consumable object is consumed
        /// </summary>
        /// <returns>Returns a string that contains the statement for when a Consumable object is consumed</returns>
        public virtual string Consume()
        {
            return $"You consumed the {Name}.";
        }


        /// <summary>
        /// Returns a string that contains the purchase information for the Consumable
        /// </summary>
        /// <returns>Returns a string that contains the purchase information for the Consumable</returns>
        public virtual string PurchaseInfo() 
        {
            //Checks if the Quantity is greater than 0, if it's not, then the Consumable is displayed as SOLD OUT
            if (Quantity > 0)
            {
                return $"{Name} | {Price:C} | {ItemType} {Recovery}%";
            }
            else 
            {
                return $"{Name} | SOLD OUT";
            }
        }
    }
}
