# Symulacja bohaterów: model 1D v1

To narzędzie **orientacyjne**, niezależne od Unity, napisane w Pythonie bez dodatkowych bibliotek. Wczytuje pliki z `dokumentacja/bohaterowie-16/` i symuluje przebieg ucieczki. Pozwala szybko wychwycić wpływ zmian danych i odtwarzać te same warunki. **Nie jest symulacją całych meczów 5 na 5, siedmiu map ani walidacją balansu gry.**

## Uruchamianie

W katalogu repozytorium:

```powershell
# Wszystkie kombinacje pięciu z 16: 4368 składów, trzy warianty obrony
python tools/symulacja_bohaterow.py wszystkie --proby 3 --seed 0 --csv riftoryn-unity/Temp/symulacja.csv

# Konkretny skład lub samotna postać
python tools/symulacja_bohaterow.py druzyna kael nera zet koneva sarna --proby 20 --seed 100
python tools/symulacja_bohaterow.py bohater miko --proby 20 --seed 100

# Dokładna lista tego, co model obejmuje i czego nie obejmuje
python tools/symulacja_bohaterow.py pokrycie

# Testy samego narzędzia i plików danych
python -m unittest tests/test_symulacja_bohaterow.py tests/test_hero_design_data.py
```

`--seed` to pierwszy numer wariantu, `--proby` to liczba kolejnych numerów. Każdy skład otrzymuje ten sam zestaw seedów. CSV zawiera wszystkie składy, a plik `.csv.meta.json` obok niego zapisuje seedy, SHA-256 danych wejściowych, wersję modelu i pokrycie zdolności. Katalog `Temp` może zostać wyczyszczony przez Unity; w razie potrzeby wybierz inny plik CSV. Uruchomienia z identycznymi danymi, kodem i seedami dają identyczne wyniki.

## Co jest liczone

- Prosta trasa 472,5 m i limit 137 s, pięć równomiernie rozstawionych wież o 100 HP. Każdy seed losuje ich typy oraz wybór celu podczas ostrzału. Obrażenia, nominalne zasięgi i odstępy strzałów pochodzą z obecnego prototypu Unity.
- Bohaterowie mają HP i szybkość ze swoich plików, poruszają się automatycznie, mogą otrzymywać osłony i leczenie. Osłona spada o 10% na sekundę. Podstawowy atak nie jest symulowany.
- Model obejmuje **część efektów 17 z 48 aktywnych zdolności**. Przykładowo uwzględnia osłonę Q Kael, leczenie E Liory, obrażenia Q Rian i wyłączenie Q Saily; część zdolności, takich jak E Thory, odwzorowuje tylko część efektu. `pokrycie` wypisuje dokładne pary bohater/klawisz. Pomijane są wszystkie pasywki oraz zachowania i HP przywołań, więc umiejętności czworga przywoływaczy nie działają w modelu. Q Miko i jego losowe wyniki również nie są modelowane.
- Wszystkie odległości są mierzone wzdłuż jednej linii. Wszyscy gracze i wieże są widoczni; nie ma geometrii, kolizji, uników, leczenia w czasie, mgły jako obrażeń, statusów żywiołów, pocisków w locie, synergii niewymienionych efektów ani rzeczywistego AI. Limit 137 s jest uproszczeniem zamiast symulacji mgły. Nie ma dwóch drużyn ani interakcji między ich obronami.
- Krok symulacji wynosi 1 s; jedna wieża strzela maksymalnie raz na krok. Wartości pomiędzy krokami są zaokrąglane przez przebieg symulacji: np. odstęp 1,4 s daje efektywnie 2 s, a efekt trwający 1,2 s może objąć dwa kroki ruchu. Pozycje wież są stałe, seed zmienia typy i losowanie celu. Wyniki nie odwzorowują dokładnej szybkostrzelności Unity.
- Automatyczna polityka używa obsługiwanych zdolności w zasięgu, bez przewidywania przyszłości. Nie jest to optymalny gracz. Test samotnego bohatera szczególnie zaniża użyteczność wsparcia i nie pozwala ocenić jego roli w drużynie.

## Jak czytać wynik

`mean_arrived` to średnia liczba osób na mecie; `all_arrived_pct` to odsetek prób z całą drużyną na mecie. `mean_time_s` to średni czas **do zakończenia symulacji**, także w przypadku śmierci lub limitu, a nie średni czas zwycięstw. Sortowanie uwzględnia czas tylko przy 100% pełnych sukcesów. Trzy seedy nie dają wiarygodnych wniosków statystycznych. Małe różnice rankingu i przewaga szybkości mogą wynikać z założeń modelu.

Przy porównywaniu zmian używaj tych samych seedów i sprawdzaj hash danych, a następnie powtórz badanie na większym zestawie seedów. Żeby ocenić rzeczywisty balans, potrzebna jest implementacja brakujących umiejętności i towarzyszy w Unity oraz testy z realnymi mapami, kolizją, AI i graczami.
