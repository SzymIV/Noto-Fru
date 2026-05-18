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
            Console.WriteLine("4. Wyjdź");
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
            Console.WriteLine("2. Pokaż dni");
            Console.WriteLine("3. Powrót");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajDzien(kalendarz);
                    break;

                case "2":
                    kalendarz.PokazDni();
                    break;

                case "3":
                    dziala = false;
                    break;

                default:
                    Console.WriteLine("Niepoprawna opcja!");
                    break;
            }
        }
    }

    private void DodajDzien(Kalendarz kalendarz)
    {
        Console.Write("Podaj datę (rrrr-mm-dd): ");

        DateTime data = DateTime.Parse(Console.ReadLine());

        Dzien dzien = new Dzien(data);

        kalendarz.DodajDzien(dzien);

        Console.WriteLine("Dodano dzień!");
    }
}