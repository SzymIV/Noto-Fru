using System;

public abstract class Aktywnosc
{
    public int id { get; set; }
    protected DateTime czasStart;
    protected DateTime czasKoniec;
    protected string opis;

    public DateTime CzasStart { get => czasStart; set => czasStart = value; }
    public DateTime CzasKoniec { get => czasKoniec; set => czasKoniec = value; }
    public string Opis { get => opis; set => opis = value; }
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