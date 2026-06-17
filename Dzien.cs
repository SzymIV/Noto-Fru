using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Dzien
{
    public DateTime Data { get; set; }
    [JsonInclude]
    private List<Aktywnosc> aktywnosci = new List<Aktywnosc>();
    public Dzien(DateTime data)
    {
        Data = data;
    }

    public Dzien() { }

    public void DodajAktywnosc()
    {
        try
        {
            Console.WriteLine($"\n===Dodawanie Aktywności na dzień: {Data.ToShortDateString()}===");
            Console.Write("1. Podaj godzinę początku aktywności (format HH:mm, np. 14:30): ");
            string wpisanyStartInput = PobierzLinie();
            if (!DateTime.TryParseExact(wpisanyStartInput, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime wpisanyStart))
            {
                Console.WriteLine("Błąd: Niepoprawny format godziny początku!");
                return;
            }
            DateTime czasStart = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyStart.Hour, wpisanyStart.Minute, 0);
            Console.Write("2. Podaj godzinę końca aktywności (format HH:mm, np. 16:00): ");
            string wpisanyKoniecInput = PobierzLinie();
            if (!DateTime.TryParseExact(wpisanyKoniecInput, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime wpisanyKoniec))
            {
                Console.WriteLine("Błąd: Niepoprawny format godziny końca!");
                return;
            }
            DateTime czasKoniec = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyKoniec.Hour, wpisanyKoniec.Minute, 0);

            if (czasKoniec <= czasStart)
            {
                Console.WriteLine("Błąd: Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia!");
                return;
            }

            Console.WriteLine("3. Sprawdzanie konfliktów czasowych...");
            if (SprawdzKonfliktyCzasowe(czasStart, czasKoniec))
            {
                Console.WriteLine("W podanym przedziale czasowym masz już zaplanowaną inną aktywność!");
                return;
            }
            Console.WriteLine("\n. Wybierz rodzaj aktywności:");
            Console.WriteLine("1. Atrakcja");
            Console.WriteLine("2. Nocleg");
            Console.WriteLine("3. Przejazd");
            Console.Write("Twój wybór (1-3): ");
            string rodzajWybor = PobierzLinie();

            if (rodzajWybor != "1" && rodzajWybor != "2" && rodzajWybor != "3")
            {
                Console.WriteLine("Wybierz liczbę od 1 do 3");
                return;
            }

            string opis = "";
            while (true)
            {
                Console.Write("5. Podaj opis aktywności (maksymalnie 15 znaków): ");
                opis = PobierzLinie();

                if (opis.Length <= 15)
                {
                    break;
                }
                Console.WriteLine($"Błąd: Twój opis ma {opis.Length} znaków. Maksymalna dozwolona długość to 15 znaków!");
            }

            Aktywnosc? nowaAktywnosc = null;

            switch (rodzajWybor)
            {
                case "1": 
                    Console.Write("   Podaj nazwę atrakcji: ");
                    string nazwaAtrakcji = PobierzLinie();
                    Console.Write("   Czy biletowane? (tak/nie): ");
                    bool czyBilet = PobierzLinie().Equals("tak", StringComparison.OrdinalIgnoreCase);

                    nowaAktywnosc = new Atrakcja(czasStart, czasKoniec, opis, nazwaAtrakcji, czyBilet);
                    break;

                case "2": 
                    Console.Write("   Podaj nazwę obiektu: ");
                    string nazwaObiektu = PobierzLinie();
                    Console.Write("   Podaj adres: ");
                    string adres = PobierzLinie();

                    nowaAktywnosc = new Nocleg(czasStart, czasKoniec, opis, nazwaObiektu, adres);
                    break;

                case "3":
                    Console.Write("   Podaj środek transportu: ");
                    string transport = PobierzLinie();
                    Console.Write("   Podaj stację początkową: ");
                    string startStacja = PobierzLinie();
                    Console.Write("   Podaj stację końcową: ");
                    string koniecStacja = PobierzLinie();

                    nowaAktywnosc = new Przejazd(czasStart, czasKoniec, opis, transport, startStacja, koniecStacja);
                    break;
            }

            if (nowaAktywnosc != null)
            {
                nowaAktywnosc.id = aktywnosci.Count + 1;
                aktywnosci.Add(nowaAktywnosc);
                Console.WriteLine("Aktywność została pdodana do harmonogramu!");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono niepoprawny format godziny! Używaj formatu HH:mm (np. 08:15).");
        }
    }
    public void EdytujAktywnosc()
    {
       try
        {
            Console.WriteLine($"\n===Edycja aktywności w dniu: {Data.ToShortDateString()}===");

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
                Console.WriteLine($"Nie znaleziono aktywności");
                return;
            }

            Console.WriteLine($"\n Edycja aktywnosci: [{akt.id}] {akt.Opis} ({akt.CzasStart:HH:mm}-{akt.CzasKoniec:HH:mm})");
            Console.WriteLine("Co chcesz zmienić?");
            Console.WriteLine("1. Przedział czasowy");
            Console.WriteLine("2. Rodzaj aktywności");
            Console.WriteLine("3. Opis");
            Console.Write("Twój wybór (1-3): ");
            string coZmienic = PobierzLinie();

            switch (coZmienic)
            {
                case "1": 
                    Console.Write("Podaj nową godzinę początku (format HH:mm, np. 09:00): ");
                    string wpisanyStartNowyInput = PobierzLinie();
                    if (!DateTime.TryParseExact(wpisanyStartNowyInput, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime wpisanyStart))
                    {
                        Console.WriteLine("Błąd: Niepoprawny format godziny początku!");
                        return;
                    }
                    DateTime nowyStart = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyStart.Hour, wpisanyStart.Minute, 0);

                    Console.Write("Podaj nową godzinę końca (format HH:mm, np. 11:30): ");
                    string wpisanyKoniecNowyInput = PobierzLinie();
                    if (!DateTime.TryParseExact(wpisanyKoniecNowyInput, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime wpisanyKoniec))
                    {
                        Console.WriteLine("Błąd: Niepoprawny format godziny końca!");
                        return;
                    }
                    DateTime nowyKoniec = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyKoniec.Hour, wpisanyKoniec.Minute, 0);

                    if (nowyKoniec <= nowyStart)
                    {
                        Console.WriteLine("Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia!");
                        return;
                    }

                    Console.WriteLine("Sprawdzanie konfliktów czasowych...");
                    if (SprawdzKonfliktyCzasowe(nowyStart, nowyKoniec))
                    {
                        Console.WriteLine("Godziny kolidują z inną zaplanowaną aktywnością w tym dniu!");
                        return;
                    }

                    akt.CzasStart = nowyStart;
                    akt.CzasKoniec = nowyKoniec;
                    break;

                case "2": 
                    Console.WriteLine("\n Wybierz nowy rodzaj aktywności:");
                    Console.WriteLine("1. Atrakcja");
                    Console.WriteLine("2. Nocleg");
                    Console.WriteLine("3. Przejazd");
                    Console.Write("Twój wybór (1-3): ");
                    string nowyRodzaj = PobierzLinie();

                    if (nowyRodzaj != "1" && nowyRodzaj != "2" && nowyRodzaj != "3")
                    {
                        Console.WriteLine("Wybierz lidzbe od 1 do 3");
                        return;
                    }

                    Aktywnosc? nowoUtworzona = null;
                    DateTime obecnyStart = akt.CzasStart;
                    DateTime obecnyKoniec = akt.CzasKoniec;
                    string obecnyOpis = akt.Opis;

                    switch (nowyRodzaj)
                    {
                        case "1":
                            Console.Write("   Podaj nazwę atrakcji: ");
                            string nazwaAtrakcji = PobierzLinie();
                            Console.Write("   Czy biletowane? (tak/nie): ");
                            bool czyBilet = PobierzLinie().Equals("tak", StringComparison.OrdinalIgnoreCase);
                            nowoUtworzona = new Atrakcja(obecnyStart, obecnyKoniec, obecnyOpis, nazwaAtrakcji, czyBilet);
                            break;

                        case "2":
                            Console.Write("   Podaj nazwę obiektu: ");
                            string nazwaObiektu = PobierzLinie();
                            Console.Write("   Podaj adres: ");
                            string adres = PobierzLinie();
                            nowoUtworzona = new Nocleg(obecnyStart, obecnyKoniec, obecnyOpis, nazwaObiektu, adres);
                            break;

                        case "3":
                            Console.Write("   Podaj środek transportu: ");
                            string transport = PobierzLinie();
                            Console.Write("   Podaj stację początkową: ");
                            string startStacja = PobierzLinie();
                            Console.Write("   Podaj stację końcową: ");
                            string koniecStacja = PobierzLinie();
                            nowoUtworzona = new Przejazd(obecnyStart, obecnyKoniec, obecnyOpis, transport, startStacja, koniecStacja);
                            break;
                    }

                    if (nowoUtworzona != null)
                    {
                        nowoUtworzona.id = akt.id; 
                        int indeks = aktywnosci.IndexOf(akt);
                        aktywnosci[indeks] = nowoUtworzona; 
                        Console.WriteLine("[Sukces] Rodzaj aktywności i jej specyficzne dane zostały zmienione.");
                    }
                    break;

                case "3": 
                    string nowyOpis = "";
                    while (true)
                    {
                        Console.Write("Podaj nowy opis aktywności (maksymalnie 15 znaków): ");
                        nowyOpis = PobierzLinie();

                        if (nowyOpis.Length <= 15)
                        {
                            break;
                        }
                        Console.WriteLine($"Twój opis ma {nowyOpis.Length} znaków. Maksymalna dozwolona długość to 15 znaków!");
                    }

                    akt.Opis = nowyOpis;
                    Console.WriteLine("Opis aktywności został zaktualizowany.");
                    break;

                default:
                    Console.WriteLine("Niepoprawna opcja menu edycji.");
                    break;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Wprowadzono nieprawidłowe dane liczbowe lub błędny format godziny!");
        }
    }
    public void UsunAktywnosc()
    {
      try
        {
            Console.WriteLine($"\n===Usuwanie aktywnosci z dnia: {Data.ToShortDateString()}===");

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

            if (doUsuniecia != null)
            {
                aktywnosci.Remove(doUsuniecia);

                for (int i = 0; i < aktywnosci.Count; i++)
                {
                    aktywnosci[i].id = i + 1;
                }

                Console.WriteLine($"Aktywność została usunięta.");
            }
            else
            {
                Console.WriteLine($"Nie znaleziono aktywności");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Musisz podać liczbę jako numer aktywności!");
        }
    }

    public bool SprawdzKonfliktyCzasowe(DateTime start, DateTime koniec)
    {
        foreach (var akt in aktywnosci)
        {
            if (start < akt.CzasKoniec && koniec > akt.CzasStart)
            {
                return true;
            }
        }

        return false;
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
            if (rodzaj == "Zwiedzanie") rodzaj = "Atrakcja";

            string przedzialCzasowy = $"{akt.CzasStart:HH:mm}-{akt.CzasKoniec:HH:mm}";

            Console.WriteLine($"{akt.id,-3} | {przedzialCzasowy,-17} | {rodzaj,-17} | {akt.Opis}");
        }
        Console.WriteLine();
    }
}