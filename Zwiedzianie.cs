using System;

public class Zwiedzanie : Aktywnosc
{
    private string nazwaAtrakcji;
    private bool czyBiletowane;

    public Zwiedzanie(
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
    string biletInfo = czyBiletowane ? "Bilet" : "Brak biletu";
    Console.WriteLine($"ZWIEDZANIE | {nazwaAtrakcji} ({biletInfo}) - {opis}");
    }
}