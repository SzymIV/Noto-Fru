using System;
using System.Collections.Generic;

public class Kalendarz
{
    private string tytulDestynacji;
    private List<Dzien> dni = new List<Dzien>();

    public Kalendarz(string tytul)
    {
        tytulDestynacji = tytul;
    }

    public string TytulDestynacji() => tytulDestynacji;
    public string PobierzTytul() => tytulDestynacji;

    public void DodajDzien(Dzien dzien)
    {
        dni.Add(dzien);
    }

    public bool UsunDzien(int indeks)
    {
        if (indeks < 1 || indeks > dni.Count)
        {
            return false;
        }

        dni.RemoveAt(indeks - 1);
        return true;
    }

    public int PobierzLiczbeDni()
    {
        return dni.Count;
    }

public Dzien PobierzDzien(int indeks)
    {
        if (indeks < 1 || indeks > dni.Count)
        {
            return null;
        }
        return dni[indeks - 1];
    }
    public void PokazDni()
    {
        Console.WriteLine();
        Console.WriteLine("=== dni ===");

        if (dni.Count == 0)
        {
            Console.WriteLine("Brak dni.");
            return;
        }

        for (int i = 0; i < dni.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dni[i].Data.ToShortDateString()}");
        }
    }

    public void PokazWidokMiesiaca(int rok, int miesiac)
    {
        WidokMiesiaca(rok, miesiac, this.dni);
    }

    public static void WidokMiesiaca(int rok, int miesiac, List<Dzien> listaDni = null)
    {
        DateTime pierwszyDzien = new DateTime(rok, miesiac, 1);
        int dniWMiesiacu = DateTime.DaysInMonth(rok, miesiac);

        Console.WriteLine($"\n=== {pierwszyDzien.ToString("MMMM yyyy").ToUpper()} ===");
        Console.WriteLine(" Pn  Wt  Śr  Cz  Pt  Sb  Nd");

        int dzienTygodnia = (int)pierwszyDzien.DayOfWeek;
        int przesuniecie = dzienTygodnia == 0 ? 6 : dzienTygodnia - 1;

        for (int i = 0; i < przesuniecie; i++)
        {
            Console.Write("    ");
        }

        for (int dzien = 1; dzien <= dniWMiesiacu; dzien++)
        {
            DateTime aktualnaData = new DateTime(rok, miesiac, dzien);
            bool czyMamyTenDzien = listaDni != null && listaDni.Exists(d => d.Data.Date == aktualnaData.Date);

            if (czyMamyTenDzien)
            {
                Console.Write($"{dzien + "*",3}");
            }
            else
            {
                Console.Write($"{dzien,3}");
            }

            if ((dzien + przesuniecie) % 7 == 0 || dzien == dniWMiesiacu)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
    }
}