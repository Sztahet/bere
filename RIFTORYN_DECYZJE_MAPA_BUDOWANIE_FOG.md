# Riftoryn — decyzje do GDD: mapa, budowanie i Fog of War

**Cel:** wybierz reguły potrzebne do dopisania spójnego opisu mapy, budowania i widoczności w GDD. To decyzje projektowe dla PoC; liczby balansu mogą pozostać startowe.

## Jak odpowiedzieć

Przy każdym punkcie wpisz `A`, `B` albo `C`. Jeśli wybierasz C, dopisz własną regułę. Możesz też zmienić dowolną propozycję. Skopiuj na końcu sekcję **Odpowiedź do wklejenia** i uzupełnij ją.

## Ustalenia już zapisane w GDD

- Pierwsza mapa to stały, jednopoziomowy labirynt. Runnerzy biegną od środka na zewnątrz; w MVP nie ma skrótów ani ślepych zaułków.
- Wieże w MVP stawia się na zaprojektowanych polach na murach. Runnerzy nie mogą wejść na mury. Skupianie wielu wież blisko siebie jest dozwolone.
- Obie drużyny budują w tym samym czasie. Jeśli dwie próby zajęcia tego samego pola ścierają się, rozstrzyga serwer.
- Droga musi pozostać drożna i mieć co najmniej szerokość dwóch jednostek postaci. Efekty obszarowe mogą czasowo wejść na trasę, ale nie blokują jej fizycznie.
- Podczas budowania drużyna nie widzi wrogich wież. Runnerzy odkrywają wrogą obronę podczas ataku i zachowują ostatnio poznany stan na kolejne próby. Defenderzy nie widzą Runnerów poza sytuacjami wykrycia.
- Mgła przemieszcza się po drodze labiryntu, a jej czoło dociera do mety po 120 s. Dotarcie mgły do mety samo w sobie nie kończy ataku.

## 1. Pola i zajmowanie miejsca

### 1.1 Ile wież może zajmować jedno pole?

- [ ] **A — Jedna wieża na pole.** Każde pole ma jeden slot; wieże mogą stać na sąsiednich polach i koncentrować się w jednym miejscu.
- [ ] **B — Pole może mieć kilka slotów.** Każde pole ma ustalony limit miejsc; interfejs pokazuje, ile jest wolnych.
- [ ] **C — Własna reguła:**

### 1.2 Co się dzieje, gdy gracze z tej samej drużyny wybiorą to samo wolne pole?

- [ ] **A — Wygrywa pierwsze żądanie odebrane przez serwer.** Drugiemu graczowi pole zostaje odrzucone, a jego środki nie są pobierane.
- [ ] **B — Pole rezerwuje się na krótko po pierwszym kliknięciu.** Pozostali gracze widzą rezerwację; jeśli pierwszy gracz nie zatwierdzi budowy, pole wraca do puli.
- [ ] **C — Własna reguła:**

### 1.3 Co się dzieje przy równoczesnej próbie budowy przez przeciwników na odpowiadających sobie polach?

> Mapy drużyn są identycznymi kopiami; zdecyduj, czy konflikt dotyczy wyłącznie pola na własnej mapie, czy serwer ma też synchronizować wybór odpowiadającego pola przez obie drużyny.

- [ ] **A — Brak konfliktu między drużynami.** Każda drużyna buduje na swojej kopii mapy, więc obie mogą zajmować odpowiadające sobie pola.
- [ ] **B — Wspólna rezerwacja lustrzanych pól.** Jeśli drużyna A zajmie pole, odpowiadające pole drużyny B jest niedostępne w tej fazie.
- [ ] **C — Własna reguła:**

## 2. Zmiana i usuwanie świeżo postawionych wież

### 2.1 Co może zrobić właściciel z wieżą postawioną w bieżącej fazie budowy?

- [ ] **A — Dowolnie ją przestawić lub usunąć do końca fazy.** Zmiana miejsca jest darmowa; usunięcie zwraca pełny koszt, a zajęte przez wieżę limity wracają do właściciela.
- [ ] **B — Może ją przestawić, ale nie usunąć za zwrotem.** Przestawienie jest darmowe; usunięcie nie zwraca waluty, choć zwalnia pole i odpowiedni limit.
- [ ] **C — Własna reguła:**

### 2.2 Czy gracz może zmienić typ świeżo postawionej wieży?

- [ ] **A — Tak, w bieżącej fazie.** Zmiana zachowuje pole i przelicza koszt; nadpłata jest pobierana, a różnica w dół wraca do gracza.
- [ ] **B — Nie.** Typ wybiera się przy stawianiu; aby go zmienić, trzeba usunąć wieżę zgodnie z regułą 2.1 i postawić nową.
- [ ] **C — Własna reguła:**

### 2.3 Kto może zmieniać wieżę?

- [ ] **A — Tylko właściciel wieży.** Sojusznicy nie mogą jej przestawiać ani usuwać.
- [ ] **B — Każdy członek drużyny.** Zmiany są wspólną odpowiedzialnością zespołu, a limit i koszt nadal obciążają właściciela wieży.
- [ ] **C — Własna reguła:**

## 3. Widoczność obrony i pamięć odkrytych wież

### 3.1 Kiedy Runner odkrywa wrogą wieżę?

- [ ] **A — Gdy wieża znajdzie się w promieniu widzenia Runnera.** Odkrycie jest natychmiastowe; ściany mogą blokować widok, jeśli używamy linii widzenia.
- [ ] **B — Gdy Runner lub jego drużyna znajdzie się w ustalonej odległości od wieży.** Ściany nie blokują wykrycia; reguła jest prostsza i przewidywalna.
- [ ] **C — Własna reguła:**

### 3.2 Co dokładnie pozostaje widoczne po odkryciu wieży, gdy Runner odejdzie?

- [ ] **A — Ostatni znany stan mapy pozostaje bezterminowo.** Wieża może być oznaczona jako niepotwierdzona, jeśli od czasu odkrycia mogło się coś zmienić.
- [ ] **B — Ostatnia pozycja pozostaje, ale szczegóły znikają.** Gracze pamiętają położenie, lecz nie typ, stan ani aktualizacje wieży poza zasięgiem widzenia.
- [ ] **C — Własna reguła:**

### 3.3 Kiedy aktualizuje się zapamiętana informacja?

- [ ] **A — Tylko gdy wieża ponownie zostanie odkryta.** Nowe budowy, ulepszenia, zniszczenie i przestawienie nie są widoczne zdalnie.
- [ ] **B — Drużyna dostaje aktualizacje przez cały atak.** Po pierwszym odkryciu zna dalsze zmiany stanu tej wieży, nawet gdy nie ma jej w polu widzenia.
- [ ] **C — Własna reguła:**

### 3.4 Co widzą Defenderzy o Runnerach?

- [ ] **A — Tylko Runnerów aktualnie wykrytych.** Po utracie wykrycia znika ich pozycja; nie zostaje ślad na minimapie.
- [ ] **B — Pozycję wykrytą przez chwilę pamięta minimapa.** Po utracie wykrycia zostaje ostatnia pozycja z oznaczeniem, że może być nieaktualna.
- [ ] **C — Własna reguła:**

### 3.5 Czy Runnerzy oglądający rozgrywkę po eliminacji zachowują pełną widoczność?

- [ ] **A — Nie.** Obserwacja nie ujawnia informacji niedostępnych żywym sojusznikom; widok pozostaje ograniczony do zasad widoczności drużyny.
- [ ] **B — Tak.** Po eliminacji można obserwować sojuszników i widzieć to, co oni widzą, ale nie otrzymuje się dodatkowej wizji.
- [ ] **C — Własna reguła:**

## 4. Mgła na zakrętach i przy mecie

### 4.1 Jak porusza się czoło mgły po labiryncie?

- [ ] **A — Wzdłuż środka trasy, według przebytej odległości.** Czoło pokonuje równe odcinki drogi w równym czasie, także na zakrętach; 120 s oznacza dotarcie czoła do mety.
- [ ] **B — Jako wizualna ściana za Runnerami.** Mgła przesuwa się według pozycji Runnerów i utrzymuje stały dystans do najwolniejszych, z limitem dojścia do mety w 120 s.
- [ ] **C — Własna reguła:**

### 4.2 Co robi mgła po dotarciu do mety po 120 s?

- [ ] **A — Zatrzymuje się na mecie.** Pozostaje tam, a Runnerzy nadal podlegają jej zasadom, jeśli w niej stoją; atak trwa do dotarcia lub eliminacji wszystkich Runnerów.
- [ ] **B — Zaczyna przesuwać się wzdłuż trasy w stronę startu.** Stopniowo obejmuje całą drogę; atak nadal trwa do dotarcia lub eliminacji wszystkich Runnerów.
- [ ] **C — Własna reguła:**

### 4.3 Czy geometria mgły może blokować Runnerów lub pociski?

- [ ] **A — Nie.** Mgła jest efektem wizualnym i zestawem debuffów; nie zmienia kolizji ani fizycznych osłon.
- [ ] **B — Może ograniczać widoczność, ale nie ruch ani pociski.** Efekt widzenia jest wyłącznie prezentacyjny i nie zmienia serwerowego celowania.
- [ ] **C — Własna reguła:**

## Odpowiedź do wklejenia

Skopiuj ten blok, wpisz literę i dopisz szczegóły tam, gdzie wybierasz C:

```text
1.1 —
1.2 —
1.3 —
2.1 —
2.2 —
2.3 —
3.1 —
3.2 —
3.3 —
3.4 —
3.5 —
4.1 —
4.2 —
4.3 —
Uwagi / wyjątki —
```

## Odpowiedzi projektanta — 2026-11-06

```text
1.1 — A
1.2 — B
1.3 — A
2.1 — A
2.2 — B
2.3 — A
3.1 — B
3.2 — A; odkryta wieża nie znika, ale jej zapamiętany stan nie aktualizuje się zdalnie. Runnerzy mogą zobaczyć ją w stanie z poprzedniej obserwacji i dopiero po ponownym odkryciu zauważyć ulepszenie.
3.3 — A
3.4 — A
3.5 — B; wyeliminowani gracze mogą swobodnie obserwować w widoku strategicznym, z tym samym Fog of War co żywi gracze.
4.1 — A
4.2 — C; po dotarciu mgły do mety cała trasa zostaje skażona. Mgła nie przesuwa się dalej, tylko zwiększa zasięg skażenia. Tempo i dokładny sposób rozszerzania zasięgu pozostają do doprecyzowania.
4.3 — A
```
