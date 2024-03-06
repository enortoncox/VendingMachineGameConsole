using VendingMachineLibrary.Enums;
using VendingMachineLibrary.Interfaces;

namespace VendingMachineLibrary.Entities
{
    public class Recyclable : IStorable
    {
        //The Name of the Recyclable
        public string Name { get; set; }

        //The amount of money the user gets from recycling this Recyclable
        public double RecycleMoney { get; set; }

        //The ItemType of the Recyclable
        public ItemType ItemType { get; set; }

        //The Random object used to generate random numbers
        public Random rand = new Random();

        public Recyclable(string name, ItemType itemType)
        {
            Name = name;
            ItemType = itemType;
            RecycleMoney = GenerateRecycleMoney();
        }

        /// <summary>
        /// Randomly generates the amount of money that the user earns from recycling this Recyclable
        /// </summary>
        /// <returns>Returns a double of the amount of money that the user earns from recycling this Recyclable</returns>
        private double GenerateRecycleMoney()
        {
            //Generate a random number between 45 and 80
            double randomMoney = rand.Next(45, 81);

            //Divide randomMoney by 100 to make it the correct money format
            randomMoney /= 100;

            //Return the amount generated from recycling
            return randomMoney;
        }

        /// <summary>
        /// Returns a string that contains the Name and Itemtype of the Recyclable
        /// </summary>
        /// <returns>Returns a string that contains the Name and Itemtype of the Recyclable</returns>
        public string ItemInventoryInfo()
        {
            return $"{this.Name} - ({ItemType})";
        }
    }

   
}
