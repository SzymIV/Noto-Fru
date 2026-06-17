using System;
using System.Collections.Generic;

public class Dzien
{
    public DateTime Data { get; set; }
    private List<Aktywnosc> aktywnosci = new List<Aktywnosc>();
    public Dzien(DateTime data)
    {
        Data = data;
    }

    
    public void EdytujAktywnosc()
    {
        while (true)
        {
            Console.Write("\nWpisz 0, aby zakończyć dodawanie aktywności: ");
        if (Console.ReadLine() == "0")
        {
            break; 
        }
            
       try
        {
            Console.WriteLine($"\n===Edycja aktywności w dniu: {Data.ToShortDateString()}===");

            if (aktywnosci.Count == 0)
            {
                Console.WriteLine("Nie ma aktywności do zmodyfikowania.");
                return;
            }

            Console.Write("Podaj numer aktywności, którą chcesz zmodyfikować: ");
            int wybranyId = int.Parse(Console.ReadLine());

            Aktywnosc akt = aktywnosci.Find(a => a.id == wybranyId);

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
            string coZmienic = Console.ReadLine();

            switch (coZmienic)
            {
                case "1": 
                    Console.Write("Podaj nową godzinę początku (format HH:mm, np. 09:00): ");
                    DateTime wpisanyStart = DateTime.ParseExact(Console.ReadLine(), "HH:mm", null);
                    DateTime nowyStart = new DateTime(Data.Year, Data.Month, Data.Day, wpisanyStart.Hour, wpisanyStart.Minute, 0);

                    Console.Write("Podaj nową godzinę końca (format HH:mm, np. 11:30): ");
                    DateTime wpisanyKoniec = DateTime.ParseExact(Console.ReadLine(), "HH:mm", null);
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
                    string nowyRodzaj = Console.ReadLine();

                    if (nowyRodzaj != "1" && nowyRodzaj != "2" && nowyRodzaj != "3")
                    {
                        Console.WriteLine("Wybierz lidzbe od 1 do 3");
                        return;
                    }

                    Aktywnosc nowoUtworzona = null;
                    DateTime obecnyStart = akt.CzasStart;
                    DateTime obecnyKoniec = akt.CzasKoniec;
                    string obecnyOpis = akt.Opis;

                    switch (nowyRodzaj)
                    {
                        case "1":
                            Console.Write("   Podaj nazwę atrakcji: ");
                            string nazwaAtrakcji = Console.ReadLine();
                            Console.Write("   Czy biletowane? (tak/nie): ");
                            bool czyBilet = Console.ReadLine().ToLower() == "tak";
                            nowoUtworzona = new Atrakcja(obecnyStart, obecnyKoniec, obecnyOpis, nazwaAtrakcji, czyBilet);
                            break;

                        case "2":
                            Console.Write("   Podaj nazwę obiektu: ");
                            string nazwaObiektu = Console.ReadLine();
                            Console.Write("   Podaj adres: ");
                            string adres = Console.ReadLine();
                            nowoUtworzona = new Nocleg(obecnyStart, obecnyKoniec, obecnyOpis, nazwaObiektu, adres);
                            break;

                        case "3":
                            Console.Write("   Podaj środek transportu: ");
                            string transport = Console.ReadLine();
                            Console.Write("   Podaj stację początkową: ");
                            string startStacja = Console.ReadLine();
                            Console.Write("   Podaj stację końcową: ");
                            string koniecStacja = Console.ReadLine();
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
                        nowyOpis = Console.ReadLine();

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
            int wybranyId = int.Parse(Console.ReadLine());

            Aktywnosc doUsuniecia = aktywnosci.Find(a => a.id == wybranyId);

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