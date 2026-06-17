using System;
using System.Text.Json.Serialization;

[JsonDerivedType(typeof(Atrakcja), typeDiscriminator: "Atrakcja")]
[JsonDerivedType(typeof(Nocleg), typeDiscriminator: "Nocleg")]
[JsonDerivedType(typeof(Przejazd), typeDiscriminator: "Przejazd")]
public abstract class Aktywnosc
{

    public int id { get; set; }
    [JsonInclude]
    protected DateTime czasStart;
    [JsonInclude]
    protected DateTime czasKoniec;
    [JsonInclude]
    protected string opis;

    public Aktywnosc() { }

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