using System;

public class Przejazd : Aktywnosc
{
    private string srodekTransportu;
    private string stacjaPoczatkowa;
    private string stacjaKoncowa;

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