# Riftoryn — decyzje do GDD: pet, moby i kryteria PoC

**Cel:** doprecyzować otwarte elementy wybranego PoC: siódmego bohatera z petem, automatycznie przywoływaną jednostkę, ekonomię po parze oraz to, po czym poznamy, że prototyp spełnił swoje zadanie. Opcje liczbowe są propozycjami do testów.

## Jak odpowiedzieć

Wpisz `A`, `B` lub `C` przy każdym numerze. Przy C dopisz własną regułę. Możesz poprawić dowolną propozycję. Skopiuj blok **Odpowiedź do wklejenia** na końcu.

## A. Przywoływacz i pet

### 1. Jak pet chroni Przywoływacza?

- [ ] **A — Przejmuje część obrażeń.** Ma własne HP; część obrażeń zadanych Przywoływaczowi jest przekierowana na peta.
- [ ] **B — Fizycznie przechwytuje pociski.** Pet porusza się przy Przywoływaczu i może zasłonić go przed pociskiem; obrażenia nie są przekierowywane automatycznie.
- [ ] **C — Własna reguła:**

### 2. Kogo pet chroni w PoC?

- [ ] **A — Tylko swojego Przywoływacza.** To prostszy, czytelny zakres dla pierwszego testu.
- [ ] **B — Przywoływacza i wskazanego sojusznika.** Gracz może wydać komendę, by pet osłonił inną osobę.
- [ ] **C — Własna reguła:**

### 3. Jak gracz wydaje petowi polecenia?

- [ ] **A — Jedna komenda aktywna.** Przycisk kieruje peta do wskazanego Runnera lub miejsca; poza tym pet podąża automatycznie.
- [ ] **B — Bez komend w PoC.** Pet automatycznie trzyma się Przywoływacza i wykonuje jedną prostą funkcję ochronną.
- [ ] **C — Własna reguła:**

### 4. Jak pet jest eliminowany?

- [ ] **A — Ma własne HP.** Po utracie HP znika do końca ataku; wraca na początku kolejnego ataku.
- [ ] **B — Nie ma osobnego paska HP.** Może przechwycić ograniczoną liczbę pocisków lub działa przez określony czas, po czym wraca.
- [ ] **C — Własna reguła:**

### 5. Czy pet działa podczas obrony w PoC?

- [ ] **A — Nie.** Sprawdzamy go tylko w ataku Runnerów.
- [ ] **B — Tak.** Ma jedną prostą funkcję wspierającą pilotowaną wieżę; szczegółowy efekt wpisz w C.
- [ ] **C — Własna reguła:**

## B. Automatycznie przywoływany mob

### 6. Czy mob z węzła technologii jest tym samym szybkim, kruchym mobem, którego testujemy w PoC?

- [ ] **A — Tak.** Jeden typ jednostki, przywoływany automatycznie raz na atak.
- [ ] **B — Nie.** Węzeł przywołuje odrębnego moba wsparcia, a szybki mob jest osobną jednostką.
- [ ] **C — Własna reguła:**

### 7. Kiedy pojawia się mob z węzła?

- [ ] **A — Na początku ataku.** Pojawia się przy Runnerze, który w tej chwili jest najdalej od mety.
- [ ] **B — Gdy Runner zostaje z tyłu.** Pojawia się raz, gdy różnica postępu między pierwszym a ostatnim Runnerem przekroczy ustalony próg.
- [ ] **C — Własna reguła:**

### 8. Jaka jest podstawowa funkcja szybkiego, kruchego moba?

- [ ] **A — Odciąga ogień wież.** Nie zadaje obrażeń; jego wartością jest to, że wieża może wybrać go zamiast Runnera.
- [ ] **B — Odciąga ogień i zadaje małe obrażenia.** Atakuje najbliższą wrogą jednostkę, ale nie wpływa na wynik pary.
- [ ] **C — Własna reguła:**

### 9. Jak określić jego statystyki początkowe?

- [ ] **A — Bardzo kruchy i szybki.** Około 40 HP i prędkość 6 m/s; ma zwykle przeżyć jedno trafienie.
- [ ] **B — Kruchy, ale nie jednorazowy.** Około 70 HP i prędkość 5,5 m/s; ma przeżyć kilka sekund ostrzału.
- [ ] **C — Własne wartości:**

### 10. Ile takich mobów może być aktywnych naraz po stronie drużyny?

- [ ] **A — Maksymalnie jeden.** Jeśli poprzedni nadal żyje, kolejne automatyczne przywołanie czeka lub przepada.
- [ ] **B — Maksymalnie trzy.** Jednostki mogą się kumulować, ale jest limit zapobiegający zalaniu trasy.
- [ ] **C — Własny limit:**

## C. Ekonomia po parze ataków

### 11. Jaki podstawowy dochód otrzymuje każdy gracz po parze ataków?

- [ ] **A — 100 g na gracza.** Niewielki wzrost, który zachęca do planowania inwestycji.
- [ ] **B — 150 g na gracza.** Szybszy rozwój drzewek w krótszym meczu PoC.
- [ ] **C — Inna kwota:**

### 12. Jaka premia trafia do każdego gracza zwycięskiej drużyny?

- [ ] **A — +50 g.** Zwycięstwo daje odczuwalną, ale umiarkowaną przewagę ekonomiczną.
- [ ] **B — +100 g.** Zwycięstwo wyraźnie przyspiesza rozwój drużyny.
- [ ] **C — Inna kwota / premia drużynowa:**

### 13. Co dzieje się z premią, gdy para kończy się remisem?

- [ ] **A — Nikt nie dostaje premii zwycięzcy.** Obie drużyny otrzymują tylko podstawowy dochód.
- [ ] **B — Obie drużyny otrzymują premię.** Remis przyznaje punkt obu stronom i premię obu drużynom.
- [ ] **C — Własna reguła:**

## D. Kryteria i telemetria PoC

### 14. Co jest minimalnym sukcesem pierwszego PoC?

- [ ] **A — Da się ukończyć pełną parę ataków bez blokady meczu.** Obie strony mogą budować, atakować i rozliczyć wynik.
- [ ] **B — Da się rozegrać kilka par pod rząd.** Oprócz pełnej pary działa też ekonomia między parami i kolejna faza budowy.
- [ ] **C — Własne kryterium:**

### 15. Jakie dane zbieramy w każdym ataku?

- [ ] **A — Podstawowe.** Wynik pary, liczba Runnerów na mecie, czas dotarcia ostatniego Runnera i najdalszy postęp.
- [ ] **B — Podstawowe plus walka.** Dodatkowo obrażenia według źródła, strzały i trafienia wież, użycia zdolności oraz czas życia mobów i peta.
- [ ] **C — Własny zestaw:**

### 16. Jak sprawdzamy, czy obrona i trasa działają poprawnie?

- [ ] **A — Ręczny przegląd jednej pary.** Obserwujemy, czy ataki i wynik są zrozumiałe; zapisujemy problemy do poprawy.
- [ ] **B — Z góry ustalone warunki zaliczenia.** Na przykład: żadna wieża nie blokuje drogi, każda drużyna może zakończyć atak, a wynik jest identyczny przy powtórzeniu tej samej sytuacji.
- [ ] **C — Własne kryteria:**

### 17. Ile pełnych par rozgrywamy w pierwszej sesji testowej?

- [ ] **A — Jedną parę.** Wystarczy sprawdzić główną pętlę bez inwestycji między wieloma rundami.
- [ ] **B — Trzy pary.** Pozwala zobaczyć rozwój technologii i wpływ premii za wygraną.
- [ ] **C — Inną liczbę:**

### 18. Kto zatwierdza, że PoC spełnił kryteria?

- [ ] **A — Projektant po wspólnej sesji gry.** Decyzja opiera się na obserwacji i zebranej telemetrii.
- [ ] **B — Z góry opisany zestaw testów.** Każdy scenariusz musi przejść checklistę, a projektant zatwierdza wyjątki.
- [ ] **C — Własna reguła:**

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
Uwagi / wartości do testu —
```

## Odpowiedzi projektanta — 2026-11-07

```text
1 — A; pet ma własne HP i przekierowuje część obrażeń z Przywoływacza na siebie.
2 — A; chroni tylko Przywoływacza.
3 — B; nie ma komend, automatycznie podąża za Przywoływaczem i chroni go.
4 — A; po wyeliminowaniu wraca na początku następnego ataku.
5 — A; pet działa tylko w ataku.
6 — A; węzeł przywołuje ten sam szybki, kruchy mob.
7 — C; spawn co 30 s podczas ataku, przy Runnerze najdalej od mety.
8 — A; mob nie zadaje obrażeń, tylko odciąga ogień wież.
9 — A; startowo 40 HP i prędkość 6 m/s.
10 — C; brak limitu aktywnych mobów; częstość spawnu kontroluje drzewko przez zmianę odstępu między przywołaniami.
11 — B; podstawowy dochód 150 g na gracza po każdej parze.
12 — A; premia zwycięzcy +50 g na gracza.
13 — B; przy remisie obie drużyny otrzymują premię zwycięzcy.
14 — B; PoC ma umożliwiać rozegranie kilku par.
15 — B; zbieramy także dane o walce, wieżach, umiejętnościach oraz życiu mobów i peta.
16 — A; sprawdzenie działania drogi i walki przez ręczny przegląd jednej pary.
17 — B; pierwsza sesja testowa obejmuje trzy pełne pary.
18 — B; testy mają checklistę, a projektant zatwierdza wyjątki.
```
