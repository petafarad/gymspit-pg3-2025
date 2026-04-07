using System;
using System.Collections.Generic;
using System.Linq;

// Tyto Listy (seznamy) budou udržovat všechny momentálně aktivní hrdiny a nepřátele na bojišti.
// Používáme List<T> místo polí (arrays), protože Listy se mohou dynamicky zvětšovat a zmenšovat, když postavy umírají nebo přibývají.
List<Hero> heroes = new List<Hero>();
List<Enemy> enemies = new List<Enemy>();

int heroCount = 0;

// Spouštěcí smyčka hry zjišťuje, kolik hrdinů hráč chce.
// Smyčka 'while (true)' vytváří nekonečnou smyčku, kterou lze opustit pouze pomocí příkazu 'break'.
while (true)
{
    Console.WriteLine("Enter number of heroes:");
    // TryParse přijme vstup z konzole a pokusí se jej převést na celé číslo.
    // Pokud je to úspěšné, uloží výstup do 'heroCount'. Také zkontrolujeme, zda je číslo > 0.
    if (int.TryParse(Console.ReadLine(), out heroCount) && heroCount > 0)
    {
        break; // Vstup byl platný, opusť smyčku!
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
// Cyklus typu for běží přesný počet opakování. Iterujeme 'heroCount'-krát, abychom vytvořili správný počet hrdinů.
for (int i = 0; i < heroCount; i++)
{
    // Zavoláme náš statický generátor a přidáme výsledného Hero (Hrdinu) do našeho seznamu (List) hrdinů.
    heroes.Add(CharacterGenerator.GenerateRandomHero(1));
}

Console.WriteLine("Generating enemies...");
for (int i = 0; i < enemyCount; i++)
{
    // Generátor může také zplodit dračího jezdce na základě své interní 10% šance.
    enemies.Add(CharacterGenerator.GenerateRandomEnemy(1));
}

// Vytvoříme speciálně dvacetistěnnou kostku pro hody na iniciativu (určující pořadí tahů).
Die initiativeDie = new Die(20);

// Spojíme všechny dohromady do jednoho seznamu, abychom je mohli všechny seřadit podle toho, kdo jde první.
List<Character> allCharacters = new List<Character>();
allCharacters.AddRange(heroes);
allCharacters.AddRange(enemies);

Console.WriteLine("\n--- Rolling Initiative ---");

// Projdeme smyčkou každou postavu a jedenkrát jí hodíme iniciativu před samotným začátkem bitvy.
foreach (Character character in allCharacters)
{
    // Vypočte skóre iniciativy (Hod D20 kostkou + modifikátor Agility/mrštnosti).
    character.Initiative = initiativeDie.Roll() + character.Agility;
    Console.WriteLine($"{character.Name} rolled an initiative of {character.Initiative}");
}

// Seřadí seznam podle vypočítané vlastnosti Initiative.
// Lambda výraz (a, b) => b.Initiative.CompareTo(a.Initiative) zadá funkci Sort k porovnání
// sestupně (největší číslo půjde první).
allCharacters.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));


int roundNumber = 1;

// Tento seznam (List) si bude pamatovat opuštěné draky na bojišti po smrti jejich jezdce.
List<Dragon> freeDragons = new List<Dragon>();

// Základní smyčka Hry (Core Game Loop). Běží tak dlouho, dokud mají OBĚ strany alespoň jednoho žijicího člena.
// .Any() je silná LINQ metoda, která vrací "true" už i při jedné položce splňující podmínku.
while (heroes.Any(h => h.IsAlive) && enemies.Any(e => e.IsAlive))
{
    Console.WriteLine("\n==============================");
    Console.WriteLine($"--- Round {roundNumber} ---");

    // Každým opakováním jdeme přes náš už seřazený seznam postav v určeném pořadí tahů.
    foreach (Character currentCharacter in allCharacters)
    {
        // 1. Je momentálně hodnocená postava mrtvá?
        if (!currentCharacter.IsAlive)
        {
            // Pokud jsou mrtví a přesto drží odkaz na draka, znamená to, že zemřeli dříve v tomto kole.
            // Musíme uvolnit jejich draka do divočiny (seznam freeDragons).
            if (currentCharacter.Mount != null)
            {
                currentCharacter.Mount.RoundsToFlee = 3; // Zresetuj dračí odpočet útěku
                freeDragons.Add(currentCharacter.Mount);
                Console.WriteLine($"\n{currentCharacter.Name} died and their dragon '{currentCharacter.Mount.Name}' is now free! Anyone can mount it in the next {currentCharacter.Mount.RoundsToFlee} rounds.");

                // Vyčisti pozůstatek mounta v záznamu mrtvoly k zamezení opakovaného přidání v dalším kole.
                currentCharacter.Mount = null;
            }

            // 'continue' přeskočí zbytek kódu v cyklu 'foreach' a přejde k další postavě na listu tahů.
            continue;
        }

        // 2. Konec bitevních podmínek uprostřed kola
        // Je tu možnost, že někdo zabil posledního nepřítele při svém tahu. Musíme provést kontrolu, před tahem někoho dalšího.
        if (!heroes.Any(h => h.IsAlive) || !enemies.Any(e => e.IsAlive))
        {
            // 'break' násilně ukončí celou strukturu cyklu (zde náš foreach cyklus).
            break;
        }

        // 3. Expirace obranného postoje (Stance)
        // Pokud si postava došla k dalšímu svému tahu, rušíme její obranný postoj z předešlého kola.
        currentCharacter.IsDefending = false;

        // 4. Tahy Akcí
        // Předáme instanci bojiště metodě, aby příslušná AI nebo Vstup hráče (Player Input) rozhodl co dělat.
        currentCharacter.ChooseAction(heroes, enemies, freeDragons);
    }

    // 5. Časovač úletu draka (Nyní vyhodnocováno na konci kola)
    // Iterujeme přes seznam opuštěných draků pozpátku. 
    // Jdeme odzadu (od Count - 1 dolů do 0), protože když bychom při procházení odpředu odstranili prvek ze seznamu, 
    // seznam se posune a mohli bychom omylem přeskočit další prvek.
    for (int i = freeDragons.Count - 1; i >= 0; i--)
    {
        freeDragons[i].RoundsToFlee--; // Snížit časovač o 1
        if (freeDragons[i].RoundsToFlee <= 0)
        {
            Console.WriteLine($"\nThe free dragon '{freeDragons[i].Name}' flew away from the battlefield!");
            freeDragons.RemoveAt(i); // Drak je navždy pryč z bojiště
        }
    }

    // Konec kola, zvyš počítadlo.
    roundNumber++;
}

// Smyčka (while loop) skončila, což znamená selhání výherních podmínek (jedna strana je celá mrtvá).
// My zkontrolujeme, která strana je stále naživu a vytiskneme zprávu o vítězství.
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