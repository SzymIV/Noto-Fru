using System;

public class Nocleg : Aktywnosc
{
    private string nazwaObiektu;
    private string adres;

    public Nocleg(
        DateTime start,
        DateTime koniec,
        string opis,
        string nazwaObiektu,
        string adres
    ) : base(start, koniec, opis)
    {
        this.nazwaObiektu = nazwaObiektu;
        this.adres = adres;
    }

    public override void WyswietlSzczegoly()
    {
        Console.WriteLine($"Nocleg: {nazwaObiektu}");
        Console.WriteLine($"Adres: {adres}");
    }
}