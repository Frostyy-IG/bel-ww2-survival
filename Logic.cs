namespace Utils
{
    using System;
    using System.Media;

    class Utils
    {
        Random random = new Random();

        #region Sounds
        //SmallArms sounds
        List<string> smallArms = new List<string> {
            @"C:\Users\Frostyy\source\repos\WW2SurvivalGame\Sounds\SmallArmsFire0.wav",
            @"C:\Users\Frostyy\source\repos\WW2SurvivalGame\Sounds\SmallArmsFire1.wav",
            @"C:\Users\Frostyy\source\repos\WW2SurvivalGame\Sounds\SmallArmsFire2.wav",
        };

        //Move sounds
        List<string> move = new List<string> {
            @"C:\Users\Frostyy\source\repos\WW2SurvivalGame\Sounds\Move0.wav",
            @"C:\Users\Frostyy\source\repos\WW2SurvivalGame\Sounds\Move1.wav",
            @"C:\Users\Frostyy\source\repos\WW2SurvivalGame\Sounds\Move2.wav",
        };
        #endregion

        public void SmallArms()
        {
            //Get a random sound
            int randomIndex = random.Next(smallArms.Count);
            string randomSmallArms = smallArms[randomIndex];

            using (var smallArms = new SoundPlayer(randomSmallArms))
            {
                smallArms.PlaySync(); //Use PlaySync to play the sound synchronously
            }
        }

        public void Move()
        {
            //Get a random sound
            int randomIndex = random.Next(move.Count);
            string randomMove = move[randomIndex];

            using (var move = new SoundPlayer(randomMove))
            {
                move.PlaySync(); //Use PlaySync to play the sound synchronously
            }
        }

        public void WriteColor(string message, ConsoleColor color, string lineEnding)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.Write(lineEnding);
        }
    }
}

namespace Enemy
{
    class Enemy
    {
        public int health { get; set; }
        public int damage { get; set; }

        public Enemy(int health, int damage)
        {
            this.health = health;
            this.damage = damage;
        }
    }
}

namespace Core
{
    using System;
    using Enemy;
    using Utils;
    class Logic
    {
        static List<Item> inventory = new List<Item>();

        static Utils utils = new Utils();

        static Random rand = new Random();

        static Enemy enemy = new Enemy(100, rand.Next(50, 91));
        static Enemy animal = new Enemy(100, rand.Next(0, 0));

        static int health = 100;
        static int energy = 100;
        static int hunger = 100;
        static int thirst = 100;

        static int ammo = 10;
        static int bandages = 1;
        static int marks = 5;

        static int foodCount = 0;
        static int waterCount = 0;

        static bool inInventory = false;
        static bool inCombat = false;
        static bool inShop = false;


        static void Main(string[] args)
        {
            inventory.Add(new Item("Ammunition", 10));
            inventory.Add(new Item("Bandages", 1));
            inventory.Add(new Item("Marks", 5));

            Console.Write("Welcome to the ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Behind Enemy Lines: WW2 Survival" + Environment.NewLine);
            Console.ResetColor();

            while (health > 0 && inCombat == false && inShop == false)
            {
                StatusMenu();

                string? choice = Console.ReadLine();
                if (!int.TryParse(choice, out int menuChoice))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ResetColor();
                    continue;
                }

                bool validChoice = true;
                switch (menuChoice)
                {
                    case 1:
                        if (HasEnoughEnergy(1))
                        {
                            KeepMoving();
                        }
                        break;
                    case 2:
                        if (HasEnoughEnergy(1))
                        {
                            SearchForEquipment();
                        }
                        break;
                    case 3:
                        if (HasEnoughEnergy(1))
                        {
                            SearchForFood();
                        }
                        break;
                    case 4:
                        if (HasEnoughEnergy(1))
                        {
                            SearchForWater();
                        }
                        break;
                    case 5:
                        if (HasEnoughEnergy(1))
                        {
                            CheckInventory();
                        }
                        break;
                    case 6:
                        Rest();
                        break;
                    case 7:
                        break;
                    case 9:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Quitting game..");
                        Console.ResetColor();
                        Environment.Exit(1);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        validChoice = false;
                        break;
                }
                if (validChoice)
                {
                    UpdateStatus();
                }
                energy = Math.Clamp(energy, 0, 100);
            }
        }

        #region Menus
        static void StatusMenu()
        {
            Console.WriteLine("########################");
            CheckStatus();
            Console.WriteLine("########################");
            Console.WriteLine("1. Keep moving towards allied lines");
            Console.WriteLine("2. Scavange for equpiment");
            Console.WriteLine("3. Scavange for food");
            Console.WriteLine("4. Scavange for water");
            Console.WriteLine("5. Check your inventory");
            Console.WriteLine("6. Rest");
            Console.WriteLine("9. Quit the game");
            Console.WriteLine("########################");
        }

        static void CombatMenu()
        {
            Console.WriteLine("##############################");
            Console.WriteLine("What do you want to do?" + Environment.NewLine);
            Console.WriteLine("1. Attempt to fire at the enemy again.");
            Console.WriteLine("2. Attempt to take cover.");
            Console.WriteLine("3. Attempt to retreat away.");
            Console.WriteLine("4. Use a bandage.");
            Console.WriteLine("##############################");
        }

        static void ShopMenu()
        {
            Console.WriteLine("##############################");
            Console.WriteLine("What do you want to do?" + Environment.NewLine);
            Console.WriteLine("1. Purchase some ammunition. Price: 10 marks / 5 bullets");
            Console.WriteLine("2. Sell some ammunition. Price: 5 bullets / 5 marks");
            Console.WriteLine("3. Purchase some food. Price: 15 marks / 1 meal");
            Console.WriteLine("4. Purchase some water. Price: 15 marks / 1 drink");
            Console.WriteLine("5. Sell some food. Price: 1 meal / 10 marks");
            Console.WriteLine("6. Sell some water. Price: 1 drink / 10 marks");
            Console.WriteLine("7. Continue moving.");
            Console.WriteLine("##############################");
        }

        static void AnimalMenu()
        {
            Console.WriteLine("##############################");
            Console.WriteLine("What do you want to do?" + Environment.NewLine);
            Console.WriteLine("1. Attempt to fire at the animal.");
            Console.WriteLine("2. Retreat.");
            Console.WriteLine("##############################");
        }
        #endregion

        static bool HasEnoughEnergy(int requiredEnergy)
        {
            if (energy < requiredEnergy)
            {
                Console.WriteLine("Not enough energy. Take some rest.");
                energy = 0;
                return false;
            }
            else
            {
                return true;
            }
        }

        static void KeepMoving()
        {
            Random rand = new Random();
            int eventChance = rand.Next(0, 101);

            if (eventChance > 75)
            {
                Console.WriteLine("You encounter an enemy!" + Environment.NewLine);
                Combat();
            }
            else if (eventChance > 45)
            {
                Villager();
            }
            else if (eventChance > 20)
            {
                Animal();
            }
        }

        static void SearchForEquipment()
        {
            Console.WriteLine("You search for equipment.." + Environment.NewLine);

            Random rand = new Random();
            int eventChance = rand.Next(0, 101);
            int lootChance = rand.Next(1, 13); // Generate a random number between 1 and 10

            if (lootChance <= 3) // 30% chance of finding bandages
            {
                bandages++;
                Console.WriteLine("You find a bandage!");

                Item? bandagesItem = inventory.Find(item => item.Name == "Bandages");
                if (bandagesItem != null)
                {
                    // If the food item already exists, add the quantity
                    bandagesItem.Quantity += 1;
                }
                else
                {
                    // If the food item does not exist, create a new item in the inventory
                    inventory.Add(new Item("Bandages", 1));
                }
            }
            else if (lootChance <= 6) // 30% chance of finding ammo
            {
                int numAmmo = rand.Next(1, 4); // Generate a random number of ammo between 1 and 5
                ammo += numAmmo;
                Console.WriteLine($"You find {numAmmo} bullets!");

                Item? ammoItem = inventory.Find(item => item.Name == "Ammunition");
                if (ammoItem != null)
                {
                    // If the food item already exists, add the quantity
                    ammoItem.Quantity += numAmmo;
                }
                else
                {
                    // If the food item does not exist, create a new item in the inventory
                    inventory.Add(new Item("Ammunition", numAmmo));
                }
            }
            else if (lootChance <= 9) // 30% chance of finding marks
            {
                int numMarks = rand.Next(1, 6); // Generate a random number of marks between 1 and 5
                marks += numMarks;
                Console.WriteLine($"You find {numMarks} marks!");

                Item? marksItem = inventory.Find(item => item.Name == "Marks");
                if (marksItem != null)
                {
                    // If the food item already exists, add the quantity
                    marksItem.Quantity += numMarks;
                }
                else
                {
                    // If the food item does not exist, create a new item in the inventory
                    inventory.Add(new Item("Marks", numMarks));
                }
            }
            else // 30% chance of finding nothing
            {
                Console.WriteLine("You didn't find anything.");
            }

            if (eventChance > 85)
            {
                Console.WriteLine("You encounter an enemy!" + Environment.NewLine);
                Combat();
            }
        }

        static void SearchForFood()
        {
            Console.WriteLine("You search for food.." + Environment.NewLine);

            List<string> messages = new List<string>()
            {
                "You find some canned food!",
                "You find some chocolate!",
                "You find some bread!",
                "You find some meat!",
                "You find some fat!"
            };

            // Random
            Random rand = new Random();

            // Random foot chance
            int result = rand.Next(1, 6);

            // Random message
            int index = rand.Next(messages.Count);
            string randomMessage = messages[index];

            if (result == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(randomMessage + Environment.NewLine);
                Console.ResetColor();

                foodCount += 1;

                // Check if the food item already exists in the inventory
                Item? foodItem = inventory.Find(item => item.Name == "Food");
                if (foodItem != null)
                {
                    // If the food item already exists, add the quantity
                    foodItem.Quantity += 1;
                }
                else
                {
                    // If the food item does not exist, create a new item in the inventory
                    inventory.Add(new Item("Food", 1));
                }
            }
            else
            {
                Console.WriteLine("You fail to find any food." + Environment.NewLine);
            }
        }

        static void SearchForWater()
        {
            Console.WriteLine("You search for water.." + Environment.NewLine);

            List<string> messages = new List<string>()
            {
                "You find some water!",
                "You find some juice!",
                "You find some wine!",
                "You find some vodka!"
            };

            // Random
            Random rand = new Random();

            // Random foot chance
            int result = rand.Next(1, 6);

            // Random message
            int index = rand.Next(messages.Count);
            string randomMessage = messages[index];

            if (result == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(randomMessage + Environment.NewLine);
                Console.ResetColor();

                waterCount += 1;

                // Check if the water item already exists in the inventory
                Item? waterItem = inventory.Find(item => item.Name == "Water");
                if (waterItem != null)
                {
                    // If the water item already exists, add the quantity
                    waterItem.Quantity += 1;
                }
                else
                {
                    // If the water item does not exist, create a new item in the inventory
                    inventory.Add(new Item("Water", 1));
                }
            }
            else
            {
                Console.WriteLine("You fail to find any water." + Environment.NewLine);
            }
        }

        static void Eat()
        {
            Item? foodItem = inventory.Find(item => item.Name == "Food");
            if (foodItem != null && foodItem.Quantity > 0)
            {
                foodItem.Quantity -= 1;
                foodCount -= 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You eat some food and restore some hunger." + Environment.NewLine);
                Console.ResetColor();
                hunger += 30;
                hunger = Math.Clamp(hunger, 0, 100);
            }
            else
            {
                Console.WriteLine("You do not have any food to eat." + Environment.NewLine);
            }
        }

        static void Drink()
        {
            Item? waterItem = inventory.Find(item => item.Name == "Water");
            if (waterItem != null && waterItem.Quantity > 0)
            {
                waterItem.Quantity -= 1;
                waterCount -= 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You drink some water and restore some thirst." + Environment.NewLine);
                Console.ResetColor();
                thirst += 30;
                thirst = Math.Clamp(thirst, 0, 100);
            }
            else
            {
                Console.WriteLine("You do not have any water to drink." + Environment.NewLine);
            }
        }

        static void Bandage()
        {
            if (bandages >= 1)
            {
                Item? healthItem = inventory.Find(item => item.Name == "Bandages");
                if (healthItem != null && healthItem.Quantity > 0)
                {
                    healthItem.Quantity -= 1;
                    bandages -= 1;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You use a bandage to heal.");
                    Console.ResetColor();
                    health += 30;
                    health = Math.Clamp(health ,0, 100);
                }
            }
            else
            {
                Console.WriteLine("Not enough bandages.");
            }
        }

        static void CheckInventory()
        {
            inInventory = true;

            while (inInventory)
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("Inventory:");
                foreach (Item item in inventory)
                {
                    Console.WriteLine(item.Name + " x" + item.Quantity);
                }
                Console.Write(Environment.NewLine);

                Console.WriteLine("########################");
                Console.WriteLine("1. Eat some food");
                Console.WriteLine("2. Drink some water");
                Console.WriteLine("3. Use a bandage");
                Console.WriteLine("4. Close inventory");
                Console.WriteLine("########################");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Eat();
                        break;
                    case "2":
                        Drink();
                        break;
                    case "3":
                        Bandage();
                        break;
                    case "4":
                        inInventory = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static void Rest()
        {
            Console.WriteLine("You rest and regain some energy.");

            energy += 50;
            energy = Math.Clamp(energy, 0, 100);
            thirst -= 20;
            hunger -= 20;
        }

        static void UpdateStatus()
        {
            Console.WriteLine("One turn passes..");

            hunger -= 5;
            thirst -= 5;
            energy -= 10;          

            if (health < 0)
            {
                GameOver();
            }

            if (hunger <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You died of hunger.");
                Console.ResetColor();
                Environment.Exit(2);
            }

            if (thirst <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You died of thirst.");
                Console.ResetColor();
                Environment.Exit(2);
            }
        }

        static void CheckStatus()
        {
            Console.WriteLine("Current status:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Health: {0}", health);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Energy: {0}", energy);
            Console.WriteLine("Hunger: {0}", hunger);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Thirst: {0}", thirst);
            Console.ResetColor();
        }

        static void Combat()
        {
            inCombat = true;

            Console.WriteLine("You open fire on the enemy!" + Environment.NewLine);

            while (health > 0 && inCombat && inShop == false)
            {
                Item? ammoItem = inventory.Find(item => item.Name == "Ammunition");
                if (ammoItem != null && ammoItem.Quantity > 0)
                {
                    ammoItem.Quantity -= 1;
                    ammo -= 1;
                }

                utils.SmallArms();

                if (ammo <= 0)
                {
                    Console.WriteLine("You have no ammunition left.");

                    Console.WriteLine("What do you want to do?" + Environment.NewLine);
                    Console.WriteLine("1. Attempt to take cover.");
                    Console.WriteLine("2. Attempt to retreat away.");
                    Console.WriteLine("3. Use a bandage.");
                    Console.WriteLine("##############################");

                    string? choice = Console.ReadLine();
                    if (!int.TryParse(choice, out int menuChoice))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        continue;
                    }

                    switch (menuChoice)
                    {
                        case 1:
                            Console.WriteLine("You attempt to take cover." + Environment.NewLine);

                            int enemyHitChance = rand.Next(0, 3);

                            if (enemyHitChance == 0)
                            {
                                Console.WriteLine("The enemy missed you while you were in cover.");
                            }
                            else
                            {
                                utils.SmallArms();

                                health -= enemy.damage;

                                Console.Write("The enemy hits you while you were in cover and deals ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(enemy.damage);
                                Console.ResetColor();
                                Console.Write(" damage.");
                                Console.Write(Environment.NewLine);

                                if (health <= 0)
                                {
                                    GameOver();
                                }
                            }
                            break;
                        case 2:
                            Console.WriteLine("You attempt to retreat." + Environment.NewLine);

                            int retreatChance = rand.Next(0, 2);

                            if (retreatChance == 0)
                            {
                                Console.WriteLine("You successfully retreat." + Environment.NewLine);
                                utils.Move();
                                inCombat = false;
                            }
                            else
                            {
                                utils.SmallArms();

                                Console.WriteLine("You attempt to retreat but fail." + Environment.NewLine);

                                health -= enemy.damage;

                                Console.Write("The enemy hits you and deals ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(enemy.damage);
                                Console.ResetColor();
                                Console.Write(" damage.");
                                Console.Write(Environment.NewLine);

                                if (health <= 0)
                                {
                                    GameOver();
                                }
                            }
                            break;
                        case 3:
                            Bandage();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You now have " + health + " health.");
                            Console.ResetColor();
                            break;
                        default:
                            utils.WriteColor("Invalid input.", ConsoleColor.Red, Environment.NewLine);
                            break;
                    }
                }

                int dealDamage = rand.Next(50, 100);
                int hitChance = rand.Next(0, 2);

                if (hitChance == 1)
                {
                    enemy.health -= dealDamage;

                    Console.Write("You manage to hit the enemy and deal ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(dealDamage);
                    Console.ResetColor();
                    Console.Write(" damage. ");
                    Console.Write(Environment.NewLine);

                    if (enemy.health <= 0)
                    {
                        inCombat = false;

                        enemy.health = 100;

                        Console.Write("You successfully kill the enemy!" + Environment.NewLine);
                    }
                    else
                    {
                        CombatMenu();

                        string? choice = Console.ReadLine();
                        if (!int.TryParse(choice, out int menuChoice))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Invalid choice. Please try again.");
                            Console.ResetColor();
                            continue;
                        }

                        switch (menuChoice)
                        {
                            case 1:
                                Console.WriteLine("You open fire on the enemy!" + Environment.NewLine);
                                break;
                            case 2:
                                Console.WriteLine("You attempt to take cover." + Environment.NewLine);

                                int enemyHitChance = rand.Next(0, 3);

                                if (enemyHitChance == 0)
                                {
                                    Console.WriteLine("The enemy missed you while you were in cover.");
                                }
                                else
                                {
                                    utils.SmallArms();

                                    health -= enemy.damage;

                                    Console.Write("The enemy hits you while you were in cover and deals ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write(enemy.damage);
                                    Console.ResetColor();
                                    Console.Write(" damage.");
                                    Console.Write(Environment.NewLine);

                                    if (health <= 0)
                                    {
                                        GameOver();
                                    }
                                }
                                break;
                            case 3:
                                Console.WriteLine("You attempt to retreat." + Environment.NewLine);

                                int retreatChance = rand.Next(0, 2);

                                if (retreatChance == 0)
                                {
                                    Console.WriteLine("You successfully retreat." + Environment.NewLine);
                                    utils.Move();
                                    inCombat = false;
                                }
                                else
                                {
                                    utils.SmallArms();

                                    Console.WriteLine("You attempt to retreat but fail." + Environment.NewLine);

                                    health -= enemy.damage;

                                    Console.Write("The enemy hits you and deals ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write(enemy.damage);
                                    Console.ResetColor();
                                    Console.Write(" damage.");
                                    Console.Write(Environment.NewLine);

                                    if (health <= 0)
                                    {
                                        GameOver();
                                    }
                                }
                                break;
                            case 4:
                                Bandage();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("You now have " + health + " health.");
                                Console.ResetColor();
                                break;
                            default:
                                utils.WriteColor("Invalid input.", ConsoleColor.Red, Environment.NewLine);
                                break;
                        }
                    }
                }
                else if (hitChance == 0)
                {
                    Console.Write("You fire at the enemy but miss." + Environment.NewLine);

                    if (rand.Next(0, 2) == 0)
                    {
                        Console.WriteLine("The enemy fires but misses you.");
                    }
                    else
                    {
                        health -= enemy.damage;

                        Console.Write("The enemy hits you and deals ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(enemy.damage);
                        Console.ResetColor();
                        Console.Write(" damage.");
                        Console.Write(Environment.NewLine);

                        if (health <= 0)
                        {
                            GameOver();
                        }
                        else
                        {
                            CombatMenu();

                            string? choice = Console.ReadLine();
                            if (!int.TryParse(choice, out int menuChoice))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Invalid choice. Please try again.");
                                Console.ResetColor();
                                continue;
                            }

                            switch (menuChoice)
                            {
                                case 1:
                                    Console.WriteLine("You open fire on the enemy!" + Environment.NewLine);
                                    break;
                                case 2:
                                    Console.WriteLine("You attempt to take cover." + Environment.NewLine);

                                    int enemyHitChance = rand.Next(0, 3);

                                    if (enemyHitChance == 0)
                                    {
                                        Console.WriteLine("The enemy missed you while you were in cover.");
                                    }
                                    else
                                    {
                                        utils.SmallArms();

                                        health -= enemy.damage;

                                        Console.Write("The enemy hits you while you were in cover and deals ");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(enemy.damage);
                                        Console.ResetColor();
                                        Console.Write(" damage.");
                                        Console.Write(Environment.NewLine);

                                        if (health <= 0)
                                        {
                                            GameOver();
                                        }
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("You attempt to retreat." + Environment.NewLine);

                                    int retreatChance = rand.Next(0, 2);

                                    if (retreatChance == 0)
                                    {
                                        Console.WriteLine("You successfully retreat." + Environment.NewLine);
                                        utils.Move();
                                        inCombat = false;
                                    }
                                    else
                                    {
                                        utils.SmallArms();

                                        Console.WriteLine("You attempt to retreat but fail." + Environment.NewLine);

                                        health -= enemy.damage;

                                        Console.Write("The enemy hits you and deals ");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(enemy.damage);
                                        Console.ResetColor();
                                        Console.Write(" damage.");
                                        Console.Write(Environment.NewLine);

                                        if (health <= 0)
                                        {
                                            GameOver();
                                        }
                                    }
                                    break;
                                case 4:
                                    Bandage();

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("You now have " + health + " health.");
                                    Console.ResetColor();
                                    break;
                                default:
                                    utils.WriteColor("Invalid input.", ConsoleColor.Red, Environment.NewLine);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        static void Villager()
        {
            inShop = true;

            Console.WriteLine("You encounter a villager!" + Environment.NewLine);
            Console.WriteLine("He seems to have a few items to trade." + Environment.NewLine);

            while (health > 0 && inShop && inCombat == false)
            {
                ShopMenu();

                string? choice = Console.ReadLine();
                if (!int.TryParse(choice, out int menuChoice))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ResetColor();
                    continue;
                }

                switch (menuChoice)
                {
                    case 1:
                        if (HasEnoughEnergy(1))
                        {
                            BuyAmmo();
                        }
                        break;
                    case 2:
                        if (HasEnoughEnergy(1))
                        {
                            SellAmmo();
                        }
                        break;
                    case 3:
                        if (HasEnoughEnergy(1))
                        {
                            BuyFood();
                        }
                        break;
                    case 4:
                        if (HasEnoughEnergy(1))
                        {
                            BuyWater();
                        }
                        break;
                    case 5:
                        if (HasEnoughEnergy(1))
                        {
                            SellFood();
                        }
                        break;
                    case 6:
                        if (HasEnoughEnergy(1))
                        {
                            SellWater();
                        }
                        break;
                    case 7:
                        if (HasEnoughEnergy(1))
                        {
                            inShop = false;
                            break;
                        }
                        break;
                    case 9:
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static void Animal()
        {
                inCombat = true;

                Console.WriteLine("You encounter an animal!" + Environment.NewLine);
                Console.WriteLine("You open fire on the animal!" + Environment.NewLine);

            while (health > 0 && inShop == false && inCombat == true)
            {
                    Item? ammoItem = inventory.Find(item => item.Name == "Ammunition");
                    if (ammoItem != null && ammoItem.Quantity > 0)
                    {
                        ammoItem.Quantity -= 1;
                        ammo -= 1;
                    }

                    utils.SmallArms();
                    AnimalMenu();

                    if (ammo <= 0)
                    {
                        Console.WriteLine("You have no ammunition left.");

                        Console.WriteLine("What do you want to do?" + Environment.NewLine);
                        Console.WriteLine("1. Retreat.");
                        Console.WriteLine("##############################");

                        string? choice = Console.ReadLine();
                        if (!int.TryParse(choice, out int menuChoice))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Invalid choice. Please try again.");
                            Console.ResetColor();
                            continue;
                        }

                        switch (menuChoice)
                        {
                            case 1:
                                if (HasEnoughEnergy(1))
                                {
                                    Console.WriteLine("You retreat." + Environment.NewLine);
                                    utils.Move();
                                    inCombat = false;
                                }
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Invalid choice. Please try again.");
                                Console.ResetColor();
                                break;
                        }
                    }
                else
                {
                        int dealDamage = rand.Next(30, 100);
                        int hitChance = rand.Next(0, 2);

                    if (hitChance == 1)
                    {
                        animal.health -= dealDamage;

                        Console.Write("You manage to hit the animal and deal ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(dealDamage);
                        Console.ResetColor();
                        Console.Write(" damage. ");
                        Console.Write(Environment.NewLine);

                        if (animal.health <= 0)
                        {
                            inCombat = false;

                            Console.Write("You successfully kill the animal!" + Environment.NewLine);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("You gain 2 meat." + Environment.NewLine);
                            Console.ResetColor();

                            Item? foodItem = inventory.Find(item => item.Name == "Food");
                            if (foodItem != null && foodItem.Quantity >= 0)
                            {
                                foodItem.Quantity += 2;
                                foodCount += 2;
                            }
                            else
                            {
                                inventory.Add(new Item("Food", 2));
                            }
                        }
                        else
                        {
                            string? choice = Console.ReadLine();
                            if (!int.TryParse(choice, out int menuChoice))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Invalid choice. Please try again.");
                                Console.ResetColor();
                                continue;
                            }

                            switch (menuChoice)
                            {
                                case 1:
                                    if (HasEnoughEnergy(1))
                                    {
                                        Console.WriteLine("You open fire on the animal!" + Environment.NewLine);

                                        int runChance = rand.Next(0, 3);
                                        if (runChance == 0)
                                        {
                                            Console.WriteLine("The animal runs away!" + Environment.NewLine);
                                            inCombat = false;
                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (HasEnoughEnergy(1))
                                    {
                                        Console.WriteLine("You retreat." + Environment.NewLine);
                                        inCombat = false;
                                    }
                                    break;
                                default:
                                    utils.WriteColor("Invalid input.", ConsoleColor.Red, Environment.NewLine);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("You fire at the animal but miss!");
                    }
                }
            }
        }

        #region Buy/Sell
        static void BuyAmmo()
        {
            Item? marksItem = inventory.Find(item => item.Name == "Marks");
            if (marksItem != null && marksItem.Quantity >= 10)
            {
                marksItem.Quantity -= 10;
                marks -= 10;

                Item? ammoItem = inventory.Find(item => item.Name == "Ammunition");
                if (ammoItem != null)
                {
                    ammoItem.Quantity += 5;
                    ammo += 5;
                }
                else
                {
                    inventory.Add(new Item("Ammunition", 5));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You purchase some ammunition." + Environment.NewLine);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You do not have enough money." + Environment.NewLine);
            }
        }

        static void SellAmmo()
        {
            Item? ammoItem = inventory.Find(item => item.Name == "Ammunition");
            if (ammoItem != null && ammoItem.Quantity >= 5)
            {
                ammoItem.Quantity -= 5;
                ammo -= 5;

                Item? marksItem = inventory.Find(item => item.Name == "Marks");
                if (marksItem != null)
                {
                    marksItem.Quantity += 5;
                    marks += 5;
                }
                else
                {
                    inventory.Add(new Item("Marks", 5));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You sell some ammunition." + Environment.NewLine);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You do not have enough ammunition." + Environment.NewLine);
            }
        }

        static void BuyFood()
        {
            Item? marksItem = inventory.Find(item => item.Name == "Marks");
            if (marksItem != null && marksItem.Quantity >= 15)
            {
                marksItem.Quantity -= 15;
                marks -= 15;

                Item? foodItem = inventory.Find(item => item.Name == "Food");
                if (foodItem != null)
                {
                    foodItem.Quantity += 1;
                    foodCount += 1;
                }
                else
                {
                    inventory.Add(new Item("Food", 1));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You purchase some food." + Environment.NewLine);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You do not have enough money." + Environment.NewLine);
            }
        }

        static void BuyWater()
        {
            Item? marksItem = inventory.Find(item => item.Name == "Marks");
            if (marksItem != null && marksItem.Quantity >= 15)
            {
                marksItem.Quantity -= 15;
                marks -= 15;

                Item? waterItem = inventory.Find(item => item.Name == "Water");
                if (waterItem != null)
                {
                    waterItem.Quantity += 1;
                    waterCount += 1;
                }
                else
                {
                    inventory.Add(new Item("Water", 1));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You purchase some water." + Environment.NewLine);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You do not have enough money." + Environment.NewLine);
            }
        }

        static void SellFood()
        {
            Item? foodItem = inventory.Find(item => item.Name == "Food");
            if (foodItem != null && foodItem.Quantity >= 1)
            {
                foodItem.Quantity -= 1;
                foodCount -= 1;

                Item? marksItem = inventory.Find(item => item.Name == "Marks");
                if (marksItem != null)
                {
                    marksItem.Quantity += 10;
                    marks += 10;
                }
                else
                {
                    inventory.Add(new Item("Marks", 10));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You sell some food." + Environment.NewLine);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You do not have enough food." + Environment.NewLine);
            }
        }

        static void SellWater()
        {
            Item? waterItem = inventory.Find(item => item.Name == "Water");
            if (waterItem != null && waterItem.Quantity >= 1)
            {
                waterItem.Quantity -= 1;
                waterCount -= 1;

                Item? marksItem = inventory.Find(item => item.Name == "Marks");
                if (marksItem != null)
                {
                    marksItem.Quantity += 10;
                    marks += 10;
                }
                else
                {
                    inventory.Add(new Item("Marks", 10));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You sell some water." + Environment.NewLine);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You do not have enough water." + Environment.NewLine);
            }
        }
        #endregion

        static void GameOver()
        {
            if (health < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You die from major injuries.");
                Console.ResetColor();
                Environment.Exit(2);
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Item(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}