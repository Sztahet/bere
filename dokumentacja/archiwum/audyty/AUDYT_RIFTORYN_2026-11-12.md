# Audyt Riftoryn — świat, bohaterowie i stan gry

**Data:** 12 listopada 2026. **Metoda:** game-world-audit; analiza dokumentów, zasobów i wybranych fragmentów kodu. Oceny są subiektywną oceną projektu na podstawie dostępnych materiałów, a nie pomiarem opinii graczy. Nie przeprowadzono w tej sesji testu gry, buildu ani badań odbiorców. Kanon fabuły określa `world_history.md`, a decyzje projektowe — `RIFTORYN_GDD.md`. Propozycje poniżej nie zmieniają kanonu ani zasad.

## Ocena

Riftoryn ma wyrazistą wizję: drużyny na zmianę pokonują i budują obronę, a historyczny spór o kontrolę zasobów nadaje zawodom znaczenie. Istnieje kod próby 3D w Unity, ale analiza plików nie dowodzi, że działa poprawnie ani że jest przyjemny. Najpilniejsze jest sprawdzenie czytelności i powtarzalności podstawowej próby z ludźmi; pełna gra 5v5 i jej wyróżniki pozostają wizją do walidacji.

- **Spójność świata: 79/100.** Historia, traktat i szlaki są powiązane, lecz ich zrozumienie wymaga lektury wielu dokumentów; potrzeba krótkiego wejścia dla gracza.
- **Sens gry i kierunek narracji: 75/100.** Cel próby i punktacja meczu są opisane, a dwa warianty zarządzania mają wyraźną stawkę. Nie sprawdzono, czy gracz rozumie ją podczas gry.
- **Oryginalność: 73/100.** Zmienianie ról ataku i obrony oraz trwała rozbudowa dają obiecującą kombinację; żywioły i turniej same w sobie są znanymi motywami. Bez testu porównawczego nie jest to ocena rynku.
- **Potencjał zaangażowania: 68/100.** Relacje bohaterów, presja mgły i adaptacja są obiecujące, lecz chęć powrotu i sposób odkrywania historii w grze nie zostały wykazane.
- **Logika i reguły: 76/100.** Procedury traktatu i przebieg meczu mają konkretne zasady. Pozostają otwarte interakcje mechanik, a dokumenty techniczne zawierają nieaktualne opisy.

**Średnia opisowej oceny koncepcji: 74/100** (zaokrąglona średnia pięciu ocen). Nie jest to procent ukończenia, ocena jakości działającego buildu ani prognoza sukcesu.

## Co potwierdzają materiały

- `RIFTORYN_GDD.md`, rozdz. 0.1 i 1–4, definiuje minimalną próbę, docelowy mecz oraz zasady zwycięstwa. Pełna rozgrywka i minimalny pre-PoC mają różne zakresy.
- `world_history.md`, rozdz. o Traktacie i Wielkich Igrzyskach, rozróżnia zwykłe Próby, sport i wyjątkowe Igrzyska oraz opisuje dwa warianty, chronione usługi i odwołania.
- `RiftorynTrial.cs` zawiera kod mapy, ustawiania wież, ruchu, mgły, umiejętności, wyniku, sojusznika AI i trybu LAN. Obecność kodu nie potwierdza poprawności ani pełnej dostępności tych funkcji.
- Ostatni znany wpis projektu informuje o buildzie Windows; instrukcje Unity opisują udany build z 8 listopada. To historia, nie wynik aktualnej sesji.
- W repozytorium znajdują się portrety 31 postaci i 24 arkusze modelowania; same referencje nie są modelami 3D. Historie bohaterów są w różnym stopniu zatwierdzone.

## Problemy i propozycje

### A1 — Brak aktualnego dowodu, że podstawowa próba działa i zachęca do powtórki

**Waga:** wysoka. **Źródło:** GDD 0.1, plan klienta 3D i kod `Assets/Scripts/RiftorynTrial.cs`. Kod obejmuje potrzebne elementy, ale nie zastępuje przejścia od uruchomienia do wyniku, restartu i niezależnego testu. Nie stwierdzono w tym audycie, że takich testów nigdy nie było.

**Przyczyna:** cele pre-PoC i kod istnieją, lecz niniejsza analiza nie zawiera aktualnego raportu wykonania ani wyników graczy.

**Propozycja:** przeprowadzić kontrolowane przejście solo: start, obrażenia, umiejętność, meta lub przegrana, wynik i restart; sprawdzić również drogę bez przeszkód i pod ostrzałem. Potem poprosić 3–5 osób o dwie próby bez objaśniania sterowania: zanotować, czy rozumieją cel, skąd otrzymały obrażenia, czy dostrzegły mgłę i czy chcą ponowić grę. To badanie rozpoznawcze, nie reprezentatywny pomiar rynku.

**Realizacja:** projektant gry i osoba testująca; scenariusz i zapis wyników; orientacyjnie 1–3 dni robocze plus dostępność testerów. **Efekt:** decyzje wynikające z obserwacji zamiast samej wizji. Nie wymaga zmiany kanonu ani rozszerzenia pre-PoC.

### A2 — Najważniejsza obietnica gry nie jest jeszcze zweryfikowana w pełnej pętli

**Waga:** wysoka. **Źródło:** GDD rozdz. 1–2 i 0.1. Wyróżnik opiera się na naprzemiennym ataku, budowie oraz wnioskach z wcześniejszych prób. Samotny bieg testuje tylko fragment tego doświadczenia; kod LAN nie stanowi dowodu działania pełnych par, ekonomii i obrony.

**Przyczyna:** projekt celowo realizuje zakres etapami, a nie wszystkie filary naraz.

**Propozycja:** po ocenie pre-PoC przetestować w prostym prototypie 2v2 trzy pary ataków, zgodnie z GDD: dwie obrony, naprzemienny atak, ograniczoną budowę i wspólne rozliczenie. Zapisać, jakie decyzje gracze zmienili po poprzedniej próbie, ile trwał mecz i co uznali za sprawiedliwe.

**Realizacja:** projektant i programista rozgrywki; opis scenariusza, zapis trzech par i lista decyzji; orientacyjnie 1–3 tygodnie na ograniczony prototyp, zakres i czas wymagają estymacji po teście solo. **Efekt:** sprawdzenie głównego wyróżnika bez deklarowania gotowości 5v5. Obowiązują aktualne reguły GDD; ewentualne uproszczenia oznaczyć jawnie.

### A3 — Gracz może nie poznać stawki świata podczas pierwszej gry

**Waga:** wysoka. **Źródło:** `world_history.md`, „Wielkie Igrzyska i przyszłość krain”; historia Tarena i Lucena. Dwóm wariantom zarządzania i ich konsekwencjom poświęcono szczegółowy tekst, lecz w tej sesji nie zweryfikowano, czy klient przekazuje go w odpowiednim momencie.

**Przyczyna:** silnie rozwinięty kanon nie ma tu potwierdzonego sposobu prezentacji w przebiegu pierwszej próby.

**Propozycja:** przygotować opcjonalny ekran lub krótki dialog z jednym konkretnym konfliktem: opóźnioną dostawą części do stacji i argumentem obu stron. Zakończyć pytaniem, co może zyskać i stracić każda z nich. Dać możliwość pominięcia i powrotu w kodeksie. Sprawdzić po próbie, czy tester potrafi własnymi słowami opowiedzieć o sporze.

**Realizacja:** projektant narracji i UI; tekst 80–120 słów i prosty prototyp; 1–2 dni. **Efekt:** sprawdzenie czytelności stawki bez narzucania zwycięskiej strony i bez zmiany kanonu.

### A4 — Stary audyt i dokumenty techniczne mogą wprowadzić zespół w błąd

**Waga:** wysoka. **Źródło:** `AUDYT_RIFTORYN_2026-11-11.md`, `world_history.md` i GDD. Stary audyt twierdzi, że stawka Igrzysk i wynik meczu są niejasne, choć bieżący kanon i GDD je opisują; proponuje też określony udział Rian w przyczynie Nocy, podczas gdy kanon potwierdza tylko sabotaż jednej bramy, a przyczynę katastrofy pozostawia nierozstrzygniętą. `RIFTORYN_PLAN_KLIENT_3D.md` i część GDD nadal przedstawiają UE5 jako kandydata, mimo istnienia bieżącego projektu Unity.

**Przyczyna:** dokumenty powstały na różnych etapach; status nie zawsze odróżnia historię od bieżącego stanu wdrożenia.

**Propozycja:** oznaczyć dawny audyt i plan techniczny jako materiały historyczne, a w GDD doprecyzować obecny stan Unity i nadal otwarte decyzje bez dopisywania niepodjętej decyzji o docelowym silniku. Zachować pierwszeństwo kanonu, historii i potwierdzonych decyzji użytkownika.

**Realizacja:** właściciel dokumentacji; krótka notka statusowa i przegląd linków; 0,5–1 dnia. **Efekt:** mniejsze ryzyko potraktowania starej propozycji jako faktu. Propozycja nie rozstrzyga przyczyny Nocy.

### A5 — Odchylenia próby od docelowych reguł wymagają jawnego opisu

**Waga:** średnia. **Źródło:** GDD 0.1 i rozdz. 4; `RiftorynTrial.cs:202–203`. Kod testowej próby zawiera zwykły atak, który zadaje obrażenia wieży, podczas gdy docelowa zasada GDD rezerwuje niszczenie i wyłączanie wież dla umiejętności specjalnych. GDD wymienia podobne odstępstwo eksperymentalne w teście przeglądarkowym; nie należy automatycznie uznać tego za błąd implementacji prototypu.

**Przyczyna:** zakres eksperymentu i przyszłe reguły rozwijają się osobno, a opis odstępstw wersji Unity wymaga sprawdzenia.

**Propozycja:** sporządzić krótką listę różnic: mechanika w próbie, reguła docelowa, przyczyna eksperymentu i decyzja po teście. W pierwszej kolejności porównać atak zwykły, dobór dostępnych bohaterów i sposób rozliczenia próby.

**Realizacja:** projektant i programista; jedna tabela w dokumentacji testu; 0,5–1 dnia. **Efekt:** wyniki testów nie zostaną pomylone z zatwierdzeniem docelowej mechaniki.

### A6 — Osobiste konflikty wielu postaci są słabiej powiązane z decyzjami gracza

**Waga:** średnia. **Źródło:** `RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md`. Historie mają konkretne zawody i wydarzenia, ale znaczna część krótszych biografii kończy się ogólnym opisem postawy w Próbach. Sam tekst biografii nie potwierdza, jak konflikt pojawia się w grze.

**Przyczyna:** szeroka obsada jest rozwijana równolegle, zanim wybrano pełny skład MVP i sposób podawania narracji.

**Propozycja:** dla małej puli testowej opisać po jednym konflikcie, przeciwnym interesie konkretnej osoby, możliwym koszcie i jednym krótkim momencie prezentacji. Zacząć od Tarena i Lucena, a następnie wybrać 2–3 postacie dostępne w próbie. Nie dodawać automatycznie traumy, antagonisty ani nowej mechaniki każdej postaci.

**Realizacja:** projektant narracji; karta jednej strony na postać i 1–2 kwestie; 2–4 dni dla pierwszej grupy. **Efekt:** lepsze rozróżnienie postaci bez narzucania ich relacji drużynowej czy zmiany zatwierdzonych historii.

### A7 — Tajemnice mają materiał źródłowy, lecz sposób ich odkrywania pozostaje do zaprojektowania

**Waga:** średnia. **Źródło:** `world_history.md`, siedem raportów o Nocy oraz rozdział o Morraku i historia Noctena. Raporty zawierają świadectwa i ograniczenia; nie jest problemem samo pozostawienie przyczyny otwartej. Nie potwierdzono natomiast doświadczenia odkrywania tych materiałów w grze.

**Przyczyna:** materiał lore jest pełniejszy niż plan jego dawkowania i informacja zwrotna dla gracza.

**Propozycja:** rozpisać trzy dostępne tropy i jedną sprzeczność, każde z określonym źródłem oraz tym, czego nie dowodzi. Użyć np. odczytu przekaźnika, relacji świadka i raportu o jednej bramie. Przetestować, czy gracz potrafi odróżnić fakt od hipotezy, a nie wymagać ujawnienia rozwiązania.

**Realizacja:** projektant narracji; karta tropów i makieta jednego wpisu kodeksu; 1–2 dni. **Efekt:** nagroda za ciekawość przy zachowaniu niepewności kanonu.

## Bohaterowie — próbka oceny

Oceny dotyczą materiału pisemnego, nie grywalności; nie są pełnym audytem 31 historii ani zatwierdzeniem produkcyjnym.

- **Taren i Lucen — 84/100 jako relacja.** Ojciec i syn mają różne stanowiska, wspólną stratę i konkretną nierozstrzygniętą decyzję Tarena. Następny krok: sprawdzić scenę pierwszej rozmowy bez wymuszenia pojednania.
- **Rian — 81/100.** Konkretny sabotaż jednej bramy, odpowiedzialność i granice wiedzy tworzą napięcie. Następny krok: pokazać konsekwencję jej decyzji bez przypisania jej przyczyny całej Nocy.
- **Morrak — 78/100.** Rozpoznawalny kodeks i napięcie między obrońcą a mścicielem. Następny krok: sytuacja, w której jakość dowodu ogranicza jego działanie; nie rozstrzygać pochodzenia.
- **Saila — 76/100.** Konkretne zapisy, dostawy i ograniczenia bezpieczeństwa łączą ją ze światem. Następny krok: pokazać koszt opóźnienia z perspektywy drugiej osoby.
- **Varek — 70/100.** Dylemat szybkości ostrzeżenia i weryfikacji jest czytelny. Następny krok: jeden powracający rozmówca z odmiennym priorytetem i decyzja pod presją czasu.

Pozostałe postacie wymagają osobnej, równej analizy przed indywidualną punktacją. Żadnej nie ogłaszamy gotową do produkcji wyłącznie na podstawie tej próbki.

## Kolejność działań

1. **Najpierw A1:** przejście aktualnej próby i mały test z ludźmi; zebrać obserwacje, a nie tylko wynik wygrana/przegrana.
2. **Równolegle A4 i A5:** uporządkować status źródeł oraz spisać świadome odstępstwa prototypu.
3. **Po pierwszych obserwacjach A3 i A6:** przygotować krótki sposób przedstawienia stawki i kilku bohaterów, sprawdzić zrozumienie.
4. **Następnie A2:** najtańszy wiarygodny test pełnej pętli 2v2; A7 rozwijać jako osobny materiał narracyjny.

Riftoryn ma materiał na rozpoznawalną grę drużynową i historię z realnym konfliktem interesów. Gotowość do wydania, balans, sieć, odbiór graczy i pozycja rynkowa nie są na podstawie tej analizy potwierdzone. Kolejną decyzję o zakresie warto oprzeć na wynikach prób, nie na liczbie przygotowanych postaci.
