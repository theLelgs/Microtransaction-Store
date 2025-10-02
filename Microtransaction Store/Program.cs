using System.Xml;
using Microtransaction_Store;

Item w1 = new()
{
    Name = "Pistol",
    Sprite = "Pistol",
    Rarity = "Legendary"
};
Item w2 = new()
{
    Name = "Sword",
    Sprite = "Sword",
    Rarity = "Epic"
};
Item w3 = new()
{
    Name = "Pencil",
    Sprite = "Pencil",
    Rarity = "Rare"
};
Item w4 = new()
{
    Name = "Singular Carbon Atom",
    Sprite = "Singular Carbon Atom",
    Rarity = "Common"
};
Item currentLootPoolResult;
List<Item> weaponlist = [w1, w2, w3, w4];
List<ConsoleColor> ColorList = [ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Yellow];
List<string> RarityList = ["Common", "Rare", "Epic", "Legendary"];

Dictionary<string, int> GetLBLootWeight(string LootBoxName) //Tar fram data från LootBoxes.txt och omvandlar till en dictionary, där namnet på varje item kopplas tills dens vikt.
{
    Dictionary<string, int> tempDictionary = [];
    StreamReader sr = new(@"LootBoxes.txt");
    string line = sr.ReadLine();
    while (line != null && line.Split("|")[0] != LootBoxName)
    {
        line = sr.ReadLine();
    }
    for (int a = 1; a < 5; a++)
    {
        string templine = line.Split("|")[a];
        tempDictionary[templine.Split(":")[0].ToString()] = Int32.Parse(templine.Split(":")[1]);
    }
    return tempDictionary;
}
List<string> makeLootPool(Dictionary<string, int> LootPoolDictionary) //Tar namnet på varje Item och lägger till den till en lista ett antal ggr lika med dens vikt.
{
    List<string> templist = [];
    foreach (string a in LootPoolDictionary.Keys)
    {
        for (int x = 0; x < LootPoolDictionary[a]; x++)
        {
            templist.Add(a);
        }
    }
    return templist;
}
Item runLootPool(List<string> LootPool) //Tar fram ett slumpat namn ur listan och tar fram vilket item det passar till.
{
    
    string randomItem = LootPool[Random.Shared.Next(LootPool.Count)];
    foreach (Item x in weaponlist)
    {
        if (x.Name == randomItem)
        {
            return x;
        }
    }
    return null;
}
string keyTest() //Kollar efter inputs
{
    string a = null;

    while (a == null)
    {
        if (Console.KeyAvailable)
        {
            a = Console.ReadKey(true).KeyChar.ToString();
        }
    }

    return a;
}
void writeTotalLoot(Dictionary<string,int> tempDictionary, bool writeRarity) //tar en Dictionary med olika items och deras mängd och skriver ut dem. bool om rarity också ska skrivas
{
    Console.WriteLine("Total Loot:\n");
    foreach (string key in tempDictionary.Keys)
    {
        Console.WriteLine($"You have gotten {key} * {tempDictionary[key]}");

        if (writeRarity)
        {
            foreach (Item weapon in weaponlist)
            {
                if (weapon.Name == key)
                {
                    Console.Write($"Rarity: ");
                    Console.ForegroundColor = ColorList[RarityList.IndexOf(weapon.Rarity)];
                    Console.Write(weapon.Rarity + "\n\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        else
        {
            Console.Write("\n");
        }
    }
}

int Coins = 100;
Dictionary<string, int> totalLoot = [];
Dictionary<string, int> currentLoot = [];



while (Coins > 0)
{
    Console.WriteLine($"What lootbox do you want to open?\nHold Shift to buy multiple.\nYou have {Coins} coins.\n1. Normal lootbox, 1 coin\n2. Special Lootbox, 3 Coins\n3. Lootbox Deluxe, 7 coins\nE. See total loot.");
    string input = "test";
    List<string> inputList = ["1", "2", "3", "!", "\"", "#", "e", "E"];
    while (inputList.Contains(input) == false)
    {
        input = keyTest();
    }
    if (input.ToLower() == "e")
    {
        writeTotalLoot(totalLoot, false);
    }
    else
    {
        int BuyAmount = 1;
        List<string> capitalInput = ["!", "\"", "#"];
        List<string> LBInputs = ["1", "2", "3", "!", "\"", "#"];
        if (capitalInput.Contains(input))
        {
            Console.WriteLine("\nHow many do you want to buy?\n");
            bool success = false;
            while (success == false)
            {
                string buying = Console.ReadLine();
                success = int.TryParse(buying, out int tempInt);
                if (success)
                {
                    BuyAmount = tempInt;
                }
            }

            input = inputList[capitalInput.IndexOf(input)];
        }

        if (Coins >= ((Math.Pow(2, inputList.IndexOf(input.ToLower()) + 1) - 1) * BuyAmount))
        {
            Coins -= (int)((Math.Pow(2, inputList.IndexOf(input) + 1) - 1) * BuyAmount);
            List<string> currentLootPool;

            if (capitalInput.Contains(input))
            {
                currentLootPool = makeLootPool(GetLBLootWeight($"LB{inputList[capitalInput.IndexOf(input)]}"));
            }
            else
            {
                currentLootPool = makeLootPool(GetLBLootWeight($"LB{input}"));
            }
            for (int a = 0; a < BuyAmount; a++)
            {
                currentLootPoolResult = runLootPool(currentLootPool);

                if (!currentLoot.TryAdd(currentLootPoolResult.Name, 1))
                {
                    currentLoot[currentLootPoolResult.Name] += 1;
                }
                if (!totalLoot.TryAdd(currentLootPoolResult.Name, 1))
                {
                    totalLoot[currentLootPoolResult.Name] += 1;
                }
            }
            foreach (var key in currentLoot.Keys)
            {
                Console.WriteLine($"You got {key} * {currentLoot[key]}");
                foreach (Item weapon in weaponlist)
                {
                    if (weapon.Name == key)
                    {
                        Console.Write($"Rarity: ");
                        Console.ForegroundColor = ColorList[RarityList.IndexOf(weapon.Rarity)];
                        Console.Write(weapon.Rarity +"\n\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            currentLoot = [];


        }
        else
        {
            Console.WriteLine("Not enough money");
        }
    }
    keyTest();
    Console.Clear();
}

keyTest();
if (Random.Shared.Next(2) == 1) writeTotalLoot(totalLoot, true); else writeTotalLoot(totalLoot, false);
keyTest();