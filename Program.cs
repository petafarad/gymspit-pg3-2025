Console.WriteLine("Zadejte celé kladné číslo:");
int N;
while (!int.TryParse(Console.ReadLine(), out N) || N <= 0)  //copilot
{
    Console.WriteLine("Zadaná hodnota není celé kladné číslo. Zkuste to znovu:");
}
for (int i = 1; i <= N; i+=1)
{
    if (i % 3 == 0 && i % 5 == 0)
        Console.Write("FizzBuzz");
    else if (i % 3 == 0)
        Console.Write("Fizz");
    else if (i % 5 == 0)
        Console.Write("Buzz");
else

    Console.Write(i);
    if (i < N) Console.Write(", "); // copilot
}

// + úprava syntaxe copilot
