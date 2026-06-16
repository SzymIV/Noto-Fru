using System;

public class Nocleg : Aktywnosc
{
    public string NazwaObiektu { get; set; }
    public string Adres { get; set; }

    public Nocleg() { }

    public Nocleg(DateTime start, DateTime koniec, string opis, string nazwaObiektu, string adres) 
        : base(start, koniec, opis)
    {
        NazwaObiektu = nazwaObiektu;
        Adres = adres;
    }

    public override void WyswietlSzczegoly()
    {
        Console.WriteLine($"Nocleg: {NazwaObiektu}");
        Console.WriteLine($"Adres: {Adres}");
    }
}