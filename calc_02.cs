using System;

static void PrintMenu()
{
    Console.WriteLine("Calculator");
    Console.WriteLine("Choose an operation:");
    Console.WriteLine("+ : Addition");
    Console.WriteLine("- : Subtraction");
    Console.WriteLine("* : Multiplication");
    Console.WriteLine("/ : Division");
    Console.WriteLine("Type 'end' to exit the calculator");
}

static char ReadOperation()
{
    string operation = Console.ReadLine() ?? "";
    if (operation == "end")
    {
        Console.WriteLine("calculator terminated");
        Environment.Exit(0);
    }
    while (operation != "+" && operation != "-" && operation != "*" && operation != "/")
    {
        Console.WriteLine("Invalid operation. Please select a valid operation (+, -, *, /):");
        operation = Console.ReadLine() ?? "";
        if (operation == "end")
        {
            Console.WriteLine("calculator terminated");
            Environment.Exit(0);
        }
    }
    return operation[0];
}

static (double operand1, double operand2) ReadDouble(char operation)
{
    double operand1 = 0;
    double operand2 = 0;
    Console.WriteLine("Enter first number:");
    while (!double.TryParse(Console.ReadLine(), out operand1))
    {
        Console.WriteLine("Invalid input. Please enter a valid integer for the first number:");
    }
    Console.WriteLine("Enter second number:");
    while (true)
    {
        if (!double.TryParse(Console.ReadLine(), out operand2))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer for the second number:");
            continue;
        }
        if (operation == '/' && operand2 == 0)
        {
            Console.WriteLine("Error: Division by zero is not allowed.");
            Console.WriteLine("Enter second number:");
            continue;
        }
        break;
    }
    return (operand1, operand2);
}

static double Compute(char operation, double operand1, double operand2)
{ 
    if (operation == '+')
    {
        return operand1 + operand2;
    }
    else if (operation == '-')
    {
        return operand1 - operand2;
    }
    else if (operation == '*')
    {
        return operand1 * operand2;
    }
    else if (operation == '/')
    {
        return operand1 / operand2;
    }
    else
    {
        throw new InvalidOperationException("Invalid operation");
    }
}

static void PrintResult(char operation, double operand1, double operand2, double result)
{
    Console.WriteLine("{0} {1} {2} = {3}", operand1, operation, operand2, result);
}
                                               // loop od copilota
while (true)
{
    PrintMenu();
    char operation = ReadOperation();
    var (operand1, operand2) = ReadDouble(operation);
    double result = Compute(operation, operand1, operand2);
    PrintResult(operation, operand1, operand2, result);
    Console.WriteLine();
    
}