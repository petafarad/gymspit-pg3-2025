using System;


public static class CharacterGenerator
{
    private static string[] enemyNames = { "Goblin", "Orc", "Troll", "Skeleton", "Zombie" };
    private static string[] heroNames = { "John", "Lancelot", "Merlin", "Gimli", "Aragorn" };

    public static Enemy GenerateRandomEnemy(int level)
    {
        string name = enemyNames[Random.Shared.Next(enemyNames.Length)];
        int hp = Random.Shared.Next(30, 50) + (level * 10);
        int attack = Random.Shared.Next(3, 8) + level;
        int defense = Random.Shared.Next(1, 4) + level;
        int agility = Random.Shared.Next(5, 12);

        Enemy newEnemy = new Enemy(name, hp, attack, defense, agility);
        if (Random.Shared.Next(100) < 10)
        {
            newEnemy.Mount = new Dragon("Wild " + name);
            newEnemy.Name = "Dragon Rider " + name;
        }

        return newEnemy;
    }

    public static Hero GenerateRandomHero(int level)
    {
        string name = heroNames[Random.Shared.Next(heroNames.Length)];
        int hp = Random.Shared.Next(50, 80) + (level * 20);
        int attack = Random.Shared.Next(5, 12) + level;
        int defense = Random.Shared.Next(3, 8) + level;
        int agility = Random.Shared.Next(8, 15);

        return new Hero(name, hp, attack, defense, agility);
    }
}
