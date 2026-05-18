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

    public string PobierzTytul()
    {
        return tytulDestynacji;
    }

    public void DodajDzien(Dzien dzien)
    {
        dni.Add(dzien);
    }

    public void PokazDni()
    {
        Console.WriteLine();
        Console.WriteLine("=== DNI ===");

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
}