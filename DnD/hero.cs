using System;
using System.Collections.Generic;
using System.Linq;

public class Hero : Character
{
    public Hero(string name, int maxHealth, int attack, int defense, int agility) : base(name, maxHealth, attack, defense, agility)
    {
    }

    public override void ChooseAction(List<Hero> heroes, List<Enemy> enemies, List<Dragon> freeDragons)
    {
        var aliveEnemies = enemies.Where(e => e.IsAlive).ToList();
        if (aliveEnemies.Count == 0) return;

        bool validChoice = false;

        while (!validChoice)
        {
            Console.WriteLine($"\n--- {Name}'s turn ---");
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Defend");

            if (Mount == null && freeDragons.Count > 0)
            {
                Console.WriteLine("3. Tame a free dragon");
            }

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Choose a target:");
                        for (int i = 0; i < aliveEnemies.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {aliveEnemies[i].Name} (HP: {aliveEnemies[i].CurrentHealth}/{aliveEnemies[i].MaxHealth})");
                        }

                        if (int.TryParse(Console.ReadLine(), out int targetChoice) && targetChoice > 0 && targetChoice <= aliveEnemies.Count)
                        {
                            Fight(aliveEnemies[targetChoice - 1]);
                            validChoice = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid target!");
                        }
                        break;
                    case 2:
                        if (Mount != null)
                        {
                            Mount.DragonDefend(this);
                        }
                        else
                        {
                            Defend();
                        }
                        validChoice = true;
                        break;
                    case 3:
                        if (Mount == null && freeDragons.Count > 0)
                        {
                            Mount = freeDragons[0];
                            freeDragons.RemoveAt(0);
                            Console.WriteLine($"{Name} tamed a free dragon from the battlefield!");
                            validChoice = true;
                        }
                        else
                        {
                            Console.WriteLine("You cannot tame a dragon right now.");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid action number!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please, enter a valid number!");
            }
        }
    }
}
