string zprava = System.String.Empty;
string sifra = System.String.Empty;
int posun = 0;
static void zasifruj (string zprava, int posun)
{
    foreach (char znak in zprava)
    {
        if (char.IsLetter(znak))
        {
            char zaklad = char.IsUpper(znak) ? 'A' : 'a';
            char sifrovanyZnak = (char)((((znak + posun) - zaklad) % 26) + zaklad);
            System.Console.Write(sifrovanyZnak);
        }
        else
        {
            System.Console.Write(znak);
        }
    }
    System.Console.WriteLine();
}

static void desifruj (string sifra, int posun)
{
    foreach (char znak in sifra)
    {
        if (char.IsLetter(znak))
        {
            char zaklad = char.IsUpper(znak) ? 'A' : 'a';
            char desifrovanyZnak = (char)((((znak - posun + 26) - zaklad) % 26) + zaklad);
            System.Console.Write(desifrovanyZnak);
        }
        else
        {
            System.Console.Write(znak);
        }
    }
    System.Console.WriteLine();
}

System.Console.Write("Vyberte akci (1 - zašifrovat, 2 - dešifrovat): ");
string volba = System.Console.ReadLine();   
if (volba == "1")
{
    System.Console.Write("Zadejte zprávu k zašifrování: ");
    zprava = System.Console.ReadLine();
    System.Console.Write("Zadejte posun (číslo): ");
    posun = int.Parse(System.Console.ReadLine());
    System.Console.Write("Zašifrovaná zpráva: ");
    zasifruj(zprava, posun);
}
else if (volba == "2")
{
    System.Console.Write("Zadejte zprávu k dešifrování: ");
    sifra = System.Console.ReadLine();
    System.Console.Write("Zadejte posun (číslo): ");
    posun = int.Parse(System.Console.ReadLine());
    System.Console.Write("Dešifrovaná zpráva: ");
    desifruj(sifra, posun);
}
else
{
    System.Console.WriteLine("Neplatná volba.");
}
