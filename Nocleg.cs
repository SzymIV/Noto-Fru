using System;
using System.Text.Json.Serialization;

public class Nocleg : Aktywnosc
{
    [JsonInclude]
    private string nazwaObiektu = string.Empty;
    [JsonInclude]
    private string adres = string.Empty;

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