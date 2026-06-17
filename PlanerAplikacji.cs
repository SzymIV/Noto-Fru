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

            string wybor = Console.ReadLine();

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
        string nazwa = Console.ReadLine();
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
        int numer = int.Parse(Console.ReadLine());

        if (numer < 1 || numer > kalendarze.Count)
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

            string wybor = Console.ReadLine();

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
            int numer = int.Parse(Console.ReadLine());

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
        DateTime data = DateTime.Parse(Console.ReadLine());
        Dzien dzien = new Dzien(data);
        kalendarz.DodajDzien(dzien);
        Console.WriteLine("Dodano dzień!");
    }

    private void DodajDniHurtowo(Kalendarz kalendarz)
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

    private void WyswietlMiesiacWKalendarzu(Kalendarz kalendarz)
    {
        try
        {
            Console.Write("Podaj rok: ");
            int rok = int.Parse(Console.ReadLine());

            Console.Write("Podaj numer miesiąca (1-12): ");
            int miesiac = int.Parse(Console.ReadLine());

            if (miesiac < 1 || miesiac > 12)
            {
                Console.WriteLine("Błąd: Miesiąc musi być liczbą z przedziału od 1 do 12!");
                return;
            }

            kalendarz.PokazWidokMiesiecy(rok, miesiac);
        }
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby! Operacja anulowana.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Błąd: Wpisana liczba jest za duża lub za mała!");
        }
    }

    private void WyswietlMiesiac()
    {
        try
        {
            Console.Write("Podaj rok: ");
            int rok = int.Parse(Console.ReadLine());

            Console.Write("Podaj numer miesiąca (1-12): ");
            int miesiac = int.Parse(Console.ReadLine());

            if (miesiac < 1 || miesiac > 12)
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
        catch (FormatException)
        {
            Console.WriteLine("Błąd: Wprowadzono tekst zamiast liczby!");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Błąd: Wpisana liczba jest za duża lub za mała!");
        }
    }

    private void OtworzDzien(Kalendarz kalendarz)
    {
    WyswietlDniWKalendarzu(kalendarz);

    if (kalendarz.PobierzLiczbeDni() == 0) return;

        try
        {
            Console.Write("\nWybierz numer dnia, aby zaplanować w nim aktywności: ");
            int numer = int.Parse(Console.ReadLine());

            // Korzystamy z nowej metody z Kroku 1
            Dzien wybranyDzien = kalendarz.PobierzDzien(numer); 

            if (wybranyDzien != null)
            {
                MenuDnia(wybranyDzien); // Przechodzimy poziom niżej!
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
            // Za każdym obrotem pętli pokazujemy ładną tabelkę z aktywnościami z Twojego kodu
            dzien.WyswietlWidokDnia(); 

            Console.WriteLine("=== ZARZĄDZANIE DNIEM ===");
            Console.WriteLine("1. Dodaj aktywność");
            Console.WriteLine("2. Edytuj aktywność");
            Console.WriteLine("3. Usuń aktywność");
            Console.WriteLine("4. Powrót");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            // UWAGA: Jeśli w Kroku 2 nie usunąłeś parametrów w klasie Dzien.cs, 
            // musisz w nawiasach wstawić np. (null) i (0) aby kod się kompilował.
            switch (wybor)
            {
                case "1": dzien.DodajAktywnosc(null); break; 
                case "2": dzien.EdytujAktywnosc(0); break; 
                case "3": dzien.UsunAktywnosc(0); break; 
                case "4": dziala = false; break;
                default: Console.WriteLine("Niepoprawna opcja!"); break;
            }
        }
    }


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