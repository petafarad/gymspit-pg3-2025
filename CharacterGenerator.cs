using System;

// Statickou třídu nelze instanciovat pomocí 'new'. Slouží jako kontejner pro statické metody a pole.
// To je užitečné pro pomocné (utility) třídy jako je tato, která poskytuje funkce pro generování postav.
public static class CharacterGenerator
{
    // 'private' znamená, že tato pole jsou přístupná pouze zevnitř této třídy.
    // 'static' znamená, že napříč celou aplikací existuje pouze jedna sdílená kopie těchto polí, což šetří paměť.
    private static string[] enemyNames = { "Goblin", "Orc", "Troll", "Skeleton", "Zombie" };
    private static string[] heroNames = { "John", "Lancelot", "Merlin", "Gimli", "Aragorn" };

    // Tato metoda vygeneruje náhodného Nepřítele a vrátí ho. Přijímá celé číslo 'level' pro škálování statistik.
    public static Enemy GenerateRandomEnemy(int level)
    {
        // Vybere náhodné jméno z pole pomocí Random.Shared.Next(maxValue)
        string name = enemyNames[Random.Shared.Next(enemyNames.Length)];

        // Vypočte statistiky na základě základních náhodných hodnot plus škálovací faktor odvozený z úrovně.
        int hp = Random.Shared.Next(30, 50) + (level * 10);
        // Statistika útoku je náhodná mezi 3 a 7 (horní hranice u Next je exkluzivní), plus level.
        int attack = Random.Shared.Next(3, 8) + level;
        // Obrana je náhodná mezi 1 a 3, plus level.
        int defense = Random.Shared.Next(1, 4) + level;
        int agility = Random.Shared.Next(5, 12);

        // Vytvoří novou instanci objektu přítel s vypočítanými hodnotami.
        Enemy newEnemy = new Enemy(name, hp, attack, defense, agility);

        // Logika: 10% šance, že se nepřítel objeví s drakem.
        // Next(100) vygeneruje číslo od 0 do 99. Pokud je menší než 10, představuje to 10% pravděpodobnost.
        if (Random.Shared.Next(100) < 10)
        {
            newEnemy.Mount = new Dragon("Wild " + name);
            newEnemy.Name = "Dragon Rider " + name; // Dáme jim super titul
        }

        return newEnemy;
    }

    // Tato metoda vygeneruje náhodného Hrdinu za použití stejných principů škálování.
    public static Hero GenerateRandomHero(int level)
    {
        string name = heroNames[Random.Shared.Next(heroNames.Length)];
        int hp = Random.Shared.Next(50, 80) + (level * 20);
        int attack = Random.Shared.Next(5, 12) + level;
        int defense = Random.Shared.Next(3, 8) + level;
        int agility = Random.Shared.Next(8, 15);

        // Vrátí nově vytvořenou instanci Hrdiny.
        return new Hero(name, hp, attack, defense, agility);
    }
}
