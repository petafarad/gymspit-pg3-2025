using System;

// Výraz enum (výčet) definuje vlastní typ, který může mít pouze jednu z několika předdefinovaných hodnot.
// Zde ho používáme k určení, zda je drak útočného (Attack) nebo obranného (Defense) typu.
public enum DragonType { Attack, Defense }

// Třída Dragon reprezentuje jezdecké zvíře. Nedědí od Character, protože nemá vlastní zdraví ani tahy.
public class Dragon
{
    // Vlastnosti (properties) definují data, která objekt může uchovávat. 'set' umožňuje přiřadit hodnotu, 'get' umožňuje její přečtení.
    public string Name { get; set; }
    public DragonType Type { get; set; }
    // Tyto vlastnosti uchovávají bonusové statistiky, které drak poskytuje svému jezdci.
    public int AttackBonus { get; set; }
    public int DefenseBonus { get; set; }

    // Výchozí hodnota inicializovaná přímo na vlastnosti. Pokud jezdec zemře, drak zůstane na bojišti po dobu 3 kol, než uletí.
    public int RoundsToFlee { get; set; } = 3;

    // Konstruktor je speciální metoda volaná při vytvoření nové instance třídy (pomocí klíčového slova 'new').
    public Dragon(string name)
    {
        Name = name;

        // Random.Shared.Next(2) vrátí 0 nebo 1. Přetypujeme to (cast) na náš DragonType enum a tím náhodně vybereme jeho typ.
        Type = (DragonType)Random.Shared.Next(2); // 50/50 šance na útočného nebo obranného draka

        // V závislosti na náhodně vygenerovaném typu přiřadíme různé statistické bonusy.
        if (Type == DragonType.Attack)
        {
            AttackBonus = 15;
            DefenseBonus = 5;
        }
        else
        {
            AttackBonus = 5;
            DefenseBonus = 15;
        }

        // Vypíše informace o vygenerovaném objektu draka do konzole.
        Console.WriteLine($"Generated {Type} dragon named {Name}! (+{AttackBonus} ATK, +{DefenseBonus} DEF)");
    }

    // Metody definují akce, které objekt může vykonávat.
    // Tuto metodu voláme, když jezdec zaútočí, zatímco sedí na tomto drakovi.
    public void DragonFight(Character attacker, Character target, int totalDamage)
    {
        Console.WriteLine($"{attacker.Name} on a dragon breathes fire on {target.Name}! (+{AttackBonus} Attack bonus!)");
        // Předáme vypočítané posílené poškození do metody pro obdržení poškození na cílové postavě.
        target.TakeDamage(totalDamage);
    }

    // Tuto metodu voláme, když se jezdec brání, zatímco sedí na tomto drakovi.
    public void DragonDefend(Character defender)
    {
        Console.WriteLine($"{defender.Name}'s dragon wraps itself in scales! (+{DefenseBonus} Defense bonus!)");
        // Nastavíme pro jezdce obranný postoj, což spustí logiku snížení poškození při jeho dalším výpočtu škod.
        defender.IsDefending = true;
    }
}
