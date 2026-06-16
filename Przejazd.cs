using System;

public class Przejazd : Aktywnosc
{
        public string SrodekTransportu { get; set; }
        public string StacjaPoczatkowa { get; set; }
        public string StacjaKoncowa { get; set; }

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
        SrodekTransportu = transport;
        StacjaPoczatkowa = startStacja;
        StacjaKoncowa = koniecStacja;
    }

    public override void WyswietlSzczegoly()
    {
        Console.WriteLine($"Przejazd: {SrodekTransportu}");
        Console.WriteLine($"{StacjaPoczatkowa} -> {StacjaKoncowa}");
    }
}