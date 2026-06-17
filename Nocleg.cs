using System;
using System.Text.Json.Serialization;

public class Nocleg : Aktywnosc
{
    [JsonInclude]
    private string nazwaObiektu;
    [JsonInclude]
    private string adres;

    public Nocleg() { }

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