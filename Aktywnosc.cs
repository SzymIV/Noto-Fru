using System;

public abstract class Aktywnosc
{
    protected DateTime czasStart;
    protected DateTime czasKoniec;
    protected string opis;

    public DateTime CzasStart => czasStart;
    public DateTime CzasKoniec => czasKoniec;

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