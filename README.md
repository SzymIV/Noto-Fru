# 🗺️ Planer Podróży

Konsolowa aplikacja w C# do planowania wyjazdów turystycznych. Pozwala zarządzać wieloma kalendarzami podróży, planować aktywności (atrakcje, noclegi, przejazdy) z walidacją konfliktów czasowych oraz zapisywać stan do pliku JSON.

Projekt zaliczeniowy z Programowania Obiektowego — Wydział Informatyki Politechniki Białostockiej, 2026.

---

## Funkcjonalności

- 📅 Tworzenie wielu niezależnych kalendarzy podróży
- ➕ Dodawanie dni pojedynczo lub jako zakres dat
- 🎯 Planowanie aktywności trzech typów: **Atrakcja**, **Nocleg**, **Przejazd**
- ⚠️ Automatyczne wykrywanie konfliktów czasowych
- ✏️ Edycja i usuwanie aktywności oraz dni
- 🗓️ Widok miesięczny z podświetleniem zaplanowanych dni
- 💾 Zapis i odczyt stanu aplikacji do pliku `kalendarze.json`

---

## Wymagania

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) lub nowszy
- System operacyjny: Windows, macOS lub Linux

---

## Uruchomienie

```bash
# Sklonuj repozytorium
git clone https://github.com/nazwa/noto-fru.git
cd noto-fru

# Uruchom aplikację
dotnet run
```

Plik `kalendarze.json` tworzony jest automatycznie w katalogu roboczym po wybraniu opcji zapisu.

---

## Struktura projektu

```
.
├── Program.cs            # Punkt wejścia aplikacji
├── PlanerAplikacji.cs    # Kontroler główny, menu, I/O
├── Kalendarz.cs          # Encja wyjazdu, widok miesięczny
├── Dzien.cs              # Dzień z listą aktywności, walidacja godzin
├── Aktywnosc.cs          # Abstrakcyjna klasa bazowa aktywności
├── Atrakcja.cs           # Typ aktywności: atrakcja turystyczna
├── Nocleg.cs             # Typ aktywności: nocleg
└── Przejazd.cs           # Typ aktywności: przejazd
```

---

## Hierarchia klas

```
PlanerAplikacja
└── Kalendarz  (kompozycja, 0..*)
    └── Dzien  (kompozycja, 0..*) ── implementuje IComparable<Dzien>
        └── Aktywnosc  (kompozycja, 0..*) «abstract»
            ├── Atrakcja
            ├── Nocleg
            └── Przejazd
```

---

## Przykładowy przepływ pracy

```
=== PLANER PODRÓŻY ===
1. Dodaj kalendarz        ← utwórz np. "Włochy 2026"
3. Otwórz kalendarz
  └── 2. Dodaj okres      ← data startowa + liczba dni
  └── 6. Widok dnia
        └── 1. Dodaj aktywność  ← wybierz typ, podaj godziny i szczegóły
5. Zapisz do pliku        ← zapisuje do kalendarze.json
```

---

## Zespół

| Osoba | Zakres prac |
|---|---|
| **Szymon Ciż** | `PlanerAplikacji.cs` — menu, nawigacja, serializacja JSON |
| **Marek Czech** | `Aktywnosc.cs`, `Atrakcja.cs`, `Nocleg.cs`, `Przejazd.cs`, `Dzien.cs` — logika aktywności |
| **Dawid Żmijewski** | `Kalendarz.cs` — zarządzanie dniami, widok miesięczny |

---

## Licencja

Projekt edukacyjny — brak licencji komercyjnej.
