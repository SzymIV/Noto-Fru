using System;
using System.Collections.Generic;

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
            Console.WriteLine("4. Kalendarz");
            Console.WriteLine("5. Wyjdź");
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
            Console.WriteLine($"{i + 1}. {kalendarze[i].TytulDestynacji()}");
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
            Console.WriteLine("2. Usuń dzień");
            Console.WriteLine("3. Pokaż dni (lista)");
            Console.WriteLine("4. Pokaż widok miesiąca");
            Console.WriteLine("5. Powrót");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajDzien(kalendarz);
                    break;
                case "2":
                    UsuwanieDnia(kalendarz);
                    break;
                case "3":
                    WyswietlDniWKalendarzu(kalendarz);
                    break;
                case "4":
                    WyswietlMiesiacWKalendarzu(kalendarz);
                    break;
                case "5":
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
        Console.Write("Podaj datę (dd-mm-rrrr): ");
        DateTime data = DateTime.Parse(Console.ReadLine());
        Dzien dzien = new Dzien(data);
        kalendarz.DodajDzien(dzien);
        Console.WriteLine("Dodano dzień!");
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

            kalendarz.PokazWidokTrzechMiesiecy(rok, miesiac);
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
}