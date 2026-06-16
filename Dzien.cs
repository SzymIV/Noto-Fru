using System;
using System.Collections.Generic;

public class Dzien
{
    public DateTime Data { get; set; }
    private List<Aktywnosc> aktywnosci = new List<Aktywnosc>();
    public Dzien(DateTime data)
    {
        Data = data;
    }

    public void DodajAktywnosc(Aktywnosc akt)
    {
        akt.id = aktywnosci.Count + 1;
        aktywnosci.Add(akt);
    }
    public void EdytujAktywnosc(int id)
    {
        Console.WriteLine($"Wybierz numer aktywności do modyfikacji: ");
    }
    public void UsunAktywnosc(int id)
    {
        Console.WriteLine($"Wybierz numer aktywności do usunięcia: ");
    }

    public bool SprawdzKonfliktyCzasowe(DateTime start, DateTime koniec)
    {
        foreach (var akt in aktywnosci)
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
        Console.WriteLine($"\n=== WIDOK DNIA: {Data.ToShortDateString()} ===");
        Console.WriteLine($"{"Numer",-3} | {"Przedział czasowy",-17} | {"Rodzaj aktywności",-17} | {"Opis aktywności"}");
        Console.WriteLine(new string('-', 75));

        if (aktywnosci.Count == 0)
        {
            Console.WriteLine(" Brak zaplanowanych aktywności na ten dzień.");
            return;
        }

        foreach (var akt in aktywnosci)
        {
            string rodzaj = akt.GetType().Name;
            if (rodzaj == "Zwiedzanie") rodzaj = "Atrakcja";

            string przedzialCzasowy = $"{akt.CzasStart:HH:mm}-{akt.CzasKoniec:HH:mm}";

            Console.WriteLine($"{akt.id,-3} | {przedzialCzasowy,-17} | {rodzaj,-17} | {akt.Opis}");
        }
        Console.WriteLine();
    }
}