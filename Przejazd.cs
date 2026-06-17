using System;
using System.Text.Json.Serialization;

public class Przejazd : Aktywnosc
{
    [JsonInclude]
    private string srodekTransportu;
    [JsonInclude]
    private string stacjaPoczatkowa;
    [JsonInclude]
    private string stacjaKoncowa;

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