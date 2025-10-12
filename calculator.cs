float num1 = 0;
float num2 = 0;
float result = 0;
bool end = false;
while (end == false)
{
 
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
Console.WriteLine("Select operation (+, -, *, /) or type 'end' to exit:");
string operation = Console.ReadLine();
if (operation == "end")
{
    Console.WriteLine("calculator terminated");
    end = true;
    break;
    }
while (operation != "+" && operation != "-" && operation != "*" && operation != "/") //copilot
{
    Console.WriteLine("Invalid operation. Please select a valid operation (+, -, *, /):");
    operation = Console.ReadLine();
}
if (operation == "+")
{
    result = num1 + num2;
}
else if (operation == "-")
{
    result = num1 - num2;
}
else if (operation == "*")
{
    result = num1 * num2;
}
else if (operation == "/")
{
    if (num2 == 0)
    {
        Console.WriteLine("Error: Division by zero is not allowed.");
        return;
    }
    result = num1 / num2;
}
Console.WriteLine("{0} {1} {2} = {3}", num1, operation, num2, result);
}