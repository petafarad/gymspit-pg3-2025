using System;

int delkaSeznamu = 10;
int poradi = 0;
bool konec = false;
string[] seznamStudentu = new string[delkaSeznamu];

static void pridejStudenta(string[] seznamStudentu)
{
    Console.WriteLine("Zadejte jmeno studenta:");
    string? jmeno = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(jmeno))
    {
        // Najdi první volné místo v poli
        for (int i = 0; i < seznamStudentu.Length; i++)
        {
            if (string.IsNullOrEmpty(seznamStudentu[i]))
            {
                seznamStudentu[i] = jmeno.ToLower();
                Console.WriteLine("Student přidán na pozici: " + (i + 1));
                return;
            }
        }
        Console.WriteLine("Seznam je plný, nelze přidat dalšího studenta.");
    }
    else
    {
        Console.WriteLine("Jméno nesmí být prázdné.");
    }
}

while (konec == false)
{
    Console.WriteLine("Zadejte příkaz (pridej, vypis, konec):");
    string? prikaz = Console.ReadLine();
    if (prikaz != null)
    {
        prikaz = prikaz.ToLower();
        switch (prikaz)
        {
            case "pridej":
                pridejStudenta(seznamStudentu);
                break;
            case "vypis":
                Console.WriteLine("Seznam studentů:");
                foreach (var student in seznamStudentu)
                {
                    if (!string.IsNullOrEmpty(student))
                    {
                        Console.WriteLine(student);
                    }
                }
                break;
            case "konec":
                konec = true;
                break;
            default:
                Console.WriteLine("Neznámý příkaz.");
                break;
        }



    }
}
Console.WriteLine("Konec programu.");