# Riftoryn — kolejne decyzje do GDD

**Cel:** zamknąć kolejne ważne luki projektowe poza mapą, rozmieszczaniem wież i Fog of War. Wybory dotyczą reguł i zakresu PoC; liczby balansu mogą być wartościami startowymi.

## Jak odpowiedzieć

Przy każdym numerze wpisz `A`, `B` albo `C`. Opcja C oznacza własny wariant — dopisz go krótko. Możesz dodać wyjątek lub skorygować dowolną propozycję. Skopiuj blok **Odpowiedź do wklejenia** na końcu.

## A. Przebieg meczu i rozłączenia

### 1. Która drużyna rozpoczyna atakiem?

- [ ] **A — Losowanie na początku meczu.** Zwycięzca losowania atakuje pierwszy; kolejność ról zmienia się w kolejnych parach.
- [ ] **B — Naprzemienny pierwszy atak.** Drużyna A atakuje jako pierwsza w pierwszej parze, a potem drużyny zmieniają kolejność.
- [ ] **C — Własna reguła:**

### 2. Jak długo trwa faza budowy?

- [ ] **A — Stały czas dla wszystkich faz.** Każda faza budowy ma taki sam limit; przykładowo 45 s.
- [ ] **B — Dłuższa pierwsza faza, potem krótsze.** Przykładowo 90 s przed pierwszym atakiem i 45 s po kolejnych parach.
- [ ] **C — Własna reguła:**

### 3. Kiedy rozpoczyna się atak po fazie budowy?

- [ ] **A — Automatycznie po odliczaniu.** Atak rusza po upływie czasu niezależnie od gotowości graczy.
- [ ] **B — Po gotowości obu drużyn albo po limicie czasu.** Drużyny mogą rozpocząć wcześniej, ale brak gotowości nie blokuje meczu bez końca.
- [ ] **C — Własna reguła:**

### 4. Co dzieje się, jeśli gracz nie wróci w 30 s pauzy reconnect?

- [ ] **A — Mecz wznawia się bez niego.** Jego bohater pozostaje bierny; jego wieże działają automatycznie.
- [ ] **B — Mecz kończy się techniczną porażką jego drużyny.** Zasada dotyczy fazy ataku i przygotowania.
- [ ] **C — Własna reguła:**

### 5. Co dzieje się przy ponownym rozłączeniu gracza, gdy jego jedyna pauza została już wykorzystana?

- [ ] **A — Gra toczy się dalej bez kolejnej pauzy.** Nieobecny gracz pozostaje bierny, a jego wieże działają automatycznie.
- [ ] **B — Drużyna przegrywa technicznie.** Ponowne rozłączenie po wykorzystaniu pauzy kończy mecz.
- [ ] **C — Własna reguła:**

## B. Runnerzy, wieże i walka

### 6. Jak Runner unika ataków?

- [ ] **A — Unik z ładunkami lub energią.** Można wykonać kilka uników pod rząd, a zasób odnawia się z czasem.
- [ ] **B — Unik na stałym odnowieniu.** Jeden unik naraz; po użyciu trzeba poczekać na cooldown.
- [ ] **C — Własna reguła:**

### 7. Jak Runnerzy zadają obrażenia wrogim jednostkom?

- [ ] **A — Tylko umiejętności bohatera.** Brak zwykłego ataku; podstawowa walka opiera się na zdolnościach z cooldownem.
- [ ] **B — Zwykły atak i umiejętności.** Bohater ma słaby atak dostępny stale oraz mocniejsze zdolności specjalne.
- [ ] **C — Własna reguła:**

### 8. Jak automatyczna wieża wybiera cel, gdy widzi różne typy przeciwników?

- [ ] **A — Najbliższy cel dowolnego ważnego typu.** Runner, mob lub pet może być celem; zdolności i taunt mogą zmienić wybór.
- [ ] **B — Stały priorytet kategorii.** Na przykład Runner przed mobem, mob przed petem; w obrębie kategorii wygrywa najbliższy.
- [ ] **C — Własna reguła:**

### 9. Jak długo działa ogłuszenie lub wyłączenie wieży?

- [ ] **A — Efekty tego samego typu odświeżają czas.** Kolejne trafienie nie sumuje sekund ani intensywności.
- [ ] **B — Czas efektów się sumuje do ustalonego limitu.** Kolejne trafienie wydłuża wyłączenie, ale istnieje maksymalny czas.
- [ ] **C — Własna reguła:**

### 10. Które jednostki mają przypisany żywioł dla obliczania przewag i odporności?

- [ ] **A — Bohaterowie, wieże i moby.** Każdy typ ma swój żywioł; pety dziedziczą żywioł bohatera.
- [ ] **B — Tylko bohaterowie i wieże.** Moby oraz pety są neutralne dla macierzy żywiołów.
- [ ] **C — Własna reguła:**

### 11. Jak działa atak na cel tego samego żywiołu?

- [ ] **A — Obrażenia neutralne.** Relacja tego samego żywiołu daje mnożnik 100%.
- [ ] **B — Lekka odporność.** Ten sam żywioł zadaje zmniejszone obrażenia; przykładowo 90%.
- [ ] **C — Własna reguła:**

### 12. Jak łączyć przewagę żywiołu, bonus technologii i odporność celu?

- [ ] **A — Mnożnik po mnożniku.** Każdy bonus jest nakładany kolejno na wynik poprzedniego.
- [ ] **B — Bonusy sumują się w jedną zmianę procentową.** Ogranicza to skrajne wzrosty i spadki obrażeń.
- [ ] **C — Własna reguła:**

### 13. Jak kumulują się powtarzane statusy od różnych źródeł?

- [ ] **A — Zostaje silniejszy efekt.** Słabszy nie dodaje czasu ani mocy; może odświeżyć silniejszy tylko według jego reguły.
- [ ] **B — Efekty działają osobno.** Każde źródło ma własny czas i może równolegle wpływać na cel.
- [ ] **C — Własna reguła:**

## C. Ekonomia, technologie i moby

### 14. Jak drużyna otrzymuje środki po parze ataków?

- [ ] **A — Stały dochód dla każdego gracza.** Obie drużyny dostają tyle samo, niezależnie od wyniku pary.
- [ ] **B — Dochód zależny od wyniku.** Zwycięstwo daje drużynie premię; podstawowy dochód pozostaje równy.
- [ ] **C — Własna reguła:**

### 15. Co robi autoinwestowanie, gdy nie ma dostępnego legalnego węzła?

- [ ] **A — Niewydane środki przepadają po fazie.** Nie zmienia się wcześniejsza zasada, że wolna gotówka nie przechodzi dalej.
- [ ] **B — Środki zostają w puli do następnej fazy.** Przechodzą tylko wtedy, gdy autoinwestowanie nie znalazło legalnego węzła.
- [ ] **C — Własna reguła:**

### 16. Ile reakcji między żywiołami obejmuje pierwsze grywalne PoC?

- [ ] **A — Wszystkie 21 reakcji.** Każda para żywiołów ma zaprojektowany efekt i koszt przed PoC.
- [ ] **B — Ograniczony zestaw reakcji.** PoC zawiera kilka wybranych reakcji; pozostałe są oznaczone jako przyszła zawartość.
- [ ] **C — Własna reguła:**

### 17. Kto i kiedy przywołuje ofensywne moby?

- [ ] **A — Gracze kupują i przywołują je podczas ataku.** Dostęp zależy od technologii i dostępnych środków lub ładunków.
- [ ] **B — Technologie przywołują automatyczne fale.** Gracze inwestują przed atakiem, a jednostki pojawiają się według ustalonego rytmu.
- [ ] **C — Własna reguła:**

### 18. Czy dotarcie moba do mety wpływa na wynik pary?

- [ ] **A — Nie.** Wynik liczy wyłącznie Runnerów; mob jest wsparciem i nie może zdobyć punktu.
- [ ] **B — Tak, ale tylko jako tie-breaker.** Po podstawowych kryteriach liczy się postęp lub liczba mobów, które dotarły.
- [ ] **C — Własna reguła:**

## D. Zakres prototypu

### 19. Jaką liczebność drużyn obsługuje pierwszy PoC?

- [ ] **A — Pełne 5v5 od początku.** Wcześnie sprawdzamy docelowe obciążenie i współpracę całego zespołu.
- [ ] **B — Mniejszy skład, potem skalowanie do 5v5.** Na przykład 2v2 lub 3v3, żeby szybciej sprawdzić pętlę meczu.
- [ ] **C — Własna reguła:**

### 20. Jakie minimum zawartości powinien mieć pierwszy grywalny PoC?

- [ ] **A — Pionowy wycinek.** Jedna mapa, kilka bohaterów, kilka wież, jeden mob, podstawowe drzewko i jedna kompletna para ataków.
- [ ] **B — Pełna pętla meczu.** Jedna mapa, wszystkie planowane bohaterowie MVP, bazowe wieże i ekonomia wystarczająca do rozegrania całego meczu.
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
19 —
20 —
Uwagi / wyjątki —
```

## Odpowiedzi projektanta — 2026-11-06

```text
1 — A; losowo wybrać drużynę rozpoczynającą atak, a w razie remisowego wyniku losowo ustalić pierwszeństwo następnej pary.
2 — B; pierwsza faza budowy dłuższa, kolejne krótsze; przykładowe czasy z pytania nie są jeszcze zatwierdzone.
3 — A; atak rozpoczyna się automatycznie po odliczaniu.
4 — A; w MVP nieobecny gracz pozostaje bierny, a jego wieże działają automatycznie. Docelowo ma go zastępować bot.
5 — A; bez kolejnej pauzy gra toczy się dalej, a wieże nieobecnego gracza działają automatycznie. Docelowo ma go zastępować bot.
6 — B; unik to dash na stałym czasie odnowienia.
7 — B; zwykłe ataki i umiejętności mogą trafiać wrogie jednostki; wieże można wyłączyć lub zniszczyć wyłącznie umiejętnościami.
8 — A; automatyczna wieża atakuje najbliższy ważny cel spośród dostępnych typów.
9 — A; powtórzenie efektu odświeża czas, nie sumuje jego intensywności.
10 — A; bohaterowie, wieże i moby mają żywioł, pet dziedziczy żywioł bohatera.
11 — C; roboczo atak tego samego żywiołu zadaje 50% obrażeń ze względu na odporność celu; wartość i dokładny zakres wymagają namysłu.
12 — B; modyfikatory żywiołu, technologii i odporności sumują się addytywnie.
13 — A; dla powtarzanego statusu działa silniejszy efekt, bez kumulowania mocy.
14 — B; obie drużyny otrzymują równą podstawę, zwycięzca dodatkową premię.
15 — C; automatyczne wydawanie: najpierw węzły z częściowymi wpłatami, potem ulepszenia z najbardziej rozwiniętych drzewek, a gdy nie ma już takich ulepszeń — losowa kolejna dostępna umiejętność.
16 — B; PoC zawiera ograniczony zestaw reakcji, wybór konkretnych reakcji pozostaje otwarty.
17 — C; węzły drzewka mogą automatycznie przywoływać sojusznicze jednostki, np. przy Runnerze najdalej od mety. Szczegóły będą projektowane wraz z drzewkiem.
18 — A; dotarcie moba do mety nie wpływa na wynik pary.
19 — B; PoC zaczyna od mniejszej drużyny niż 5v5; dokładna liczebność pozostaje do wyboru.
20 — A; pionowy wycinek: jedna mapa, kilka bohaterów i wież, jeden mob, podstawowe drzewko i pełna para ataków.
```
