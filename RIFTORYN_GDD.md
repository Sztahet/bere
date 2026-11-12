# RIFTORYN — GDD: aktualne źródło projektu gry

**Wersja:** 2.0 · **Stan:** koncepcja i preprodukcja · **Aktualizacja:** 2026-11-06  
**Nazwa:** robocza; dostępność i prawa niepotwierdzone. **Hasło:** BUILD. BREAK. ADAPT.  
**Gatunek:** BERE — Build, Escape, React, Evolve; zespołowe PvP 5v5, obrona wieżowa i akcja TPP.

## 0. Jak korzystać z dokumentu

Ten plik jest jedynym aktualnym GDD i źródłem decyzji dotyczących projektu gry. Jedynym źródłem kanonu historii świata jest [world_history.md](./world_history.md); w kwestiach fabularnych ma pierwszeństwo przed GDD i innymi materiałami. Materiały w `archive/` są historią projektu. Interaktywne menu `riftoryn-menu.html` jest prototypem do oglądania.

**[USTALONE]** — decyzja użytkownika. **[PROPOZYCJA]** — hipoteza do zatwierdzenia i testów. **[OTWARTE]** — kwestia bez ostatecznej decyzji. Wartości mechanik i indywidualne koncepcje bohaterów pozostają propozycjami, chyba że ustalono inaczej; kanon historii określa `world_history.md`. Szczegóły mapy i portrety pozostają do akceptacji.

## 0.1. Pre-PoC — pierwsza grywalna próba

[USTALONE — ZAKRES] Najpierw budujemy **pre-PoC**: minimalną próbę przejścia bohaterem przez labirynt ostrzeliwany przez automatyczne wieże. „Bez obrony” oznacza tu brak drużyny Defenderów, ręcznego pilotowania wież i fazy rozgrywania obrony przez przeciwnika — same wieże są aktywną przeszkodą. Celem jest sprawdzenie, czy ruch, unikanie ostrzału, użycie umiejętności i presja czasu są czytelne oraz dają satysfakcję.

**Minimum pre-PoC:**

- Jeden gracz steruje przynajmniej jednym grywalnym bohaterem, który ma HP, prędkość i działającą umiejętność ataku; wartości są punktami startowymi do testów.
- Jedna stała, drożna trasa w kształcie spirali prowadzi ze środka labiryntu do wyjścia. Kilka wcześniej ustawionych wież automatycznie wykrywa i ostrzeliwuje bohatera. Rodzaje wież, ich dokładne liczby i parametry są do testów.
- Próbę kończy dotarcie bohatera do mety albo utrata HP. Widoczne są co najmniej HP i czas oraz wynik próby; można ją powtórzyć.
- Mgła naciera po trasie. W przeglądarkowym teście rusza po około 5 s, a do mety dociera po 120 s od startu; opóźnienie i tempo wymagają dalszego sprawdzenia.

[POZA MINIMUM] Własne ustawienie wież, wielu bohaterów, towarzysz AI, wspólna gra, mastery, draft, ekonomia, drzewko technologii, moby i pety nie są warunkiem ukończenia pre-PoC. Mogą być testowane osobno, jeśli są już dostępne. Obecny przeglądarkowy trening jest uproszczoną demonstracją tego kierunku, a nie pełną implementacją docelowych reguł. W szczególności testowy zwykły atak Kael na wieże jest eksperymentem i nie zmienia ustalonej w sekcji 4 zasady, że w grze docelowej wieże niszczą lub wyłączają wyłącznie umiejętności specjalne.

**Następny etap — wcześniejszy zakres PoC 2v2:** zachowujemy opisany dalej, bardziej rozbudowany wariant jako kolejny etap po pre-PoC, nie jako warunek pierwszej próby. Obejmuje on m.in. dwie drużyny, draft, budowę i pary ataków; wcześniejsze oznaczenia „PoC” w GDD odnoszą się do tego późniejszego wariantu, chyba że wyraźnie wskazano pre-PoC.

## 1. Wizja i filary

[USTALONE] Dwie drużyny po pięciu graczy naprzemiennie rozwijają własną obronę i próbują przejść przez labirynt rywala. Gracze jako Runnerzy poruszają się w TPP, wspierają się i unikają ostrzału; jako Defenderzy ręcznie przejmują sojusznicze wieże. Wieże i technologie rozwijają się przez cały mecz, a niepełna wiedza i wnioski z poprzednich prób wymuszają adaptację. Wspólny rozwój nie ogranicza bohaterów do wybranego żywiołu.

Filary: budowanie trwałej obrony, zręcznościowa ucieczka, zespołowe decyzje, ręczny pilotaż, informacja i adaptacja. Siedem żywiołów, różne role oraz kompozycje służą tym filarom; balans jest do testów.

## 2. Przebieg meczu i wynik

[USTALONE] Docelowo matchmaking → draft bohaterów (docelowo z banami, w MVP bez banów) → obie drużyny jednocześnie inwestują → obie równocześnie budują → drużyna wybrana losowaniem atakuje obronę przeciwnika jako pierwsza → druga drużyna atakuje obronę pierwszej bez ponownego inwestowania lub budowy w środku pary → rozstrzygnięcie pary i przyznanie środków → kolejna faza inwestowania i budowy. Kolejność pierwszego ataku zmienia się między parami; jeżeli drużyny mają remisowy wynik, losowanie rozstrzyga, kto rozpoczyna następną parę. Powtarzać do końca meczu; nie kończyć go w środku pary. [OTWARTE] Sposób i moment losowania oraz obsługa sytuacji remisowego wyniku.

[USTALONE] Wynik pary: (1) więcej Runnerów dociera do mety; (2) przy równej dodatniej liczbie wygrywa krótszy czas ostatniego Runnera na mecie; (3) jeżeli nikt nie dotarł, wygrywa drużyna, której Runner dotarł najdalej. Gdy wyniki nadal są równe, obie drużyny otrzymują po jednym punkcie. Mecz może zakończyć się remisem. Obrażenia zadane w obronie nie są tie-breakerem.

[USTALONE] Runda punktowana oznacza pełną parę ataków. Faza zasadnicza trwa maksymalnie 10 takich rund; zdobycie 6 punktów kończy mecz natychmiast po rozliczeniu pełnej pary, np. 6:0 po sześciu parach. Przy wyniku 5:5 po dziesięciu rundach rozgrywa się jedną dodatkową, pełną parę; jej zwycięzca wygrywa mecz, a remis w tej parze oznacza remis meczu. Wyjątkowo cały mecz może więc obejmować 11 par. [USTALONE] Jeżeli po remisowej parze obie drużyny jednocześnie osiągają 6 punktów, czyli 6:6, mecz kończy się remisem. [USTALONE] Atak kończy się, gdy ostatni żywy Runner dotrze do mety albo wszyscy Runnerzy zostaną wyeliminowani. Dotarcie mgły do końca trasy po 120 s nie kończy ataku: trwa on dalej, dopóki wszyscy Runnerzy nie dotrą do mety lub nie zostaną wyeliminowani. [USTALONE] Draft korzysta ze wspólnej puli bez duplikatów: raz wybrany bohater nie może zostać wybrany ponownie przez żadną drużynę. Kolejność: A–B–B–A–A–B–B–A–A–B. W MVP bez banów. [USTALONE — PoC] Przed pierwszym atakiem drużyny mają 90 s na budowę; po kolejnych parach mają 45 s. Faza budowy kończy się automatycznie, a atak rusza po odliczaniu bez wymogu gotowości graczy. Draft 2v2 używa skróconej kolejności A–B–B–A. [OTWARTE] Docelowe reguły banów; czasy PoC mogą wymagać korekty po testach.

### Mecz a Wielkie Igrzyska

[USTALONE — FABUŁA] Championi zgłaszają się po stronie jednego z dwóch wariantów zarządzania. Każda strona wybiera składy finałowe na podstawie serii testów taktyki, wykonania zadań i współpracy. Najemnicy przechodzą te same kwalifikacje. Obok jawnych kontraktów zdarzają się zakulisowe oferty pieniędzy za wystąpienie po wybranej stronie. [OTWARTE] Format i punktacja kwalifikacji.

[USTALONE — FABUŁA] Finał Wielkich Igrzysk 12 ET obejmuje serię meczów wielu drużyn. Zakwalifikowani uczestnicy rejestrują, kogo reprezentują i na rzecz którego wariantu zarządzania liczą się ich punkty. Podział punktów drużyn mieszanych uzgadnia się przed startem. Pochodzenie i żywioł nie narzucają reprezentacji. Punkty uzyskane w meczach są przypisywane reprezentowanym stronom i sumowane dla obu wariantów, bez podwójnego liczenia.

[USTALONE — FABUŁA] Obie strony ogłaszają programy i konkretne obietnice dla krain; reprezentanci dowolnej krainy mogą wesprzeć dowolną stronę i wybraną obietnicę. Wynik serii wybiera wariant zarządzania; wyniki indywidualne, drużyn i krain wpływają na kolejność realizacji obietnic. [OTWARTE] Sposób przeliczania wyników na kolejność, remisy, finansowanie, zakres wiążących obietnic i los programu przegranej strony. Chronione dostawy i ratownictwo obowiązują niezależnie od kolejności inwestycji.

Punkty za pary ataków rozstrzygają pojedynczy mecz; punkty klasyfikacji Igrzysk rozlicza się osobno według wcześniej ogłoszonych zasad. Mecz nadal może zakończyć się remisem zgodnie z regułami powyżej. Jeśli po zaplanowanej serii klasyfikacja obu wariantów jest równa, drużyny rozgrywają dodatkowe mecze, aż po rozliczeniu meczu jeden wariant uzyska przewagę. Dopóki wynik pozostaje równy, całe Igrzyska nie są rozstrzygnięte. Prawomocny wynik serii wybiera wariant z większą liczbą punktów.

[OTWARTE] Liczba drużyn i meczów, punktacja klasyfikacji za zwycięstwo, remis i przegraną, podział punktów w mieszanych składach oraz dobór par dodatkowych meczów wymagają zaprojektowania i ogłoszenia przed Igrzyskami. Opis fabularny nie ustanawia jeszcze implementacji turnieju ani nie zmienia zasad zwykłego meczu lub zakresu pre-PoC.

### Pomiar postępu i czasu

[USTALONE] W trakcie ataku boczna tabela pokazuje w czasie rzeczywistym odległość Runnerów do mety, mierzoną **najkrótszą możliwą trasą**, a nie w linii prostej. Przy rozstrzyganiu, gdy nikt nie dotarł, porównujemy najmniejszą odległość do mety osiągniętą przez dowolnego Runnera danej drużyny — jego najdalszy postęp. W MVP jest tylko jedna trasa; przy przyszłych rozgałęzieniach nadal liczy się najkrótsza dostępna droga.

[USTALONE] Czas gry i pomiary związane z rozgrywką, w tym licznik mgły, rozliczamy według czasu serwera z dokładnością **0,1 s**. Dokładny sposób rozstrzygania jednoczesnych zdarzeń w tym samym kroku oraz to, czy pomiar jest zapamiętywany przy każdym kroku, są kwestiami implementacyjnymi do opisania. [OTWARTE] Dostęp do tabeli odległości dla Defenderów musi być zgodny z Fog of War — nie zakładamy, że zdradza pozycję ukrytych Runnerów.

### Rozłączenie i poddanie

[USTALONE] Przewidujemy reconnect. Po rozłączeniu gracza mecz otrzymuje pauzę do **30 s** na jego powrót; limit to **jedna taka pauza na gracza w całym meczu**. Jeśli gracz nie wróci w tym czasie lub rozłączy się ponownie po wykorzystaniu pauzy, mecz wznawia się bez niego. W MVP jego bohater pozostaje bierny, a jego wieże działają automatycznie. [PLAN NA PÓŹNIEJ] W docelowej wersji nieobecnego gracza ma przejmować bot. [OTWARTE] Zachowanie przy równoczesnym rozłączeniu wielu graczy, ponownym połączeniu po wznowieniu oraz dokładne fazy obowiązywania pauzy.

[USTALONE — kierunek] Nie wprowadzamy zwykłego poddania dostępnego w każdej sytuacji. [PROPOZYCJA] Jeśli gracz opuści mecz i jego drużyna przegrywa, można rozważyć głosowanie nad poddaniem. Liczba głosów, dokładny warunek odejścia, moment dostępności oraz konsekwencje nie zostały ustalone; funkcja pozostaje do decyzji, nie jest jeszcze gotową regułą.

## 3. Mapa i widoczność

[USTALONE] W meczu istnieją dwie identyczne kopie układu mapy, na jednym serwerze; każda drużyna rozbudowuje własną obronę. Pierwsza mapa jest stałym jednopoziomowym labiryntem: start w środku, kręta droga na zewnątrz, bez skrótów i ślepych zaułków w MVP. Układ ścian jest znany obu drużynom. Defenderzy bronią z murów; Runnerzy nie mogą się na nie dostać. TPP za plecami Runnera, strategiczna kamera budowy, lokalna kamera wieży. Dalsze mapy i losowość między meczami mogą powstać później.

[USTALONE] Wieże stawia się wyłącznie na murach, na zaprojektowanej siatce; jedno pole mieści jedną wieżę. Koncentracja wielu wież na sąsiednich polach jest dozwolona. Obie drużyny budują jednocześnie na niezależnych, lustrzanych kopiach mapy, więc zajęcie pola przez jedną drużynę nie blokuje odpowiadającego pola przeciwnikowi. Przy próbie zajęcia tego samego pola przez graczy jednej drużyny pole zostaje krótko zarezerwowane dla pierwszego gracza; jeśli ten nie zatwierdzi budowy, rezerwacja wygasa. Wartość czasu rezerwacji jest do określenia implementacyjnego. Trasa ma mieć szerokość dwóch jednostek postaci i nie może zostać całkowicie zablokowana. Szczegóły wymiarów w metrach, kolizji oraz innych obiektów i kontroli drożności pozostają otwarte. Efekty obszarowe mogą czasowo zajmować fragment trasy; przykładowo Magma ma zostawiać plamę lawy na kilka sekund, lecz jej obrażenia, czas i zasady kumulacji wymagają decyzji. Plama nie jest automatycznie fizyczną blokadą przejścia.

[USTALONE] Układ wrogiej obrony jest ukryty w fazie budowy. Runner odkrywa wieżę po wejściu w ustaloną odległość od niej; wykrycie nie wymaga linii widzenia. Odkryta wieża pozostaje w pamięci drużyny bezterminowo, ponieważ wież nie można usuwać po zakończeniu fazy, a jej zapamiętany stan nie aktualizuje się zdalnie. Drużyna widzi ostatnio poznaną pozycję i stan wieży; jeśli obrona została ulepszona od poprzedniej obserwacji, aktualna zmiana ujawni się dopiero po ponownym odkryciu. Defenderzy widzą Runnerów wyłącznie podczas bieżącego wykrycia i nie zachowują ich ostatniej pozycji. Gracze wyeliminowani mogą swobodnie obserwować rozgrywkę w widoku strategicznym, lecz podlegają temu samemu Fog of War co żywi sojusznicy i nie otrzymują dodatkowej wizji. [OTWARTE] Dokładne zasięgi wykrycia, prezentacja i oznaczanie nieaktualnej pamięci, minimapa, dostęp do danych sojuszników i sposób egzekwowania widoczności. [PROPOZYCJA] Serwer jest autorytatywny w kwestii widoczności.

### Długość trasy i czas ataku

[USTALONE — wartość startowa] Mgła dociera do końca trasy po **120 s**; atak może trwać dłużej, aż wszyscy Runnerzy dotrą do mety lub zostaną wyeliminowani. Punktem wyjścia do projektu pierwszej trasy jest **około 360 m rzeczywistej długości drogi po zakrętach**, nie odległość w linii prostej. Przy najwolniejszej prędkości referencyjnej 4 m/s niezakłócony bieg trwa 90 s, czyli 75% czasu dojścia mgły do końca; zostaje 30 s marginesu na uniki, zatrzymania i inne zdarzenia.

[PROPOZYCJA DO TESTÓW] Dla zgodności z nową prędkością referencyjną przyjmujemy dla Broma 4,0 m/s zamiast wcześniejszych 4,2 m/s. To nie przesądza o finalnym balansie postaci. Długość trasy jest założeniem do weryfikacji w silniku: przyspieszenie, zakręty, szerokość, kolizje i rzeczywista droga zmieniają czas przejścia. Zapas 30 s przed dotarciem mgły do końca nie gwarantuje przejścia pod ostrzałem lub silną kontrolą; gwarancję należy odnieść do zdefiniowanych warunków, a trasę zmierzyć testem bez przeszkód i w walce.

### Mgła ścigająca Runnerów

[USTALONE] Mgła przemieszcza się **wyłącznie po drodze labiryntu**, wzdłuż jej przebiegu i według pokonanej odległości; mury znajdują się ponad nią. Jej czoło dociera do końca trasy po **120 s** od początku ataku. Po dotarciu do mety mgła nie przesuwa się dalej: zwiększa zasięg skażenia, aż obejmie całą trasę. Samo dojście czoła do mety nie kończy ataku; walka trwa, dopóki wszyscy Runnerzy nie dotrą do mety lub nie zostaną wyeliminowani. [OTWARTE] Tempo i sposób rozszerzania skażonego obszaru po dojściu czoła do mety wymagają doprecyzowania.

Każdy Runner ma widoczny, indywidualny **licznik skażenia mgłą**. Rośnie o jedną sekundę na sekundę przebywania w mgle i maleje w tym samym tempie poza nią, aż do zera: pięć sekund ekspozycji wymaga pięciu sekund poza mgłą, aby licznik zniknął. Licznik jest nadrzędnym debuffem: nie można go rozproszyć ani zmienić jego czasu przez inne efekty, odporności lub modyfikatory. Niewrażliwość na obrażenia może tymczasowo chronić przed trafieniami mgły, ale nie zatrzymuje narastania licznika.

Podczas przebywania w mgle Runner jest spowolniony o **10%** i co sekundę otrzymuje obrażenia zależne od licznika, liczone od **maksymalnego HP**: pierwsza sekunda 1%, druga 2%, trzecia 3% itd. Przy ciągłej ekspozycji, bez leczenia, osłon ani niewrażliwości, suma po pięciu sekundach wynosi 15% maksymalnego HP, po sześciu 21%, a po siedmiu 28%. Sam licznik narasta również pod niewrażliwością; po jej zakończeniu kolejne obrażenia wynikają z aktualnej wartości licznika. Obrażenia nie są zadawane poza mgłą, gdzie licznik maleje.

[USTALONE] Licznik zmienia się zgodnie z czasem serwera co 0,1 s. Mgła jest efektem wizualnym i zestawem debuffów; nie blokuje ruchu, pocisków ani kolizji. [OTWARTE] Dokładny moment zadania pełnosekundowego tiku obrażeń i zaokrąglanie przy przejściu przez granicę mgły, tempo i sposób rozszerzania skażenia po dojściu do mety, wpływ na moby, interakcja z osłonami i odpornościami oraz łączenie jej 10% spowolnienia z innymi spowolnieniami. Prędkość i czytelność mgły oraz liczby należy sprawdzić w grywalnym PoC.

## 4. Wieże i walka

[USTALONE] Wszystkie bazowe typy siedmiu żywiołów są dostępne od początku. Każda wieża ma właściciela; gracz ma wspólny limit bazowy i dowolnie dobiera typy, również jeden typ wielokrotnie. Technologie mogą dodawać ponad limit miejsca dla określonej kategorii. Limit bazowy wynosi pięć wież na gracza. W fazie przygotowania, w której wieża została postawiona, wyłącznie jej właściciel może ją bezpłatnie przestawić lub usunąć; usunięcie zwraca pełny koszt, a zajęte przez nią pole i limit budowy wracają do puli właściciela. Typu postawionej wieży nie można zmienić — trzeba ją usunąć i postawić inną. Po zakończeniu fazy przygotowania wieża pozostaje na miejscu do końca meczu; nie istnieje sprzedaż ani zwrot za trwałą wieżę. Zasady rozliczania dodatkowych uprawnień po usunięciu wieży pozostają do doprecyzowania.

[USTALONE — PoC] W pierwszym PoC używamy tylko automatycznych wież T1 Ognia, Lodu i Błyskawic, z limitem pięciu na gracza. Nie ma ulepszeń T2/T3 ani ręcznego pilotażu. Drużyny bronią na dwóch identycznych kopiach mapy jednocześnie.

[USTALONE] Wieże automatyczne celują w najbliższy ważny cel dowolnego dostępnego typu, w tym Runnera, moba lub peta; zdolności i taunt mogą to zmienić. Fizyczne pociski blokują ściany. Runnerzy mają zwykły atak oraz umiejętności specjalne przeciwko wrogim jednostkom; wieże można czasowo wyłączyć lub zniszczyć wyłącznie specjalnymi umiejętnościami Runnerów, nie zwykłym atakiem. Powtarzane efekty tego samego typu na wieży odświeżają czas zamiast sumować intensywność. Różne źródła tego samego statusu nie kumulują jego mocy; działa najsilniejsza aktywna wartość. Zniszczona wieża wraca przed kolejnym atakiem na tę samą obronę. Szczegóły trafień, leczenia, tarcz, wykrywania i kolizji wymagają testów.

[USTALONE] Defender wybiera wolną sojuszniczą wieżę z mapy; natychmiast ją zajmuje, ale krótko trwa przejście do sterowania ręcznego. W tym czasie wieża działa automatycznie. Jeden Defender steruje co najwyżej jedną wieżą, a wieża ma co najwyżej jednego operatora. Można pilotować własne i sojusznicze; własna zapewnia większy bonus. Po wyjściu wraca automat. Defenderzy są nieśmiertelni, ale podlegają ogłuszeniom i zakłóceniom. 2 s i mnożniki z rozdziału balansu to propozycja.

[USTALONE] Runner wyeliminowany nie odradza się w bieżącym ataku. Runner ma dash do uniku na stałym czasie odnowienia oraz zwykły atak i umiejętności przeciw wrogim jednostkom; zwykły atak nie może wyłączać ani niszczyć wież. Drużyna może wykorzystywać ofensywne moby jako osłonę i wsparcie; ich dotarcie do mety nie wpływa na wynik pary. Niektóre technologie mogą automatycznie przywoływać sojuszniczą jednostkę w trakcie ataku, np. przy Runnerze najdalej od mety. Taunt nie powinien pozbawiać operatora ręcznego swobody wyboru celu. [OTWARTE] Zakres i zachowanie automatycznych przywołań z technologii, ewentualne inne metody przywołania mobów, ich limity i priorytety, szczegóły umiejętności i obserwacja po śmierci.

[USTALONE — OSŁONY RUNNERÓW] Osłona jest osobnym zasobem ponad HP i przyjmuje obrażenia przed HP. Nowe punkty osłony dodają się natychmiast do aktualnej wartości; od pierwszej pełnej sekundy od przyznania cała aktualna osłona traci 10% swojej bieżącej wartości na sekundę (100 → 90 → 81 → 72,9 itd.). Nie ma osobnego czasu wygaśnięcia. Kolejne źródła sumują się z istniejącą osłoną. **Iskar — Lodowa zbroja:** regeneruje 2 punkty osłony na sekundę do maksymalnie 10; regeneracja działa tylko, gdy łączna osłona nie przekracza 10 punktów, i wznawia się, gdy większa osłona spadnie do tego progu.

[USTALONE — PoC] Zwykły atak Runnera jest atakiem wręcz, a dash ma roboczy czas odnowienia **10 s**. PoC zawiera jednego szybkiego, kruchego moba bez ataku: jego rolą jest odciąganie ognia wież. Ma roboczo **40 HP** i prędkość **6 m/s**. Węzeł automatycznie przywołuje takiego moba co **30 s podczas ataku**, przy Runnerze najdalej od mety. Nie ma sztywnego limitu liczby mobów; drzewko technologii może zmieniać odstęp między przywołaniami. Dokładne węzły i wartości zmian tempa są do dopracowania.

## 5. Ekonomia, drzewko i obrażenia

[USTALONE] Jedna waluta. Na początku każdy gracz otrzymuje 400 g do wydania, a dodatkowe 100 g za niego automatycznie zasila początkowy węzeł żywiołu wybranego bohatera. Wpłaty sumują się; w 5v5 jest to 2000 g gotówki oraz 500 g wpłaconych do węzłów. Wszystkie bazowe wieże są dostępne bez zakupu węzła. Początkowy płatny węzeł to ikona żywiołu, koszt 800 g. Skład nie zabrania inwestycji w inne żywioły.

[USTALONE — FABUŁA] Waluta świata to selen. Organizator zapewnia wspólny budżet na labirynty, tymczasowe, celowo osłabione wieże i potrzebne materiały. Igrzyska przyciągają do Val Meris uzdrowicieli, projektantów, budowniczych i innych specjalistów, a ich praca i zamówienia wspierają lokalną gospodarkę. Po zawodach wieże są demontowane, a duża część elementów odzyskiwana; gracz nie może sprzedać ich ani uzyskać zwrotu podczas meczu. Meczowe g to wirtualne seleny przyznawane przez organizatora uczestnikom niezależnie od ich majątku. Służą wyborowi dostępnych technologii, których wartości nawiązują do cen świata, ale nie stają się prywatnymi pieniędzmi po meczu. Zakup wspólnego ulepszenia działa na całą drużynę zgodnie z zasadami gry. Szczegółowe koszty organizatora nie wymagają przedstawiania graczowi.

[USTALONE] Początkowa faza inwestowania trwa około 50 s indywidualnie + 10 s wspólnej puli; po kolejnych parach 20 + 10 s. Kliknięcie wpłaca dostępną kwotę do wysokości brakującego kosztu; nadwyżka pozostaje do rozdysponowania. Niewydane środki przechodzą do wspólnej puli, a następnie są automatycznie inwestowane; wolna gotówka nie przechodzi na kolejną rundę. Częściowe wpłaty w węzłach pozostają. Wspólna pula działa na zasadzie pierwszeństwa, konflikty rozstrzyga serwer. Priorytet autoinwestycji: najpierw węzły z częściowymi wpłatami, potem ulepszenia należące do najbardziej rozwiniętych drzewek, a gdy nie ma już dostępnych ulepszeń — losowa kolejna dostępna umiejętność. Szczegóły wyboru między równorzędnymi opcjami wymagają doprecyzowania.

[USTALONE] Każdy węzeł daje korzyść w ataku i obronie, a wspólne bonusy działają na drużynę niezależnie od żywiołu bohatera. Każdy żywioł ma podstawę T1, dwa warianty T2 oraz jeden wspólny T3. Pierwsza wpłata, również częściowa, w T2 blokuje konkurencyjny wariant. Pełne sfinansowanie automatycznie zmienia wszystkie odpowiednie wieże drużyny. [PROPOZYCJA] Koszt T2 1400 g i T3 2200 g; efekty w rozdziale technologii. [OTWARTE — DECYZJA UŻYTKOWNIKA] Działanie i koszty 21 reakcji między żywiołami; PoC obejmuje ograniczony, jeszcze niewybrany zestaw reakcji.

[USTALONE — PoC] Drzewko PoC obejmuje rdzenie T1 wszystkich siedmiu żywiołów. Nie zawiera specjalizacji T2/T3 ani reakcji między żywiołami; obowiązuje podstawowa macierz obrażeń.

[USTALONE — PoC] Węzeł przywołania tworzy szybkiego, kruchego moba co 30 s podczas ataku; limitu aktywnych jednostek nie ma. Drzewko może skracać odstęp przywołań. Dokładne ulepszenia wpływające na ten czas pozostają do zaprojektowania.

[PROPOZYCJA] Bazowa macierz żywiołów: każda para różnych żywiołów to główna przewaga/słabość 120%/80%, zwykła 110%/90% lub neutralność 100%. Każdy żywioł ma po jednej przewadze i słabości obu stopni oraz dwie neutralne relacje. Główne przewagi: Ogień → Natura; Lód → Światło; Błyskawice → Lód; Natura → Ziemia; Ziemia → Błyskawice; Światło → Ciemność; Ciemność → Ogień. Zwykłe przewagi: Ogień → Lód; Lód → Natura; Błyskawice → Ogień; Natura → Światło; Ziemia → Ciemność; Światło → Ziemia; Ciemność → Błyskawice. [PROPOZYCJA DO TESTÓW] Atak przeciw celowi tego samego żywiołu zadaje roboczo 50% obrażeń, jako silna odporność celu na własny żywioł. Dokładny zakres tej odporności i jej łączenie z innymi modyfikatorami pozostają do weryfikacji.

[PROPOZYCJA DO TESTÓW — ODPORNOŚCI BOHATERÓW] Bohater dziedziczy odporności z relacji między swoim żywiołem a żywiołem atakującego: neutralny 0%; zwykła przewaga bohatera +5%, główna przewaga +10%; zwykła słabość −5%, główna słabość −10%. Relacja z własnym żywiołem bohatera jest neutralna. Tankowiec otrzymuje pasywnie dodatkowe +5% odporności na każdy żywioł; premie dodają się do wartości bazowej. Wartości i sposób łączenia wymagają testów.

[USTALONE] Bohaterowie, wieże i moby mają żywioł; pet dziedziczy żywioł bohatera. Modyfikatory przewagi lub słabości żywiołu, technologii i odporności celu sumują się addytywnie. [PROPOZYCJA DO TESTÓW] Trafienie celu tym samym żywiołem zadaje roboczo 50% obrażeń; wartość i zakres wrogów objętych tą odpornością wymagają weryfikacji. [USTALONE — PoC] Po każdej parze ataków każdy gracz otrzymuje **150 g**; każdy gracz drużyny, która wygrała parę, otrzymuje dodatkowe **50 g**. Przy remisie obie drużyny dostają premię zwycięzcy. Są to wartości startowe do testów. [OTWARTE] Zachowanie odporności na ten sam żywioł wobec mobów i petów, kolejność łączenia modyfikatorów z osłonami, kolizje równoczesnych wpłat oraz zasady AFK. Cel około 20% kosztu całego drzewka na mecz wymaga wyceny po zdefiniowaniu reakcji i długości meczu.

## 6. Bohaterowie i zawartość

[USTALONE] Siatka 4 ról × 7 żywiołów: Tank, Wsparcie, Specjalista i Przywoływacz (Summoner); 28 kombinacji. W kodeksie jest 30 postaci głównej puli oraz Lucen jako dodatkowa alternatywa dla kombinacji Specjalista / Światło. Brakujące portrety, statystyki i umiejętności nadal wymagają opracowania. Do PoC zostanie wybranych kilka postaci. MVP obejmie 12 grywalnych bohaterów, po trzech z każdej z czterech ról; konkretna lista pozostaje do ustalenia. Docelowo dwa warianty na kombinację dają 56 postaci; nie ustalono, czy mają odmienne umiejętności.

[PROPOZYCJA DO TESTÓW — BONUSY KLASOWE] Bonus klasy jest oddzielny od unikalnej pasywki bohatera i w kodeksie ma ikonę swojej roli. Tank otrzymuje dodatkowe +5% odporności na każdy żywioł; Wsparcie wzmacnia leczenie i naprawy o 10%; Specjalista zwiększa o 25% szybkość ataku wieży, którą ręcznie pilotuje w obronie; Przywoływacz ma Peta-towarzysza z własnym HP, który automatycznie za nim podąża. Wartości i szczegóły wymagają testów.

[USTALONE] Rola nadaje zadanie, żywioł różnicuje sposób gry. Tank jest raczej wolny i wytrzymały. Niva, Elira i Liora są proponowanymi siostrami; Thora jest młodą dorosłą strażniczką, ma odrębny wygląd i nie ma ustalonego pokrewieństwa z nimi. [PROPOZYCJA] Koncepcje umiejętności, statystyki, pochodzenie i historie bohaterów znajdują się w kompletnych rozdziałach poniżej; dla pozostałych dziewięciu specjalizacje są ogólnym kierunkiem, nie zatwierdzonym zestawem umiejętności.

[USTALONE — PoC] Pierwszy PoC celuje w 2v2. Główna pula obejmuje sześciu bohaterów: Brom i Thora (Tank), Liora i Varek (Wsparcie), Saila i Zet (Specjalista). Dodatkową, siódmą postacią testową jest jeden Przywoływacz z petem, aby sprawdzić tę mechanikę. Wszystkie siedem postaci należy do tej samej puli draftu i nie można wybrać bohatera ponownie przez żadną drużynę. Przywoływacz jest wyjątkiem testowym i nie przesądza, czy pozostanie pełnoprawną rolą w docelowej puli.

[USTALONE — PoC] Pet ma własne HP i automatycznie podąża za Przywoływaczem; gracz nie wydaje mu komend. W ataku pełni funkcję ochronną i przekierowuje na siebie część obrażeń Przywoływacza; po wyeliminowaniu wraca na początku jego następnego ataku. Nie chroni innych Runnerów. **W PoC pet nie ma zdolności obronnej.** Wartość przekierowania obrażeń pozostaje do ustalenia.

[OTWARTE] Szczegółowa propozycja 12 bohaterów obecnie opisuje po cztery Tanki, Wsparcia i Specjalistów, natomiast główny zakres MVP przewiduje po trzech bohaterów w czterech rolach, w tym Przywoływaczy. Skład i podział ról pełnego MVP wymagają ujednolicenia.

### Zestawienie podstawowych statystyk 30 bohaterów głównej puli i alternatywy

Poniższe liczby to **propozycja bazowego balansu do testów**, a nie zatwierdzony balans. Lucen jest dodatkową alternatywą Specjalisty Światła; jego poniższe wartości są propozycją do testów. Zwykły atak zadaje obrażenia wrogim jednostkom, ale nie wyłącza ani nie niszczy wież; wieże są celem zdolności specjalnych. Obrażenia podano na trafienie. Bazowa hierarchia ruchu: **Specjalista 5,5–5,8 m/s > Przywoływacz 5,1–5,4 m/s > Wsparcie 4,7–4,9 m/s > Tank 4,0–4,4 m/s**. Bazowe obrażenia zwykłego ataku: Specjalista 20, Tank 14, Wsparcie 10, Przywoływacz 8. Szczegóły postaci mogą dodawać premie lub wyjątki.

Przywoływacz ma dwa pełne zestawy statystyk: własne oraz swojego peta. Przywoływacz zaczyna z około 60 HP — najniższą pulą wśród bohaterów. HP peta zależy od funkcji i sylwetki: kruchy kruk/kotoperz ma mało HP, a osłonowa panda dużo. Petów nie wybiera się osobno w draftcie; są przypisani do bohaterów. Wartości obrażeń peta oznaczają jego zwykły atak przeciw jednostkom; zdolności specjalne mogą zadawać inne obrażenia. Ruch peta jest jego własną prędkością, niezależną od Przywoływacza.

| Bohater | Rola / żywioł | HP bohatera | Obrażenia ataku bohatera | Prędkość bohatera (m/s) | Pet | HP peta | Obrażenia ataku peta | Prędkość peta (m/s) |
|---|---|---:|---:|---:|---|---:|---:|---:|
| Brom | Tank / Ziemia | 180 | 14 | 4,0 | — | — | — | — |
| Thora | Tank / Natura | 165 | 14 | 4,1 | — | — | — | — |
| Morrak | Tank / Ciemność | 170 | 14 | 4,2 | — | — | — | — |
| Kael | Tank / Ogień | 170 | 14 | 4,4 | — | — | — | — |
| Iskar | Tank / Lód | 175 | 14 | 4,1 | — | — | — | — |
| Taren | Tank / Błyskawice | 165 | 14 | 4,2 | — | — | — | — |
| Orien | Tank / Światło | 170 | 14 | 4,3 | — | — | — | — |
| Liora | Wsparcie / Natura | 120 | 10 | 4,8 | — | — | — | — |
| Elira | Wsparcie / Światło | 120 | 10 | 4,8 | — | — | — | — |
| Varek | Wsparcie / Błyskawice | 115 | 10 | 4,9 | — | — | — | — |
| Niva | Wsparcie / Lód | 125 | 10 | 4,7 | — | — | — | — |
| Solan | Wsparcie / Ogień | 120 | 10 | 4,8 | — | — | — | — |
| Dara | Wsparcie / Ziemia | 125 | 10 | 4,7 | — | — | — | — |
| Nocten | Wsparcie / Ciemność | 115 | 10 | 4,9 | — | — | — | — |
| Rian | Specjalista / Ogień | 110 | 20 | 5,6 | — | — | — | — |
| Saila | Specjalista / Lód | 105 | 20 | 5,5 | — | — | — | — |
| Zet | Specjalista / Błyskawice | 100 | 20 | 5,8 | — | — | — | — |
| Morven | Specjalista / Ciemność | 105 | 20 | 5,6 | — | — | — | — |
| Miko | Specjalista / Błyskawice | 105 | 20 | 5,8 | — | — | — | — |
| Eron | Specjalista / Natura | 110 | 20 | 5,5 | — | — | — | — |
| Kiran | Specjalista / Ziemia | 115 | 20 | 5,5 | — | — | — | — |
| Adrien | Specjalista / Światło | 105 | 20 | 5,7 | — | — | — | — |
| Lucen | Specjalista / Światło (alternatywa) | 105 | 20 | 5,6 | — | — | — | — |
| Pyrra | Przywoływacz / Ogień | 60 | 8 | 5,2 | Lis Iskra | 65 | 12 | 6,0 |
| Eira | Przywoływacz / Lód | 60 | 8 | 5,2 | Ryś Szron | 85 | 14 | 5,5 |
| Volt | Przywoływacz / Błyskawice | 60 | 8 | 5,4 | Sokół Grom | 55 | 10 | 7,0 |
| Ana | Przywoływacz / Natura | 60 | 8 | 5,1 | Panda Bob | 150 | 10 | 3,8 |
| Rokan | Przywoływacz / Ziemia | 60 | 8 | 5,1 | Pancernik Krzemień | 120 | 8 | 3,8 |
| Lumen | Przywoływacz / Światło | 60 | 8 | 5,3 | Jeleń Blask | 95 | 8 | 5,0 |
| Koneva | Przywoływacz / Ciemność | 60 | 8 | 5,3 | Kotoperz Patka | 50 | 10 | 6,5 |
| Neris | Przywoływacz / Ciemność | 60 | 8 | 5,2 | Kruk Cień | 45 | 8 | 6,5 |

Obrażenia specjalnych zdolności pozostają osobną statystyką i wynikają z opisu zdolności: np. Q Riana zadaje 100 obrażeń wieży, a obronny atak Boba zadaje 10 bazowych obrażeń Natury. Liczby z tabeli należy weryfikować w testach, szczególnie relację niskiego HP Przywoływacza do HP i prędkości peta. Pula 30 nazw i ról odpowiada symulatorowi draftu; wartości brakujące wcześniej dla 18 postaci są propozycjami do testów.

## 7. Świat, estetyka i technologia

[USTALONE — KIERUNEK] Po wyniszczającej wojnie krainy używają obowiązkowych Prób do rozstrzygania części sporów. Drużyny mogą być mieszane, reprezentując społeczność lub wspólną sprawę, niekoniecznie krainę pochodzenia. [PROPOZYCJA] Nazwy Riftoryn, splotyt, krainy, instytucje, chronologia, ośrodki i biografie poniżej wymagają akceptacji. Pokonanie w Próbie może być magicznym wycofaniem, a nie śmiercią fabularną.

[USTALONE] Czytelna stylizowana grafika fantasy 3D: proste, lekko geometryczne bryły, łatwo rozpoznawalne postacie, wieże i pociski; bez fotorealizmu i estetyki voxel. Portrety w `assets/heroes/portraits/` są materiałami wizualnymi, a mapy w kodeksie koncepcjami. Żywioły powinny różnić się sylwetką i efektem, nie samym kolorem.

[OTWARTE] Silnik, wersja, docelowe platformy, sieć, model serwera, sposób przechowywania danych, matchmaking, dostępność i produkcyjny pipeline modeli. UE5 jest wcześniejszą preferencją, nie zatwierdzonym wyborem. [PROPOZYCJA] Autorytatywny serwer: stan meczu, własność, ekonomia, obrażenia i widoczność; klient odpowiada za sterowanie i prezentację.

### 7.1 Wstępna propozycja technologii — nie decyzja

[PROPOZYCJA] **Unreal Engine 5** jako pierwszy kandydat do oceny: gra ma kamery TPP, interaktywny świat 3D, wiele umiejętności oraz drużynowy multiplayer. Konkretna wersja pozostaje otwarta; wybrać ją dopiero podczas testu zgodności narzędzi, platform i licencji.

[PROPOZYCJA] **Klient–serwer z autorytatywnym serwerem**, docelowo dedykowanym dla uczciwej rywalizacji. Serwer zatwierdza m.in. fazy, budowę, wpłaty, pilotaż, obrażenia i informacje udostępniane graczom. Klient prezentuje obraz, interfejs i lokalne sterowanie. Nie zakładamy, że sam wybór silnika automatycznie zapewnia bezpieczeństwo lub gotowy Fog of War. Według [dokumentacji sieci Unreal](https://dev.epicgames.com/documentation/unreal-engine/networking-overview-for-unreal-engine) serwer utrzymuje autorytatywny stan, a dedykowany serwer nie obsługuje lokalnego gracza; jego konfiguracja i hosting wiążą się z dodatkowymi kosztami i pracą.

[PROPOZYCJA] Ocenić **Gameplay Ability System** pod kątem umiejętności, statusów i cooldownów dopiero podczas testu technicznego; nie jest obowiązkowy. [Dokumentacja Epic](https://dev.epicgames.com/documentation/en-us/unreal-engine/understanding-the-unreal-engine-gameplay-ability-system) opisuje jego obsługę umiejętności i replikacji. Wcześniej ustalić podstawowe zasady w GDD oraz przygotować uzgodnione modele i wymagania importu.

[PLAN NA PÓŹNIEJ] Po dopracowaniu projektu wykonać ograniczony test: dwa klienty i serwer; przejście Runnera; jedna wieża automatyczna i ręczna; uproszczone, serwerowe sprawdzenie widoczności oraz jednej wpłaty. Zmierzyć stabilność, narzędzia i pracochłonność, następnie zdecydować o silniku. Unity pozostaje alternatywą do porównania. Żadne testy ani prace w silniku nie są teraz zlecane.

## 8. Co dopracować w GDD przed budową prototypu

Ta lista obejmuje wyłącznie **decyzje i opisy projektowe**. Wartości, dla których mamy już założenie — np. limit ataku 120 s, trasa około 360 m, limit pięciu wież czy robocze obrażenia — pozostają jako **wartości startowe do testów w działającym PoC**. Nie wymagają potwierdzenia przed powstaniem grywalnej wersji. Ustalenie brakującej reguły może być także jawną decyzją o uproszczeniu jej na potrzeby PoC; nie trzeba mieć finalnego balansu.

1. **Przebieg ataku i meczu:** opisano kolejność ról w parze, automatyczny start ataku i następstwa reconnectu w MVP. Nadal doprecyzować losowanie pierwszej kolejności i jej zmianę przy remisie wyniku, równoczesne rozłączenia, dokładny moment tików mgły oraz ewentualny zakres głosowania przy odejściu gracza. Docelowe boty zastępujące rozłączonych graczy są pracą na później.
2. **Mapa i budowa:** zasady pól, rezerwacji, równoczesnej budowy na lustrzanych mapach, modyfikowania świeżo postawionych wież oraz ruchu mgły po trasie zapisano. Do doprecyzowania przed implementacją: wymiarowanie pól i kolizji w metrach, kontrola drożności oraz tempo rozszerzania skażenia po dojściu mgły do mety. Długość 360 m pozostaje punktem wyjścia do testów.
3. **Podstawy walki:** dash, zwykły atak Runnera przeciw jednostkom i jego ograniczenia wobec wież oraz odświeżanie czasu wyłączenia wieży zapisano. Doprecyzować dokładne sterowanie i cooldowny, trafienia i osłony, wpływ przeszkód, zakłócenie Defendera, priorytety celów wież i szczegóły statusów. Wskazać, które wartości przyjmujemy jako początkowe dla PoC.
4. **Widoczność i pilotaż:** podstawowe zasady wykrywania, pamięci odkrytych wież, informacji o Runnerach i obserwacji po eliminacji zapisano. Do doprecyzowania pozostają dokładne zasięgi i prezentacja nieaktualnego stanu. Nadal opisać wybór, przejęcie i opuszczenie wieży oraz to, kiedy można używać umiejętności podczas pilotażu.
5. **Ekonomia po parze ataków:** PoC ma startowe wartości 150 g na gracza i premię 50 g dla każdego gracza zwycięskiej drużyny; przy remisie premię dostają obie drużyny. To punkt startowy do testów, a nie domknięta ekonomia pełnej gry. Priorytet autoinwestowania zapisano; doprecyzować rozstrzygnięcie losowe między równorzędnymi ulepszeniami.
6. **Reakcje i obrażenia żywiołów:** PoC nie zawiera reakcji między żywiołami; używa podstawowej macierzy. Żywioły jednostek, addytywne łączenie modyfikatorów i roboczą odporność 50% na ten sam żywioł zapisano. Do weryfikacji: zakres 50% odporności i jej połączenie z osłonami oraz statusem.
7. **Moby:** pierwszy mob PoC ma 40 HP, 6 m/s i odciąga ogień wież, nie zadając obrażeń. Pojawia się co 30 s przy Runnerze najdalej od mety; limitu liczby aktywnych mobów nie ma, a drzewko może skracać odstęp. Ich dotarcie nie zmienia wyniku. Doprecyzować ulepszenia zmieniające odstęp i interakcję z wyborem celu przez wieże.
8. **Bohaterowie i drzewko:** zakres bohaterów PoC jest wybrany; pełna lista 12 bohaterów MVP i niespójność podziału ról wymagają ujednolicenia. Przejrzeć zestawy bohaterów oraz węzły i opisać brakujące zasady ich działania w ataku i obronie. Wartości HP, prędkości, czasów odnowienia i kosztów oznaczyć jako startowe do testów. W symulatorze dostępnych jest 31 postaci (30 głównych oraz Lucen jako dodatkowa alternatywa Specjalisty Światła); umiejętności i balansem pozostałej zawartości trzeba zająć się przed pełnym wdrożeniem.
9. **Zakres i kryteria późniejszego PoC 2v2:** to etap po minimalnym pre-PoC opisanym w sekcji 0.1; ustalono 2v2; sześciu głównych bohaterów z trzech ról plus dodatkowego, siódmego Przywoływacza do testu peta; draft bez duplikatów; dwie identyczne kopie mapy o trasie około 360 m; automatyczne wieże T1 Ognia, Lodu i Błyskawic, pięć na gracza; bez pilotażu, T2/T3 i reakcji; siedem rdzeni T1; szybki mob 40 HP / 6 m/s bez ataku, pojawiający się co 30 s przy Runnerze najdalej od mety, bez limitu aktywnych jednostek; pet bez komend, w ataku chroni Przywoływacza przez przekierowanie części obrażeń i nie ma zdolności obronnej; zwykły atak wręcz; dash z odnowieniem 10 s; budowa 90 s przed pierwszym atakiem i 45 s po kolejnych parach; dochód 150 g na gracza i premia 50 g dla każdego gracza zwycięskiej drużyny, przyznawana obu drużynom w remisie. PoC ma obsługiwać kilka par; pierwsza sesja testowa obejmuje trzy pełne pary. Zbieramy wynik, postęp i czasy Runnerów oraz dane o obrażeniach, strzałach/trafieniach wież, użyciach zdolności i przeżyciu mobów/peta. Działanie drogi i walki oceniamy ręcznie; kryteria i checklista testów oraz wyjątki zatwierdzane przez projektanta wymagają jeszcze zapisania.
10. **Wymagania techniczne i produkcyjne w GDD:** opisać docelowe platformy, oczekiwania wobec multiplayera i serwera, danych oraz importu modeli. Zachować UE5 jako propozycję, a nie zatwierdzony wybór; wybór i praktyczną weryfikację narzędzi przeprowadzić później.
11. **Końcowy przegląd dokumentu:** oznaczyć ustalone zasady, wartości startowe i rzeczy odłożone; przejść przez przykładową pełną parę ataków oraz sprawdzić, czy opisane interakcje nie przeczą sobie. Dopiero po zamknięciu potrzebnych luk przygotować zadania implementacyjne dla agentów.

## 9. Ocena gotowości i otwarte ryzyka

**Ocena: 6/10 jako dokument do przekazania agentom do samodzielnej implementacji całej gry; 8/10 jako opis wizji do planowania preprodukcji i budowy ograniczonego PoC.** Ostatnie decyzje domknęły ważne zasady meczu, budowania, widoczności, podstaw walki, wyniku mobów, premii ekonomicznej i kierunku PoC. Nadal brakuje wielu liczb oraz specyfikacji zawartości wymaganych do samodzielnej implementacji pełnej gry. Ocena jest osądem projektowym, nie wynikiem testów.

**Najważniejsze pozostałe ryzyka:** (1) zakres i efekty reakcji żywiołów oraz kompletne zdolności bohaterów są niegotowe; (2) ekonomia 150 g + 50 g to wartości startowe PoC, a nie gotowy model pełnej gry; (3) 50% odporności na własny żywioł jest hipotezą do testów, a nie zatwierdzonym balansem; (4) liczby peta, wartości węzła przywołania, wymiary pól, dokładne zasięgi wykrywania i tempo rozszerzania skażenia mgły pozostają do doprecyzowania; (5) specyfikacje mobów, wież i technologii nie pokrywają jeszcze całej zawartości MVP; (6) role i skład pełnego MVP wymagają ujednolicenia; (7) silnik, platformy i model sieciowy nie są wybrane ani sprawdzone. Liczby i równowaga nie zostały zweryfikowane w grze.

## 10. Co sprawdzimy dopiero po powstaniu grywalnego PoC

- Zmierzymy rzeczywisty czas przejścia trasy około 360 m, w tym zakręty i ruch w walce, oraz ocenimy, czy czas 120 s dojścia mgły do końca i zapas czasu są odpowiednie.
- Sprawdzimy obrażenia, HP, cooldowny, efekty i zależności żywiołów, przeżywalność, skuteczność automatu i ręcznego pilotażu oraz możliwość kontrowania zagęszczonych wież.
- Zmierzymy czas meczu, dochód, tempo inwestycji oraz rozwój obrony i ataku. Cel około 20% kosztu pełnego drzewka ocenimy, gdy znane będą koszty reakcji i dane z meczów.
- Zweryfikujemy czytelność interfejsu, efektów, widoczności, komunikacji i czas oczekiwania po eliminacji; później porównamy w praktyce workflow oraz multiplayer wybranych narzędzi i silnika.

To nie są kryteria ukończenia samego GDD. Wyniki PoC mogą zmienić zapisane dziś liczby i założenia.

---

# CZĘŚĆ II — SZCZEGÓŁOWE MATERIAŁY PROJEKTOWE

Wszystkie poniższe rozdziały są scalone do jednego dokumentu. Ich oznaczenia propozycji i otwarte pytania zachowują moc. Przy różnicach pierwszeństwo ma część I; dawne nazwy i liczby w tej części nie tworzą nowych zatwierdzonych decyzji.

## Riftoryn — 12 bohaterów: specyfikacja alfa v0.1

**Status:** kompletne propozycje do przeczytania i pierwszych testów; wartości nie były testowane. Pierwsza pula obejmuje 12 bohaterów, po czterech na rolę. Można ją uwzględnić w MVP; balans będzie dostrajany w alfie i becie. Nazwy są robocze. Technologie drużynowe działają niezależnie od żywiołu bohatera i każda daje korzyść w ataku oraz obronie.

### Wspólne zasady — propozycja startowa

- Każdy bohater ma pasywkę i aktywną umiejętność pod klawiszem Q. Umiejętność działa inaczej w ataku i obronie. Nie ma osobnego ultimate. Czas odnowienia biegnie od użycia; na początku każdego ataku i każdej obrony zdolność jest gotowa. Nie ma dodatkowych ładunków. Poza fazą akcji nie można jej używać.
- Dystanse poniżej podano w metrach świata gry. Prędkość przeciętnego Runnera to 5 m/s. Runner odzyskuje pełne HP na początku kolejnego własnego ataku; po utracie HP zostaje wyeliminowany bez wskrzeszenia do końca ataku. Naturalna regeneracja nie występuje.
- Defender jest nieśmiertelny. W obronie wybiera zdolnością cel z mapy, także podczas przejmowania i pilotowania wieży; użycie nie przerywa pilotażu. Zmiana wieży nie resetuje czasu odnowienia. Wyłączenie wieży blokuje strzały automatyczne i ręczne; operator w niej pozostaje. Po zakończeniu wyłączenia wieża jest odporna na ponowne wyłączenie przez 2 s.
- Na potrzeby poniższych zdolności przyjmujemy tymczasowo **300 HP i 100 punktów osłony** standardowej wieży. Osłona przyjmuje obrażenia przed HP. Każdy kolejny atak na tę obronę przywraca pełne HP i osłonę, także zniszczonym wieżom. Naprawa w trakcie ataku nie przywraca zniszczonej wieży. Parametry innych wież i technologie będą wymagały osobnej specyfikacji.
- Leczenie i naprawa nie przekraczają maksimum. Ponowne nałożenie tego samego efektu nie sumuje jego siły; utrzymuje silniejszy efekt i odświeża czas. Różne spowolnienia nie sumują się — działa najsilniejsze. Wartości są bez bonusów drzewka.
- Zdolność użyta na nieprawidłowym celu nie uruchamia czasu odnowienia. Serwer sprawdza fazę, cel, zasięg, widoczność i wynik. Runner może wybrać jako cel wyłącznie odkrytą przez drużynę wieżę; jeśli opis wymaga widoczności, ściana blokuje celowanie. Defender widzi własne wieże. Wszystkie czasy, obrażenia i ograniczenia poniżej są propozycją do balansu.

### Tankowie

#### 1. Brom — Tank Ziemi: Kamienna Tarcza
**HP:** 180. **Prędkość:** 4,0 m/s (wartość startowa do testów). Powolny obrońca osłaniający przejście.

**Pasywka — Kamienny pancerz:** otrzymuje o 10% mniej obrażeń od bezpośrednich trafień. W obronie jego ręcznie pilotowana wieża otrzymuje o 10% mniej takich obrażeń.

**Q w ataku — Mur tarcz:** przez 4 s niesie przed sobą tarczę o szerokości i wysokości 2,5 m, która zatrzymuje trafiające w nią pociski do łącznej wartości 160 obrażeń. Za tarczą mogą schować się sojusznicy. Podczas jej działania Brom porusza się z 70% swojej prędkości. Tarcza nie zatrzymuje już powstałych efektów obszarowych. **Odnowienie: 20 s.**

**Q w obronie — Kamienna zbroja:** wybrana żywa wieża otrzymuje o 30% mniej obrażeń przez 5 s. Nie chroni przed wyłączeniem. **Odnowienie: 20 s.** Kontra: obejście tarczy albo przeczekanie zbroi.

#### 2. Thora — Tank Natury: Strażniczka Korzeni
**HP:** 165. **Prędkość:** 4,1 m/s. Wytrzymuje ostrzał i podtrzymuje pobliskich sojuszników.

**Pasywka — Żywotne korzenie:** otrzymuje o 15% więcej leczenia. W obronie jej naprawa osłony jest silniejsza; poniższa wartość już to uwzględnia.

**Q w ataku — Żywa ostoja:** przez 5 s leczy siebie, żywych Runnerów i sojusznicze moby w promieniu 4 m o 8 HP/s. Pole przemieszcza się wraz z Thorą. **Odnowienie: 22 s.**

**Q w obronie — Korzenna osłona:** wybrana żywa wieża natychmiast odzyskuje 46 punktów zwykłej osłony i przez 4 s otrzymuje o 15% mniej obrażeń. **Odnowienie: 22 s.** Kontra: rozproszenie drużyny lub ostrzał przekraczający tempo leczenia.

#### 3. Morrak — Tank Ciemności: Strażnik Cienia
**HP:** 170. **Prędkość:** 4,2 m/s. Tworzy przestrzeń do unikania pocisków.

**Pasywka — Oczy mroku:** efekty ograniczające jego widoczność trwają o 20% krócej; dotyczy to również widoku wieży podczas jego ręcznego pilotażu.

**Q w ataku — Gęsty mrok:** przez 4 s wrogie lecące pociski w promieniu 4 m od Morraka poruszają się o 50% wolniej. Po opuszczeniu pola wracają do pierwotnej prędkości. Pole nie zmienia wiązek, natychmiastowych efektów ani wybuchów. **Odnowienie: 24 s.**

**Q w obronie — Cień bastionu:** przez 5 s Runnerzy oddaleni o ponad 8 m od wybranej wieży nie mogą wskazać jej jako celu nowej zdolności. Aktywne efekty trwają dalej; wieża pozostaje widoczna. **Odnowienie: 24 s.** Kontra: zbliżenie, ominięcie pola lub przeczekanie.

#### 4. Kael — Tank Ognia: Żarowy Bastion
**HP:** 170. **Prędkość:** 4,4 m/s. Chroni zespół krótkimi osłonami.

**Pasywka — Hartowanie:** otrzymuje o 20% mniej obrażeń okresowych; w obronie ta sama premia dotyczy ręcznie pilotowanej przez niego wieży.

**Q w ataku — Żarowa bariera:** natychmiast otrzymuje 100 punktów osłony. Obecni w promieniu 3 m sojuszniczy Runnerzy otrzymują po 50 punktów osłony. Osłony sumują się z aktualnymi zasobami i zanikają zgodnie z ogólną regułą 10% bieżącej wartości na sekundę. **Odnowienie: 22 s.**

**Q w obronie — Rozgrzana zbroja:** wybrana żywa wieża otrzymuje natychmiast 80 dodatkowych punktów osłony; są zużywane przed zwykłą osłoną i zanikają zgodnie z ogólną regułą osłon. **Odnowienie: 22 s.** Kontra: wyczerpanie lub przeczekanie bariery.

### Miko — Specjalista Błyskawic: Szermierz-Inżynier (druga seria)

[USTALONE — koncepcja] Miko to alternatywa dla Zeta: improwizujący szermierz i inżynier w charakterystycznym kapeluszu. Jego efekty bywają nieprzewidywalne, ale zasadniczo korzystne przy odpowiednim podejściu.

[USTALONE — wartości do testów] **Ucieczka — One Lucky Bastard:** Miko próbuje sabotować przeciwną wieżę. Każdy z czterech wyników ma 25% szans: (1) zatrzymanie wieży na 5 s; (2) trwałe wyłączenie jej w tej obronie; (3) 50% szansy na pudło do końca rundy; (4) pech: +50% szybkości ataku wieży przeciwnika przez 5 s. To zastępuje wcześniejszy pomysł zamiany krytyka. [OTWARTE] Jak długo trwa „trwałe” wyłączenie w strukturze wielu ataków i co oznacza koniec rundy dla pudłowania; zasięg, limit użyć, odnowienie, szczegóły pudłowania oraz interakcja z pilotem i odpornością Adriena.

[PROPOZYCJA — wartości do testów] **Obrona — Się Naprawi:** Miko próbuje naprawić pilotowaną wieżę; losowany jest jeden z czterech wyników z szansą 25% każdy: (1) przywrócenie 50 punktów osłony; (2) +50% szybkości ataku na 5 s; (3) +50% prędkości pocisków na 5 s; (4) pech: wieża przestaje strzelać na 3 s. Trzy wyniki są korzystne, a jeden pechowy. Efekt zastępuje poprzednią propozycję; liczby, interakcje i dostępność zdolności wymagają decyzji.

### Nowa rola: Przywoływacz (Summoner)

[USTALONE] Przywoływacz ma mało HP oraz towarzyszącego mu peta z własnym HP. W PoC pet automatycznie podąża za właścicielem; nie wydaje mu się komend. Wartości do testów dla Neris i kruka Cienia znajdują się w tabeli statystyk. Ana to Przywoływaczka Natury, a jej towarzyszem jest panda Bob. Jej portret inspirowany zdjęciem przedstawia ją z pełnowymiarową pandą Bobem.

[PROPOZYCJA] W ataku pet osłania właściciela przez przekierowanie części otrzymywanych przez niego obrażeń. W PoC pety nie mają zdolności w obronie. Poza PoC poszczególne pety mogą otrzymać zdolności obronne, takie jak koncepcyjna Klątwa Cienia kruka Neris. Żywioł bohatera nie ogranicza wyboru wież. Towarzysze są odrębną mechaniką od ogólnodrużynowych mobów z technologii.

[OTWARTE] Wartość przekierowania obrażeń, interakcje peta z mgłą, celowaniem wież, blokowaniem trasy i rozliczeniem wyników oraz zachowanie podczas powrotu po wyeliminowaniu wymagają doprecyzowania. Pozostałe pety nie otrzymują komend w PoC.

**Siedem koncepcji — nazwy i zdolności robocze:**

- **Ogień — Pyrra i lis Iskra:** lis przechwytuje pocisk i tworzy krótką strefę żaru; w obronie pomaga wieży wywierać presję obszarową.
- **Lód — Eira i ryś Szron:** ryś zasłania bohatera i zostawia śliski ślad; w obronie wzmacnia kontrolę ruchu w pobliżu wieży.
- **Błyskawice — Volt i sokół Grom:** sokół szybko dociera do wskazanego miejsca i przechwytuje pocisk; w obronie pomaga oznaczyć cel dla operatora.
- **Natura — Ana i panda Bob:** panda osłania Anę oraz wskazanego sojusznika; w obronie wspiera wieżę ochroną lub kontrolą obszaru. Portret przedstawia Anę z pełnowymiarowym Bobem.
  - [PROPOZYCJA] **Bambus dla Boba:** Ana przywołuje Boba do siebie, wyciąga bambus z plecaka i karmi go, regenerując część jego HP. O charakterze umiejętności decydują wspólna animacja i wybór bezpiecznego momentu. Do zaprojektowania: ilość leczenia, czas animacji, odnowienie lub limit użyć, przerwanie i działanie, gdy Bob jest poza zasięgiem albo został wyeliminowany.
  - [USTALONE — wartości do testów] **Obrona — atak Boba:** co 30 s Bob zeskakuje z murów, biegnie do najbliższego przeciwnika, zadaje mu 10 bazowych obrażeń Natury i wraca do Any. Podczas tej akcji jest niezniszczalny i nie można go zatrzymać ani zakłócić; Runnerzy mogą korzystać z własnych zdolności ochronnych. Żywiołowe modyfikatory i efekty ochronne stosuje się zgodnie z ogólnymi zasadami obrażeń. [OTWARTE] Pierwszy moment aktywacji, wybór celu przy remisie lub braku przeciwnika, zachowanie po utracie celu, warunki przebywania Boba przy Anie oraz szczegóły interakcji z osłonami wymagają doprecyzowania.
- **Ziemia — Rokan i pancernik Krzemień:** opancerzony towarzysz osłania przejście przed pociskami, nie blokując Runnerów; w obronie daje krótką osłonę wieży.
- **Światło — Lumen i jeleń Blask:** jeleń osłania sojusznika świetlistą barierą; w obronie pomaga ujawniać przeciwników w obszarze wieży.
- **Ciemność — Koneva i Patka:** Konevie towarzyszy mały demoniczny kotoperz Patka; umiejętność obronna wymaga opracowania. Koneva jest pierwszą postacią tej kombinacji.
  - [USTALONE — wartość do testów] **Ucieczka — aktywna zdolność Patki:** Patka przez chwilę bardzo szybko lata w kółko, skupiając na sobie ogień wieży; podczas manewru ma 75% szansy na uniknięcie obrażeń. Po zakończeniu wraca i lata w pobliżu Konevy. [OTWARTE] Czas działania, odnowienie, zasięg, sposób wskazania i liczba wież, szczegóły losowania uników i interakcja z operatorem ręcznym oraz zachowanie po wyeliminowaniu Patki.
- **Ciemność — Neris i kruk Cień:** kruk przyjmuje część obrażeń dzięki więzi z bohaterką.
  - **Pasywka — Czujny lot:** kruk automatycznie utrzymuje się około 3 m przed Neris, licząc wzdłuż trasy w kierunku mety. Każde trafienie skierowane w kruka ma 25% szansy na uniknięcie obrażeń.
  - **Q w ataku — Ucieczka kruka:** kruk wzbija się w powietrze i opuszcza mapę; w tym stanie nie może być trafiony ani przejmować obrażeń. Odzyskuje 2 HP/s i wraca natychmiast po osiągnięciu pełnego HP. Czas odnowienia wynosi 30 s i zaczyna się po powrocie kruka.
  - **Koncepcja zdolności obronnej — Klątwa Cienia (poza PoC):** co 20 s kruk wybiera żywego Runnera-bohatera z najmniejszą aktualną liczbą HP i nakłada na niego klątwę na 5 s. Petów i mobów nie uwzględnia przy wyborze celu; remis HP rozstrzyga losowanie. Klątwa zmniejsza o 80% zasięg, z którego przeciwni Defenderzy i wieże mogą wykryć tego Runnera (pozostaje 20% normalnego zasięgu). Nie zmienia widoczności innych jednostek ani wiedzy własnej drużyny. [DO DOPRECYZOWANIA] Pierwsza aktywacja po rozpoczęciu obrony; zachowanie, gdy nie ma żywego Runnera-bohatera; możliwość nałożenia klątwy na Runnera już wykrytego; minimalny zasięg wykrywania po modyfikatorze. Ta zdolność nie wchodzi do PoC.

### Prompty portretowe: Przywoływacze

[PROPOZYCJA] Wspólny format: pionowy portret 4:5, kadr od piersi w górę, bez tekstu i znaków wodnych, jednolite ciemnofioletowe tło, stylizowana gra fantasy, ręcznie malowane światło i delikatna geometryczna faktura materiałów; wygląd zgodny z dotychczasowymi portretami Riftoryn. Każda postać ma odmienną pozę, sylwetkę i rysy. Pet jest rozpoznawalny, lecz nie zasłania twarzy. Ana i Bob mają osobny portret inspirowany zdjęciem referencyjnym.

**Pyrra — Ogień / lis Iskra**
> Stylized fantasy character selection portrait, adult female fire summoner, angular heart-shaped face, warm olive skin, short asymmetrical black hair with a copper streak, narrow amber eyes, an old notch in one eyebrow. In a lively three-quarter profile, she looks over her shoulder toward a small ember-red fox perched on her raised forearm. Lean silhouette, layered charcoal-and-rust robes, subtle glowing ember seams, small brass charms. The fox has distinct ears and a softly glowing tail. Firelight warms one side of her face. No large armor, no braid, no text. [Wspólny format].

**Eira — Lód / ryś Szron**
> Stylized fantasy character selection portrait, mature male ice summoner, long narrow face, pale weathered skin, close-cropped silver hair, one clouded eye and a prominent hooked nose, clean-shaven. Calm downward gaze, body angled sideways, one hand resting on the shoulder of a pale tuft-eared lynx beside him. Tall slender silhouette, deep blue layered coat with fur collar and translucent frost accents, small ice crystals on the lynx's fur. Quiet, introspective mood. No helmet, no scars shared with other heroes, no text. [Wspólny format].

**Volt — Błyskawice / sokół Grom**
> Stylized fantasy character selection portrait, young male lightning summoner, dark brown skin, round face, freckles across the nose, close shaved sides and compact high coiled hair, expressive wide eyes. Dynamic upward gaze as he lifts a gloved arm toward a lightning-marked falcon about to land; wings partially visible without covering his face. Asymmetric navy and ochre flight jacket, copper rings and small sparks, athletic and lively silhouette. No long hair, no heavy armor, no text. [Wspólny format].

**Rokan — Ziemia / pancernik Krzemień**
> Stylized fantasy character selection portrait, older male earth summoner, deep tan skin, square weathered face, broad nose, white bushy eyebrows, bald head and short grey goatee. Seated, leaning slightly forward, both hands gently holding a small armadillo with layered stone-like plates near his chest. Stocky silhouette, practical clay-brown work clothes, ochre scarf, chipped stone beads and simple leather straps. Patient expression. No glowing lava, no knight armor, no text. [Wspólny format].

**Lumen — Światło / jeleń Blask**
> Stylized fantasy character selection portrait, adult female light summoner, medium-brown skin, long oval face, prominent cheekbones, a small beauty mark under one eye, long dark hair gathered in a high sculptural bun with gold pins. Graceful side-facing pose, looking toward the viewer, one hand extended next to a young luminous deer whose small antlers frame the edge of the composition. Cream and muted gold ceremonial mantle, soft luminous details, slender silhouette. No halo obscuring the face, no braid, no text. [Wspólny format].

**Neris — Ciemność / kruk Cień**
> Stylized fantasy character selection portrait, middle-aged female darkness summoner, cool brown skin, diamond-shaped face, short tightly curled salt-and-pepper hair, deep-set grey eyes, distinctive long silver earring. Slightly hunched thoughtful pose, face in profile turned toward a black raven on her shoulder, the raven's violet eye visible. Long plum cloak with simple ink-like motifs, layered soft fabrics, subtle wisps of violet shadow, reserved and perceptive expression. No hood, no facial tattoo, no text. [Wspólny format].

### Wsparcie

#### 5. Liora — Wsparcie Natury: Tkaczka Odnowy
**HP:** 120. **Prędkość:** 4,8 m/s. Leczy grupę i naprawia obronę.

**Pasywka — Troska:** jej leczenie oraz naprawa są o 10% silniejsze; wartości poniżej już uwzględniają premię.

**Q w ataku — Pole życia:** umieszcza w punkcie oddalonym maksymalnie o 8 m pole o promieniu 4 m. Przez 5 s leczy żywych sojuszniczych Runnerów i moby w polu o 11 HP/s, również Liorę. **Odnowienie: 20 s.**

**Q w obronie — Naprawa:** wybrana żywa wieża natychmiast odzyskuje 66 HP i 44 punkty zwykłej osłony. **Odnowienie: 20 s.** Kontra: zmuszenie grupy do wyjścia z pola lub zniszczenie wieży przed naprawą.

#### 6. Elira — Wsparcie Światła: Latarniczka
**HP:** 120. **Prędkość:** 4,8 m/s. Ułatwia drużynie zdobywanie informacji.

**Pasywka — Jasny wzrok:** obiekty wykryte przez sojuszników pozostają aktualnie widoczne dla drużyny o 1 s dłużej po utracie wykrycia. W obronie dotyczy to wykrytych Runnerów i mobów; nie odkrywa obiektów samodzielnie.

**Q w ataku — Promień zwiadu:** wybiera widoczny punkt do 15 m; przez 4 s odkrywa wrogie wieże w promieniu 6 m od niego. Ściana blokuje wybór punktu, ale nie ogranicza wykrycia w samym obszarze. **Odnowienie: 20 s.**

**Q w obronie — Świetlny dozór:** przez 5 s wykrywa Runnerów i moby w promieniu 6 m od wybranego punktu własnej mapy. Informację otrzymuje tylko jej drużyna. **Odnowienie: 20 s.** Kontra: opuścić sektor i wykorzystać przerwę w wykrywaniu.

#### 7. Varek — Wsparcie Błyskawic: Przewodnik Impulsu
**HP:** 115. **Prędkość:** 4,9 m/s. Synchronizuje ruch grupy i wspiera automatykę wież.

**Pasywka — Resztkowy impuls:** po użyciu Q porusza się o 10% szybciej przez 2 s. W obronie przygotowanie do ręcznego pilotażu trwa u niego 1,8 s zamiast roboczych 2 s.

**Q w ataku — Wspólny impuls:** Runnerzy i sojusznicze moby w promieniu 5 m, włącznie z Varekiem, otrzymują 25% prędkości na 3 s. **Odnowienie: 20 s.**

**Q w obronie — Szybki zapłon:** wybrana wieża strzela automatycznie o 20% szybciej przez 5 s; nie zmienia szybkostrzelności ręcznej. **Odnowienie: 20 s.** Kontra: wymuszenie wczesnego użycia lub czasowe wyłączenie wieży.

#### 8. Niva — Wsparcie Lodu: Mroźna Opiekunka
**HP:** 125. **Prędkość:** 4,7 m/s. Usuwa wybrane efekty kontroli.

**Pasywka — Mrozoodporność:** działające na nią spowolnienia ruchu są o 20% słabsze; wyłączenia wieży, którą ręcznie pilotuje, są o 20% krótsze.

**Q w ataku — Chłodna głowa:** usuwa spowolnienie ruchu z wybranego żywego Runnera w promieniu 8 m i zapewnia mu odporność na spowolnienie przez 2 s. Niva może wskazać siebie. **Odnowienie: 18 s.**

**Q w obronie — Odmrożenie:** usuwa obecne czasowe wyłączenie jednej żywej sojuszniczej wieży. Po zdjęciu efektu przysługuje jej standardowa 2-sekundowa odporność na wyłączenie. **Odnowienie: 18 s.** Kontra: rozłożone w czasie źródła kontroli i zadawanie obrażeń wieży.

### Specjaliści

#### 9. Saila — Specjalistka Lodu: Lodowa Sabotażystka
**HP:** 105. **Prędkość:** 5,5 m/s. Czasowo unieszkodliwia obronę.

**Pasywka — Zimny krok:** spowolnienia ruchu na Saili są o 15% słabsze. W obronie ręcznie pilotowana przez nią wieża ma o 10% większy promień wykrywania.

**Q w ataku — Zamrożenie mechanizmu:** wyłącza jedną odkrytą wieżę na 3 s; zasięg 12 m, wymagany bezpośredni widok. **Odnowienie: 24 s.**

**Q w obronie — Lodowa strefa:** na 4 s umieszcza na trasie obszar o promieniu 3 m. Runnerzy i wrogie moby wewnątrz są spowolnieni o 30%; efekt znika przy wyjściu. Strefa nie blokuje przejścia. **Odnowienie: 24 s.** Kontra: ochrona wieży, oczyszczanie lub ominięcie pola.

#### 10. Zet — Specjalista Błyskawic: Burzowy Zwiadowca
**HP:** 100. **Prędkość:** 5,8 m/s. Szybko dociera do zagrożeń i je zakłóca.

**Pasywka — Iskra manewru:** po Q otrzymuje 15% prędkości na 2 s. W obronie ręcznie pilotowana przez niego wieża ma o 10% większy promień wykrywania.

**Q w ataku — Elektroimpuls:** wyłącza jedną odkrytą wieżę w zasięgu 8 m na 2 s; wymagany bezpośredni widok. Zdolność nie przemieszcza Zeta. **Odnowienie: 18 s.**

**Q w obronie — Impuls zwiadu:** wykrywa Runnerów i moby w promieniu 5 m od wybranego punktu mapy przez 3 s. **Odnowienie: 18 s.** Kontra: krótki zasięg i mała pula HP Zeta.

#### 11. Rian — Specjalista Ognia: Łamacz Żaru
**HP:** 110. **Prędkość:** 5,6 m/s. Zadaje obrażenia wybranej wieży.

**Pasywka — Żaroodporność:** otrzymuje o 15% mniej obrażeń Ognia; w obronie ta sama premia chroni ręcznie pilotowaną wieżę przed zdolnościami Ognia.

**Q w ataku — Skupiony żar:** zadaje 100 obrażeń jednej odkrytej wieży w zasięgu 12 m przy bezpośrednim widoku. Obrażenia najpierw trafiają w osłonę; wieża zostaje zniszczona po spadku HP do zera. **Odnowienie: 16 s.**

**Q w obronie — Ognisty znacznik:** oznacza wykrytego Runnera na 4 s. Sojusznicze automatyczne wieże preferują go jako cel, jeśli pozostaje w ich zwykłym zasięgu i polu widzenia. Nie ogranicza ręcznego celowania. **Odnowienie: 16 s.** Kontra: naprawa, osłony i ukrycie za ścianą.

#### 12. Morven — Specjalista Ciemności: Mistrz Zakłóceń
**HP:** 105. **Prędkość:** 5,6 m/s. Utrudnia automatyczne celowanie i zdobywanie informacji.

**Pasywka — Krok w cień:** po Q promień wykrywania Morvena przez wrogie wieże jest o 20% mniejszy przez 2 s. W obronie ręcznie pilotowana przez niego wieża zostaje odkryta przez Runnerów dopiero z odległości o 2 m mniejszej; nie ukrywa informacji już odkrytej.

**Q w ataku — Zaburzenie celowania:** przez 5 s automatyczne strzały jednej odkrytej wieży w zasięgu 12 m otrzymują losowe odchylenie do 8° od kierunku celu. Wymagany bezpośredni widok. Ręczny ogień nie jest zmieniany. **Odnowienie: 20 s.**

**Q w obronie — Zasłona obrony:** przez 5 s w obszarze o promieniu 5 m Runnerzy nie odkrywają dotąd nieznanych wież. Wcześniej odkryte wieże pozostają w pamięci i są widoczne przy normalnym bezpośrednim widoku. Zasłona nie blokuje zdolności wymierzonych w odkrytą wieżę. **Odnowienie: 20 s.** Kontra: ręczny pilotaż, wcześniejszy zwiad i przeczekanie.

### Rzeczy wymagające dalszych decyzji

- Draft: kolejność A–B–B–A–A–B–B–A–A–B i wspólna pula bez duplikatów są ustalone; pozostaje doprecyzować widoczność wyborów. W MVP nie ma banów; ewentualna nierówność wyboru jest akceptowana podczas alfy/bety.
- Konkretne bazowe typy wież, ich pociski i zasięgi, HP mobów, podstawowe sterowanie Runnera, długość faz, szczegółowy system wykrywania i indywidualne bonusy węzłów technologii.
- Drugi wariant każdej kombinacji jest planowany docelowo, ale jego umiejętności oraz to, czy ma być mechanicznie odrębny, nie zostały ustalone.
- Zmiana wieży ma roboczo 2 s przygotowania, zgodnie z wcześniejszym przykładem; bonus pilotażu swojej i cudzej wieży wymaga osobnej decyzji. Nie przypisujemy zdolnościom balansu potwierdzonego testami.

---

## Riftoryn — wieże i balans początkowy v0.1

**Status: propozycja liczb do pierwszych testów, nie zatwierdzony balans.** Siedem wież bazowych T1; każda ma dwie wykluczające się dla drużyny specjalizacje T2 i ulepszenie T3. Pierwsza wpłata w specjalizację blokuje konkurencyjną; pełny zakup automatycznie ulepsza wszystkie odpowiednie wieże drużyny. Każdy węzeł ma proponowany bonus ofensywny opisany w rozdziale technologii.

### Zasady wspólne — robocze

- Dystans w metrach, czas w sekundach symulacji. Wieża ma 300 HP i 100 osłony; osłona pochłania obrażenia przed HP. Przy 0 HP wieża jest wyłączona do następnego ataku na tę obronę, gdy wraca z pełnym HP i osłoną. Naprawa w trakcie ataku nie przywraca zniszczonej wieży.
- Automat wybiera najbliższego żywego Runnera lub wrogiego moba w zasięgu i bezpośredniej widoczności. Równy dystans rozstrzyga stały identyfikator celu; prowokacja może zmienić priorytet. Pierwszy strzał może nastąpić natychmiast po wykryciu, kolejne po podanym odstępie.
- Pocisk leci w kierunku pozycji celu z chwili strzału, bez śledzenia. Trafia pierwszy obiekt: wroga, tarczę lub ścianę. Na ścianie znika bez obrażeń obszarowych; tarczy zadaje obrażenia bez efektu obszarowego. Nie rani sojuszników. Maksymalna długość lotu równa się zasięgowi wieży.
- Trafienie obszarowe zadaje podane obrażenia każdemu wrogowi w promieniu, w tym trafionemu bezpośrednio, jeden raz. Ściana między punktem trafienia a jednostką blokuje obrażenia obszarowe.
- Obrażenia okresowe zadają podaną wartość co sekundę, pierwszy raz sekundę po trafieniu, ostatni na końcu trwania. Ten sam status nie sumuje obrażeń: działa najsilniejszy; równy odnawia czas, słabszy nie odnawia. Różne statusy współistnieją. Spowolnienia nie sumują się: działa najsilniejsze. Identyczne podatności nie sumują się: działa najwyższa.
- Ręczny operator wybiera kierunek strzału; obowiązują ten sam zasięg, odstęp i zasady pocisków. Roboczy mnożnik obrażeń bezpośrednich i okresowych: automat ×1,00; pilotowanie cudzej wieży ×1,05; własnej ×1,15. Mnożnik przypisywany przy strzale; nie wpływa na czas statusu ani siłę spowolnienia. W czasie roboczych 2 s przejęcia wieża strzela automatycznie.
- Reakcje żywiołów i odporności nie mają tu jeszcze ustalonych zasad dla Riftoryn; nie doliczamy ich domyślnie. Premie bohaterów i technologii stosujemy według ich odrębnych opisów.

### Wieże podstawowe T1

| Żywioł | Obrażenia | Odstęp | Zasięg | Prędkość pocisku | Efekt |
|---|---:|---:|---:|---:|---|
| Fire | 20 | 1,5 s | 16 m | 15 m/s | Obszar 2 m; podpalenie 3 obrażenia/s przez 3 s. |
| Cold | 12 | 1,4 s | 15 m | 16 m/s | Jeden cel; spowolnienie 15% przez 2 s. |
| Lightning | 13 | 1,0 s | 16 m | 22 m/s | Jeden przeskok na drugi cel w promieniu 4 m, z 50% obrażeń. |
| Nature | 10 | 1,3 s | 15 m | 14 m/s | Jeden cel; trucizna 4 obrażenia/s przez 3 s. |
| Earth | 32 | 2,0 s | 15 m | 11 m/s | Jeden cel; wolny, mocny pocisk. |
| Light | 15 | 1,2 s | 18 m | 25 m/s | Jeden cel; ujawnia go obronie na 2 s. |
| Darkness | 14 | 1,4 s | 16 m | 15 m/s | Jeden cel; podatność +10% otrzymywanych obrażeń przez 2 s. |

### Specjalizacje T2 i T3

Dwie wartości zapisane jako **T2 / T3** zastępują wartość podstawową; niewymienione parametry pozostają jak T1. Obie gałęzie są wzajemnie wykluczające się dla drużyny. Liczby mają dać umiarkowaną przewagę i różne zastosowania.

#### Fire
- **Magma:** jeden cel, obrażenia 23 / 26; czasowa plama lawy na trasie (parametry do ustalenia); podpalenie 3 / 4 obrażenia/s przez 3 s; podatność +12% / +15% przez 3 s. To ta sama kategoria podatności co Darkness — działa najwyższa.
- **Inferno:** obszar 2 / 2,3 m; obrażenia 20 / 22; podpalenie 5 / 6 obrażeń/s przez 3 s.

#### Cold
- **Glacier:** obszar 1,8 / 2 m; obrażenia 12 / 14; spowolnienie 20% / 25% przez 2 s.
- **Ice Lance:** jeden cel; obrażenia 15 / 18; spowolnienie 25% / 30% przez 2 s. Folder modelu `Shatter` nie jest nazwą tej specjalizacji dla gracza.

#### Lightning
- **Tempest:** obrażenia pierwszego celu 13 / 15; maksymalnie 2 / 3 dodatkowe cele; każdy kolejny przeskok zadaje połowę obrażeń poprzedniego.
- **Lancer:** jeden cel; obrażenia 21 / 24; zasięg 19 / 20 m; bez przeskoków. Jest to wybór roboczy dla Riftoryn — dokumenty źródłowe różnią się co do przeskoków Lancera.

Łańcuch szuka najbliższego kolejnego żywego celu do 4 m od poprzedniego, z bezpośrednią widocznością; każdy cel trafia najwyżej raz. Równy dystans rozstrzyga stały identyfikator.

#### Nature
- **Vines:** jeden cel; obrażenia 10 / 12; trucizna 4 / 5 obrażeń/s przez 3 s oraz spowolnienie 10% / 15% przez 2 s.
- **Thorns:** jeden cel; obrażenia 12 / 14; trucizna 6 / 7 obrażeń/s przez 3 s; zasięg 13 / 14 m.

#### Earth — nowa propozycja
- **Quake:** obrażenia 27 / 30 w obszarze o promieniu 2 / 2,3 m.
- **Boulder:** jeden cel; obrażenia 38 / 43; prędkość pocisku 10 m/s. Nie przebija ścian.

#### Light — nowa propozycja
- **Beacon:** jeden cel; obrażenia 15 / 17; ujawnia go obrońcom przez 4 / 5 s.
- **Ray:** jeden cel; obrażenia 19 / 22; zasięg 19 / 20 m, pocisk 30 m/s. To szybki pocisk, nie natychmiastowa wiązka.

#### Darkness — nowa propozycja
- **Curse:** jeden cel; obrażenia 14 / 16; podatność +15% / +18% przez 3 s.
- **Void:** jeden cel; obrażenia 20 / 23; podatność +10% przez 2 s.

### Pierwsze testy i kwestie otwarte

Bohater o 120 HP bez leczenia i osłon wytrzyma sześć bezpośrednich trafień podstawowej Fire po 20 obrażeń, nie licząc podpalenia. To jedynie punkt odniesienia: realne tempo walki zależy od mapy, unikania, liczby wież, mobów i zdolności.

Do testów: czas przeżycia, liczba graczy na mecie, różnica automat/pilot, skutki koncentracji wież i możliwości kontr. Otwarte: dokładne reakcje i odporności, bonus ofensywny każdego węzła, skala mapy i hitboksy, liczba wież, długość ataku, statystyki mobów, widoczność wież z trasy oraz ograniczenia efektów. Bez tych danych balans nie jest zweryfikowany.

---

## Riftoryn — drzewko technologii v0.2

**Status: pełna propozycja do testów, bez określania reakcji żywiołów.** Wszystkie wartości do balansu. 400 g do rozdysponowania na gracza oraz dodatkowe 100 g automatycznej wpłaty do rdzenia żywiołu wybranego bohatera. W drużynie 5v5 daje to 2000 g dostępnej gotówki i 500 g wpłaconych do rdzeni. Bazowe wieże są dostępne bez zakupu węzła.

Koszty w kolejności: rdzeń **800 g**, jedna z dwóch wykluczających się specjalizacji **1400 g**, wspólne ulepszenie T3 **2200 g**. Każdy węzeł wymaga ukończenia poprzedniego; pierwsza wpłata w specjalizację blokuje drugą. Wpłaty mogą być częściowe. Wszystkie premie działają po pełnym sfinansowaniu. T3 zastępuje, a nie sumuje, premie T2. Docelowo przewidziano 21 par reakcji; PoC obejmie ograniczony zestaw wybrany do osobnego opracowania.

Węzły bazowe wpływają na wszystkich Runnerów, niezależnie od ich żywiołu. Odporności i podatności odnoszą się do typu otrzymywanych obrażeń, a nie do żywiołu wieży czy bohatera. Mnożnik silny/słaby i efekty technologii są oddzielne: propozycja do testów — mnożyć je, np. 100 × 1,20 × 0,90 = 108; premie z różnych węzłów mnożą się. Dotyczy bezpośrednich i okresowych obrażeń danego żywiołu. Szczegóły tarcz i reakcji nadal wymagają decyzji.

Premie ofensywne T2/T3 dotyczą wszystkich Runnerów lub wszystkich sojuszniczych mobów, jak określono w węźle, i nie wymagają konkretnego bohatera. Wieże T2/T3 używają wartości z dokumentu balansu; premia rdzenia +10% obrażeń działa dodatkowo na bezpośrednie i okresowe obrażenia danego typu wieży. Nie zmienia siły statusów kontroli.

Łączna wartość początkowego finansowania nadal wynosi 2500 g, ale 500 g jest już przypisane do żywiołów wybranych bohaterów. Liczba ukończonych węzłów zależy od składu i rozdziału pozostałych 2000 g. Faza początkowa trwa orientacyjnie 50+10 s, kolejne 20+10 s; pozostała waluta jest automatycznie inwestowana.

### Ogień

- **Rdzeń — 800 g:** obrona: Wieże Ognia: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Natury, +10% obrażeń Ciemności.
- **Magma T2 — 1400 g:** obrona: Magma T2: 23 obrażenia, podatność celu +12% przez 3 s; podpalenie 3/s przez 3 s. Plama lawy na trasie: czas, obrażenia i kumulacja do ustalenia. Atak: Runnerzy: +5% obrażeń zdolności specjalnych zadawanych wieżom.
- **Inferno T2 — 1400 g:** obrona: Inferno T2: 20 obrażeń w promieniu 2 m; podpalenie 5/s przez 3 s. Atak: Runnerzy: +5% prędkości ruchu przez 2 s po użyciu zdolności, raz na 10 s.
- **Wspólny T3 — 2200 g:** obrona: Magma: 26 obrażeń, 4/s podpalenia, podatność 15%; Inferno: 22 obrażenia, obszar 2,3 m, 6/s podpalenia. Atak: Premia ofensywna wybranej specjalizacji wzrasta dwukrotnie: 5% → 10%.

### Lód

- **Rdzeń — 800 g:** obrona: Wieże Lodu: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Światła, +10% obrażeń Błyskawic.
- **Glacier T2 — 1400 g:** obrona: Glacier T2: 12 obrażeń w promieniu 1,8 m, spowolnienie 20% przez 2 s. Atak: Runnerzy: spowolnienia ruchu są o 10% słabsze.
- **Ice Lance T2 — 1400 g:** obrona: Ice Lance T2: 15 obrażeń jednemu celowi, spowolnienie 25% przez 2 s. Atak: Runnerzy: czas wyłączenia wież wywołanego ich zdolnościami +10%.
- **Wspólny T3 — 2200 g:** obrona: Glacier: 14 obrażeń, obszar 2 m, spowolnienie 25%; Ice Lance: 18 obrażeń, spowolnienie 30%. Atak: Premia wybranej specjalizacji wzrasta dwukrotnie: 10% → 20%.

### Błyskawice

- **Rdzeń — 800 g:** obrona: Wieże Błyskawic: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Lodu, +10% obrażeń Ziemi.
- **Tempest T2 — 1400 g:** obrona: Tempest T2: 13 obrażeń i do dwóch dodatkowych celów łańcucha; każde kolejne trafienie zadaje połowę poprzedniego. Atak: Runnerzy: +5% prędkości ruchu przez 2 s po użyciu zdolności, raz na 10 s.
- **Lancer T2 — 1400 g:** obrona: Lancer T2: 21 obrażeń jednemu celowi, zasięg 19 m, bez przeskoków. Atak: Runnerzy: zasięg zdolności specjalnych wymierzonych we wrogie wieże +1 m.
- **Wspólny T3 — 2200 g:** obrona: Tempest: 15 obrażeń i do trzech przeskoków; Lancer: 24 obrażenia, zasięg 20 m. Atak: Premia wybranej specjalizacji wzrasta dwukrotnie: 5% → 10% albo 1 m → 2 m.

### Natura

- **Rdzeń — 800 g:** obrona: Wieże Natury: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Ziemi, +10% obrażeń Ognia.
- **Vines T2 — 1400 g:** obrona: Vines T2: 10 obrażeń, trucizna 4/s przez 3 s i spowolnienie 10% przez 2 s. Atak: Runnerzy: otrzymywane leczenie +10%.
- **Thorns T2 — 1400 g:** obrona: Thorns T2: 12 obrażeń, trucizna 6/s przez 3 s; zasięg 13 m. Atak: Moby: maksymalne HP +10%.
- **Wspólny T3 — 2200 g:** obrona: Vines: 12 obrażeń, trucizna 5/s i spowolnienie 15%; Thorns: 14 obrażeń, trucizna 7/s, zasięg 14 m. Atak: Premia wybranej specjalizacji wzrasta dwukrotnie: 10% → 20%.

### Ziemia

- **Rdzeń — 800 g:** obrona: Wieże Ziemi: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Błyskawic, +10% obrażeń Światła.
- **Quake T2 — 1400 g:** obrona: Quake T2: 27 obrażeń w promieniu 2 m. Atak: Runnerzy: wytrzymałość osłon tworzonych zdolnościami +10%.
- **Boulder T2 — 1400 g:** obrona: Boulder T2: 38 obrażeń jednemu celowi; pocisk 10 m/s. Atak: Moby: maksymalne HP +10%.
- **Wspólny T3 — 2200 g:** obrona: Quake: 30 obrażeń w promieniu 2,3 m; Boulder: 43 obrażenia jednemu celowi. Atak: Premia wybranej specjalizacji wzrasta dwukrotnie: 10% → 20%.

### Światło

- **Rdzeń — 800 g:** obrona: Wieże Światła: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Ciemności, +10% obrażeń Lodu.
- **Beacon T2 — 1400 g:** obrona: Beacon T2: 15 obrażeń jednemu celowi i ujawnienie go obronie na 4 s. Atak: Runnerzy: czas ujawnienia wież przez zdolności +1 s.
- **Ray T2 — 1400 g:** obrona: Ray T2: 19 obrażeń jednemu celowi, zasięg 19 m; pocisk 30 m/s. Atak: Runnerzy: zasięg zdolności specjalnych wymierzonych we wrogie wieże +1 m.
- **Wspólny T3 — 2200 g:** obrona: Beacon: 17 obrażeń i ujawnienie na 5 s; Ray: 22 obrażenia, zasięg 20 m. Atak: Premia wybranej specjalizacji wzrasta dwukrotnie: 1 s → 2 s albo 1 m → 2 m.

### Ciemność

- **Rdzeń — 800 g:** obrona: Wieże Ciemności: +10% obrażeń. Atak: Runnerzy: −10% otrzymywanych obrażeń Ognia, +10% obrażeń Natury.
- **Curse T2 — 1400 g:** obrona: Curse T2: 14 obrażeń, podatność celu +15% przez 3 s. Atak: Runnerzy: czas wyłączenia wież wywołanego ich zdolnościami +10%.
- **Void T2 — 1400 g:** obrona: Void T2: 20 obrażeń, podatność celu +10% przez 2 s. Atak: Runnerzy: +5% obrażeń zdolności specjalnych zadawanych wieżom.
- **Wspólny T3 — 2200 g:** obrona: Curse: 16 obrażeń i podatność 18%; Void: 23 obrażenia i podatność 10%. Atak: Premia wybranej specjalizacji wzrasta dwukrotnie: 10% → 20% albo 5% → 10%.

### Do ustalenia przed pełną implementacją

- Dokładne reakcje między żywiołami i ich koszt — do zaprojektowania przez użytkownika.
- Dochód po parze rund i wpływ remisów na liczbę punktów: bez nich nie da się potwierdzić tempa rozwoju ani celu około 20% całkowitego kosztu drzewka.
- Co robi autoinwestowanie, gdy wszystkie dostępne opcje są pełne lub węzły reakcji nie mają kosztu; zasady jednoczesnych wpłat serwerowych.
- Kolejność liczona względem osłon, obszaru, odporności bohatera oraz innych statusów; szczegóły i balans wymagają testów.

### Doprecyzowanie rdzenia i składu

Początkowy węzeł za 800 g to sama ikona żywiołu na kole, nie dodatkowy sąsiedni węzeł. 100 g za każdego bohatera trafia do odpowiadającego mu rdzenia przed inwestycją; wpłaty tych samych żywiołów sumują się. Efekty wchodzą w życie dopiero po zebraniu pełnych 800 g. Skład wpływa na początkowe wpłaty, ale nie ogranicza wyboru pozostałych żywiołów ani wspólnych bonusów.

---

## RIFTORYN — historia świata i oś czasu

**Skrót fabularny.** Jedynym źródłem kanonu jest [historia świata](./world_history.md). Poniższy opis i oś czasu należy dostosowywać do niej. Lata liczymy od podpisania Traktatu Równych Ścieżek: **PT — przed traktatem; ET — era traktatu**.

### Opowieść o świecie

Kontynent **Riftoryn** nie został stworzony przez siedem żywiołów jako siedem odrębnych krain. Żywioły przenikają cały kontynent; ich źródło pozostaje nieznane, a siedem nurtów to główne tradycje historycznie utrwalone w traktatowych zapisach. Najstarsze znane szkoły rozwinęły się przy siedmiu dawnych ośrodkach: Ogień w Vareth, Lód w Nivoren, Błyskawice w Zerath, Natura w Verdan, Ziemia w Kharum, Światło w Aurel i Ciemność w Vesper. To tradycje nauki i praktyki, nie dowód, że żywioły pochodzą z tych miejsc lub są do nich przypisane. Mieszkańcy każdej krainy mogą uczyć się różnych nurtów.

Po osiedleniu ludzi odkryto **splotyt** — minerał przyjmujący, przechowujący i przewodzący energię magiczną. Najstarszy znany zachowany opis ładowania go Błyskawicami pochodzi z Zerath; nie wiadomo, kto pierwszy wykorzystał tę właściwość. Zerath spopularyzowało jej użycie, a później rozwijano zastosowania pozostałych żywiołów. Splotyt występuje w różnych krainach, najłatwiej dostępny jest w Zerath. Kryształ nie tworzy energii sam z siebie. Zasilane nim urządzenia ogrzewają domy, pomagają w rolnictwie i pozwalają wieżom działać bez ciągłej obecności maga. Większy kryształ porównywalnej jakości może zwykle przechować więcej energii. Naładowane kryształy, zwłaszcza duże lub skupione razem, mogą przyciągać bestie, a rzadko mityczne potwory; Vesper rozwinęło maskowanie ich promieniowania, zmniejszając to ryzyko. Przyciąganie nie oznacza niestabilności: przy prawidłowym użytkowaniu splotyt pozostaje stabilny. Szczegóły i ograniczenia opisuje [historia świata](./world_history.md#splotyt-i-zależność-krain).

Przez wiele pokoleń działała **Umowa Siedmiu Szlaków** — dawne, wielostronne porozumienie handlowe między niezależnymi miastami. Ustalała zasady dostępu do dróg, kopalń i urządzeń, a jej przedstawiciele negocjowali spory. Nie była instytucją rządzącą krainami ani późniejszą Radą Siedmiu Szlaków. Nie był to świat pozbawiony konfliktów, ale koszt wojny przewyższał korzyści płynące z handlu.

Badacze Zerath pierwsi połączyli oddalone kryształy w **sieci splotu**, którymi można przesyłać wcześniej zgromadzoną energię. Sieci łączą miasta i główne szlaki, ograniczając przewóz dużych naładowanych zapasów, ale wymagają ochrony i utrzymania. Pojedyncza wieża mogła automatycznie wykrywać i atakować zagrożenie. W sieci grupa wież dzieliła zasilanie i korzystała ze wspólnych badań. Obrońca mógł również nawiązać z jedną z nich bezpośrednią więź i przejąć jej sterowanie. Własną konstrukcję obsługiwał sprawniej, bo znał jej zachowanie i sposób dostrojenia.

Magowie mogą ładować kryształy, lecz w dużej skali jest to mało wydajne. Zerath wykorzystuje m.in. wyładowania atmosferyczne, Vareth — ciepło wulkaniczne, a inne krainy mają własne metody. Kryształy można ładować wielokrotnie, choć awarie mogą je uszkodzić; kolejne urządzenia i stacje zwiększają popyt. Przesył dotyczy energii, nie samych kryształów; zużywa energię, a większy dystans zwykle zwiększa straty. Odpowiednio gęsta sieć może je ograniczyć, ale jej budowa i utrzymanie wymagają kryształów i pieniędzy. Sąsiednie stacje mogą współpracować, daleki przesył służy głównie sytuacjom awaryjnym. W finansowanym przez Aurel eksperymencie w 8 PT kryształ w Zerath oddał cały zapas, a podobny kryształ w Aurel osiągnął około 10% pojemności; oba były dostrojone do Błyskawic. To wynik tej próby, nie uniwersalna stawka ani wyjaśnienie Nocy. Niedawny przesył między różnie dostrojonymi kryształami wymaga konwersji, powoduje dodatkowe straty i zależy od kierunku relacji żywiołów. Bezpieczeństwo konwersji pozostaje sporne; zasady te nie zmieniają wartości obrażeń w grze.

[USTALONE — FABUŁA] Konwersja zachowuje około 95% energii docierającej do konwersji przy głównej przewadze wysyłanego żywiołu nad odbiorczym, 90% przy zwykłej przewadze, 80% przy neutralności, 70% przy zwykłej słabości i 65% przy głównej słabości. Straty sieci i obsługi są dodatkowe. Kryształy tego samego żywiołu nie wymagają konwersji; na krótkim, sprawnym połączeniu przesył może być bliski 100%. To zasady fabularne, oddzielne od mnożników obrażeń.

Sieci miały chronić miasta. Gdy zaczęło brakować łatwo dostępnego splotytu, stały się narzędziem nacisku. Kraj, który kontrolował kopalnię, mógł zasilić więcej wież. Kraj, który kontrolował drogę, mógł odciąć sąsiada od kryształów. Każdy kolejny mur wzmacniał potrzebę postawienia następnego.

Napięcie doprowadziło do **Wojny Siedmiu Bram**. Krainy walczyły o złoża i drogi, a dowódcy wymyślali coraz trudniejsze do przejścia fortyfikacje. W odpowiedzi powstawały drużyny przełamujące obrony: wojownicy niosący osłony, zwiadowcy, uzdrowiciele, sabotażyści i przywołane konstrukty. Nie walczono już wyłącznie o zwycięstwo w jednej bitwie — walczono też o możliwość dowiezienia żywności i uruchomienia miejskich urządzeń.

Kulminacją była **Noc Zgasłych Wież**, która wydarzyła się w **3 PT**. W angielskich materiałach kalendarz zapisuje tę datę jako **3 BT** (*Before the Treaty*); lata po podpisaniu Traktatu oznacza się **AT** (*After the Treaty*), a sam rok podpisania to 0 AT. Val Meris, niezależne miasto-węzeł szlaków, oblegały oddziały twardogłowych dowódców z Vareth i Kharum, chcące przejąć węzeł i zapasy splotytu przed rywalami. Broniła go miejscowa straż i ochotnicze oddziały z Nivoren, Verdan i Zerath, walczące o utrzymanie szlaku i ewakuację cywilów. Nie były to całe krainy stojące w dwóch jednolitych obozach; Aurel i Vesper pozostały podzielone, a ich ratownicy i obserwatorzy nie tworzyli wspólnej armii. Podczas oblężenia połączone sieci przestały działać prawidłowo. Wieże gasły i uruchamiały się w nieprzewidziany sposób, drogi ewakuacyjne zostały odcięte, a cywile utknęli pomiędzy fortyfikacjami. W ciągu dwóch tygodni przed Nocą i około tygodnia po niej poważne awarie dotknęły także pozostałe krainy. Zdarzenia są bliskie sobie w czasie, ale wojna i uszkodzona łączność nie pozwalają ustalić ich kolejności ani potwierdzić wspólnej przyczyny. Zachowane świadectwa z Val Meris są sprzeczne: komisja Vareth wskazuje na możliwość jednoczesnego przeciążenia sieci przez obie walczące strony; ocalała z dzielnicy mostowej twierdzi, że pierwsza zgasła wieża po stronie, która nie prowadziła wtedy ostrzału; niepełny zapis przekaźnika wskazuje zaś na przerwę w zasilaniu poprzedzającą oba odnotowane przeciążenia. Żadne z tych świadectw nie jest rozstrzygające, a liczba ofiar także pozostaje niepewna. Po tej nocy nawet dowódcy domagający się dalszej walki musieli uznać, że nikt nie kontroluje już skutków wojny.

Po rozejmie siedem niezależnych komisji cywilnych bada Noc i regionalne awarie. W 0 ET archiwum Val Meris przyjmuje kopie ich [raportów](./world_history.md#siedem-raportów-komisji-o-nocy). Wnioski obejmują przeciążenie, wcześniejszy zanik zasilania, dwa możliwe rodzaje sabotażu, nieoczekiwane zachowanie partii splotytu, magiczny rezonans i kilka niezależnych awarii. Komisje różnie oceniają te same ślady, a wspólna przyczyna pozostaje niepotwierdzona.

Rozejm nie rozwiązał jednak najtrudniejszego problemu: kryształów i bezpiecznych szlaków nadal nie starczało wszystkim. Trzeba było znaleźć sposób podejmowania decyzji, który nie prowadziłby automatycznie do kolejnej wojny.

Pierwszy krok wykonali dowódcy dwóch zwaśnionych miast. Zgodzili się rozstrzygnąć ograniczony spór o konwój w **Pojedynku Bram**. Każda strona otrzymała taki sam układ murów, zbudowała własną obronę i wysłała drużynę przez labirynt przeciwnika. Dopiero kiedy obie drużyny zagrały obie role, porównano wyniki. Próba nie usunęła nieufności, ale pozwoliła dostarczyć zaopatrzenie bez bitwy. Ten precedens stał się podstawą traktatowego systemu Prób.

W **0 ET** przedstawiciele siedmiu krain podpisali **Traktat Równych Ścieżek**. Traktat powołał **Radę Siedmiu Szlaków** — organ wspólnego nadzoru nad traktatem, rejestrem zasobów i przydziałami — oraz niezależny od pojedynczej krainy **Urząd Prób**, który przygotowuje areny, publikuje zasady i zatwierdza wyniki. Rada nie jest dawną Umową Siedmiu Szlaków: Umowa była porozumieniem handlowym, a Rada instytucją ustanowioną przez Traktat.

Traktat ustanowił **Igrzyska Szlaków** jako system wiążących Prób. Zwykłe Próby zwołuje się w miarę potrzeb, aby rozstrzygać określone impasy o wspólną nadwyżkę lub kolejność działań po negocjacjach. Wielkie Igrzyska w 12 ET są pierwszym wydarzeniem tej rangi. Wybiorą model zarządzania na mocy osobnego Porozumienia Val Meris. Żadna Próba nie ustala winy ani przyczyny Nocy; fakty badają niezależni arbitrzy i inspektorzy. Stawka oraz warunki są jawne, drużyny mogą być mieszane, a podział punktów uzgadnia się przed startem.

Krainy wnoszą ograniczony udział z wybranych źródeł lub opłat za szlaki. Otrzymują w zamian ochronę tras, pomoc przy naprawach i uzgodnione minimum dostaw kryzysowych. Rezerwy podstawowych usług i ratownictwa są chronione; dopiero nadwyżkę można rozdzielać w Próbach. Rada prowadzi rejestr i wykonuje wyniki, nie zmieniając stawki po rozgrywce. Zespoły audytowe obejmują inspektorów spoza stron sporu. Arbitrów losuje się z jawnej listy zatwierdzonej przez krainy, bez udziału delegatów stron; osobny skład rozpatruje zarzuty wobec Urzędu i może unieważnić wynik za naruszenie zasad. Odmowa wykonania wyniku grozi utratą przyszłej nadwyżki lub preferencji we wspólnych usługach, ale nie podstawowych dostaw dla ludności.

Pierwsze traktatowe Próby w ramach Igrzysk nadal przypominały dawne zmagania krain: strony sporów wysyłały championów wychowanych we własnej tradycji żywiołu. Zachowane kroniki opisują serię pojedynków Ognia z Lodem, w której Ogień zwyciężał niemal za każdym razem; inne zestawienia również pokazywały, jak skład i przygotowanie wpływały na wynik. Była to historyczna tendencja w ówczesnym formacie i doborze drużyn, nie powszechne prawo magii ani gwarancja wyniku pojedynczego starcia.

Z czasem reprezentanci zaczęli dobierać składy według potrzeb konkretnej Próby i uzupełniających się umiejętności, tworząc drużyny złożone z osób z różnych krain. Przed startem rejestrują, jak podzielą punkty; przynależność do drużyny nie zmienia ich pochodzenia ani nie tworzy trwałego sojuszu między krainami. Bogatsze krainy często zatrudniają najemników, co daje im dostęp do doświadczonych zawodników, ale wywołuje spory o kupioną przewagę, lojalność i udział w punktach. Kontrakty najemnicze zgłasza się przed daną Próbą lub cyklem Prób; określają wynagrodzenie, reprezentowaną krainę i uzgodniony podział punktów. Miasta, gildie i stowarzyszenia mogą wspierać nabór, lecz nie zmieniają zasad reprezentacji ani wyniku.

Zwykłe Próby traktatowe wiążą strony w zakresie ogłoszonej stawki; wyjątkowe Wielkie Igrzyska 12 ET wybierają uzgodniony wcześniej model zarządzania na podstawie Porozumienia Val Meris. Już przed 12 ET w każdej krainie regularnie odbywają się zawody sportowe. Mniejsze ligi mają drużyny, kibiców i sponsorów, a widzowie śledzą wyniki i zawierają zakłady. Zawodnicy sprawdzają umiejętności i zdobywają nagrody. Wynik zawodów sportowych sam z siebie nie ma mocy traktatowej.

Nie wszyscy wierzą jednak, że pokój jest trwały. W rejestrach Urzędu zaczynają pojawiać się rozbieżności dotyczące starych złóż. Niektóre sieci wież reagują na kryształy inaczej, niż wynika z ich dokumentacji. Rada podejrzewa fałszerstwa, lecz nie wie jeszcze, czy stoją za nimi pojedynczy handlarze, przedstawiciele krain, czy zwykłe błędy po latach wojny. Tajemnica istnieje jako punkt wyjścia do przyszłych historii — nie przesądza o istnieniu sekretnej organizacji ani nadnaturalnego wroga.

Po Nocy wspólne inspekcje wybranych kopalń i sieci wprowadzono z powodu braków danych i zagrożeń. Traktat utrwalił awaryjne ograniczenia z okresowym przeglądem, nie przejmując kopalń. W 12 ET zwolennicy samodzielności chcą przekazać władzom krain kontrolę nad wydobyciem i przydziałami; zwolennicy wspólnego nadzoru chcą zachować uprawnienia Rady, obawiając się nowej katastrofy. Po zatrzymaniu spornego transportu i mobilizacji straży uprawnieni przedstawiciele podpisują Porozumienie Val Meris: Wielkie Igrzyska wskażą jeden z dwóch jawnych wariantów. Oba chronią podstawowe dostawy, ratownictwo, rejestry i niezależne śledztwo. Wynik nie przesądza przyczyny Nocy; audyt rejestrów trwa osobno. Pełna opowieść: [historia świata](./world_history.md).

Pamięć o wojnie sprawia, że wielu mieszkańców sprzeciwia się nowym walkom i oczekuje jawnych decyzji oraz dotrzymywania słowa. Strony nie potrafią wybrać jednego wariantu w negocjacjach, a część obawia się zakwestionowania zwykłego głosowania. Uzgadniają więc publiczne zasady Igrzysk; uczciwie przegrana strona może uznać wynik, nadal go krytykując i korzystając z audytu oraz odwołania proceduralnego. Wynik nie dowodzi, który wariant jest bezpieczniejszy ani kto miał rację co do Nocy.

Wielkie Igrzyska obejmują wiele drużyn i kolejnych meczów. Ich punkty są rozliczane dla reprezentowanych stron i dwóch wariantów zgodnie z wcześniej uzgodnionymi zasadami. Pojedynczy mecz może zakończyć się remisem; przy równej klasyfikacji po zaplanowanej serii rozgrywa się kolejne mecze, aż jeden wariant uzyska przewagę. Dopiero prawomocny wynik całej serii wskazuje wybrany model.

Protokół Val Meris określa zakres obu wariantów i gwarancje ich wdrożenia. Lokalne kopalnie nie zmieniają właściciela, a istniejących wkładów, przydziałów i umów nie można anulować wstecznie. Przy wspólnym nadzorze Rada ustala, kto, ile i do czego może używać splotytu w krainach; w wariancie krain każda z nich sama decyduje o zasadach wydobycia, przydziału i wykorzystania swoich kryształów oraz odpowiada za lokalny nadzór. Przez co najmniej rok nie można jednostronnie cofnąć wybranego wariantu ani wymusić kolejnych Wielkich Igrzysk; krytyka, odwołania od decyzji i działania ratunkowe pozostają możliwe. Pełne zasady i gwarancje opisuje [historia świata](./world_history.md).

### Codzienność siedmiu krain

To przykłady miejsc i zwyczajów, a nie opis wszystkich mieszkańców. Pełniejsze obrazy codzienności i krótkie sceny znajdują się w [historii świata](./world_history.md#siedem-krain-po-wojnie).

- **Vareth:** przy warsztatowych dziedzińcach sąsiedzi wymieniają naprawy, materiały i umiejętności; w jadłodajniach czekają na dostawy i rozmawiają o zawodach.
- **Nivoren:** schroniska przy przełęczach dają podróżnym miejsce na odpoczynek i wymianę wieści; część mieszkańców zostawia zapasy dla kolejnych wędrowców.
- **Zerath:** place przy przekaźnikach i pobliskie warsztaty łączą operatorów, posłańców i mieszkańców; ważne wiadomości bywają sprawdzane niezależnym kanałem.
- **Verdan:** targi i stoły przy ogrodach są miejscem wymiany jedzenia, sadzonek i pomocy w naprawach; rytm części dolin wyznaczają prace sezonowe.
- **Kharum:** place przy wyrobiskach i warsztatach gromadzą pracowników oraz ich rodziny; ekipy sprawdzają narzędzia i dzielą się uwagami o bezpieczeństwie.
- **Aurel:** czytelnie, nabrzeżne targi i ławy przy latarni służą spotkaniom oraz porównywaniu map z opowieściami podróżnych.
- **Vesper:** w gospodach i przy magazynach załogi wymieniają ostrzeżenia i umawiają przejazdy, ważąc bezpieczeństwo tras i poufność informacji.

---

### Oś czasu

#### Około 180–120 PT — epoka miast i szlaków

- **180 PT:** Siedem dużych ośrodków handlowych tworzy pierwsze stałe porozumienie o bezpiecznych drogach. Najstarsze zachowane zapisy łączą te ośrodki z rozwojem siedmiu tradycji magicznych; nie dowodzą, że żywioły powstały w tych miastach. Mieszkańcy swobodnie uczą się i łączą żywioły.
- **154 PT:** Najstarszy znany zachowany zapis z Zerath opisuje ładowanie splotytu magią Błyskawic i zasilanie mechanizmu. Kto pierwszy użył tej właściwości, pozostaje nieznane; Zerath spopularyzowało technikę. Później rozwijane są zastosowania pozostałych żywiołów.
- **128 PT:** Podpisano Umowę Siedmiu Szlaków, określającą zasady korzystania z głównych dróg i części złóż. Traktat nie tworzy jednego państwa.

#### 119–42 PT — rozwój sieci

- **109 PT:** Powstaje pierwsza udokumentowana wieża korzystająca z kryształu splotytu. Chroni stację na szlaku handlowym.
- **91 PT:** Badacze Zerath łączą oddalone kryształy, umożliwiając przesył zgromadzonej energii; pierwsza udokumentowana sieć obejmuje kilka wież. Późniejsze sieci łączą miasta i ważne szlaki.
- **76 PT:** Operatorzy odkrywają możliwość ręcznego przejmowania wież. Umiejętności i znajomość własnych urządzeń stają się ważne w ich obsłudze.
- **54 PT:** Zespoły badaczy zaczynają systematycznie opisywać reakcje żywiołów. Konkretne reakcje pozostają do zaprojektowania przez twórcę gry.
- **43 PT:** Pierwsze konstrukty pomocnicze zasilane splotytem zaczynają pracować przy transporcie i ratownictwie. Później powstają również odmiany stosowane w Próbach.

#### 41–2 PT — kryzys, wojna i rozejm

- **41 PT:** Spada wydobycie łatwo dostępnych złóż. Krainy wprowadzają ograniczenia eksportu, przez co rośnie cena splotytu i koszty utrzymania infrastruktury.
- **35 PT:** Spór o konwoje na Szlaku Popiołów prowadzi do pierwszych starć między oddziałami granicznymi. Negocjacje w ramach Umowy Siedmiu Szlaków nie przynoszą trwałego rozwiązania.
- **30 PT:** Rozpoczyna się Wojna Siedmiu Bram. Nazwa odnosi się do siedmiu najważniejszych przejść handlowych, nie do siedmiu pojedynczych bitew.
- **26–18 PT:** Powstają rozbudowane linie wież i oddziały wyspecjalizowane w ich przełamywaniu. Wojna stopniowo rozprzestrzenia się na ziemie, które początkowo nie były stroną sporu.
- **8 PT:** Uniwersytet w Aurel finansuje laboratoryjny przesył z Zerath do Aurelu między dwoma podobnymi kryształami Błyskawic; odbiorczy osiąga około 10% pojemności. Próba nie jest powiązana z późniejszą Nocą.
- **3 PT:** Podczas Nocy Zgasłych Wież awaria sieci w Val Meris odcina drogi ewakuacyjne. Miasto oblegają oddziały twardogłowych dowódców z Vareth i Kharum; bronią go miejscowa straż i ochotnicy z Nivoren, Verdan i Zerath. Spór dotyczy kontroli nad węzłem i zapasami splotytu; obrońcy chcą utrzymać szlak i ewakuować cywilów. Są to obozy złożone z lokalnych oddziałów, nie całe krainy. W Nivoren urządzenia magazynowe zasilane kryształem eksplodują na oczach małej Saily; część zapasów ulega zniszczeniu, a przełęcz zostaje zamknięta. W ciągu dwóch tygodni przed Nocą i około tygodnia po niej poważne awarie występują także w pozostałych krainach. Ich kolejność i wspólna przyczyna pozostają nieustalone.
- **2 PT:** Podpisano kruchy rozejm. Utrzymano część fortyfikacji, a niedobór zasobów pozostaje nierozwiązany.

#### 1 PT – 0 ET — droga do traktatu

- **1 PT:** Spór o konwój do dwóch odciętych miast grozi zerwaniem rozejmu. Zamiast nowego oblężenia strony organizują pierwszy Pojedynek Bram; obie drużyny otrzymują szansę ataku i obrony.
- **1 PT:** Rzemieślnicy i medycy rozwijają zabezpieczenia aren. Kilka miast testuje dobrowolne sezonowe Próby z mieszanymi składami i dzielonymi punktami.
- **Miesiące przed 0 ET:** Negocjatorzy ustalają ograniczone stawki Prób dla impasów o wspólne zasoby i kolejność działań. Fakty i odpowiedzialność badają arbitrzy; podstawowe rezerwy są chronione.
- **0 ET:** Siedem krain podpisuje Traktat i powołuje Radę oraz Urząd Prób. Krainy uzgadniają wkłady i wzajemne korzyści; awaryjne ograniczenia wydobycia mają podlegać przeglądom.

#### 1–12 ET — pierwsze Próby i początek gry

- **1 ET:** W pierwszych traktatowych Próbach strony wskazują championów. Kroniki odnotowują serię zwycięstw Ognia nad Lodem w ówczesnym formacie; nie ustanawia to uniwersalnej przewagi żywiołu. Uczestnicy zaczynają negocjować mieszane składy i podział punktów.
- **2 ET:** Urząd ujednolica bliźniacze układy aren; w danym meczu obie strony korzystają z identycznej mapy.
- **4 ET:** Urząd ujednolica pełną parę ataków w formatach labiryntowych, rozwijając zasadę znaną z pierwszego Pojedynku Bram.
- **5 ET:** Powstaje wspólna szkoła operatorów wież i Runnerów. Pochodzenie ani żywioł nie ograniczają wyboru technologii drużyny. Najem specjalistów staje się powszechnie akceptowany; bogatsze krainy często zatrudniają doświadczonych zawodników.
- **7 ET:** Próby stają się szeroko śledzonym sportem i widowiskiem. Mniejsze ligi, kluby i festiwale przyciągają kibiców oraz sponsorów; mieszkańcy obserwują zawodników i zawierają zakłady. Zawody sportowe nie mają same z siebie mocy traktatowej.
- **9 ET:** Powstaje Kodeks Prób z regułami, opisami wież i dokumentacją bezpieczeństwa, w tym zasadami zgłaszania zadań, stawek, punktacji i podziału punktów.
- **11 ET:** Kilka miast zgłasza niezgodności w rejestrze wydobycia. Rada rozpoczyna kontrolę; nie wskazano publicznie winnych ani przyczyny.
- **12 ET — początek gry:** Po kryzysie transportowym sygnatariusze zawierają Porozumienie Val Meris. Pierwsze Wielkie Igrzyska tej rangi wybiorą jeden z dwóch uzgodnionych modeli zarządzania, by uniknąć wojny. Śledztwo dotyczące Nocy i audyt rejestrów trwają niezależnie.

---

### Siedem krain — propozycja do dalszego rozwijania

Nazwy i tradycje są robocze. Kraina nie ogranicza żywiołu ani roli mieszkańca; bohaterowie mogą uczyć się różnych technik, podróżować i działać w mieszanych drużynach.

- **Vareth — tradycje Ognia:** miasta kuźni i pieców, znane z obróbki minerałów i utrzymania urządzeń cieplnych. Ich przedstawiciele często argumentują, że bezpieczne dostawy wymagają silnej infrastruktury.
- **Nivoren — tradycje Lodu:** północne osady i magazyny chroniące żywność oraz kryształy. Słyną z cierpliwego planowania i wiedzy o trudnych szlakach.
- **Zerath — tradycje Błyskawic:** miasta przekaźników i warsztatów, rozwijające łączność oraz szybki transport wiadomości. Cenią eksperyment i szybkość działania.
- **Verdan — tradycje Natury:** społeczności żyznych dolin, gdzie rozwinięto leczenie i odbudowę terenów zniszczonych przez wojnę. Ich wpływ wynika również z produkcji żywności.
- **Kharum — tradycje Ziemi:** ośrodki górnicze i kamieniarskie, pilnujące stabilności tuneli oraz zabezpieczania złóż. Mają długą pamięć o kosztach przeciążenia sieci.
- **Aurel — tradycje Światła:** miasta latarni, archiwów i obserwatoriów. Wnoszą do Rady wiedzę o mapowaniu szlaków, wykrywaniu zagrożeń i dokumentowaniu wyników.
- **Vesper — tradycje Ciemności:** społeczności znane z pracy w głębokich kopalniach, maskowania i badań nad osłoną energii. Ciemność nie oznacza zła: ich techniki pomagają chronić konwoje i ratowników.

Krainy nie są jednolitymi frakcjami moralnymi. W każdej mogą działać zwolennicy traktatu, sceptycy, bohaterowie i osoby korzystające na napięciach. Dawne granice nie muszą pokrywać się z miejscem urodzenia postaci ani jej żywiołem.

### Próby w codziennym życiu

Urząd Prób utrzymuje areny i zabezpieczenia, a mieszkańcy śledzą wyniki. Zwykła Próba traktatowa dotyczy ograniczonej nadwyżki albo kolejności działań po nieudanych negocjacjach; wyjątkowe Wielkie Igrzyska mają osobną stawkę określoną w Porozumieniu Val Meris. Fakty, odpowiedzialność i odwołania proceduralne badają niezależni arbitrzy. Zawodnicy trenują i rywalizują także w ligach sportowych wspieranych przez sponsorów; widzowie kibicują i zawierają zakłady. Wyniki zawodów sportowych nie zmieniają same z siebie traktatowych przydziałów.

**Bezpieczeństwo areny:** po osłabieniu uczestnika stosuje się specjalny gaz usypiający, a następnie przewozi go do centrum leczenia. Po kilku dniach może ponownie walczyć. Fabularnie kolejne ataki dzieli czas potrzebny na leczenie, ale w grze odbywają się kolejno bez wielodniowego oczekiwania. Wyeliminowany Runner nie wraca w tym samym ataku; może wystąpić w następnym zgodnie z zasadami meczu. [OTWARTE] Dokładny sposób działania zabezpieczeń obrońcy, ochrona towarzyszy i postępowanie przy awarii systemu.

**Historie bohaterów:** docelowo gracz poznaje kolejne części historii bohatera w miarę grania nim. [OTWARTE] Progi, zapis postępów, sposób ujawniania informacji i rozdzielenie wiedzy gracza od wiedzy innych postaci; obecny kierunek nie oznacza gotowej implementacji.

Każda drużyna przygotowuje własną obronę na identycznym układzie labiryntu. Urząd sprawdza równość tras, natomiast pozycje wież i inwestycje są decyzją drużyn. Węzły technologii można wyjaśnić jako wspólne dostrajanie sieci: wybór wpływa na wieże obronne oraz narzędzia wykorzystywane przez drużynę podczas ucieczki. Stan wrogiej obrony jest ukryty do czasu odkrycia, a wcześniejsze obserwacje służą jako wiedza na następną próbę.

### Bohaterowie i osobiste historie

Historie Dary, Noctena, Rian oraz Nivy, Liory i Eliry zostały zatwierdzone na potrzeby PoC; pozostałe poniższe powiązania są propozycjami, nie zatwierdzonymi biografiami. Konkretne zdolności pozostają w dokumencie bohaterów. Strony objęte daną Próbą wyznaczają reprezentantów; ich osobiste motywacje nie zmieniają formalnej krainy reprezentowanej w tej sprawie. Mieszane drużyny mogą dzielić punkty zgodnie z umową zarejestrowaną przed Próbą.

Wiek podajemy dla początku gry w 12 ET. Poza wcześniejszymi ustaleniami dotyczącymi Saily, Dary, Noctena i sióstr wartości są propozycjami, z wyjątkiem przyjętych 52 lat Tarena i 29 lat Lucena: Kael 46, Iskar 43, Taren 52, Thora 42, Brom 51, Orien 62, Morrak — nieznany, Solan 34, Niva 25, Varek 33, Liora 24, Dara 37, Elira 24, Nocten 68, Rian 32, Saila około 23, Zet 27, Eron 35, Kiran 36, Adrien 30, Morven 39, Miko 29, Pyrra 28, Eira 56, Volt 26, Ana 27, Rokan 65, Lumen 34, Koneva 31, Neris 48 i Lucen 29 lat. Szczegóły oraz rozwinięte historie w obu wersjach językowych znajdują się w [historiach bohaterów](./RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md).

Bohaterowie mają własne cele i mogą przyjmować różne stanowiska lub zmieniać zdanie. Saila chce zbadać zapisy z Białego Przesmyku i zabezpieczyć dostawy dla przełęczy. Widzi, że większa swoboda używania kryształów pozwoliłaby jej pomóc Nivoren, ale obawia się skutków zniesienia wspólnych ograniczeń; nie wybrała jeszcze strony. Przynależność do drużyny nie przesądza poglądów uczestnika.

- **Brom:** doświadczony Tank Ziemi, który uczy nowych Runnerów, jak chronić drużynę podczas przejścia. Mógł pracować przy zabezpieczaniu tuneli, zanim dołączył do Prób.
- **Thora:** Tank Natury i opiekunka, która traktuje wytrzymałość jako sposób zapewnienia innym czasu na działanie. Jej ewentualne pokrewieństwo z siostrami pozostaje otwarte.
- **Niva, Elira i Liora:** trzy siostry urodzone w Korzennej Przystani, osierocone w końcowej fazie wojny i wychowane przez rodziny z różnych krain. W 12 ET Niva ma 25 lat, a młodsze od niej o rok bliźniaczki Liora i Elira mają po 24 lata. Bliźniaczki szczególnie mocno przeżyły rozłąkę, choć każda odnalazła też bliskość w nowej rodzinie. Odmienne żywioły wynikają z ich późniejszej nauki i praktyki, nie z dziedziczonego przeznaczenia. Jako dorosłe Niva i Liora spotkały się pierwsze na górskim szlaku. Elira szukała obu sióstr w rejestrach ewakuacyjnych. Odszukała Liorę w jej domu w Verdan, gdzie była już Niva. Od tego spotkania wspólnie pomagają potrzebującym. Historia powodzi w 9 ET pokazuje, jak ich różne umiejętności się uzupełniają, ale nie zawsze prowadzą do zgodnych decyzji. Mogą grać razem lub po przeciwnych stronach; pokrewieństwo nie daje premii mechanicznych.
- **Dara:** rzemieślniczka z Kamiennego Progu zabiega o bezpieczeństwo ekip i liczy, że na Igrzyskach odnajdzie dawnych towarzyszy, by zapytać o wydarzenia sprzed Nocy. Jej podejrzenie związku z katastrofą nie jest dowodem jej przyczyny.
- **Morrak:** Tank Ciemności, który unika ludzi, ale chroni niewinnych. Uratowani nazywają go Morrakiem („Obrońcą”), a świadkowie jego wyroków — Veshem („Mścicielem”); są to przydomki nadane przez ludzi, nie potwierdzone prawdziwe imię. Tropi i zabija wyłącznie osoby, których świadome krzywdzenie innych zostało potwierdzone; nie działa na podstawie plotek ani samego związku z wydarzeniami sprzed piętnastu lat. Pierwsze znane zapisy o nim pochodzą z okresu po Nocy Zgasłych Wież, co nie wyjaśnia jego przeszłości.
- **Kael:** Tank Ognia, której znajomość kuźni i ochronnych pancerzy może wiązać ją z dawną infrastrukturą Vareth.
- Pozostali bohaterowie otrzymają osobne historie po uzgodnieniu ich pochodzenia, charakterów i wzajemnych relacji.

### Konflikty na przyszłość

1. **Kto kontroluje szlaki?** Wyniki prób mogą rozstrzygać dostęp do zasobów, ale bogatsze miasta łatwiej utrzymują szkoły i sponsorują drużyny.
2. **Czy Rada jest wystarczająco niezależna?** Traktat rozdziela zadania Rady i Urzędu Prób, lecz ich przedstawiciele nadal pochodzą z siedmiu krain.
3. **Co wydarzyło się w Val Meris?** Znane są skutki katastrofy, ale nie ma jednej zatwierdzonej wersji jej przyczyn.
4. **Dlaczego rejestry się nie zgadzają?** Błąd, przemyt i świadoma manipulacja pozostają możliwymi hipotezami — żadna nie jest ujawnionym faktem.
5. **Czy system jest sprawiedliwy dla mniejszych miast?** Próby ograniczają przemoc, ale sam turniej nie rozwiązuje nierówności.

### Otwarte decyzje

- Nazwy kontynentu, krain, minerału, instytucji i kalendarza.
- Jak krainy wybierają delegacje reprezentantów do konkretnej sprawy lub cyklu Prób; zawodnik rejestruje reprezentowaną krainę, a jego pochodzenie nie musi się z nią pokrywać.
- Jak dokładnie negocjacje przekładają złożony spór między krainami na zadanie, stawkę, punktację i termin wyniku; sprawy wewnętrzne i prywatne pozostają lokalne.
- Szczegóły formatu Wielkich Igrzysk: przebieg kwalifikacji, liczba drużyn i meczów, punktacja klasyfikacji, podział punktów drużyn mieszanych oraz dobór par w dodatkowych meczach.
- Szczegółowa procedura odwołania od naruszeń reguł i proporcjonalne sankcje za niewykonanie rozstrzygnięcia, z zachowaniem podstawowych dostaw dla cywilów.
- Czy i jakie limity najmu zawodników wprowadzić; bogatsze krainy często korzystają z najemników, a ich kontrakty trzeba rejestrować przed daną Próbą lub cyklem Prób.
- Charakter splotytu i pochodzenie konstruktów; czy wszystkie moby są konstrukcjami, czy część jest przywoływanymi istotami.
- Ewentualna relacja Thory do rodziny sióstr.
- Pozostałe pełne biografie, antagonistyczne postacie, przyczyna katastrofy i rozwój wątku rejestrów.
- Jak lore pojawia się w grze: krótkie wpisy kodeksu, komentarze bohaterów, opisy map lub inne formy.

---

## Riftoryn — mapa świata i ośrodki Prób · KONCEPCJA FABULARNA · v0.1

**Status:** mapa i ośrodki rozwijają historię świata; w sprawach fabularnych rozstrzyga [world_history.md](./world_history.md). Dokładne położenie, granice i charakter przyszłych aren wymagają dopracowania. Tradycje żywiołów nie ograniczają pochodzenia bohaterów, przynależności drużyn ani wyboru technologii. Na mapie północ znajduje się u góry.

**Kontekst:** zwykłe Próby dotyczą ograniczonych impasów o zasoby i kolejność działań. Wyjątkowe Wielkie Igrzyska 12 ET mają na mocy osobnego porozumienia wybrać jeden z dwóch uzgodnionych modeli zarządzania krainami. W obu chronione są podstawowe dostawy, a niezależne śledztwo w sprawie Nocy trwa dalej. W formacie labiryntowym obie strony mają identyczne układy i pełną parę ataków; drużyny mogą być mieszane.

### Ośrodki

1. **Val Meris — centralny węzeł szlaków.** Położenie: okolice wielkiego jeziora, przy zbiegu dróg. Podczas Nocy awaria wież odcięła ewakuację; przeciążenie jest jedną z hipotez. Miasto gości ośrodek Prób i archiwum katastrofy, a część dzielnic pozostaje opuszczona. **Arena, propozycja:** Serce Labiryntu — jednopoziomowa droga z centrum na zewnątrz, bez skrótów i ślepych zaułków w MVP.
2. **Żarowisko Vareth — północny wschód, Vareth / tradycje Ognia.** Miasto warsztatów i pieców na skraju wulkanicznych wyżyn, przy drodze do centralnego węzła. **Ośrodek Prób:** Żelazna Brama, odbudowana strażnica szlaku. Kontrolowane efekty żaru mogłyby nadać późniejszej mapie tożsamość; konkretne mechaniki nie są ustalone.
3. **Biały Przesmyk — północ, Nivoren / Lód.** Osada u wejścia do górskiego przejścia i magazynów zaopatrzenia. **Ośrodek Prób:** Lodowe Wrota. Spory dotyczące sezonowego transportu mogą trafiać do Prób. Śnieżny wygląd nie oznacza obowiązkowych poślizgów ani mechanik pogodowych.
4. **Iglica Zerath — wschód, Zerath / Błyskawice.** Miasto przekaźników na skalistym wybrzeżu, połączone z centralnymi szlakami. **Ośrodek Prób:** Dziedziniec Impulsu. Energia i architektura nawiązują do łączności, lecz elementy mapy wymagają zaprojektowania.
5. **Korzenna Przystań — południe, Verdan / Natura.** Osada w żyznej dolinie rzeki, wspierająca odbudowę i dostawy żywności. **Ośrodek Prób:** Krąg Korzeni, arena w pobliżu szlaku prowadzącego do centrum. Roślinność pozostaje czytelna i nie blokuje obowiązkowej drogi.
6. **Kamienny Próg — południowy zachód, Kharum / Ziemia.** Miasto kopalń i zakładów zabezpieczających wydobycie splotytu. **Ośrodek Prób:** Kamienny Krąg, na powierzchni w pobliżu wyrobisk. Złoża i ich ewentualne spory podlegają konkretnym regułom traktatu, które trzeba jeszcze określić.
7. **Latarnia Aurelu — zachód, Aurel / Światło.** Miasto latarni i archiwów nad jasnym wybrzeżem. **Ośrodek Prób:** Plac Siedmiu Świateł, powiązany z dokumentowaniem wyników i szkoleniem obserwatorów. Nie jest to automatycznie siedziba całego Urzędu.
8. **Cienisty Port — północny zachód, Vesper / Ciemność.** Osada przy skalistych wyżynach i szlakach do głębokich kopalń. **Ośrodek Prób:** Brama Zmierzchu. Techniki osłony i maskowania służą ochronie konwojów; Ciemność nie jest z natury zła.

### Zasady spójności

- Krainy są tradycjami i społecznościami, nie sztywnymi frakcjami wyznaczającymi żywioł lub drużynę. Mieszane drużyny reprezentują wspólne miasto, społeczność albo sprawę; osoby z różnych krain mogą walczyć po tej samej stronie.
- Lokalne ośrodki organizują Próby pod nadzorem Urzędu. Niezależni arbitrzy rozpatrują odwołania proceduralne, a zespoły audytowe kontrolują wspólny rejestr i przydziały.
- Mapa kontynentu jest ilustracją fabularną; rozmiary, granice, odległości i liczba wszystkich osad nie są określone. Zaznaczone punkty nie są listą ukończonych map do gry.
- Daty i wydarzenia zgodnie z rozdziałem historii świata powyżej; grafika i nazwy ośrodków są propozycją, nie zatwierdzonym kanonem.

---

## Riftoryn — historie i pochodzenie 21 pierwotnie zaplanowanych bohaterów

**Status:** biografie Dary, Noctena, Nivy, Liory i Eliry zatwierdzono na potrzeby PoC; pozostałe indywidualne biografie i specjalizacje są propozycjami, z wyjątkiem faktów zapisanych w [historii świata](./world_history.md), która jest źródłem kanonu. Zatwierdzenie historii nie zatwierdza mechanik ani specjalizacji postaci. Dziewięciu planowanych bohaterów pozostaje niedostępnych w drafcie. Drużyny mogą być mieszane; opisane motywacje dotyczą postaci, a nie formalnych zasad reprezentacji.

### Kael
**Pochodzenie:** Żarowisko Vareth  
**Specjalizacja:** Strażniczka żaru i osłon

Przez lata zabezpieczała piece i transport splotytu, ucząc się zatrzymywać żar, zanim zagrozi innym. W Próbach osłania drużynę i wzmacnia wieże.

**Dlaczego bierze udział w Próbach:** Reprezentuje dzielnice warsztatowe, które walczą o sprawiedliwy dostęp do paliwa.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Iskar
**Pochodzenie:** Biały Przesmyk  
**Specjalizacja:** Lodowy tarczownik — kierunek do zaprojektowania

**Pasywka — Lodowa zbroja:** regeneruje 2 punkty osłony na sekundę, maksymalnie do 10 punktów łącznej osłony; gdy otrzyma osłonę ponad ten próg, regeneracja wznawia się dopiero po jej spadku do 10.

Dawniej zabezpieczał górskie konwoje i drogi do magazynów. Lodowa tarcza jest znakiem jego sposobu ochrony grupy.

**Dlaczego bierze udział w Próbach:** Występuje dla wspólnoty przełęczy, aby utrzymać dostęp do zaopatrzenia.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Taren
**Pochodzenie:** Iglica Zerath  
**Specjalizacja:** Opancerzony przewodnik burzy — kierunek do zaprojektowania

Dorastał wśród warsztatów przekaźnikowych i pomagał osłaniać ekipy naprawiające przewodniki energii.

**Dlaczego bierze udział w Próbach:** Próby dają mu szansę wywalczyć dostęp do części potrzebnych okolicznym stacjom.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Thora
**Pochodzenie:** Korzenna Przystań  
**Specjalizacja:** Strażniczka korzeni i regeneracji

Lata odbudowy zniszczonych pól nauczyły ją, że najsilniejszy obrońca zostaje z tyłu, dopóki inni nie są bezpieczni. Łączy wytrzymałość z troską o drużynę.

**Dlaczego bierze udział w Próbach:** W Próbach walczy o zaopatrzenie dla doliny i środki na odbudowę.

**Rodzina:** Nie ustalono pokrewieństwa z trzema siostrami; jej rodzina pozostaje otwarta.

### Brom
**Pochodzenie:** Kamienny Próg  
**Specjalizacja:** Tarcza i ochrona wież

Jako strażnik wyrobisk chronił robotników podczas zawaleń i pomagał utrzymać przejścia. W labiryncie wykorzystuje tarczę, by inni mogli iść dalej.

**Dlaczego bierze udział w Próbach:** Reprezentuje górniczą społeczność zabiegającą o bezpieczny szlak zaopatrzenia.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Orien
**Pochodzenie:** Latarnia Aurelu  
**Specjalizacja:** Obrońca światła — kierunek do zaprojektowania

Wieloletni opiekun latarni i konwojów, znany z tego, że nie zostawia towarzyszy na niebezpiecznej drodze.

**Dlaczego bierze udział w Próbach:** Startuje, aby chronić niezależność ośrodka przechowującego dokumentację szlaków.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Morrak
**Pochodzenie:** Cienisty Port  
**Specjalizacja:** Osłona cienia i spowalnianie pocisków

Milczący strażnik pojawił się w porcie po wojnie. Nikt nie potwierdził, czy został skonstruowany, czy ukształtowany inaczej; sam skupia się na ochronie innych.

**Dlaczego bierze udział w Próbach:** Dołącza do drużyn strzegących konwojów, bo zna cenę utraty bezpiecznej drogi.

**Rodzina:** Jego pochodzenie i ewentualne więzi rodzinne pozostają tajemnicą.

### Solan
**Pochodzenie:** Żarowisko Vareth  
**Specjalizacja:** Wsparcie żaru — kierunek do zaprojektowania

Prowadził ruchomą latarnię dla ekip podróżujących między osadami; jego ciepło i optymizm pomagają utrzymać drużynę razem.

**Dlaczego bierze udział w Próbach:** Bierze udział dla społeczności szlaku, która chce uniezależnić dostawy od pojedynczego dostawcy.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Niva
**Miejsce urodzenia:** Korzenna Przystań
**Wychowanie:** okolice Białego Przesmyku
**Wiek w 12 ET:** 25 lat

**Specjalizacja:** Oczyszczanie i ochrona przed spowolnieniem

Po śmierci rodziców w końcowej fazie wojny przygarnęła ją rodzina z okolic Białego Przesmyku, gdzie uczyła się lodowej magii od lokalnych praktyków. Podczas powodzi w 9 ET próbowała utrzymać uszkodzoną śluzę lodem; uratowała część zapasów, ale nie zatrzymała wody ani nie ochroniła wszystkich pól. Od tamtej pory pozostaje opanowana w kryzysie, choć ma skłonność brać na siebie każdą stratę i ukrywać własny strach.

**Dlaczego bierze udział w Próbach:** Chce utrzymać wymianę zaopatrzenia między doliną a północą, nie udając, że jedna decyzja zdoła ochronić wszystkich.

**Rodzina:** O rok starsza siostra bliźniaczek Eliry i Liory; ich wybory drużyn nie muszą być jednakowe.

### Varek
**Pochodzenie:** Iglica Zerath  
**Specjalizacja:** Impuls zespołowy i gotowość wież

Pracowała przy naprawie przekaźników, gdzie sekundy często decydowały, czy wiadomość dotrze na czas. W Próbach uczy sojuszników działać w jednym rytmie.

**Dlaczego bierze udział w Próbach:** Reprezentuje mieszkańców utrzymujących łączność na mniej dochodowych odcinkach szlaku.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Liora
**Miejsce urodzenia:** Korzenna Przystań
**Wychowanie:** inna część Verdan; do doliny wróciła jako dorosła
**Wiek w 12 ET:** 24 lata

**Specjalizacja:** Leczenie i naprawa osłon

Wychowała się u rodziny rolników i zielarek, od których uczyła się magii Natury, leczenia i odbudowy. Jako dorosła wróciła do doliny, by pomagać rannym i odbudowywać miejsca po dawnych walkach. Podczas powodzi otworzyła kanał odpływowy, ratując domy i lecznicę, ale tracąc rodzinne zagony i część pól sąsiadów. Kocha swoją pracę, lecz nie chce, by inni traktowali jej poświęcenie jako oczywisty obowiązek.

**Dlaczego bierze udział w Próbach:** Zabiega o środki dla lecznic i odbudowy pól, a także uczy się prosić o pomoc, zanim sama się wyczerpie.

**Rodzina:** Bliźniaczka Eliry i młodsza siostra Nivy; podobny wygląd nie daje premii w grze.

### Dara
**Pochodzenie:** Kamienny Próg  
**Specjalizacja:** Rzemiosło i naprawa — kierunek do zaprojektowania

Rzemieślniczka odpowiedzialna za naprawę narzędzi i zabezpieczeń w kopalniach. Woli zrozumieć urządzenie, zanim podejmie ryzyko.

**Dlaczego bierze udział w Próbach:** Występuje na rzecz bezpieczeństwa ekip, a na Igrzyskach szuka dawnych towarzyszy, by zapytać o wydarzenia sprzed Nocy.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Elira
**Miejsce urodzenia:** Korzenna Przystań
**Wychowanie:** Aurel
**Wiek w 12 ET:** 24 lata

**Specjalizacja:** Światło, wykrywanie i zwiad

Po wojnie przyjęła ją rodzina obserwatorów szlaków w Aurel, gdzie uczyła się światła i mapowania. Jako dorosła szukała obu sióstr w niepełnych kopiach rejestrów ewakuacyjnych. Odszukała dom Liory w Verdan i zastała tam również Nivę. Podczas powodzi przekazała ratownikom trasę z mapy, której nie zdążyła sprawdzić po roztopach; akcja się opóźniła, choć nikt nie zginął z powodu jej pomyłki. Przyznała się do błędu i dziś odróżnia na mapach trasy potwierdzone od niepewnych.

**Dlaczego bierze udział w Próbach:** Chce zapewnić mniejszym miastom dostęp do aktualnych map i rzetelnej informacji o tym, czego jeszcze nie wiadomo.

**Rodzina:** Bliźniaczka Liory i młodsza siostra Nivy; ich rodzinne pochodzenie nie wymusza wspólnego składu.

### Nocten
**Historia zatwierdzona na potrzeby PoC.**
**Pochodzenie:** Północna osada Riftorynu (według dokumentów)
**Specjalizacja:** Magia Ciemności — ukrywanie sojuszników i osłabianie przeciwników

Nocten ma 68 lat i pamięta dwie sprzeczne przeszłości: dzieciństwo w północnej osadzie oraz miasto za białymi murami, kobietę, którą kochał, i chłopca nazywającego go ojcem. Na co dzień jest rozsądny, cierpliwy i dowcipny; jako doświadczony mag Ciemności potrafi ukrywać sojuszników i osłabiać przeciwników. Nie wie, czy może ufać własnym wspomnieniom ani niezwykłym zachowaniom swojego cienia. Żadna teoria nie jest potwierdzona.

**Dlaczego bierze udział w Próbach:** Przyjeżdża na Wielkie Igrzyska za sprawą notatki, której nie pamięta, i ma nadzieję spotkać kogoś, kto rozpozna jego wspomnienia.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Rian
**Historia zatwierdzona na potrzeby PoC.**
**Pochodzenie:** Żarowisko Vareth  
**Specjalizacja:** Precyzyjne uszkadzanie wież

Była sabotażystką, która świadomie zarabiała na uszkadzaniu wież; za działania podczas wojny została skazana i odbyła karę. Teraz uczy rzemieślników rozpoznawać słabe punkty konstrukcji, a w kontrolowanych Próbach wykorzystuje ten sam talent do sprawdzania obrony. Nie ukrywa swojej przeszłości i nie oczekuje przebaczenia.

**Dlaczego bierze udział w Próbach:** Chce pokazać, że może wykorzystywać swoje umiejętności w kontrolowanych warunkach, nie odbierając przeciwnikom prawa do przygotowania i odpowiedzi. Nadal uczy bezpiecznego projektowania.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Saila
**Pochodzenie:** Biały Przesmyk  
**Specjalizacja:** Zamrażanie wież i kontrola trasy

Saila miała około ośmiu lat, gdy wybuchy urządzeń w Białym Przesmyku zmusiły ją do ucieczki. Jeszcze w roku katastrofy spotkała mistrza w górach. Przez blisko piętnaście lat uczyła się prowadzić moc kryształów, a później pracowała przy chłodzeniu magazynów; w 12 ET ma około dwudziestu trzech lat. Chce zbadać zapisy awarii i zabezpieczyć dostawy dla przełęczy. Jej doświadczenie kontrolowanej pracy nie rozstrzyga ani przyczyny Nocy, ani bezpieczeństwa gwałtownego wzrostu wydobycia.

**Dlaczego bierze udział w Próbach:** Chce zrozumieć, co wydarzyło się podczas Nocy i znaleźć sposób bezpiecznego korzystania z kryształów, który nie odbierałby ludziom dostępu do potrzebnych zasobów.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Zet
**Pochodzenie:** Iglica Zerath  
**Specjalizacja:** Szybki zwiad i elektroimpuls

Doręczał pilne wiadomości między stacjami przekaźnikowymi. Jego szybkość i wyczucie urządzeń przydały się później w labiryntach.

**Dlaczego bierze udział w Próbach:** Bierze udział, aby wesprzeć sieć małych stacji zagrożonych zamknięciem.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Eron
**Pochodzenie:** Korzenna Przystań  
**Specjalizacja:** Zwiad i magia roślin — kierunek do zaprojektowania

Poznawał dolinę jako przewodnik ekip odnawiających stare drogi. Szuka zastosowań pnączy, które pomagają przejść trudny teren.

**Dlaczego bierze udział w Próbach:** Chce utrzymać dostęp do szlaku łączącego kilka odległych osad.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Kiran
**Pochodzenie:** Kamienny Próg  
**Specjalizacja:** Inżynieria terenowa — kierunek do zaprojektowania

Badała stabilność skalnych przejść i nauczyła się czytać teren przed rozpoczęciem prac. Łączy precyzję zwiadu z wiedzą inżynierską.

**Dlaczego bierze udział w Próbach:** W Próbach broni projektu bezpieczniejszej drogi do wyrobisk.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Adrien
[USTALONE] **Obrona — Specjalista wież:** wieży pilotowanej przez Adriena nie można w żaden sposób zatrzymać. [OTWARTE] Doprecyzować wpływ obrażeń i zniszczenia wieży, ograniczeń celowania, efektów na operatorze oraz dokładny moment rozpoczęcia i zakończenia ochrony, w tym fazę przejmowania sterowania.

**Pochodzenie:** Latarnia Aurelu  
**Specjalizacja:** Wynalazca Światła — pryzmatyczne przyrządy i stabilizacja urządzeń ze splotytem

Adrien jest genialnym, aroganckim wynalazcą, który niemal nie opuszcza pracowni. Pod koniec wojny jego pryzmatyczny sygnalizator zawiódł po tym, jak zlekceważył ostrzeżenie o mocowaniu kryształu; błędny sygnał skierował przewodnika na niebezpieczną drogę, gdzie człowiek zginął. Adrien przyznał się do wady projektu, ale przez lata podkreślał, że za użycie urządzenia odpowiadało dowództwo. Dziś jego cywilne przyrządy przechodzą niezależne testy i mają zabezpieczenia. Nadal uważa, że odpowiada za technologię, nie za cudze decyzje. Jest oskarżany o dostarczenie sprzętu sabotażystom, ale nie ma dowodu, że im go przekazał.

**Dlaczego bierze udział w Igrzyskach:** Dotąd uznawał zawody za niewarte swojego czasu. Na wyjątkowe Wielkie Igrzyska zgłasza się z próżności, by zdobyć sławę i dowieść, że jest lepszy od pozostałych.

**Rodzina:** Rodzina nie została jeszcze opisana.

### Morven
**Pochodzenie:** Cienisty Port  
**Specjalizacja:** Sabotaż i osłona informacji

Dorastał wśród załóg chroniących konwoje przed zdradzeniem ich trasy. Swoje umiejętności maskowania wykorzystuje dziś do zakłócania wykrywania.

**Dlaczego bierze udział w Próbach:** Startuje w imieniu przewoźników, którzy domagają się bezpiecznych i uczciwie przydzielanych tras.

**Rodzina:** Rodzina nie została jeszcze opisana.


### Trening bohatera i przyszły tryb prób

[USTALONE — kierunek] W świecie Riftoryn trening jest przedstawiony jako próba przygotowująca bohatera do udziału w prawdziwych rozgrywkach. Docelowo gracz może trenować sam lub z innymi, korzystając ze standardowego układu automatycznych wież albo samodzielnie ustawiając obronę. Każdy bohater ma własny poziom mastery; osiągnięcie minimalnego poziomu treningowego jest wymagane przed wybraniem go w drafcie. [OTWARTE] Próg mastery, sposób naliczania i zapisu postępów, warunki ukończenia, dostępność bohaterów dla nowych graczy oraz szczegóły treningu grupowego wymagają zaprojektowania i testów.

[POMYSŁ NA PÓŹNIEJ] Osobny tryb drużynowy, w którym skład graczy podejmuje kolejne próby przejścia labiryntu, a obrona staje się mocniejsza z każdą rundą. Zasady progresji, nagrody i związek z mastery nie są ustalone.

[PROTOTYP PRZEGLĄDARKOWY] Prosta samotna próba w kodeksie służy nauce ruchu i wstępnemu sprawdzeniu umiejętności; nie jest implementacją konta, rozgrywki wieloosobowej ani rzeczywistego odblokowania bohatera w drafcie. Widok 2D i wartości wież są uproszczeniem do testów.

### Lucen — propozycja umiejętności

- **Lightning Heritage / Dziedzictwo Błyskawic:** dodatkowe +25 punktów procentowych odporności na obrażenia Błyskawic. W obecnej Próbie zmniejsza bezpośrednie trafienia i przeskoki wieży Błyskawic o 25%.
- **Pryzmatyczna blokada (atak, Q):** wyłącza najbliższą żywą wieżę w zasięgu na 2,5 s; czas odnowienia 20 s. Dostępna w Próbie.
- **Świetlny sygnał (obrona):** proponowane ujawnienie Runnerów w wybranym sektorze na 4 s; faza obrony i ta zdolność pozostają do wdrożenia.
- **Statystyki startowe:** 105 HP, atak podstawowy 20, prędkość 5,6 m/s. W obecnej Próbie zwykły atak Lucena nie jest jeszcze dostępny, a model jest zastępczy.
