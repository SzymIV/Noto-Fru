using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

public class Dzien : IComparable<Dzien>
{
    public DateTime Data { get; set; }
    [JsonInclude]
    private List<Aktywnosc> aktywnosci = new List<Aktywnosc>();

    public Dzien(DateTime data)
    {
        Data = data;
    }

    public Dzien() { }

    public int CompareTo(Dzien? other)
    {
        if (other == null)
        {
            return 1;
        }

        return Data.CompareTo(other.Data);
    }

    private void SortujIPorządkujId()
    {
        aktywnosci.Sort((a, b) => a.CzasStart.CompareTo(b.CzasStart));
        for (int i = 0; i < aktywnosci.Count; i++)
        {
            aktywnosci[i].id = i + 1;
        }
    }

    public void DodajAktywnosc()
    {
        while (true)
        {
            if (PobierzLinie("\nWpisz 0, aby zakończyć dodawanie aktywności: ") == "0")
            {
                break;
            }

            Console.WriteLine($"\n=== Dodawanie aktywności na dzień: {Data.ToShortDateString()} ===");

            if (!SpróbujPobierzGodzine("1. Podaj godzinę początku aktywności (format HH:mm, np. 14:30): ", out DateTime wpisanyStart))
            {
                continue;
            }

            if (!SpróbujPobierzGodzine("2. Podaj godzinę końca aktywności (format HH:mm, np. 16:00): ", out DateTime wpisanyKoniec))
            {
                continue;
            }

            DateTime czasStart = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyStart.Hour, wpisanyStart.Minute, 0);
            DateTime czasKoniec = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyKoniec.Hour, wpisanyKoniec.Minute, 0);

            if (czasKoniec <= czasStart)
            {
                Console.WriteLine("Błąd: Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia!");
                continue;
            }

            Console.WriteLine("3. Sprawdzanie konfliktów czasowych...");
            if (SprawdzKonfliktyCzasowe(czasStart, czasKoniec))
            {
                Console.WriteLine("W podanym przedziale czasowym masz już zaplanowaną inną aktywność!");
                continue;
            }

            if (!SpróbujPobierzOpcje("\nWybierz rodzaj aktywności (1. Atrakcja, 2. Nocleg, 3. Przejazd): ", new[] { "1", "2", "3" }, out string rodzajWybor))
            {
                continue;
            }

            string opis = PobierzTekstOWymiarze("5. Podaj opis aktywności (maksymalnie 15 znaków): ", 15);
            Aktywnosc? nowaAktywnosc = UtworzAktywnosc(rodzajWybor, czasStart, czasKoniec, opis);

            if (nowaAktywnosc != null)
            {
                aktywnosci.Add(nowaAktywnosc);
                SortujIPorządkujId();
                Console.WriteLine("Aktywność została dodana do harmonogramu!");
            }
        }
    }

    private Aktywnosc? UtworzAktywnosc(string rodzajWybor, DateTime start, DateTime koniec, string opis)
    {
        switch (rodzajWybor)
        {
            case "1":
                string nazwaAtrakcji = PobierzLinie("   Podaj nazwę atrakcji: ");
                bool czyBilet = PobierzLinie("   Czy biletowane? (tak/nie): ").Equals("tak", StringComparison.OrdinalIgnoreCase);
                return new Atrakcja(start, koniec, opis, nazwaAtrakcji, czyBilet);

            case "2":
                string nazwaObiektu = PobierzLinie("   Podaj nazwę obiektu: ");
                string adres = PobierzLinie("   Podaj adres: ");
                return new Nocleg(start, koniec, opis, nazwaObiektu, adres);

            case "3":
                string transport = PobierzLinie("   Podaj środek transportu: ");
                string startStacja = PobierzLinie("   Podaj stację początkową: ");
                string koniecStacja = PobierzLinie("   Podaj stację końcową: ");
                return new Przejazd(start, koniec, opis, transport, startStacja, koniecStacja);

            default:
                return null;
        }
    }

    public void EdytujAktywnosc()
    {
        Console.WriteLine($"\n=== Edycja aktywności w dniu: {Data.ToShortDateString()} ===");

        if (aktywnosci.Count == 0)
        {
            Console.WriteLine("Nie ma aktywności do zmodyfikowania.");
            return;
        }

        Console.Write("Podaj numer aktywności, którą chcesz zmodyfikować: ");
        string wybranyIdInput = PobierzLinie();
        if (!int.TryParse(wybranyIdInput, out int wybranyId))
        {
            Console.WriteLine("Błąd: Musisz podać numer aktywności jako liczbę!");
            return;
        }

        Aktywnosc? akt = aktywnosci.Find(a => a.id == wybranyId);
        if (akt == null)
        {
            Console.WriteLine("Nie znaleziono aktywności.");
            return;
        }

        Console.WriteLine($"\nEdycja aktywności: [{akt.id}] {akt.Opis} ({akt.CzasStart:HH:mm}-{akt.CzasKoniec:HH:mm})");
        Console.WriteLine("Co chcesz zmienić?");
        Console.WriteLine("1. Przedział czasowy");
        Console.WriteLine("2. Rodzaj aktywności");
        Console.WriteLine("3. Opis");
        Console.Write("Twój wybór (1-3): ");
        string coZmienic = PobierzLinie();

        switch (coZmienic)
        {
            case "1":
                EdytujPrzedzialCzasowy(akt);
                break;

            case "2":
                EdytujRodzajAktywnosci(akt);
                break;

            case "3":
                string nowyOpis = PobierzTekstOWymiarze("Podaj nowy opis aktywności (maksymalnie 15 znaków): ", 15);
                akt.Opis = nowyOpis;
                Console.WriteLine("Opis aktywności został zaktualizowany.");
                break;

            default:
                Console.WriteLine("Niepoprawna opcja menu edycji.");
                break;
        }
    }

    private void EdytujPrzedzialCzasowy(Aktywnosc akt)
    {
        if (!SpróbujPobierzGodzine("Podaj nową godzinę początku (format HH:mm, np. 09:00): ", out DateTime wpisanyStart))
        {
            return;
        }

        if (!SpróbujPobierzGodzine("Podaj nową godzinę końca (format HH:mm, np. 11:30): ", out DateTime wpisanyKoniec))
        {
            return;
        }

        DateTime nowyStart = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyStart.Hour, wpisanyStart.Minute, 0);
        DateTime nowyKoniec = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyKoniec.Hour, wpisanyKoniec.Minute, 0);

        if (nowyKoniec <= nowyStart)
        {
            Console.WriteLine("Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia!");
            return;
        }

        if (SprawdzKonfliktyCzasowe(nowyStart, nowyKoniec, akt))
        {
            Console.WriteLine("Godziny kolidują z inną zaplanowaną aktywnością w tym dniu!");
            return;
        }

        akt.CzasStart = nowyStart;
        akt.CzasKoniec = nowyKoniec;
        SortujIPorządkujId();
        Console.WriteLine("Przedział czasowy został zaktualizowany.");
    }

    private void EdytujRodzajAktywnosci(Aktywnosc akt)
    {
        if (!SpróbujPobierzOpcje("\nWybierz nowy rodzaj aktywności (1. Atrakcja, 2. Nocleg, 3. Przejazd): ", new[] { "1", "2", "3" }, out string nowyRodzaj))
        {
            return;
        }

        string obecnyOpis = akt.Opis;
        DateTime obecnyStart = akt.CzasStart;
        DateTime obecnyKoniec = akt.CzasKoniec;
        Aktywnosc? nowoUtworzona = UtworzAktywnosc(nowyRodzaj, obecnyStart, obecnyKoniec, obecnyOpis);

        if (nowoUtworzona == null)
        {
            Console.WriteLine("Nie udało się utworzyć nowego typu aktywności.");
            return;
        }

        nowoUtworzona.id = akt.id;
        int indeks = aktywnosci.IndexOf(akt);
        aktywnosci[indeks] = nowoUtworzona;
        Console.WriteLine("[Sukces] Rodzaj aktywności i jej dane zostały zmienione.");
    }

    public void UsunAktywnosc()
    {
        Console.WriteLine($"\n=== Usuwanie aktywności z dnia: {Data.ToShortDateString()} ===");

        if (aktywnosci.Count == 0)
        {
            Console.WriteLine("Nie ma aktywności do usunięcia.");
            return;
        }

        Console.Write("Podaj numer aktywności, którą chcesz usunąć: ");
        string wybranyIdInput = PobierzLinie();
        if (!int.TryParse(wybranyIdInput, out int wybranyId))
        {
            Console.WriteLine("Musisz podać liczbę jako numer aktywności!");
            return;
        }

        Aktywnosc? doUsuniecia = aktywnosci.Find(a => a.id == wybranyId);
        if (doUsuniecia == null)
        {
            Console.WriteLine("Nie znaleziono aktywności.");
            return;
        }

        aktywnosci.Remove(doUsuniecia);
        SortujIPorządkujId();
        Console.WriteLine("Aktywność została usunięta.");
    }

    public bool SprawdzKonfliktyCzasowe(DateTime start, DateTime koniec, Aktywnosc? ignore = null)
    {
        foreach (var akt in aktywnosci)
        {
            if (akt == ignore)
            {
                continue;
            }

            if (start < akt.CzasKoniec && koniec > akt.CzasStart)
            {
                return true;
            }
        }

        return false;
    }

    private bool SpróbujPobierzGodzine(string prompt, out DateTime godzina)
    {
        string input = PobierzLinie(prompt);
        if (!DateTime.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out godzina))
        {
            Console.WriteLine("Błąd: Niepoprawny format godziny! Użyj formatu HH:mm (np. 08:15).");
            return false;
        }

        return true;
    }

    private bool SpróbujPobierzOpcje(string prompt, string[] dozwoloneOpcje, out string wynik)
    {
        wynik = PobierzLinie(prompt);
        if (Array.IndexOf(dozwoloneOpcje, wynik) < 0)
        {
            Console.WriteLine("Wybierz poprawną opcję.");
            return false;
        }

        return true;
    }

    private string PobierzTekstOWymiarze(string prompt, int maxDlugosc)
    {
        while (true)
        {
            string tekst = PobierzLinie(prompt);
            if (tekst.Length <= maxDlugosc)
            {
                return tekst;
            }

            Console.WriteLine($"Błąd: Twój opis ma {tekst.Length} znaków. Maksymalna dozwolona długość to {maxDlugosc} znaków!");
        }
    }

    private string PobierzLinie(string prompt)
    {
        Console.Write(prompt);
        return PobierzLinie();
    }

    private string PobierzLinie() => Console.ReadLine()?.Trim() ?? string.Empty;

    public void WyswietlWidokDnia()
    {
        Console.WriteLine($"\n=== WIDOK DNIA: {Data.ToShortDateString()} ===");
        Console.WriteLine($"{"Numer",-3} | {"Przedział czasowy",-17} | {"Rodzaj aktywności",-17} | {"Opis aktywności"}");
        Console.WriteLine(new string('-', 75));

        if (aktywnosci.Count == 0)
        {
            Console.WriteLine(" Brak zaplanowanych aktywności na ten dzień.");
            return;
        }

        foreach (var akt in aktywnosci)
        {
            string rodzaj = akt.GetType().Name;
            string przedzialCzasowy = $"{akt.CzasStart:HH:mm}-{akt.CzasKoniec:HH:mm}";
            Console.WriteLine($"{akt.id,-3} | {przedzialCzasowy,-17} | {rodzaj,-17} | {akt.Opis}");
        }

        Console.WriteLine();
    }
}
