using System;

public enum DragonType { Attack, Defense }

public class Dragon
{
    public string Name { get; set; }
    public DragonType Type { get; set; }
    public int AttackBonus { get; set; }
    public int DefenseBonus { get; set; }

    public int RoundsToFlee { get; set; } = 3;

    public Dragon(string name)
    {
        Name = name;

        Type = (DragonType)Random.Shared.Next(2);

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

        Console.WriteLine($"Generated {Type} dragon named {Name}! (+{AttackBonus} ATK, +{DefenseBonus} DEF)");
    }
    public void DragonFight(Character attacker, Character target, int totalDamage)
    {
        Console.WriteLine($"{attacker.Name} on a dragon breathes fire on {target.Name}! (+{AttackBonus} Attack bonus!)");
        target.TakeDamage(totalDamage);
    }

    public void DragonDefend(Character defender)
    {
        Console.WriteLine($"{defender.Name}'s dragon wraps itself in scales! (+{DefenseBonus} Defense bonus!)");
        defender.IsDefending = true;
    }
}
