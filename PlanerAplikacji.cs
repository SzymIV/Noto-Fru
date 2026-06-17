using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class PlanerAplikacja
{
    private List<Kalendarz> kalendarze = new List<Kalendarz>();

    public void UruchomMenuGlowne()
    {
        bool dziala = true;

        while (dziala)
        {
            Console.WriteLine();
            Console.WriteLine("=== PLANER PODRÓŻY ===");
            Console.WriteLine("1. Dodaj kalendarz");
            Console.WriteLine("2. Pokaż kalendarze");
            Console.WriteLine("3. Otwórz kalendarz");
            Console.WriteLine("4. Przegląd ogólny (widok miesiąca)");
            Console.WriteLine("5. Zapisz Do pliku");
            Console.WriteLine("6. Wczytaj z pliku");
            Console.WriteLine("7. Wyjdź");
            Console.Write("Wybierz opcję: ");

            string wybor = PobierzLinie();

            switch (wybor)
            {
                case "1":
                    DodajKalendarz();
                    break;
                case "2":
                    PokazKalendarze();
                    break;
                case "3":
                    OtworzKalendarz();
                    break;
                case "4":
                    WyswietlMiesiac();
                    break;
                case "5":
                    ZapiszDoPliku(@"c:\kalendarze.json");
                    break;
                case "6":
                    WczytajZPliku(@"c:\kalendarze.json");
                    break;
                case "7":
                    dziala = false;
                    break;
                default:
                    Console.WriteLine("Niepoprawna opcja!");
                    break;
            }
        }
    }

    private void DodajKalendarz()
    {
        Console.Write("Podaj nazwę kalendarza: ");
        string nazwa = PobierzLinie();
        Kalendarz nowy = new Kalendarz(nazwa);
        kalendarze.Add(nowy);
        Console.WriteLine("Dodano kalendarz!");
    }

    private void PokazKalendarze()
    {
        Console.WriteLine();
        Console.WriteLine("=== LISTA KALENDARZY ===");

        if (kalendarze.Count == 0)
        {
            Console.WriteLine("Brak kalendarzy.");
            return;
        }

        for (int i = 0; i < kalendarze.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {kalendarze[i].PobierzTytul()}");
        }
    }

    private void OtworzKalendarz()
    {
        PokazKalendarze();

        if (kalendarze.Count == 0)
            return;

        Console.Write("Wybierz numer kalendarza: ");
        int numer;

        try
        {
            numer = int.Parse(Console.ReadLine());
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby!");
            return;
        }

        if (!int.TryParse(numerInput, out int numer) || numer < 1 || numer > kalendarze.Count)
        {
            Console.WriteLine("Niepoprawny numer.");
            return;
        }

        Kalendarz wybrany = kalendarze[numer - 1];
        MenuKalendarza(wybrany);
    }

    private void MenuKalendarza(Kalendarz kalendarz)
    {
        bool dziala = true;

        while (dziala)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {kalendarz.PobierzTytul()} ===");
            Console.WriteLine("1. Dodaj dzień");
            Console.WriteLine("2. Dodaj okres");
            Console.WriteLine("3. Usuń dzień");
            Console.WriteLine("4. Pokaż dni (lista)");
            Console.WriteLine("5. Pokaż widok miesiąca");
            Console.WriteLine("6. Widok dnia");
            Console.WriteLine("7. Powrót");
            Console.Write("Wybierz opcję: ");

            string wybor = PobierzLinie();

            switch (wybor)
            {
                case "1":
                    DodajDzien(kalendarz);
                    break;
                case "2":
                    DodajDniHurtowo(kalendarz);
                    break;
                case "3":
                    UsuwanieDnia(kalendarz);
                    break;
                case "4":
                    WyswietlDniWKalendarzu(kalendarz);
                    break;
                case "5":
                    WyswietlMiesiacWKalendarzu(kalendarz);
                    break;
                case "6": 
                    OtworzDzien(kalendarz); 
                    break;
                case "7":
                    dziala = false;
                    break;
                default:
                    Console.WriteLine("Niepoprawna opcja!");
                    break;
            }
        }
    }

    private void WyswietlDniWKalendarzu(Kalendarz kalendarz)
    {
        Console.WriteLine();
        Console.WriteLine("=== dni ===");

        List<string> dniDoWyswietlenia = kalendarz.PobierzSformatowaneDni();

        if (dniDoWyswietlenia.Count == 0)
        {
            Console.WriteLine("Brak dni.");
            return;
        }

        for (int i = 0; i < dniDoWyswietlenia.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(dniDoWyswietlenia[i]);
            Console.ResetColor();
        }
    }

    private void UsuwanieDnia(Kalendarz kalendarz)
    {
        WyswietlDniWKalendarzu(kalendarz);

        if (kalendarz.PobierzLiczbeDni() == 0)
        {
            return;
        }

        try
        {
            Console.Write("Wybierz numer dnia do usunięcia: ");
            string numerInput = PobierzLinie();
            if (!int.TryParse(numerInput, out int numer))
            {
                Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby!");
                return;
            }

            if (kalendarz.UsunDzien(numer))
            {
                Console.WriteLine("Dzień został usunięty.");
            }
            else
            {
                Console.WriteLine("Niepoprawny numer dnia.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby!");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Błąd: Wpisana liczba jest za duża lub za mała!");
        }
    }

    private void DodajDzien(Kalendarz kalendarz)
    {
        Console.Write("Podaj datę (dd-mm-rrrr lub rrrr-mm-dd): ");
        try
        {
            DateTime data = DateTime.Parse(Console.ReadLine());
            Dzien dzien = new Dzien(data);
            kalendarz.DodajDzien(dzien);
            Console.WriteLine("Dodano dzień!");
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Niepoprawny format daty!");
        }
    }

    private void DodajDniHurtowo(Kalendarz kalendarz)
    {
        try
        {
            Console.Write("Podaj datę początkową (dd-mm-rrrr lub rrrr-mm-dd): ");
            DateTime dataPoczatkowa = DateTime.Parse(Console.ReadLine());

            Console.Write("Ile dni dodać?: ");
            int liczbaDni = int.Parse(Console.ReadLine());

            for (int i = 0; i < liczbaDni; i++)
            {
                Dzien dzien = new Dzien(dataPoczatkowa.AddDays(i));
                kalendarz.DodajDzien(dzien);
            }

            Console.WriteLine($"Pomyślnie dodano {liczbaDni} dni.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono niepoprawny format daty lub liczby!");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Błąd: Wpisana liczba jest za duża lub za mała!");
        }
    }

    private void WyswietlMiesiacWKalendarzu(Kalendarz kalendarz)
    {
        Console.Write("Podaj rok: ");
        string rokInput = PobierzLinie();
        if (!int.TryParse(rokInput, out int rok))
        {
            Console.WriteLine("Błąd: Niepoprawna wartość roku! Operacja anulowana.");
            return;
        }

        Console.Write("Podaj numer miesiąca (1-12): ");
        string miesiacInput = PobierzLinie();
        if (!int.TryParse(miesiacInput, out int miesiac) || miesiac < 1 || miesiac > 12)
        {
            Console.WriteLine("Błąd: Miesiąc musi być liczbą z przedziału od 1 do 12!");
            return;
        }

        kalendarz.PokazWidokMiesiecy(rok, miesiac);
    }

    private void WyswietlMiesiac()
    {
        Console.Write("Podaj rok: ");
        string rokInput = PobierzLinie();
        if (!int.TryParse(rokInput, out int rok))
        {
            Console.WriteLine("Błąd: Niepoprawna wartość roku!");
            return;
        }

        Console.Write("Podaj numer miesiąca (1-12): ");
        string miesiacInput = PobierzLinie();
        if (!int.TryParse(miesiacInput, out int miesiac) || miesiac < 1 || miesiac > 12)
        {
            Console.WriteLine("Brak takiego miesiąca (wybierz 1-12).");
            return;
        }

        DateTime wybranaData = new DateTime(rok, miesiac, 1);
        DateTime poprzedniaData = wybranaData.AddMonths(-1);
        DateTime nastepnaData = wybranaData.AddMonths(1);

        Kalendarz.RysujKalendarz(poprzedniaData.Year, poprzedniaData.Month);
        Kalendarz.RysujKalendarz(wybranaData.Year, wybranaData.Month);
        Kalendarz.RysujKalendarz(nastepnaData.Year, nastepnaData.Month);
    }

    private void OtworzDzien(Kalendarz kalendarz)
    {
    WyswietlDniWKalendarzu(kalendarz);

    if (kalendarz.PobierzLiczbeDni() == 0) return;

        try
        {
            Console.Write("\nWybierz numer dnia, aby zaplanować w nim aktywności: ");
            string numerInput = PobierzLinie();
            if (!int.TryParse(numerInput, out int numer))
            {
                Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby!");
                return;
            }

            Dzien wybranyDzien = kalendarz.PobierzDzien(numer); 

            if (wybranyDzien != null)
            {
                MenuDnia(wybranyDzien); 
            }
            else
            {
                Console.WriteLine("Niepoprawny numer dnia.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby!");
        }
    }


    private void MenuDnia(Dzien dzien)
    {
        bool dziala = true;

        while (dziala)
        {
      
            dzien.WyswietlWidokDnia(); 

            Console.WriteLine("=== ZARZĄDZANIE DNIEM ===");
            Console.WriteLine("1. Dodaj aktywność");
            Console.WriteLine("2. Edytuj aktywność");
            Console.WriteLine("3. Usuń aktywność");
            Console.WriteLine("4. Powrót");
            Console.Write("Wybierz opcję: ");

            string wybor = PobierzLinie();

            switch (wybor)
            {
                case "1": dzien.DodajAktywnosc(); break; 
                case "2": dzien.EdytujAktywnosc(); break; 
                case "3": dzien.UsunAktywnosc(); break; 
                case "4": dziala = false; break;
                default: Console.WriteLine("Niepoprawna opcja!"); break;
            }
        }
    }


    private string PobierzLinie() => Console.ReadLine()?.Trim() ?? string.Empty;

    private void ZapiszDoPliku(string sciezka)
    {
        try
        {
            var opcje = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string jsonString = JsonSerializer.Serialize(kalendarze, opcje);
            File.WriteAllText(sciezka, jsonString);
            Console.WriteLine("Pomyślnie zapisano stan aplikacji do pliku JSON!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zapisu: {ex.Message}");
        }
    }

    private void WczytajZPliku(string sciezka)
    {
        if (!File.Exists(sciezka))
        {
            Console.WriteLine("Brak zapisanego pliku danych.");
            return;
        }

        try
        {
            var opcje = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            string jsonString = File.ReadAllText(sciezka);
            var wczytaneKalendarze = JsonSerializer.Deserialize<List<Kalendarz>>(jsonString, opcje);

            if (wczytaneKalendarze != null)
            {
                kalendarze = wczytaneKalendarze;
                Console.WriteLine("Pomyślnie wczytano dane z pliku JSON!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas odczytu danych: {ex.Message}");
        }
    }
}