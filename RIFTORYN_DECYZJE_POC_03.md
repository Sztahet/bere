# Riftoryn — decyzje do GDD: zakres pierwszego PoC

**Cel:** wybrać najmniejszy grywalny wycinek, który sprawdzi główną pętlę Riftoryn: przygotowanie, obronę wieżami, atak Runnerów i wspólny rozwój drużyny. Opcje liczbowe są propozycjami do PoC, nie finalnym balansem.

## Jak odpowiedzieć

Przy każdym punkcie wpisz `A`, `B` albo `C`. Przy C dopisz własny wariant. Możesz dopisać wyjątki albo zmienić każdą propozycję. Skopiuj blok **Odpowiedź do wklejenia** na końcu.

## A. Gracze i bohaterowie

### 1. Ile osób ma być w każdej drużynie pierwszego PoC?

- [ ] **A — 2v2.** Mniejszy test, szybciej można sprawdzić podstawową pętlę i poprawiać błędy.
- [ ] **B — 3v3.** Testuje jednocześnie trzy główne role bohaterów i bliżej przypomina docelowe 5v5.
- [ ] **C — Własna liczebność:**

### 2. Jak duża ma być pula bohaterów dostępnych w PoC?

- [ ] **A — 6 bohaterów.** Wystarczy wybór dla meczu 3v3 bez duplikatów i pozwala szybciej dopracować ich umiejętności.
- [ ] **B — 12 bohaterów.** Wykorzystujemy wszystkie istniejące specyfikacje alfa, choć potrzebują one jeszcze przeglądu.
- [ ] **C — Inna liczba:**

### 3. Który układ ról przyjmujemy na PoC?

> W GDD jest rozbieżność: główna część mówi o czterech rolach z Przywoływaczem, a szczegółowy zestaw 12 bohaterów opisuje Tanków, Wsparcie i Specjalistów.

- [ ] **A — Trzy role.** Tank, Wsparcie i Specjalista; Przywoływaczy i pety odkładamy.
- [ ] **B — Cztery role.** Włączamy Przywoływacza; trzeba przygotować co najmniej jedną grywalną postać tej roli z działającym petem.
- [ ] **C — Własny układ ról:**

### 4. Których bohaterów bierzemy do sześciopostaciowego PoC?

- [ ] **A — Proponowany skład:** Brom i Thora (Tankowie), Liora i Varek (Wsparcie), Saila i Zet (Specjaliści). Obejmuje Ziemię, Naturę, Błyskawice i Lód.
- [ ] **B — Inny zestaw sześciu.** Wpisz imiona oraz rolę każdej postaci w C lub w bloku odpowiedzi.
- [ ] **C — Własny skład / inne kryterium wyboru:**

### 5. Jak uprościć draft w PoC?

- [ ] **A — Zachować naprzemienny draft bez duplikatów.** Użyć krótkiej kolejności wyborów dostosowanej do liczby graczy.
- [ ] **B — Jednoczesny wybór bez draftu.** Każdy gracz wybiera postać; na potrzeby PoC duplikaty są dozwolone.
- [ ] **C — Własna reguła:**

## B. Mapa i wieże

### 6. Jak odtworzyć dwie kopie mapy w PoC?

- [ ] **A — Dwie identyczne mapy jednocześnie.** Obie drużyny mają własną obronę, zgodnie z docelową pętlą meczu.
- [ ] **B — Jedna mapa używana kolejno.** Drużyny na zmianę atakują tę samą mapę; skraca to pierwszy test, ale nie sprawdza dwóch kopii naraz.
- [ ] **C — Własna reguła:**

### 7. Jaką długość trasy testujemy?

- [ ] **A — Około 360 m.** Zachowujemy bieżące założenie i sprawdzamy bieg bezpośrednio w docelowej skali.
- [ ] **B — Około 240 m.** Skracamy pierwszą iterację, aby szybciej rozgrywać ataki; później wydłużamy trasę.
- [ ] **C — Inna długość:**

### 8. Które trzy bazowe wieże T1 włączamy?

- [ ] **A — Ogień, Lód, Błyskawice.** Obszar i podpalenie, spowolnienie oraz łańcuchowy atak.
- [ ] **B — Ogień, Ziemia, Światło.** Obszar i podpalenie, mocny pojedynczy pocisk oraz ujawnianie celu.
- [ ] **C — Inny zestaw:**

### 9. Jaki limit wież na gracza stosujemy w PoC?

- [ ] **A — 3 wieże.** Mniej obiektów i szybsze rundy; nie sprawdza pełnego założenia limitu.
- [ ] **B — 5 wież.** Zachowujemy zapisany limit bazowy i od razu testujemy zagęszczanie obrony.
- [ ] **C — Inny limit:**

### 10. Czy PoC ma zawierać specjalizacje wież T2?

- [ ] **A — Nie.** Testujemy wyłącznie wieże T1, a cały czas poświęcamy pętli atak–obrona.
- [ ] **B — Tak, po jednej gałęzi T2 dla każdej testowanej wieży.** Sprawdzamy decyzję rozwoju i automatyczne ulepszenie już postawionych wież.
- [ ] **C — Własny zakres:**

### 11. Czy w PoC włączamy ulepszenia T3?

- [ ] **A — Nie.** PoC kończy się na T2, żeby ograniczyć liczbę stanów do sprawdzenia.
- [ ] **B — Tak, ale tylko dla jednej testowanej wieży.** Sprawdzamy pełną ścieżkę T1→T2→T3 w małej skali.
- [ ] **C — Własny zakres:**

### 12. Czy ręczne pilotowanie wież jest częścią pierwszego PoC?

- [ ] **A — Tak.** Od początku testujemy automat oraz przejmowanie wieży przez Defendera.
- [ ] **B — Nie.** Najpierw sprawdzamy wyłącznie automatyczne wieże, a pilotaż dodajemy w kolejnej iteracji.
- [ ] **C — Własna reguła:**

## C. Walka i jednostki

### 13. Jaki zwykły atak ma Runner w PoC?

- [ ] **A — Krótki atak wręcz.** Łatwiejszy do wdrożenia; Runner musi zbliżyć się do wrogiej jednostki.
- [ ] **B — Prosty atak dystansowy.** Runner może atakować wrogie jednostki z bezpiecznej odległości; wieże nadal wymagają specjalnych zdolności.
- [ ] **C — Własny model ataku:**

### 14. Jaki czas odnowienia dashu przyjmujemy na start?

- [ ] **A — 6 s.** Częsty unik, bardziej zręcznościowy bieg.
- [ ] **B — 10 s.** Rzadszy unik; pozycjonowanie i osłony mają większe znaczenie.
- [ ] **C — Inny czas / zasada:**

### 15. Czy pet Przywoływacza ma być w PoC?

- [ ] **A — Nie.** Przywoływacz i pet czekają na późniejszy etap; zachowujemy wybraną pulę trzech ról.
- [ ] **B — Tak.** Włączamy jednego Przywoływacza i jego peta oraz minimalny zestaw komend i zasad przejmowania obrażeń.
- [ ] **C — Własny zakres:**

### 16. Jaki mob testujemy jako pierwszy?

- [ ] **A — Powolny, wytrzymały mob.** Służy jako osłona i absorbuje ostrzał wież.
- [ ] **B — Szybki, kruchy mob.** Służy do odciągania uwagi wież i wymusza szybką reakcję obrony.
- [ ] **C — Własny archetyp:**

### 17. Jak wprowadzić automatyczne przywołanie z technologii?

- [ ] **A — Jedno automatyczne przywołanie.** Wybrany węzeł raz w ataku przyzywa moba przy Runnerze najdalej od mety.
- [ ] **B — Odłożyć tę mechanikę.** Pierwszy PoC nie zawiera automatycznego przywołania; testuje tylko bazowego moba.
- [ ] **C — Własna reguła:**

## D. Drzewko, ekonomia i czas testu

### 18. Jak szerokie drzewko technologii włączamy?

- [ ] **A — Trzy żywioły z wybranych wież.** Tylko technologie powiązane z wieżami PoC.
- [ ] **B — Wszystkie siedem rdzeni T1, bez pełnych specjalizacji.** Sprawdzamy wspólne inwestowanie niezależnie od wybranej wieży.
- [ ] **C — Własny zakres:**

### 19. Ile reakcji żywiołów włączamy do PoC?

- [ ] **A — Żadnej.** Testujemy bazową macierz obrażeń i technologię bez reakcji.
- [ ] **B — Dwie reakcje.** Wybieramy dwie pary żywiołów i opisujemy ich efekty przed implementacją.
- [ ] **C — Inna liczba / konkretne reakcje:**

### 20. Jak ustawić długość fazy budowy w PoC?

- [ ] **A — 90 s przed pierwszym atakiem, 45 s po kolejnych parach.** Więcej czasu na pierwsze ustawienie obrony.
- [ ] **B — 60 s przed pierwszym atakiem, 30 s po kolejnych parach.** Krótsze sesje i szybsza iteracja testów.
- [ ] **C — Inne czasy:**

## Odpowiedź do wklejenia

```text
1 —
2 —
3 —
4 —
5 —
6 —
7 —
8 —
9 —
10 —
11 —
12 —
13 —
14 —
15 —
16 —
17 —
18 —
19 —
20 —
Uwagi / własne składy —
```

## Odpowiedzi projektanta — 2026-11-07

```text
1 — A; 2v2.
2 — A; sześciu bohaterów w głównej puli.
3 — A; trzy główne role: Tank, Wsparcie i Specjalista.
4 — A; Brom, Thora, Liora, Varek, Saila i Zet.
5 — A; krótki, naprzemienny draft bez duplikatów.
6 — A; dwie identyczne kopie mapy jednocześnie.
7 — A; trasa około 360 m.
8 — A; wieże Ognia, Lodu i Błyskawic.
9 — B; limit pięciu wież na gracza.
10 — A; bez T2.
11 — C; PoC zaczyna od bazowych wież T1, bez T3 i bez pilotażu — wieże strzelają automatycznie.
12 — B; bez ręcznego pilotażu.
13 — A; zwykły atak wręcz.
14 — B; dash z odnowieniem 10 s.
15 — B; dodatkowa, siódma postać testowa: Przywoływacz z petem. To test peta, nie decyzja o docelowej liczbie ról.
16 — B; szybki, mało wytrzymały mob.
17 — A; jeden węzeł przywołuje jednostkę automatycznie raz na atak, przy Runnerze najdalej od mety.
18 — B; rdzenie T1 wszystkich siedmiu żywiołów.
19 — A; bez reakcji żywiołów w PoC.
20 — A; budowa trwa 90 s przed pierwszym atakiem i 45 s po kolejnych parach.
```

**Doprecyzowanie po wyborze 3A i 15B:** PoC używa sześciu głównych postaci z trzech ról oraz dodatkowego, siódmego Przywoływacza z petem. To test peta; nie oznacza jeszcze, że docelowa lista ról będzie mieć cztery role.
