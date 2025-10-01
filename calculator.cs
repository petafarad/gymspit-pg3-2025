float num1= 0;
float num2= 0;
Console.WriteLine("Enter first number:");
while (!float.TryParse(Console.ReadLine(), out num1))
    {
    Console.WriteLine("Invalid input. Please enter a valid integer for the first number:");
}
Console.WriteLine("Enter second number:");
while (!float.TryParse(Console.ReadLine(), out num2))
{
    Console.WriteLine("Invalid input. Please enter a valid integer for the second number:");
}
Console.WriteLine("Select operation (+, -, *, /):");
string operation = Console.ReadLine();
while (operation != "+" && operation != "-" && operation != "*" && operation != "/") //copilot
{
    Console.WriteLine("Invalid operation. Please select a valid operation (+, -, *, /):");
    operation = Console.ReadLine();
}

