using System;
using System.Text.Json.Serialization;


[JsonDerivedType(typeof(Nocleg), typeDiscriminator: "Nocleg")]
[JsonDerivedType(typeof(Przejazd), typeDiscriminator: "Przejazd")]
[JsonDerivedType(typeof(Zwiedzanie), typeDiscriminator: "Zwiedzanie")]
public abstract class Aktywnosc
{

    public DateTime CzasStart { get; set; }
    public DateTime CzasKoniec { get; set; }
    public string Opis { get; set; }

    public Aktywnosc() { }

    public Aktywnosc(DateTime start, DateTime koniec, string opis)
    {
        CzasStart = start;
        CzasKoniec = koniec;
        Opis = opis;
    }

    public TimeSpan PobierzCzasTrwania()
    {
        return CzasKoniec - CzasStart;
    }

    public abstract void WyswietlSzczegoly();
}