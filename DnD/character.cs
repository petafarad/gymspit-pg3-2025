using System;
using System.Collections.Generic;

public class Character
{
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Agility { get; set; }
    public int Initiative { get; set; }
    public Dragon? Mount { get; set; }

    public int TotalAttack
    {
        get
        {
            if (Mount != null)
            {
                return Attack + Mount.AttackBonus;
            }
            else
            {
                return Attack;
            }
        }
    }

    public int TotalDefense
    {
        get
        {
            if (Mount != null)
            {
                return Defense + Mount.DefenseBonus;
            }
            else
            {
                return Defense;
            }
        }
    }
    public bool IsDefending { get; set; } = false;
    public bool IsAlive
    {
        get
        {
            return CurrentHealth > 0;
        }
    }
    public Character(string name, int maxHealth, int attack, int defense, int agility)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Attack = attack;
        Defense = defense;
        Agility = agility;
        Console.WriteLine($"{Name} created with {MaxHealth} health, {Attack} attack, {Defense} defense, and {Agility} agility!");
    }
    public void Defend()
    {
        Console.WriteLine($"{Name} takes a defensive stance!");
        IsDefending = true;
    }

    public virtual void TakeDamage(int damage)
    {
        Die defenseDie = new Die(20);
        int totalDefense = TotalDefense;
        int roll = defenseDie.Roll();

        if (roll == 20)
        {
            if (Mount != null)
            {
                Console.WriteLine($"{Name} on a dragon defends! Critical block!");
            }
            else
            {
                Console.WriteLine($"{Name} defends! Critical block!");
            }

            totalDefense = TotalDefense * 2 + roll;
            Console.WriteLine($"(Roll: {roll} + Stat: 2*{TotalDefense} = {totalDefense} Defense)");
        }
        else if (roll == 1)
        {
            if (Mount != null)
            {
                Console.WriteLine($"{Name} on a dragon defends! Critical miss!");
            }
            else
            {
                Console.WriteLine($"{Name} defends! Critical miss!");
            }

            totalDefense = 0;
            Console.WriteLine($"(Critical miss! = {totalDefense} Defense)");
        }
        else
        {
            totalDefense = TotalDefense + roll;

            if (Mount != null)
            {
                Console.WriteLine($"{Name} on a dragon defends! (Roll: {roll} + Stat: {TotalDefense} = {totalDefense} Defense)");
            }
            else
            {
                Console.WriteLine($"{Name} defends! (Roll: {roll} + Stat: {TotalDefense} = {totalDefense} Defense)");
            }
        }


        int damageTaken = damage - totalDefense;
        if (damageTaken < 0)
        {
            damageTaken = 0;
        }


        if (IsDefending)
        {
            damageTaken /= 2;
            Console.WriteLine($"{Name}'s block is effective! Damage reduced to half.");
        }

        Console.WriteLine($"{Name} took {damageTaken} damage!");


        CurrentHealth -= damageTaken;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }


        if (!IsAlive)
        {
            Die();
        }
    }


    public virtual void Fight(Character target)
    {
        Die attackDie = new Die(20);
        int damageDealt = TotalAttack;
        int roll = attackDie.Roll();

        if (roll == 20)
        {
            if (Mount != null)
            {
                Console.WriteLine($"{Name} on a dragon attacks {target.Name}! Critical hit!");
            }
            else
            {
                Console.WriteLine($"{Name} attacks {target.Name}! Critical hit!");
            }
            damageDealt = TotalAttack * 2 + roll;
            Console.WriteLine($"(Roll: {roll} + Stat: 2*{TotalAttack} = {damageDealt} Damage)");
        }
        else if (roll == 1)
        {
            if (Mount != null)
            {
                Console.WriteLine($"{Name} on a dragon attacks {target.Name}! Critical miss!");
            }
            else
            {
                Console.WriteLine($"{Name} attacks {target.Name}! Critical miss!");
            }
            damageDealt = 0;
            Console.WriteLine($"(Critical miss! = {damageDealt} Damage)");
        }
        else
        {
            damageDealt = TotalAttack + roll;

            if (Mount != null)
            {
                Console.WriteLine($"{Name} on a dragon attacks {target.Name}! (Roll: {roll} + Stat: {TotalAttack} = {damageDealt} Damage)");
            }
            else
            {
                Console.WriteLine($"{Name} attacks {target.Name}! (Roll: {roll} + Stat: {TotalAttack} = {damageDealt} Damage)");
            }
        }


        if (Mount != null)
        {
            Mount.DragonFight(this, target, damageDealt);
        }
        else
        {
            target.TakeDamage(damageDealt);
        }
    }
    public virtual void Die()
    {
        Console.WriteLine($"{Name} died!");
    }


    public virtual void ChooseAction(List<Hero> heroes, List<Enemy> enemies, List<Dragon> freeDragons)
    {

    }
}