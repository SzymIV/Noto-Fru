using System;
using System.Collections.Generic;

public class Kalendarz
{
    private string tytulDestynacji;
    private List<Dzien> dni = new List<Dzien>();

    public Kalendarz(string tytul)
    {
        tytulDestynacji = tytul;
    }

    public string TytulDestynacji()
    {
        return tytulDestynacji;
    }

    public string PobierzTytul()
    {
        return tytulDestynacji;
    }

    public void DodajDzien(Dzien dzien)
    {
        dni.Add(dzien);
    }

    public bool UsunDzien(int indeks)
    {
        if (indeks < 1 || indeks > dni.Count)
        {
            return false;
        }

        dni.RemoveAt(indeks - 1);
        return true;
    }

    public int PobierzLiczbeDni()
    {
        return dni.Count;
    }

    public List<string> PobierzSformatowaneDni()
    {
        List<string> sformatowaneDni = new List<string>();

        for (int i = 0; i < dni.Count; i++)
        {
            sformatowaneDni.Add($"{i + 1}. {dni[i].Data.ToShortDateString()}");
        }

        return sformatowaneDni;
    }

    public void PokazWidokTrzechMiesiecy(int rok, int miesiac)
    {
        DateTime wybranaData = new DateTime(rok, miesiac, 1);
        DateTime poprzedniaData = wybranaData.AddMonths(-1);
        DateTime nastepnaData = wybranaData.AddMonths(1);

        RysujKalendarz(poprzedniaData.Year, poprzedniaData.Month, this.dni);
        RysujKalendarz(wybranaData.Year, wybranaData.Month, this.dni);
        RysujKalendarz(nastepnaData.Year, nastepnaData.Month, this.dni);
    }

    public static void RysujKalendarz(int rok, int miesiac, List<Dzien> listaDni = null)
    {
        DateTime pierwszyDzien = new DateTime(rok, miesiac, 1);
        int dniWMiesiacu = DateTime.DaysInMonth(rok, miesiac);

        Console.WriteLine($"\n=== {pierwszyDzien.ToString("MMMM yyyy").ToUpper()} ===");
        Console.WriteLine($"{"Pn",4}{"Wt",4}{"Sr",4}{"Cz",4}{"Pt",4}{"Sb",4}{"Nd",4}");

        int dzienTygodnia = (int)pierwszyDzien.DayOfWeek;
        int przesuniecie = dzienTygodnia == 0 ? 6 : dzienTygodnia - 1;

        for (int i = 0; i < przesuniecie; i++)
        {
            Console.Write("    ");
        }

        for (int dzien = 1; dzien <= dniWMiesiacu; dzien++)
        {
            DateTime aktualnaData = new DateTime(rok, miesiac, dzien);
            bool czyMamyTenDzien = listaDni != null && listaDni.Exists(d => d.Data.Date == aktualnaData.Date);

            if (czyMamyTenDzien)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{dzien,3} ");
                Console.ResetColor();
            }
            else
            {
                Console.Write($"{dzien,3} ");
            }

            if ((dzien + przesuniecie) % 7 == 0 || dzien == dniWMiesiacu)
            {
                Console.WriteLine();
            }
        }
    }
}