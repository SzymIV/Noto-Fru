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
        aktywnosci.Add(akt);
    }

    public void EdytujAktywnosc(int id)
    {
        Console.WriteLine("Edycja aktywności...");
    }

    public void UsunAktywnosc(int id)
{
    Aktywnosc? doUsuniecia = aktywnosci.Find(akt => 
        id >= akt.CzasStart.Hour && id < akt.CzasKoniec.Hour);

    if (doUsuniecia != null)
    {
        aktywnosci.Remove(doUsuniecia);
        Console.WriteLine($"Sukces: Usunięto aktywność z godziny {id:D2}.00.");
    }
    else
    {
        Console.WriteLine($"Błąd: Brak aktywności w godzinie {id:D2}.00.");
    }
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
        Console.WriteLine($"\n=== WIDOK SZCZEGÓŁOWY DNIA: {Data.ToShortDateString()} ===");

    for (int godzina = 0; godzina < 24; godzina++)
    {
        Aktywnosc znaleziona = aktywnosci.Find(akt => 
            godzina >= akt.CzasStart.Hour && godzina < akt.CzasKoniec.Hour);

        string godzinaStr = $"{godzina:D2}.00";

        if (znaleziona != null)
        {
            Console.Write($"   {godzinaStr}    | ");
            znaleziona.WyswietlSzczegoly();
        }
        else
        {
            Console.WriteLine($"   {godzinaStr}    | ");
        }
    }
    }
}