using System;
using System.Collections.Generic;

public class Dzien
{
    public DateTime Data { get; set; }

    public List<Aktywnosc> Aktywnosci { get; set; } = new List<Aktywnosc>();

    public Dzien() { }
    
    public Dzien(DateTime data)
    {
        Data = data;
    }

    public void DodajAktywnosc(Aktywnosc akt)
    {
        Aktywnosci.Add(akt);
    }

    public void EdytujAktywnosc(int id)
    {
        Console.WriteLine("Edycja aktywności...");
    }

    public void UsunAktywnosc(int id)
    {
        Console.WriteLine("Usuwanie aktywności...");
    }

    public bool SprawdzKonfliktyCzasowe(DateTime start, DateTime koniec)
    {
        foreach (var akt in Aktywnosci)
        {
            if (start < akt.CzasKoniec && koniec > akt.CzasStart)
            {
                return true;
            }
        }

        return false;
    }

    public void WyswietlWidokDnia()
    {
        Console.WriteLine($"Dzień: {Data.ToShortDateString()}");

        foreach (var akt in Aktywnosci)
        {
            akt.WyswietlSzczegoly();
        }
    }
}