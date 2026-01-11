using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinancniSpravce
{
    // --- Třída reprezentující jeden záznam (Item) ---
    public class Zaznam
    {
        public int Hodnota { get; set; }
        public string Nazev { get; set; }
        public string Kategorie { get; set; }

        public Zaznam(int hodnota, string nazev, string kategorie)
        {
            Hodnota = hodnota;
            Nazev = nazev;
            Kategorie = kategorie;
        }
    }

    // --- Třída pro správu financí (Logic) ---
    public class SpravceFinanci
    {
        private List<Zaznam> zaznamy;
        private const string SouborDat = "data.txt";

        public SpravceFinanci()
        {
            zaznamy = new List<Zaznam>();
        }

        public void NacistZeSouboru()
        {
            zaznamy.Clear();
            if (File.Exists(SouborDat))
            {
                string[] radky = File.ReadAllLines(SouborDat);
                foreach (string radek in radky)
                {
                    string[] casti = radek.Split('|');
                    if (casti.Length >= 3 && int.TryParse(casti[0], out int hodnota))
                    {
                        zaznamy.Add(new Zaznam(hodnota, casti[1], casti[2]));
                    }
                }
                Console.WriteLine($"Data načteny. Počet záznamů: {zaznamy.Count}");
            }
            else
            {
                Console.WriteLine("Soubor s daty neexistuje, začínáme s prázdným seznamem.");
            }
        }

        public void UlozitDoSouboru()
        {
            using (StreamWriter sw = new StreamWriter(SouborDat))
            {
                foreach (var z in zaznamy)
                {
                    sw.WriteLine($"{z.Hodnota}|{z.Nazev}|{z.Kategorie}");
                }
            }
            Console.WriteLine("Data uložena.");
        }

        public void PridatZaznam(int hodnota, string nazev, string kategorie)
        {
            zaznamy.Add(new Zaznam(hodnota, nazev, kategorie));
            Console.WriteLine("Záznam přidán.");
        }

        public void UpravitZaznam(int index, int novaHodnota, string novyNazev, string novaKategorie)
        {
            if (index >= 0 && index < zaznamy.Count)
            {
                zaznamy[index].Hodnota = novaHodnota;
                zaznamy[index].Nazev = novyNazev;
                zaznamy[index].Kategorie = novaKategorie;
                Console.WriteLine("Záznam upraven.");
            }
            else
            {
                Console.WriteLine("Neplatný index.");
            }
        }

        public void SmazatZaznam(int index)
        {
            if (index >= 0 && index < zaznamy.Count)
            {
                zaznamy.RemoveAt(index);
                Console.WriteLine("Záznam smazán.");
            }
            else
            {
                Console.WriteLine("Neplatný index.");
            }
        }

        public void VypisZaznamy(string filtr = null)
        {
            Console.WriteLine("\n--- Výpis záznamů ---");
            Console.WriteLine("{0,-5} | {1,-10} | {2,-20} | {3,-15} | {4,-10}", "ID", "Hodnota", "Název", "Kategorie", "Zůstatek");
            Console.WriteLine(new string('-', 70));

            long zustatek = 0;
            bool nejakyVypsan = false;

            for (int i = 0; i < zaznamy.Count; i++)
            {
                var z = zaznamy[i];
                zustatek += z.Hodnota;

                bool zobrazit = true;
                if (!string.IsNullOrEmpty(filtr))
                {
                    if (!z.Nazev.Contains(filtr, StringComparison.OrdinalIgnoreCase))
                    {
                        zobrazit = false;
                    }
                }

                if (zobrazit)
                {
                    Console.WriteLine("{0,-5} | {1,-10} | {2,-20} | {3,-15} | {4,-10}", i + 1, z.Hodnota, z.Nazev, z.Kategorie, zustatek);
                    nejakyVypsan = true;
                }
            }

            if (!nejakyVypsan)
            {
                Console.WriteLine("Žádné záznamy k zobrazení.");
            }
            Console.WriteLine(new string('-', 70));
        }

        public void ZobrazitStatistiky()
        {
            if (zaznamy.Count == 0)
            {
                Console.WriteLine("Žádná data pro statistiku.");
                return;
            }

            var prijmy = zaznamy.Where(z => z.Hodnota > 0).ToList();
            var vydaje = zaznamy.Where(z => z.Hodnota < 0).ToList();

            Console.WriteLine("\n--- Statistiky ---");
            Console.WriteLine($"Počet příjmů: {prijmy.Count}, Součet příjmů: {prijmy.Sum(z => (long)z.Hodnota)}");
            Console.WriteLine($"Počet výdajů: {vydaje.Count}, Součet výdajů: {vydaje.Sum(z => (long)z.Hodnota)}");

            if (prijmy.Any())
            {
                Console.WriteLine($"Nejvyšší příjem: {prijmy.Max(z => z.Hodnota)}");
                Console.WriteLine($"Nejmenší příjem: {prijmy.Min(z => z.Hodnota)}");
            }

            if (vydaje.Any())
            {
                Console.WriteLine($"Největší výdaj (abs): {vydaje.Min(z => z.Hodnota)}");
                Console.WriteLine($"Nejmenší výdaj (abs): {vydaje.Max(z => z.Hodnota)}");
            }
            
            Console.WriteLine("\nSoučty podle kategorií:");
            var kategorieSumy = new Dictionary<string, long>();
            foreach (var z in zaznamy)
            {
                if (!kategorieSumy.ContainsKey(z.Kategorie))
                    kategorieSumy[z.Kategorie] = 0;
                kategorieSumy[z.Kategorie] += z.Hodnota;
            }

            foreach (var kvp in kategorieSumy)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        public int PocetZaznamu => zaznamy.Count;
    }

    // --- Hlavní třída programu (UI) ---
    class Program
    {
        static void Main(string[] args)
        {
            SpravceFinanci spravce = new SpravceFinanci();
            spravce.NacistZeSouboru();

            bool konec = false;
            while (!konec)
            {
                Console.WriteLine("\n=== Finanční Správce ===");
                Console.WriteLine("1. Přidat záznam");
                Console.WriteLine("2. Vypsat záznamy");
                Console.WriteLine("3. Upravit záznam");
                Console.WriteLine("4. Smazat záznam");
                Console.WriteLine("5. Statistiky");
                Console.WriteLine("6. Výpis s filtrem");
                Console.WriteLine("7. Konec");
                Console.Write("Vyberte akci: ");

                string volba = Console.ReadLine();
                switch (volba)
                {
                    case "1":
                        Pridat(spravce);
                        break;
                    case "2":
                        spravce.VypisZaznamy();
                        break;
                    case "3":
                        Upravit(spravce);
                        break;
                    case "4":
                        Smazat(spravce);
                        break;
                    case "5":
                        spravce.ZobrazitStatistiky();
                        break;
                    case "6":
                        Filtrovat(spravce);
                        break;
                    case "7":
                        spravce.UlozitDoSouboru();
                        konec = true;
                        break;
                    default:
                        Console.WriteLine("Neplatná volba, zkuste to znovu.");
                        break;
                }
            }
        }

        static void Pridat(SpravceFinanci spravce)
        {
            Console.WriteLine("--- Přidat záznam ---");
            int hodnota = NacistInt("Zadejte hodnotu (celé čislo): ");
            
            Console.Write("Zadejte název: ");
            string nazev = Console.ReadLine();

            Console.Write("Zadejte kategorii: ");
            string kategorie = Console.ReadLine();

            spravce.PridatZaznam(hodnota, nazev, kategorie);
        }

        static void Upravit(SpravceFinanci spravce)
        {
            Console.WriteLine("--- Upravit záznam ---");
            spravce.VypisZaznamy(); // Pro přehled
            int index = NacistInt("Zadejte číslo řádku pro úpravu: ") - 1;

            if (index >= 0 && index < spravce.PocetZaznamu)
            {
                int novaHodnota = NacistInt("Zadejte novou hodnotu: ");
                
                Console.Write("Zadejte nový název: ");
                string novyNazev = Console.ReadLine();

                Console.Write("Zadejte novou kategorii: ");
                string novaKategorie = Console.ReadLine();

                spravce.UpravitZaznam(index, novaHodnota, novyNazev, novaKategorie);
            }
            else
            {
                Console.WriteLine("Neplatné číslo řádku.");
            }
        }

        static void Smazat(SpravceFinanci spravce)
        {
            Console.WriteLine("--- Smazat záznam ---");
            int index = NacistInt("Zadejte číslo řádku pro smazání: ") - 1;

            if (index >= 0 && index < spravce.PocetZaznamu)
            {
                spravce.SmazatZaznam(index);
            }
            else
            {
                Console.WriteLine("Neplatné číslo řádku.");
            }
        }

        static void Filtrovat(SpravceFinanci spravce)
        {
            Console.WriteLine("--- Filtrace ---");
            Console.Write("Zadejte hledaný text: ");
            string filtr = Console.ReadLine();
            spravce.VypisZaznamy(filtr);
        }

        static int NacistInt(string vyzva)
        {
            while (true)
            {
                Console.Write(vyzva);
                string vstup = Console.ReadLine();
                if (int.TryParse(vstup, out int vysledek))
                {
                    return vysledek;
                }
                Console.WriteLine("Chyba: Zadejte platné celé číslo.");
            }
        }
    }
}
