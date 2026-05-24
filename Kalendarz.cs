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

    public string tytulDestynacji => tytulDestynacji;

    public void AddDay(Dzien dzien)
    {
        dni.Add(Dzien);
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
}