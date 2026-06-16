using System;

public abstract class Aktywnosc
{
    public int id { get; set; }
    protected DateTime czasStart;
    protected DateTime czasKoniec;
    protected string opis;

    public DateTime CzasStart => czasStart;
    public DateTime CzasKoniec => czasKoniec;
    public string Opis => opis;
    public Aktywnosc(DateTime start, DateTime koniec, string opis)
    {
        this.czasStart = start;
        this.czasKoniec = koniec;
        this.opis = opis;
    }

    public TimeSpan PobierzCzasTrwania()
    {
        return czasKoniec - czasStart;
    }

    public abstract void WyswietlSzczegoly();
}