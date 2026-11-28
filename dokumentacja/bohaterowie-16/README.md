# Bohaterowie — dane projektowe wersja 1

Pliki JSON 16 wybranych bohaterów znajdują się w `riftoryn-unity/Assets/Resources/HeroDesign/`. To **specyfikacja do implementacji i testów balansu**. Czyta je symulator Python i narzędzie analizy w Unity, ale nie sterują one jeszcze rozgrywką: dopisanie zdolności nie sprawia, że jest grywalna. Nie nadpisujemy `riftoryn-unity/Assets/Resources/hero_stats.json` ani kodu prototypu.

Każdy plik zawiera identyfikator, rolę, żywioł, HP, prędkość, proponowany atak podstawowy, pasywkę, Q/E/R, czasy odnowienia i liczbowe parametry efektów. Przywoływacze mają również dane towarzysza. `status_unity` opisuje jedynie zakres istniejącego prototypu; nie obiecuje, że każda wartość w tym pliku jest już stosowana. Teksty są po polsku; identyfikatory są stabilnymi kluczami bez polskich znaków.

## Wspólne reguły projektu

- Jednostki: czas w sekundach, odległość w metrach Unity, HP/osłona/obrażenia w punktach, wartości procentowe jako liczby 0–100. Prędkość to metry na sekundę. Odnowienie `0` oznacza pasywkę bez aktywowanego czasu odnowienia; dodatkowy wewnętrzny czas zapisano w parametrach pasywki, jeśli występuje.
- Q/E/R to zdolności aktywne, uruchamiane niezależnie. Czas odnowienia zaczyna się po skutecznym użyciu; brak prawidłowego celu lub martwy towarzysz nie zużywa odnowienia. Celowanie we wskazaną wieżę wymaga jej odkrycia, zasięgu i widoczności; w prototypie szczegóły celowania wymagają dopracowania. Gdy opis mówi o najbliższym sojuszniku, wybieramy najbliższego żywego uczestnika w zasięgu; bohater nie jest własnym sojusznikiem. Zdolność lecząca wybiera najbliższego spośród rannych żywych sojuszników, a zdolność bez odpowiedniego celu nie zostaje użyta. Remis rozstrzyga stały identyfikator uczestnika.
- Osłony sumują się i maleją co sekundę o 10% bieżącej wartości, chyba że efekt określa inaczej. Leczenie nie podnosi HP ponad maksimum i nie wskrzesza. Premie do leczenia stosuje się raz: liczby Liory już zawierają jej pasywkę. Zwiększenia szybkości z różnych źródeł nie sumują się — obowiązuje największe; ponowne użycie tego samego efektu odświeża czas. Różne redukcje obrażeń działają mnożnikowo; redukcja określonego rodzaju dotyczy tylko tego rodzaju.
- Spowolnienia tego samego typu nie sumują się: obowiązuje silniejsze, a każde źródło wygasa osobno. Procent spowolnienia ostrzału oznacza zmniejszenie liczby strzałów na sekundę, nie skrócenie przerwy między strzałami. Redukcje spowolnienia zmniejszają jego wielkość, nie czas; immunitet blokuje tylko wskazany efekt.
- Wszyscy mogą wykonywać atak podstawowy według wartości w pliku: zasięg 7,8 m i odnowienie 1 s. To **propozycja**, a nie obecny wspólny system ataku w Unity. Nie dodawaj premii klasowych z wcześniejszego dokumentu: nie są zatwierdzone ani uwzględnione w tych liczbach.
- Towarzysz automatycznie podąża za właścicielem, ma własne HP i może otrzymywać obrażenia. Przy 0 HP przestaje działać do końca rundy; nie jest automatycznie wskrzeszany. Jego proponowana wartość ataku nie oznacza, że wykonuje automatyczne ataki podczas ucieczki: w tej wersji używa się go przez umiejętności. Gdy oddali się o ponad 12 m, ma wrócić do właściciela; sposób rozwiązania przeszkód i bezpieczny punkt powrotu wymagają implementacji i testów, bez przechodzenia przez ściany.
- Opisane zestawy dotyczą **ucieczki**. Obrona przez ręczne pilotowanie wież, sieć, widoczność i interakcje ze zwierzętami muszą zostać zaprojektowane i zweryfikowane osobno; nie należy zakładać ich działania na podstawie tych plików.

## Różnice i decyzje do testów

- `hp` i prędkości postaci już grywalnych w Unity pochodzą z `RiftorynTrial.cs`; dla pozostałych punktem wyjścia jest `hero_stats.json`. Wcześniejszy katalog zawiera inne wartości dla części postaci (np. Rian, Saila, Zet). Atak podstawowy i dane brakujących towarzyszy to wartości projektowe z katalogu; nie wszystkie mają dziś działający odpowiednik w meczu.
- Q/E/R Kael, Nery, Zeta, Konevy, Miko, Sarny i Saily opisują istniejące kierunki oraz odnowienia prototypu. Niektóre szczegóły wymagają porównania podczas migracji; E/R pozostałych oraz większość zachowań pozostałych towarzyszy są nowymi propozycjami. Q Pyrry w Unity jest tymczasowe i różni się od proponowanego tutaj.
- W szczególności pełne zniszczenie wieży przez Q Miko i trwała szansa pudła mogą być zbyt mocne. Wymagają testów przeciw losowości i mapom, zanim zostaną uznane za zbalansowane.
- Wybór najbliższego sojusznika jest obecnym uproszczeniem. Celowanie przez gracza, zachowanie AI, ikony, sygnalizacja zasięgu i stany niedostępności wymagają dalszego wdrożenia.
- Proponowane współdziałanie przywołań, osłon, obrażeń, blokowania pocisków i wież wymaga implementacji oraz prób 5 na 5. Dokument nie rozstrzyga zmian w opowiadaniach ani biografiach.

## Kontrola danych

Z katalogu repozytorium uruchom `python -m unittest tests/test_hero_design_data.py`. Test sprawdza kompletność szesnastki, podział na role, jednostki podstawowych statystyk, komplet Q/E/R, unikalne identyfikatory i wymagane dane towarzyszy. Nie sprawdza działania gry ani nie dowodzi balansu.
