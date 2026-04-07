using System;
using System.Collections.Generic;
using System.Linq;


public class Enemy : Character
{
    public Enemy(string name, int maxHealth, int attack, int defense, int agility)
    : base(name, maxHealth, attack, defense, agility)
    {
    }

    public override void ChooseAction(List<Hero> heroes, List<Enemy> enemies, List<Dragon> freeDragons)
    {
        var aliveHeroes = heroes.Where(h => h.IsAlive).ToList();

        if (aliveHeroes.Count == 0) return;

        Console.WriteLine($"\n--- {Name}'s turn ---");


        if (Mount == null && freeDragons.Count > 0 && Random.Shared.Next(2) == 0)
        {
            Mount = freeDragons[0];
            freeDragons.RemoveAt(0);
            Console.WriteLine($"{Name} tamed a free dragon and transforms into a Dragon Rider!");
            Name = "Dragon Rider " + Name;
            return;
        }

        int choice = Random.Shared.Next(2);

        switch (choice)
        {
            case 0:
                Character target = aliveHeroes[Random.Shared.Next(aliveHeroes.Count)];
                Fight(target);
                break;
            case 1:
                if (Mount != null)
                {
                    Mount.DragonDefend(this);
                }
                else
                {
                    Defend();
                }
                break;
        }
    }
}