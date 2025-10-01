using Microtransaction_Store;

Item w1 = new()
{
    Name = "Pistol",
    Sprite = "Pistol"
};
Item w2 = new()
{
    Name = "Sword",
    Sprite = "Sword"
};
Item w3 = new()
{
    Name = "Pencil",
    Sprite = "Pencil"
};
Item w4 = new()
{
    Name = "Singular Carbon Atom",
    Sprite = "Singular Carbon Atom"
};


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
    List<Item> weaponlist = [w1, w2, w3, w4];
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


int Coins = 100;
Dictionary<string, int> totalLoot = [];


while (Coins > 0)
{
    Console.WriteLine($"What lootbox do you want to open?\nHold Shift to buy 10.\nYou have {Coins} coins.\n1. Normal lootbox, 1 coin\n2. Special Lootbox, 3 Coins\n3. Lootbox deluxe, 7 coins");
    string input = "test";
    List<string> inputList = ["1", "2", "3", "!", "\"", "#"];
    while (inputList.Contains(input) == false)
    {
        input = keyTest();
    }
    int BuyAmount = 1;
    List<string> capitalInput = ["!", "\"", "#"];
    if (capitalInput.Contains(input))
    {
        BuyAmount = 10;
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
            Item currentLootPoolResult = runLootPool(currentLootPool);
            Console.WriteLine($"\n{currentLootPoolResult.Sprite}");
            if (!totalLoot.TryAdd(currentLootPoolResult.Name, 1))
            {
                totalLoot[currentLootPoolResult.Name] += 1;
            }
        }
        keyTest();
    }
    Console.Clear();
}
keyTest();
Console.WriteLine("Total Loot:\n");
foreach (string key in totalLoot.Keys)
{
    Console.WriteLine($"You got {key} * {totalLoot[key]}");
}
keyTest();