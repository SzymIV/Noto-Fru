using System;
using System.Text.Json.Serialization;

public class Atrakcja : Aktywnosc
{
    [JsonInclude]
    private string nazwaAtrakcji;
    [JsonInclude]
    private bool czyBiletowane;

    public Atrakcja() { }

    public Atrakcja(
        DateTime start,
        DateTime koniec,
        string opis,
        string atrakcja,
        bool bilet
    ) : base(start, koniec, opis)
    {
        nazwaAtrakcji = atrakcja;
        czyBiletowane = bilet;
    }

    public override void WyswietlSzczegoly()
    {
        Console.WriteLine($"Atrakcja: {nazwaAtrakcji}");
        Console.WriteLine($"Biletowane: {czyBiletowane}");
    }
}