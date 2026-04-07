using System;

// Jednoduchá třída reprezentující kostku, kterou lze házet.
public class Die
{
    // Privátní statická instance třídy Random. 
    // Používáme jednu statickou instanci pro všechny kostky, abychom se vyhnuli generování stejných čísel, pokud se hází více kostkami přesně ve stejný čas.
    private static Random random = new Random();

    // Vlastnost (property) uchovávající počet stěn, které tato konkrétní kostka má (např. 20 pro d20, 6 pro d6).
    public int Sides { get; set; }

    // Konstruktor. Když vytvoříš novou kostku (Die), musíš říct, kolik má stěn.
    public Die(int sides)
    {
        Sides = sides;
    }

    // Metoda Roll simuluje hod kostkou.
    // Metoda Random.Next(min, max) vrací číslo od 'min' až po (ale bez) 'max'.
    // Proto používáme 1 jako min a Sides + 1 jako max.
    public int Roll()
    {
        return random.Next(1, Sides + 1);
    }
}
