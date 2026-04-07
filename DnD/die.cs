using System;
public class Die
{
    private static Random random = new Random();
    public int Sides { get; set; }
    public Die(int sides)
    {
        Sides = sides;
    }
    public int Roll()
    {
        return random.Next(1, Sides + 1);
    }
}
