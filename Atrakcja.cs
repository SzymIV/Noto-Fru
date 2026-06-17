using System;

public class Atrakcja : Aktywnosc
{
    private string nazwaAtrakcji;
    private bool czyBiletowane;

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