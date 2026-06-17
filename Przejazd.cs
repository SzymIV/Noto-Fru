using System;
using System.Text.Json.Serialization;

public class Przejazd : Aktywnosc
{
    [JsonInclude]
    private string srodekTransportu = string.Empty;
    [JsonInclude]
    private string stacjaPoczatkowa = string.Empty;
    [JsonInclude]
    private string stacjaKoncowa = string.Empty;

    public Przejazd() { }

    public Przejazd(
        DateTime start,
        DateTime koniec,
        string opis,
        string transport,
        string startStacja,
        string koniecStacja
    ) : base(start, koniec, opis)
    {
        srodekTransportu = transport;
        stacjaPoczatkowa = startStacja;
        stacjaKoncowa = koniecStacja;
    }

    public override void WyswietlSzczegoly()
    {
        Console.WriteLine($"Przejazd: {srodekTransportu}");
        Console.WriteLine($"{stacjaPoczatkowa} -> {stacjaKoncowa}");
    }
}