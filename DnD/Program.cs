using System;
using System.Collections.Generic;
using System.Linq;

List<Hero> heroes = new List<Hero>();
List<Enemy> enemies = new List<Enemy>();

int heroCount = 0;

while (true)
{
    Console.WriteLine("Enter number of heroes:");
    if (int.TryParse(Console.ReadLine(), out heroCount) && heroCount > 0)
    {
        break;
    }
    Console.WriteLine("Invalid input! Please enter a valid positive number.");
}

int enemyCount = 0;
while (true)
{
    Console.WriteLine("Enter number of enemies:");
    if (int.TryParse(Console.ReadLine(), out enemyCount) && enemyCount > 0)
    {
        break;
    }
    Console.WriteLine("Invalid input! Please enter a valid positive number.");
}

Console.WriteLine("Generating heroes...");
for (int i = 0; i < heroCount; i++)
{
    heroes.Add(CharacterGenerator.GenerateRandomHero(1));
}

Console.WriteLine("Generating enemies...");
for (int i = 0; i < enemyCount; i++)
{
    enemies.Add(CharacterGenerator.GenerateRandomEnemy(1));
}

Die initiativeDie = new Die(20);
List<Character> allCharacters = new List<Character>();
allCharacters.AddRange(heroes);
allCharacters.AddRange(enemies);

Console.WriteLine("\n--- Rolling Initiative ---");

foreach (Character character in allCharacters)
{
    character.Initiative = initiativeDie.Roll() + character.Agility;
    Console.WriteLine($"{character.Name} rolled an initiative of {character.Initiative}");
}

allCharacters.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));


int roundNumber = 1;
List<Dragon> freeDragons = new List<Dragon>();

while (heroes.Any(h => h.IsAlive) && enemies.Any(e => e.IsAlive))
{
    Console.WriteLine("\n==============================");
    Console.WriteLine($"--- Round {roundNumber} ---");
    foreach (Character currentCharacter in allCharacters)
    {
        if (!currentCharacter.IsAlive)
        {
            if (currentCharacter.Mount != null)
            {
                currentCharacter.Mount.RoundsToFlee = 3;
                freeDragons.Add(currentCharacter.Mount);
                Console.WriteLine($"\n{currentCharacter.Name} died and their dragon '{currentCharacter.Mount.Name}' is now free! Anyone can mount it in the next {currentCharacter.Mount.RoundsToFlee} rounds.");

                currentCharacter.Mount = null;
            }

            continue;
        }
        if (!heroes.Any(h => h.IsAlive) || !enemies.Any(e => e.IsAlive))
        {
            break;
        }

        currentCharacter.IsDefending = false;
        currentCharacter.ChooseAction(heroes, enemies, freeDragons);
    }
    for (int i = freeDragons.Count - 1; i >= 0; i--)
    {
        freeDragons[i].RoundsToFlee--;
        if (freeDragons[i].RoundsToFlee <= 0)
        {
            Console.WriteLine($"\nThe free dragon '{freeDragons[i].Name}' flew away from the battlefield!");
            freeDragons.RemoveAt(i);
        }
    }

    roundNumber++;
}

if (heroes.Any(h => h.IsAlive))
{
    Console.WriteLine("\n==============================");
    Console.WriteLine("Heroes won!");
}
else
{
    Console.WriteLine("\n==============================");
    Console.WriteLine("Enemies won!");
}