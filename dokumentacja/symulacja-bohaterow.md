# Symulacja bohaterów: model 1D v2

To narzędzie **orientacyjne**, napisane w Pythonie bez dodatkowych bibliotek. Wczytuje pliki z `riftoryn-unity/Assets/Resources/HeroDesign/` i symuluje przebieg ucieczki. Pozwala szybko wychwycić wpływ zmian danych i odtwarzać te same warunki. **Nie jest symulacją całych meczów 5 na 5, siedmiu map ani walidacją balansu gry.**

## Narzędzie w aplikacji Unity

W menu wybierz **TESTY**. Ustaw liczbę wież, prób na skład, rozmieszczenie i opcjonalny atak, a następnie uruchom analizę. Obliczenia wykonywane są w tle, można je przerwać. Po zakończeniu tabela przedstawia wpływ na sojuszników, ich szansę dotarcia do mety i własną szansę dotarcia dla każdego z 16 bohaterów. Wyświetlony wpływ ma znaczenie opisane w sekcji „Kto jest pomocnym członkiem drużyny”.

Aplikacja korzysta z tych samych plików danych i analogicznego modelu, lecz ma osobną implementację w C# i inny generator liczb losowych niż Python. Wartości dla tych samych numerów seedów **nie muszą być identyczne**. W Unity ustawiono seed początkowy 100 i krok 0,5 s. Analiza w aplikacji nie zapisuje obecnie raportu do pliku; Python nadal pozwala generować CSV. Żadna z wersji nie uruchamia prawdziwej rozgrywki w Unity.

## Uruchamianie

W katalogu repozytorium:

```powershell
# Wszystkie kombinacje pięciu z 16: 4368 składów, po 3 warianty, 10 wież
python tools/symulacja_bohaterow.py wszystkie --proby 3 --seed 0 --wieze 10 --csv riftoryn-unity/Temp/symulacja.csv

# Konkretny skład lub samotna postać
python tools/symulacja_bohaterow.py druzyna kael nera zet koneva sarna --proby 20 --seed 100
python tools/symulacja_bohaterow.py bohater miko --proby 20 --seed 100

# Kto pomaga pozostałym: porównanie każdej postaci z innymi przy tej samej czwórce
python tools/symulacja_bohaterow.py teamwork --proby 10 --wieze 10 --seed 100 --csv riftoryn-unity/Temp/analiza-teamwork.csv

# Trudniejsze skupiska wież i eksperymentalny atak podstawowy
python tools/symulacja_bohaterow.py druzyna kael nera zet koneva sarna --wieze 12 --uklad skupiska --atak --krok 0.25 --proby 10

# Dokładna lista tego, co model obejmuje i czego nie obejmuje
python tools/symulacja_bohaterow.py pokrycie

# Testy samego narzędzia i plików danych
python -m unittest tests/test_symulacja_bohaterow.py tests/test_hero_design_data.py
```

`--seed` to pierwszy numer wariantu, `--proby` to liczba kolejnych numerów. Każdy skład otrzymuje ten sam zestaw seedów. CSV zawiera wszystkie składy, a plik `.csv.meta.json` obok niego zapisuje seedy, SHA-256 danych wejściowych, wersję modelu i pokrycie zdolności. Katalog `Temp` może zostać wyczyszczony przez Unity; w razie potrzeby wybierz inny plik CSV. Uruchomienia z identycznymi danymi, kodem i seedami dają identyczne wyniki.

`--wieze` wybiera od 1 do 50 wież (domyślnie 10). `--uklad` przyjmuje `rowny`, `losowy` (domyślny, odchylenia wzdłuż trasy) lub `skupiska` (pary wież). `--krok` to 0.25, 0.5 (domyślnie) albo 1 s; mniejszy krok zwiększa precyzję i czas obliczeń. `--atak` włącza opcjonalny automatyczny atak podstawowy, którego nie mają dziś wszystkie postacie w Unity. Parametry są zapisywane w metadanych CSV.

## Co jest liczone

- Prosta trasa 472,5 m i limit 137 s, domyślnie dziesięć wież o 100 HP. Każdy seed losuje ich typy i wybór celu podczas ostrzału; przy układzie `losowy` zmienia też pozycje. Obrażenia, nominalne zasięgi i odstępy strzałów pochodzą z obecnego prototypu Unity. Zasięg w modelu 1D nie uwzględnia odległości wieży od drogi.
- Bohaterowie mają HP i szybkość ze swoich plików, poruszają się automatycznie, mogą otrzymywać osłony i leczenie. Osłona spada o 10% na sekundę. Po włączeniu `--atak` każdy żywy bohater automatycznie atakuje najbliższą wieżę w swoim zasięgu, jednocześnie kontynuując ruch; obrażenia i odnowienie pochodzą z plików projektu. To eksperymentalna zasada modelu.
- Model obejmuje **część efektów 18 z 48 aktywnych zdolności**. Przykładowo uwzględnia osłonę Q Kael, leczenie E Liory, obrażenia Q Rian, wyłączenie Q Saily oraz cztery losowe wyniki Q Miko; część zdolności, takich jak E Thory, odwzorowuje tylko część efektu. `pokrycie` wypisuje dokładne pary bohater/klawisz. Pomijane są wszystkie pasywki oraz zachowania i HP przywołań, więc umiejętności czworga przywoływaczy nie działają w modelu.
- Wszystkie odległości są mierzone wzdłuż jednej linii. Wszyscy gracze i wieże są widoczni; nie ma geometrii, kolizji, uników, leczenia w czasie, mgły jako obrażeń, statusów żywiołów, pocisków w locie, synergii niewymienionych efektów ani rzeczywistego AI. Limit 137 s jest uproszczeniem zamiast symulacji mgły. Nie ma dwóch drużyn ani interakcji między ich obronami.
- Zdarzenia są rozpatrywane co `--krok`, domyślnie 0,5 s; jedna wieża strzela maksymalnie raz na krok. Odstępy zaokrąglają się do kolejnego kroku, więc przy kroku 0,5 s odstęp 1,4 s daje efektywnie 1,5 s. Przy 0,25 s daje 1,5 s również dla części faz, a wyniki mogą się różnić zależnie od momentu rozpoczęcia. Nie jest to dokładne odwzorowanie szybkostrzelności Unity.
- Automatyczna polityka używa obsługiwanych zdolności w zasięgu, bez przewidywania przyszłości. Nie jest to optymalny gracz. Test samotnego bohatera szczególnie zaniża użyteczność wsparcia i nie pozwala ocenić jego roli w drużynie.

## Jak czytać wynik

`mean_arrived` to średnia liczba osób na mecie; `all_arrived_pct` to odsetek prób z całą drużyną na mecie. `mean_time_s` to średni czas **do zakończenia symulacji**, także w przypadku śmierci lub limitu, a nie średni czas zwycięstw. Sortowanie uwzględnia czas tylko przy 100% pełnych sukcesów. Trzy seedy nie dają wiarygodnych wniosków statystycznych. Małe różnice rankingu i przewaga szybkości mogą wynikać z założeń modelu.

Przy porównywaniu zmian używaj tych samych seedów i sprawdzaj hash danych, a następnie powtórz badanie na większym zestawie seedów. Żeby ocenić rzeczywisty balans, potrzebna jest implementacja brakujących umiejętności i towarzyszy w Unity oraz testy z realnymi mapami, kolizją, AI i graczami.

## Kto jest pomocnym członkiem drużyny

Polecenie `teamwork` analizuje całą pulę składów pięcioosobowych. Dla każdej czwórki porównuje wynik tych samych czterech osób po dodaniu kolejno każdej możliwej piątej postaci; wszyscy otrzymują ten sam zestaw seedów. Przy 16 bohaterach powstaje 4368 składów i 1365 porównywanych czwórek na postać. Każdy wynik uwzględnia wskazaną liczbę prób na skład.

- `companions_arrived_pct`: jak często czterech **pozostałych** bohaterów dociera do mety w obecności danej postaci, uśrednione po czwórkach i seedach.
- `delta_pp`: różnica w punktach procentowych między tym wynikiem a średnią wyników tej samej czwórki z każdym innym dozwolonym piątym bohaterem, następnie uśredniona po czwórkach. Liczba ujemna oznacza mniej ocalałych sojuszników niż średnia zastępstw w tym modelu; dodatnia — więcej. Własne przeżycie badanej postaci nie jest wliczane do tej miary.
- `self_arrived_pct`: osobno szansa samej badanej postaci na dotarcie do mety. `contexts` to liczba czwórek, nie liczba niezależnych prób.

Ten pomiar wyłapuje również odciąganie ostrzału i niszczenie wież, nie tylko bezpośrednie leczenie. Nie jest jednak pełnym wkładem w zwycięstwo: pomija m.in. czas, własne dotarcie postaci w głównym wskaźniku i wszystkie zachowania poza modelem. Zmiana składu może też zmienić kolejność losowań celów lub sabotażu mimo tego samego seeda. Wyniki z kilkunastu seedów są wstępnym sygnałem, nie miarą istotności statystycznej. Przywoływaczy i inne postacie bez modelowanych zdolności nie wolno na ich podstawie uznawać za słabych w grze.
