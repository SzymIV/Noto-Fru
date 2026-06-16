using System;

public class Zwiedzanie : Aktywnosc
{
    public string NazwaAtrakcji { get; set; }
    public bool CzyBiletowane { get; set; }

    public Zwiedzanie(
        DateTime start,
        DateTime koniec,
        string opis,
        string atrakcja,
        bool bilet
    ) : base(start, koniec, opis)
    {
        NazwaAtrakcji = atrakcja;
        CzyBiletowane = bilet;
    }

    public override void WyswietlSzczegoly()
    {
        Console.WriteLine($"Zwiedzanie: {NazwaAtrakcji}");
        Console.WriteLine($"Biletowane: {CzyBiletowane}");
    }
}