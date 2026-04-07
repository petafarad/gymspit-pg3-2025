using System;
using System.Collections.Generic;
using System.Linq;

// Třída Hero (Hrdina) dědí ze základní třídy Character (Postava).
// To umožňuje Hero opakovaně využít základní logiku (zdraví, statistiky, zranitelnost) definovanou v Character.
public class Hero : Character
{
    // Konstruktor předává inicializační parametry dolů k základnímu konstruktoru Character.
    public Hero(string name, int maxHealth, int attack, int defense, int agility) : base(name, maxHealth, attack, defense, agility)
    {
    }

    // Přepisujeme metodu ChooseAction pro poskytnutí logiky ovládané hráče namísto logiky AI.
    public override void ChooseAction(List<Hero> heroes, List<Enemy> enemies, List<Dragon> freeDragons)
    {
        // Odfiltruj mrtvé nepřátele, aby hráč mohl mířit pouze na ty, kteří ještě stojí.
        var aliveEnemies = enemies.Where(e => e.IsAlive).ToList();
        if (aliveEnemies.Count == 0) return; // Skončí tah, pokud nezbývají žádní nepřátelé

        bool validChoice = false;

        // Smyčka 'while' slouží k opakovanému vyžádání hráče na dotazování dokud nedá platnou volbu.
        while (!validChoice)
        {
            Console.WriteLine($"\n--- {Name}'s turn ---");
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Defend");

            // Ukaž třetí volbu jen pokud hrdina už nemá draka a zároveň je k dispozici alespoň jeden volný drak.
            if (Mount == null && freeDragons.Count > 0)
            {
                Console.WriteLine("3. Tame a free dragon");
            }

            // int.TryParse zkouší přeměnit vstup uživatele z řetězce (Console.ReadLine()) na celé číslo.
            // Vrací true při úspěchu a vloží výsledek do 'choice'. To zabrání pádu hry, pokud uživatel zadá písmena.
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Choose a target:");
                        // Smyčka 'for' je použita pro iteraci skrze seznam nepřátel a jejich zobrazení jako možností menu.
                        for (int i = 0; i < aliveEnemies.Count; i++)
                        {
                            // K indexu i přidáme 1 pro srozumitelnější zobrazení číslované od jedničky.
                            Console.WriteLine($"{i + 1}. {aliveEnemies[i].Name} (HP: {aliveEnemies[i].CurrentHealth}/{aliveEnemies[i].MaxHealth})");
                        }

                        // Ověříme vstup cíle, abychom se ujistili, že jde o číslo, je větší než 0 a není větší než množství nepřátel.
                        if (int.TryParse(Console.ReadLine(), out int targetChoice) && targetChoice > 0 && targetChoice <= aliveEnemies.Count)
                        {
                            Fight(aliveEnemies[targetChoice - 1]); // Pole a seznamy jsou indexované od nuly, zmenšíme tedy uživatelův vstup o 1.
                            validChoice = true; // Nastavíme na true, čímž prolomíme while cyklus, protože akce byla úspěšně provedena.
                        }
                        else
                        {
                            Console.WriteLine("Invalid target!");
                            // Protože validChoice je stále false, cyklus while se restartuje a znovu požádá o vstup.
                        }
                        break;
                    case 2:
                        // Zkontroluj zda hrdina má draka. Pokud ano, použij vylepšenou dračí obranu, jinak klasickou.
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
                        // Dvojitá kontrola podmínek pro jistotu, kdyby uživatel zadal '3', když neměl.
                        if (Mount == null && freeDragons.Count > 0)
                        {
                            Mount = freeDragons[0];  // Vem prvního dostupného Draka
                            freeDragons.RemoveAt(0); // Odstraního ho z poolu volných draků
                            Console.WriteLine($"{Name} tamed a free dragon from the battlefield!");
                            validChoice = true;
                        }
                        else
                        {
                            Console.WriteLine("You cannot tame a dragon right now.");
                        }
                        break;
                    default:
                        // 'default' pravidlo (výchozí) řeší jakákoliv čísla, která nejsou specificky zachycena v připadech 'case' výše.
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
