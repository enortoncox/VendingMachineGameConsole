using VendingMachineLibrary.Entities;
using VendingMachineLibrary.Enums;
using VendingMachineLibrary.Interfaces;

namespace VendingMachineGameConsole
{
    internal class Program
    {
        //A List of Consumables that the user can purchase from the PurchaseMenu
        static List<Consumable> itemsInVend = new List<Consumable>();

        //A dictionary that holds the userName and moveCount of all the users that have won
        static Dictionary<string, int> leaderboard = new Dictionary<string, int>();

        //The inventory of the user, this hold all the Consumables they have bought, along with any Recyclables
        static List<IStorable> userInventory = new List<IStorable>();

        //The amount of money that the user has which can be spent in the PurchaseMenu
        static double totalMoney;

        //The amount of thirst the user has. If it reaches 0 they lose
        static int thirst;

        //The amount of hunger the user has. If it reaches 0 they lose
        static int hunger;

        //The amount of moves the user has taken
        static int moveCount;

        //The name of the current user. This is shown in the UserInformation and on the Leaderboard
        static string userName = "";

        //A bool to check if the user has won or lost the game
        static bool hasWon;

        //A bool to check if the PlayGameLoop should return to the MainMenu
        static bool returnToMainMenu;

        //Information that relates to the current context (I.E Drinking)
        static string infoString = "";

        //A Random object to generate random numbers for the EarnMoneyMenu and the ShakeVendingMachineMenu
        static Random rand = new Random();

        static void Main(string[] args)
        {
            //The MainMenu Loop. It runs until the user chooses to exit the game
            while (true)
            {
                //Execute the MainMenu method that starts the game
                MainMenu();
            }
        }

        #region Main Menu

        /// <summary>
        /// The MainMenu allows the user to start the game, go to the CustomiseVendingMachineMenu or Exit the game
        /// </summary>
        public static void MainMenu()
        {
            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in the MainMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Vending Machine Game");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"Info: {infoString}");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("1. Play\n2. Customise Vending Machine Items\n3. Exit Game");
                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //If input is equal to "1" then start the game
                if (input == "1")
                {
                    //If there are no Consumables in the itemsInVend list, then restore the default values
                    if (itemsInVend.Count <= 0)
                    {
                        //Assign a list of the default Consumable objects to itemsInVend
                        itemsInVend = FillDefaultVendingMachine();
                    }

                    //Start the main game loop
                    PlayGameLoop();

                    //Reset the returnToMainMenu bool
                    returnToMainMenu = false;
                }
                //If input is equal to "2" then go to the CustomiseVendingMachineMenu
                else if (input == "2")
                {
                    CustomiseVendingMachineMenu();
                }
                //If input is equal to "3" then Exit the game
                else if (input == "3")
                {
                    Environment.Exit(0);
                }
                //The input was invalid
                else
                {
                    //Assign input to an empty string
                    input = "";

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The CustomiseVendingMachineMenu allows the user to access the ImportVendingMachineMenu or restore the vending machine's items back to default
        /// </summary>
        public static void CustomiseVendingMachineMenu()
        {
            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in the CustomiseVendingMachineMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Customise Vending Machine");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"Info: {infoString}");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("0. Back\n1. Import Vending Machine From File\n2. Reset Vending Machine Defaults");
                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //If input is equal to "0" then return to the MainMenu
                if (input == "0")
                {
                    //Return to the MainMenu
                    return;
                }
                //If input is equal to "1" then go to the ImportVendingMachineMenu
                else if (input == "1")
                {
                    ImportVendingMachineMenu();
                }
                //If input is equal to "2" then restore itemsInVend to its default Consumables
                else if (input == "2")
                {
                    itemsInVend = FillDefaultVendingMachine();
                }
                //If the input was invalid
                else
                {
                    //Assign input to an empty string
                    input = "";

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The ImportVendingMachine Menu allows the user to import custom vending machine Consumables from a .txt file
        /// </summary>
        public static void ImportVendingMachineMenu()
        {
            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in the ImportVendingMachineMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();

                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Import Vending Machine From File");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("A Vending Machine file should be a .txt file with comma-separated values");
                Console.WriteLine("Name, Price, Recovery Amount, ItemType (Drinkable or Edible), Quantity, Flavour (Only if Drinkable)");
                Console.WriteLine("Example: Coke,1.25,50,Drinkable,5,Vanilla");
                Console.WriteLine();
                Console.Write("Flavours: ");

                //Get the names of all the Flavours and store them in a string array
                string[] flavours = Enum.GetNames(typeof(Flavour));

                //Display each of the Flavours in a comma-separated line
                for (int i = 0; i < flavours.Length; i++)
                {
                    if (i != flavours.Length - 1)
                    {
                        Console.Write(flavours[i] + ", ");
                    }
                    else
                    {
                        Console.Write(flavours[i]);
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"Info: {infoString}");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine();
                Console.Write("Please enter the path of the file or 0 to go back: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //If input is equal to "0", return to the MainMenu
                if (input == "0")
                {
                    //Return to the MainMenu
                    return;
                }
                //If input is a file path
                else
                {
                    try
                    {
                        //Check if the provided path leads to a valid file
                        if (File.Exists(input))
                        {
                            //Check if the file has a ".txt" extension
                            if (Path.GetExtension(input).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                            {
                                //Assign itemsInVend to an empty list
                                itemsInVend = new List<Consumable>();

                                //Initialise a list of strings to hold the read in lines
                                List<string> consumables = new List<string>();

                                //Read each line from the .txt file into the consumables list
                                consumables = File.ReadAllLines(input).ToList();

                                //Check if the .txt file containes any data
                                if (consumables.Count > 0)
                                {
                                    //A foreach loop to go through each CSV line in the consumables list
                                    foreach (string consumable in consumables)
                                    {
                                        //Split the CSV line on ',' and store it in a list of string
                                        List<string> properties = consumable.Split(',').ToList();

                                        //Go through each property in the list and Trim any whitespace
                                        properties = properties.Select(temp => temp.Trim()).ToList();

                                        //The property at index 3 should always be an ItemType. Check if the ItemType is Edible
                                        if (properties[3].Equals(nameof(ItemType.Edible), StringComparison.OrdinalIgnoreCase))
                                        {
                                            //Create a new Food object with all the properties
                                            Food food = new Food(properties[0], double.Parse(properties[1]), int.Parse(properties[2]), (ItemType)Enum.Parse(typeof(ItemType), properties[3], true), int.Parse(properties[4]));

                                            //Add the Food object to itemsInVend
                                            itemsInVend.Add(food);
                                        }
                                        //Check if the ItemType is Drinkable
                                        else if (properties[3].Equals(nameof(ItemType.Drinkable), StringComparison.OrdinalIgnoreCase))
                                        {
                                            //Create a new Drink object with all the properties
                                            Drink drink = new Drink(properties[0], double.Parse(properties[1]), int.Parse(properties[2]), (ItemType)Enum.Parse(typeof(ItemType), properties[3], true), int.Parse(properties[4]), (Flavour)Enum.Parse(typeof(Flavour), properties[5], true));

                                            //Add the Drink object to itemsInVend
                                            itemsInVend.Add(drink);
                                        }
                                        //The property at index 3 was not a valid ItemType
                                        else
                                        {
                                            //Throw a new FormatException passing in the "Incorrect ItemType!" message
                                            throw new FormatException("Incorrect ItemType!");
                                        }
                                    }

                                    //Check if any objects were added to itemsInVend
                                    if (itemsInVend.Count > 0)
                                    {
                                        //Assign input to an empty string
                                        input = "";

                                        //Assign infoString to the success message
                                        infoString = $"The Vending Machine has been filled with {itemsInVend.Count} Consumables!";
                                    }
                                    //If no objects were added to itemsInVend
                                    else
                                    {
                                        //Assign input to an empty string
                                        input = "";

                                        //Assign infoString to the failed message
                                        infoString = $"No Consumables were added to the Vending Machine!";
                                    }
                                }
                                //The .txt file doesn't contain any data
                                else
                                {
                                    //Assign input to an empty string
                                    input = "";

                                    //Assign infoString to the error message
                                    infoString = "File is empty!";
                                }
                            }
                            //The file that the path leads to does not have a .txt extension
                            else
                            {
                                //Assign input to an empty string
                                input = "";

                                //Assign infoString to the error message
                                infoString = "File must be of type '.txt'!";
                            }
                        }
                        //The file path does not lead to a valid file
                        else
                        {
                            //Assign input to an empty string
                            input = "";

                            //Assign infoString to the error message
                            infoString = "File Path is invalid!";
                        }
                    }
                    catch (Exception ex)
                    {
                        //Assign input to an empty string
                        input = "";

                        //Assign infoString to the exception message
                        infoString = ex.Message;

                        //Assign itemsInVend to its default Consumables
                        itemsInVend = FillDefaultVendingMachine();
                    }
                }
            }
        }

        #endregion

        #region Game Menus

        /// <summary>
        /// The PlayGameLoop allows the user to choose a username and access the game menus
        /// </summary>
        public static void PlayGameLoop()
        {
            //Start of the game
            while (true)
            {
                //Check if returnToMainMenu is true, if it is then return to MainMenu
                if (returnToMainMenu == true)
                {
                    //Return to MainMenu
                    return;
                }

                //Reset all game variables
                ResetGame();

                //Initialise input to an empty string
                string? input = "";

                //Assign infoString to an empty string
                infoString = "";

                //While the user's input is null or equal to an empty string continue to ask for a username
                while (string.IsNullOrEmpty(input))
                {
                    //Clear the Console window
                    Console.Clear();
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("Choose a Username");
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"Info: {infoString}");
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine();
                    Console.Write("Please enter your username: ");

                    //Read the user's input from ReadLine, Trim it and store it in input
                    input = Console.ReadLine()?.Trim();

                    //Check if input is not null or equal to an empty string. If it's not then assign input to userName
                    if (!string.IsNullOrEmpty(input))
                    {
                        userName = input;
                    }
                    //If input is equal to an empty string
                    else
                    {
                        //Assign infoString to the error message
                        infoString = "Username can't be blank!";
                    }
                }

                //Inner Game Loop
                while (true)
                {
                    //Check if returnToMainMenu is true, if it is then return to MainMenu
                    if (returnToMainMenu == true)
                    {
                        //Return to MainMenu
                        return;
                    }

                    //Check win condition. If hunger >= 100 and thirst >= 70
                    if (hunger >= 100 && thirst >= 70)
                    {
                        //Assign hasWon to true
                        hasWon = true;

                        //Break out of Inner Game Loop
                        break;
                    }

                    //Check lose condition. If hunger <= 0 or thirst <= 0
                    if (hunger <= 0 || thirst <= 0)
                    {
                        //Assign hasWon to false
                        hasWon = false;

                        //Break out of Inner Game Loop
                        break;
                    }

                    //Clear the Console window
                    Console.Clear();

                    //Display the title, user information and infoString
                    UserInformation("Get your Hunger up to 100% and your Thirst above 70%", infoString);

                    Console.WriteLine();
                    Console.WriteLine("1: Make a Purchase\n2: Look at Inventory\n3: Earn Money\n4: Shake vending machine\n5: Exit Game");
                    Console.WriteLine();
                    Console.Write("Please enter an option: ");

                    //Read the user's input from ReadLine, Trim it and store it in input
                    input = Console.ReadLine()?.Trim();

                    //Check if input is equal to "1", if it is then go to the PurchaseMenu
                    if (input == "1")
                    {
                        //Increase moveCount, decrease thirst and hunger
                        moveCount++;
                        thirst -= 8;
                        hunger -= 5;

                        //The PurchaseMenu allows the user to purchase items in the vending machine
                        PurchaseMenu();
                    }
                    //Check if input is equal to "2", if it is then go to the InventoryMenu
                    else if (input == "2")
                    {
                        //Increase moveCount, decrease thirst and hunger
                        moveCount++;
                        thirst -= 4;
                        hunger -= 3;

                        //The InventoryMenu allows the user to use items in their inventory
                        InventoryMenu();
                    }
                    //Check if input is equal to "3", if it is then go to the EarnMoneyMenu
                    else if (input == "3")
                    {
                        //Increase moveCount, decrease thirst and hunger
                        moveCount++;
                        thirst -= 8;
                        hunger -= 4;

                        //The EarnMoneyMenu allows the user to increase the amount of money they have
                        EarnMoneyMenu();

                    }
                    //Check if input is equal to "4", if it is then go to the ShakeVendingMachineMenu
                    else if (input == "4")
                    {
                        //Increase moveCount, decrease thirst and hunger
                        moveCount++;
                        thirst -= 8;
                        hunger -= 5;

                        //The ShakeVendingMachineMenu allows the user to shake the vending machine to try and get a free Consumable
                        ShakeVendingMachineMenu();
                    }
                    //Check if input is equal to "5", if it is then go to the ExitMenu
                    else if (input == "5")
                    {
                        //The ExitMenu allows the user to exit the game
                        ExitMenu();
                    }
                    //The user entered an invalid input
                    else
                    {
                        //Assign infoString to the error message
                        infoString = "Please enter a valid option!";
                    }
                }

                //Check if hasWon is true, if it is then the user has won
                if (hasWon == true)
                {
                    //The WinMenu shows the user the victory screen, and gives access to the Leaderboard
                    WinMenu();
                }
                //If hasWon is false, then the user has lost
                else
                {
                    //The LoseMenu shows the user the lose screen, and gives access to the Leaderboard
                    LoseMenu();
                }
            }
        }

        /// <summary>
        /// The PurchaseMenu allows the user to use their money to buy items from the vending machine
        /// </summary>
        public static void PurchaseMenu()
        {
            //Check if the user has lost by having their hunger or thirst at 0
            if (hunger <= 0 || thirst <= 0)
            {
                //Return to PlayGameLoop
                return;
            }

            //Assign infoString to an empty string
            infoString = "";

            //Initialise userSelection to -1
            int userSelection = -1;

            //Stay in PurchaseMenu while userSelection is not equal to 0
            while (userSelection != 0)
            {
                //Clear the Console window
                Console.Clear();

                //Display the title, user information and infoString
                UserInformation("Make a Purchase", infoString);

                Console.WriteLine();
                Console.WriteLine("Purchasable Items: ");
                Console.WriteLine();
                Console.WriteLine("0: Back");

                //Loop through all Consumables in itemsInVend and call their PurchaseInfo method to display their details
                for (int i = 0; i < itemsInVend.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {itemsInVend[i].PurchaseInfo()}");
                }

                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                string? input = Console.ReadLine()?.Trim();

                //Check if the user's input can't be successfully parsed into an int, if it can then assign it to userSelection
                if (!int.TryParse(input, out userSelection))
                {
                    //If the user's input can't be parsed to an int, then assign -1 to userSelection
                    userSelection = -1;
                }

                //Assign itemNumber to userSelection - 1 to make it match the itemsInVend's indexes
                int itemNumber = userSelection - 1;

                //Check if userSelection is within the available options and isn't 0
                if (userSelection > 0 && userSelection <= itemsInVend.Count)
                {
                    //Check if the user has more or equal totalMoney than the item's price and the quantity of the item is greater than 0
                    if (totalMoney >= itemsInVend[itemNumber].Price && itemsInVend[itemNumber].Quantity > 0)
                    {
                        //Decrease the user's totalMoney by the price of the item
                        totalMoney -= itemsInVend[itemNumber].Price;

                        //Assign infoString to the purchase message
                        infoString = $"You have purchased {itemsInVend[itemNumber].Name} for {itemsInVend[itemNumber].Price:C}";

                        //Cast the object at itemNumber in itemsInVend to an IStorable and then add it to the userInventory
                        userInventory.Add((IStorable)itemsInVend[itemNumber]);

                        //Decrease the quantity of the item that was purchased
                        itemsInVend[itemNumber].Quantity--;
                    }
                    //Check if the user has less totalMoney than the item's price and the quantity of the item is greater than 0
                    else if (totalMoney < itemsInVend[itemNumber].Price && itemsInVend[itemNumber].Quantity > 0)
                    {
                        //Assign infoString to the error message
                        infoString = "Insufficient Funds!";
                    }
                    //The quantity of the item is less than or equal to 0, and so it is sold out
                    else
                    {
                        //Assign infoString to the error message
                        infoString = $"{itemsInVend[itemNumber].Name} is sold out!";
                    }
                }
                //The user has chosen to go back to the PlayGameLoop
                else if (userSelection == 0)
                {
                    //Assign infoString to empty string
                    infoString = "";

                    //Return to PlayGameLoop
                    return;
                }
                //The user's input was invalid
                else
                {
                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The InventoryMenu allows the user to consume or recycle items in the userInventory
        /// </summary>
        public static void InventoryMenu()
        {
            //Check if hunger or thirst is less than or equal to 0
            if (hunger <= 0 || thirst <= 0)
            {
                //Return to PlayGameLoop
                return;
            }

            //Assign infoString to an empty string
            infoString = "";

            //Initialise userSelection to -1
            int userSelection = -1;

            //Check if there are any items in the userInventory
            if (userInventory.Count > 0)
            {
                //Stay in InventoryMenu while userSelection is equal to -1
                while (userSelection == -1)
                {
                    //Clear the Console window
                    Console.Clear();

                    //Display the title, user information and infoString
                    UserInformation("Inventory", infoString);

                    Console.WriteLine();
                    Console.WriteLine("Items in Inventory:");
                    Console.WriteLine();
                    Console.WriteLine($"0: Back");

                    //Loop through all objects in userInventory and execute the ItemInventoryInfo method to get a string of information about the item
                    for (int i = 0; i < userInventory.Count; i++)
                    {
                        Console.Write($"{i + 1}: {userInventory[i].ItemInventoryInfo()}\n");
                    }

                    Console.WriteLine();
                    Console.Write("Please enter an option: ");

                    //Read the user's input from ReadLine, Trim it and store it in input
                    string? input = Console.ReadLine()?.Trim();

                    //Check if the user's input can't be successfully parsed into an int, if it can then assign it to userSelection 
                    if (!int.TryParse(input, out userSelection))
                    {
                        //If the user's input can't be parsed to an int, then assign -1 to userSelection to stay in loop
                        userSelection = -1;

                        //Assign infoString to the error message
                        infoString = "Please enter a valid option!";
                    }
                    //Check if the user's input was less than 0, or greater than the Count of userinventory
                    else if (userSelection < 0 || userSelection > userInventory.Count)
                    {
                        //Assign userSelection to -1
                        userSelection = -1;

                        //Assign infoString to the error message
                        infoString = "Please enter a valid option!";
                    }
                    //The user's input was valid and within the range of the userInventory
                    else
                    {
                        //Assign itemSelection to userSelection - 1 to make it match the userInventory's indexes
                        int itemSelection = userSelection - 1;

                        //Check if itemSelection is within the valid index range of userInventory
                        if (itemSelection < userInventory.Count && itemSelection >= 0)
                        {
                            //Check if userSelection is not equal to 0 and the object at the itemSelection index is of type Drink
                            if (userSelection != 0 && userInventory[itemSelection] is Drink)
                            {
                                //Cast the object at the itemSelection index to the Drink type and store it in tempDrink
                                Drink tempDrink = (Drink)userInventory[itemSelection];

                                //Assign infoString to the Consume string returned from the Consume method
                                infoString = tempDrink.Consume();

                                //Increase thirst by tempDrink's recovery value
                                thirst += tempDrink.Recovery;

                                //Check if thirst is now above 130. Reduce it to 130 for game balance
                                if (thirst > 130)
                                {
                                    thirst = 130;
                                }

                                //Remove the Drink object at the itemSelection index from userInventory
                                userInventory.RemoveAt(itemSelection);

                                //Create a new Recyclable object with the ItemType of Recyclable
                                Recyclable empty = new Recyclable("Empty Bottle", ItemType.Recyclable);

                                //Add the Recyclable object to the userInventory. This simulates having an empty bottle after drinking
                                userInventory.Add(empty);
                            }
                            //Check if userSelection is not equal to 0 and the object at the itemSelection index is of type Recyclable
                            else if (userSelection != 0 && userInventory[itemSelection] is Recyclable)
                            {
                                //Cast the object at the itemSelection index to the Recyclable type and store in tempEmpty
                                Recyclable tempEmpty = (Recyclable)userInventory[itemSelection];

                                //Increase totalMoney by the amount gained from recycling tempEmpty
                                totalMoney += tempEmpty.RecycleMoney;

                                //Assign infoString to recycled message
                                infoString = $"You recycled the bottle for {tempEmpty.RecycleMoney:C}";

                                //Remove the Recyclable object at the itemSelection index from userInventory
                                userInventory.RemoveAt(itemSelection);
                            }
                            //Check if userSelection is not equal to 0 and the object at the itemSelection index is of type Food
                            else if (userSelection != 0 && userInventory[itemSelection] is Food)
                            {
                                //Cast the object at the itemSelection index to the Food type and store in tempFood
                                Food tempFood = (Food)userInventory[itemSelection];

                                //Assign infoString to the Consume string returned from the Consume method
                                infoString = tempFood.Consume();

                                //Increase hunger by tempFood's recovery value
                                hunger += tempFood.Recovery;

                                //Check if hunger is now above 130. Reduce it to 130 for game balance
                                if (hunger > 130)
                                {
                                    hunger = 130;
                                }

                                //Remove the Food object at the itemSelection index from userInventory
                                userInventory.RemoveAt(itemSelection);
                            }
                        }
                        //User has selected to go back to the PlayGameLoop
                        else if (userSelection == 0)
                        {
                            //Assign infoString to an empty string
                            infoString = "";

                            //Return to PlayGameLoop
                            return;
                        }
                    }
                }
            }
            //userInventory is empty, so return to PlayGameLoop
            else
            {
                //Assign infoString to the error message
                infoString = "Your inventory is empty!";

                //Return to PlayGameLoop
                return;
            }
        }

        /// <summary>
        /// The EarnMoneyMenu allows the user to increase their totalMoney
        /// </summary>
        public static void EarnMoneyMenu()
        {
            //Check if hunger or thirst is less than or equal to 0
            if (hunger <= 0 || thirst <= 0)
            {
                //Return to the PlayGameLoop
                return;
            }

            //Assign infoString to an empty string
            infoString = "";

            //Initialise userSelection to -1
            int userSelection = -1;

            //Stay in EarnMoneyMenu while userSelection is equal to -1
            while (userSelection == -1)
            {
                //Clear the Console window
                Console.Clear();

                //Display the title, user information and infoString
                UserInformation("Earn Money", infoString);

                Console.WriteLine();
                Console.WriteLine("Would you like to look for cash in the surrounding area?: ");
                Console.WriteLine();
                Console.WriteLine("0. Back");
                Console.WriteLine("1. Look behind the vending machine");
                Console.WriteLine("2. Look in the change draw");
                Console.WriteLine("3. Look at the ground near by");

                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                string? input = Console.ReadLine()?.Trim();

                //Check if the user's input can't be successfully parsed into an int, if it can then assign it to userSelection 
                if (!int.TryParse(input, out userSelection))
                {
                    //If the user's input can't be parsed to an int, then assign -1 to userSelection to stay in loop
                    userSelection = -1;
                }

                //Generate a random number between 1 and 3 to match userSelection options
                int randomNumber = rand.Next(1, 4);

                //Generate a random amount of money to give to the user if they guess successfully
                double randomMoney = rand.Next(145, 345);

                //Divide randomMoney by 100 to make it the correct money format
                randomMoney /= 100;

                //Check if userSelection is 1 and randomNumber is 1
                if (userSelection == 1 && randomNumber == 1)
                {
                    //Assign infoString to the success message
                    infoString = $"You found {randomMoney:C} behind the vending machine";

                    //Increase totalMoney by the randomly generated amount
                    totalMoney += randomMoney;
                }
                //Check if userSelection is 2 and randomNumber is 2
                else if (userSelection == 2 && randomNumber == 2)
                {
                    //Assign infoString to the success message
                    infoString = $"You found {randomMoney:C} in the change draw";

                    //Increase totalMoney by the randomly generated amount
                    totalMoney += randomMoney;
                }
                //Check if userSelection is 3 and randomNumber is 3
                else if (userSelection == 3 && randomNumber == 3)
                {
                    //Assign infoString to the success message
                    infoString = $"You found {randomMoney:C} on the ground near by";

                    //Increase totalMoney by the randomly generated amount
                    totalMoney += randomMoney;
                }
                //User has chosen to return to the PlayGameLoop
                else if (userSelection == 0)
                {
                    //Assign infoString to an emptyString
                    infoString = "";

                    //Return to PlayGameLoop
                    return;
                }
                //userSelection was invalid
                else if (userSelection > 3 || userSelection < 0)
                {
                    //Assign infoString to the error message
                    infoString = $"Please enter a valid option!";

                    //Assign userSelection to -1 to stay in loop
                    userSelection = -1;
                }
                //userSelection was valid but did not match the randomNumber
                else
                {
                    //Assign infoString to the failed message
                    infoString = $"You found nothing";
                }
            }
        }

        /// <summary>
        /// The ShakeVendingMachineMenu allows the user to shake the vending machine to attempt to get a free consumable
        /// </summary>
        public static void ShakeVendingMachineMenu()
        {
            //Check if hunger or thirst is less than or equal to 0
            if (hunger <= 0 || thirst <= 0)
            {
                //Return to the PlayGameLoop
                return;
            }

            //Assign infoString to an empty string
            infoString = "";

            //Initialise userSelection to -1
            int userSelection = -1;

            //Stay in ShakeVendingMachineMenu while userSelection is equal to -1
            while (userSelection == -1)
            {
                //Clear the Console window
                Console.Clear();

                //Display the title, user information and infoString
                UserInformation("Shake Vending Machine", infoString);

                Console.WriteLine();
                Console.WriteLine("You have chosen to shake the vending machine. This could set off the alarm");
                Console.WriteLine();
                Console.WriteLine("0. Back\n1. Shake Vending Machine");
                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                string? input = Console.ReadLine()?.Trim();

                //Check if the user's input can't be successfully parsed into an int, if it can then assign it to userSelection 
                if (!int.TryParse(input, out userSelection))
                {
                    //If the user's input can't be parsed to an int, then assign -1 to userSelection to stay in the loop
                    userSelection = -1;

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }

                //Check if userSelection is equal to 1, if so shake the vending machine
                if (userSelection == 1)
                {
                    //Clear the Console Window
                    Console.Clear();

                    //Display the title, user information and infoString
                    UserInformation("Shake Vending Machine", infoString);

                    //Generate a random number between 0 and itemsInVend.Count + 1
                    int randomNumber = rand.Next(0, itemsInVend.Count + 2);

                    //The user's shake was unsuccessful as the randomNumber is too high
                    if (randomNumber >= itemsInVend.Count)
                    {
                        //Assign infoString to the failed message
                        infoString = "YOU SET OFF THE ALARM. Run for it!\nYou dropped all your inventory in the panic";

                        //Remove all objects in the userInventory
                        userInventory.Clear();

                        //Return to PlayGameLoop
                        return;
                    }
                    //The user's shake was successful and so they gain a free consumable
                    else
                    {
                        //The sum of the Quantity of all items in the vending machine
                        int sumOfQuantity = itemsInVend.Sum(temp => temp.Quantity);

                        //Check if there are any items left in the vending machine
                        if (sumOfQuantity <= 0)
                        {
                            //Assign infoString to the sold out message
                            infoString = $"You shook the vending machine, but everything is sold out";

                            //Return to PlayGameLoop
                            return;
                        }

                        //While the randomNumber index is an item that is sold out
                        while (itemsInVend[randomNumber].Quantity <= 0)
                        {
                            if (randomNumber != 0)
                            {
                                randomNumber--;
                            }
                            else if (randomNumber == 0)
                            {
                                randomNumber = itemsInVend.Count - 1;
                            }
                        }

                        //Add the object that matches the randomNumber index in itemsInVend to the userInventory after casting it to an IStorable
                        userInventory.Add((IStorable)itemsInVend[randomNumber]);

                        //Decrease Quantity of added item
                        itemsInVend[randomNumber].Quantity--;

                        //Assign infoString to the success message
                        infoString = $"Shake successful! You added a {itemsInVend[randomNumber].Name} to your inventory";

                        //Return to PlayGameLoop
                        return;
                    }
                }
                //The user has chosen to return to the PlayGameLoop
                else if (userSelection == 0)
                {
                    //Assign infoString to an empty string
                    infoString = "";

                    //Return to PlayGameLoop
                    return;
                }
                //The user's input was invalid
                else
                {
                    //Assign userSelection to -1 to stay in the loop
                    userSelection = -1;

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The ExitMenu allows the user to exit the game and return to the MainMenu
        /// </summary>
        public static void ExitMenu()
        {
            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in ExitMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();

                //Display the title, user information and infoString
                UserInformation("Exit Game", infoString);

                Console.WriteLine();
                Console.WriteLine("Are you sure you want to exit the game?");
                Console.WriteLine();
                Console.WriteLine("0. Back\n1. Main menu\n2. Exit Game");
                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //Check if input is "0", if so the user has chosen to return to the PlayGameLoop
                if (input == "0")
                {
                    //Return to the PlayGameLoop
                    return;
                }
                //Check if input is "1", if so the user has chosen to return to the MainMenu
                else if (input == "1")
                {
                    //Assign returnToMainMenu bool to true to return to MainMenu
                    returnToMainMenu = true;

                    //Return to the PlayGameLoop
                    return;
                }
                //Check if input is "2", if so the user has chosen to exit the game
                else if (input == "2")
                {
                    //Execute the Environment's Exit method to close the application
                    Environment.Exit(0);
                }
                //The user's input is invalid
                else
                {
                    //Assign input to an empty string to remain in the loop
                    input = "";

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The WinMenu allows the user to win the game and view the Leaderboard
        /// </summary>
        public static void WinMenu()
        {
            //Add the userName and moveCount to the leaderboard dictionary
            leaderboard.Add(userName, moveCount);

            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in WinMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();

                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("Victory");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("You have beaten your hunger and thirst!");
                Console.WriteLine($"You took {moveCount} moves");
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine($"Info: {infoString}");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine();

                Console.WriteLine("1. Play Again\n2. Leaderboard\n3. Main Menu\n4. Exit Game");
                Console.WriteLine();
                Console.Write("Please select an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //Check if input is "1", if so the user has chosen to start a new game
                if (input == "1")
                {
                    //Return to PlayGameLoop
                    return;
                }
                //Check if input is "2", if so the user has chosen to view the Leaderboard
                else if (input == "2")
                {
                    //Execute the LeaderboardMenu method to view the Leaderboard
                    LeaderboardMenu();
                }
                //Check if input is "3", if so the user has chosen to return to the MainMenu
                else if (input == "3")
                {
                    //Assign returnToMainMenu bool to true to return to the MainMenu
                    returnToMainMenu = true;

                    //Return to PlayGameLoop
                    return;
                }
                //Check if input is "4", if so the user has chosen to exit the game
                else if (input == "4")
                {
                    //Execute the Environment's Exit method to close the application
                    Environment.Exit(0);
                }
                //The user's input was invalid
                else
                {
                    //Assign input to an empty string
                    input = "";

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The LoseMenu allows the user to lose the game and view the Leaderboard 
        /// </summary>
        public static void LoseMenu()
        {
            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in LoseMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();

                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("Defeat");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("You lose!");
                Console.WriteLine("Try to keep both Thirst and Hunger above 0%!");
                Console.WriteLine($"You took {moveCount} moves");
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine($"Info: {infoString}");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine();

                Console.WriteLine("1. Play Again\n2. Leaderboard\n3. Main Menu\n4. Exit Game");
                Console.WriteLine();
                Console.Write("Please select an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //Check if input is "1", if so the user has chosen to start a new game
                if (input == "1")
                {
                    //Return to PlayGameLoop
                    return;
                }
                //Check if input is "2", if so the user has chosen to view the Leaderboard
                else if (input == "2")
                {
                    //Execute the LeaderboardMenu method to view the Leaderboard
                    LeaderboardMenu();
                }
                //Check if input is "3", if so the user has chosen to return to the MainMenu
                else if (input == "3")
                {
                    //Assign returnToMainMenu bool to true to return to the MainMenu
                    returnToMainMenu = true;

                    //Return to PlayGameLoop
                    return;
                }
                //Check if input is "4", if so the user has chosen to exit the game
                else if (input == "4")
                {
                    //Execute the Environment's Exit method to close the application
                    Environment.Exit(0);
                }
                //The user's input was invalid
                else
                {
                    //Assign input to an empty string
                    input = "";

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        /// <summary>
        /// The LeaderboardMenu allows the user to view the Leaderboard of successful wins
        /// </summary>
        public static void LeaderboardMenu()
        {
            //Initialise input to an empty string
            string? input = "";

            //Assign infoString to an empty string
            infoString = "";

            //Stay in LeaderboardMenu while input is equal to an empty string
            while (input == "")
            {
                //Clear the Console window
                Console.Clear();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("Leaderboard");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine();

                //If the leaderboard dictionary is empty then display an error message
                if (leaderboard.Count == 0)
                {
                    Console.WriteLine("No scores to display!");
                }
                //If the leaderboard dictionary isn't empty then order the results and display them
                else
                {
                    //Order the leaderboard by value and assign it to a list of KeyValuePairs
                    List<KeyValuePair<string, int>> ordered = leaderboard.OrderBy(x => x.Value).ToList();

                    //Loop through each pair and display the userName and moveCount
                    foreach (var pair in ordered)
                    {
                        Console.WriteLine($"Username: {pair.Key} | Move Count: {pair.Value}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine($"Info: {infoString}");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("1. Play Again");
                Console.WriteLine("2. Save leaderboard to file");
                Console.WriteLine("3. Main Menu");
                Console.WriteLine("4. Exit Game");
                Console.WriteLine();
                Console.Write("Please enter an option: ");

                //Read the user's input from ReadLine, Trim it and store it in input
                input = Console.ReadLine()?.Trim();

                //Check if input is "1", if so the user has chosen to start a new game
                if (input == "1")
                {
                    //Return to PlayGameLoop
                    return;
                }
                //Check if input is "2", if so the user has chosen to save the Leaderboard to a .txt file
                else if (input == "2")
                {
                    //Assign the result of saving the leaderboard to a file to the successful bool
                    bool successful = SaveLeaderboardToFile();

                    //If saving the leaderboard to a file was successful then display a success message
                    if (successful == true)
                    {
                        //Assign input to an empty string
                        input = "";

                        //Assign infoString to the success message with the file path
                        infoString = $"Saved to: {Directory.GetCurrentDirectory()}\\leaderboard.txt";
                    }
                    //If saving the Leaderboard to a file was unsuccessful then display a failed message
                    else
                    {
                        //Assign input to an empty string
                        input = "";

                        //infoString is assigned in the SaveLeaderboardToFile method if it fails
                    }
                }
                //Check if input is "3", if so the user has chosen to return to the MainMenu
                else if (input == "3")
                {
                    //Assign returnToMainMenu bool to true to return to the MainMenu
                    returnToMainMenu = true;

                    //Return to PlayGameLoop
                    return;
                }
                //Check if input is "4", if so the user has chosen to exit the game
                else if (input == "4")
                {
                    //Execute the Environment's Exit method to close the application
                    Environment.Exit(0);
                }
                //The user's input was invalid
                else
                {
                    //Assign input to an empty string
                    input = "";

                    //Assign infoString to the error message
                    infoString = "Please enter a valid option!";
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// The SaveLeaderboardToFile method saves the current leaderboard dictionary to a .txt file
        /// </summary>
        /// <returns>Returns a bool of the result. True if successful and false if unsuccessful</returns>
        public static bool SaveLeaderboardToFile()
        {
            //Assign filePath to the current directory of the application plus the file name "\leaderboard.txt"
            string filePath = Directory.GetCurrentDirectory() + "\\leaderboard.txt";

            //Create a new list of strings to hold each line of the scores
            List<string> leaderboardLines = new List<string>();

            //Order the leaderboard by value and assign it to the list of KeyValuePairs
            var ordered = leaderboard.OrderBy(x => x.Value).ToList();

            //Loop through each pair and add a string containing the userName and moveCount to the list of strings
            foreach (var pair in ordered)
            {
                //Add the score string to leaderboardLines
                leaderboardLines.Add($"Username: {pair.Key} | Move Count: {pair.Value}");
            }

            try
            {
                //Write the leaderboard scores to the leaderboard.txt file
                File.WriteAllLines(filePath, leaderboardLines);
            }
            catch (Exception ex)
            {
                //Assign infoString to the exception message
                infoString = ex.Message;

                //Return false to show the method was unsuccessful
                return false;
            }

            //Return true to show the method was successful
            return true;
        }

        /// <summary>
        /// The UserInformation method displays the title, user information and the infoString to the Console window
        /// </summary>
        /// <param name="titleString">The title of the current menu</param>
        /// <param name="infoString">The current information for the user (Action, Success, Error, etc)</param>
        public static void UserInformation(string titleString, string infoString = "")
        {
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine(titleString);
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Username: {userName}");
            Console.WriteLine($"Move Count: {moveCount}");
            Console.WriteLine($"Money: {totalMoney:C}");
            Console.WriteLine($"Items in Inventory: {userInventory.Count()}");
            Console.WriteLine($"Hunger: {hunger}%");
            Console.WriteLine($"Thirst: {thirst}%");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Info: {infoString}");
            Console.WriteLine("-----------------------------------------------------");
        }

        /// <summary>
        /// The FillDefaultVendingMachine method returns the default list of consumables
        /// </summary>
        /// <returns>Returns a list of Consumables</returns>
        public static List<Consumable> FillDefaultVendingMachine()
        {
            //Create a new list of Consumables
            List<Consumable> listOfItems = new List<Consumable>();

            //Add the default Consumables for the vending machine to the listOfItems
            listOfItems.Add(new Drink("Strawberry Coke", 0.75, 35, ItemType.Drinkable, 6, Flavour.Strawberry));
            listOfItems.Add(new Drink("Raspberry Pepsi", 1.00, 45, ItemType.Drinkable, 5, Flavour.Raspberry));
            listOfItems.Add(new Drink("Melon Blast", 1.20, 60, ItemType.Drinkable, 5, Flavour.Melon));
            listOfItems.Add(new Drink("Schweppes Lemonade", 1.45, 65, ItemType.Drinkable, 5, Flavour.Lemon));
            listOfItems.Add(new Food("Chocolate Bar", 1.70, 25, ItemType.Edible, 1));
            listOfItems.Add(new Food("Crisps", 2.40, 35, ItemType.Edible, 0));
            listOfItems.Add(new Food("Ham Sandwich", 4.00, 45, ItemType.Edible, 5));
            listOfItems.Add(new Food("Turkey Sandwich", 6.25, 75, ItemType.Edible, 5));

            //Return the list of Consumables
            return listOfItems;
        }

        /// <summary>
        /// The ResetGame method sets all the user's information back to their default values
        /// </summary>
        public static void ResetGame()
        {
            //Set all user information variables to their default values
            userInventory = new List<IStorable>();
            totalMoney = 0;
            thirst = 92;
            hunger = 95;
            moveCount = 0;
            userName = "";
            infoString = "";
            hasWon = false;
        }

        #endregion
    }
}
