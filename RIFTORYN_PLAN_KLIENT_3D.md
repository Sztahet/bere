# Riftoryn — plan samodzielnego klienta 3D

**Status:** propozycja do omówienia · **Cel:** pierwsza paczka Windows do testów offline · **Zakres gry:** pre-PoC z sekcji 0.1 GDD. GDD pozostaje źródłem zasad gry; propozycje techniczne i kolejność poniżej nie przesądzają wyboru silnika ani docelowego balansu.

## Co chcemy sprawdzić

Tester ma pobrać i rozpakować grę, uruchomić ją bez konta, internetu i edytora, a następnie przejść spiralny labirynt z kamerą TPP. Ma czuć skalę drogi, widzieć nadlatujące pociski, korzystać z jednej umiejętności, reagować na mgłę i móc od razu ponowić próbę. Automatyczne wieże stanowią zagrożenie; nie ma graczy w obronie.

Przeglądarkowy model 2D pozostaje osobnym narzędziem do szybkich eksperymentów z liczbami i zachowaniami. Nie zakładamy, że jego kod lub balans automatycznie przeniesie się do 3D: inna kamera, geometria, pociski, skala, kolizje i sterowanie wpływają na wynik. Przyjmujemy wspólne opisane założenia, a zgodność wybranych wartości sprawdzamy testami.

## Proponowany kierunek technologiczny

Pierwszy kandydat: **Unreal Engine 5**, zgodnie z kierunkiem wskazanym w GDD. Kamera TPP i przyszła gra sieciowa czynią go sensownym kandydatem, ale wybór potwierdzimy po małym teście na naszym sprzęcie: instalacja, praca edytora, sterowanie, działający build Windows i możliwość wygodnej dalszej pracy. Sprawdzimy również aktualną licencję i wymagania projektu. Alternatywy, w tym Godot i Unity, pozostają otwarte; decyzja o silniku nie wynika automatycznie z istnienia prototypu 2D.

Pierwsza paczka jest **offline i solo**. AI sojuszników może działać lokalnie w tym samym kliencie, bez generatywnej AI, konta, usług sieciowych czy serwera. Nie jest jednak warunkiem pierwszego uruchomienia ani zastępstwem dla późniejszego testu z żywymi graczami. Przygotujemy logikę w sposób umożliwiający późniejsze testy sieciowe, ale offline nie dowiedzie poprawności replikacji ani nie wykluczy późniejszych przeróbek. Docelowe 5v5, autorytatywny serwer i Fog of War wymagają osobnego etapu.

## Etap 0 — wybór narzędzi i próba eksportu

1. Sprawdzić sprzęt i wolne miejsce, zainstalować wybraną wersję silnika i niezbędne narzędzia; zapisać wersje i wymagania dla projektu.
2. Utworzyć osobny projekt gry 3D, ustalić strukturę plików, sposób wersjonowania dużych zasobów oraz zasady przechowywania buildów. Zachować repozytorium kodeksu i dotychczasowe zmiany.
3. Uruchomić szablon postaci TPP, spakować minimalną scenę do Windows i uruchomić paczkę poza edytorem, najlepiej na drugim komputerze. To wczesne potwierdzenie procesu dystrybucji, nie dowód gotowości gry.

**Gotowe, gdy:** tester może uruchomić paczkę i poruszyć postacią bez uruchamiania edytora. Zapisać wersję silnika, rozmiar paczki i problemy.

## Etap 1 — odczucie ruchu i przestrzeni

1. Zbudować labirynt z prostych brył: spirala od środka na zewnątrz, mury, podłoga, start i meta, poprawne kolizje i czytelne zakręty. Bez gotowej oprawy graficznej.
2. Dodać poruszanie WASD, sterowanie kamerą myszy, czytelną kolizję kamery ze ścianami oraz wskazówki sterowania. Ustawić roboczą skalę i zmierzyć realną długość drogi.
3. Uruchomić pomiar czasu i ponowienie. Porównać niezakłócony czas biegu z założeniem około 360 m i prędkością wybranego bohatera; skorygować skalę na podstawie faktycznego przejścia.

**Gotowe, gdy:** tester bez instrukcji twórcy może swobodnie przebiec całą trasę, kamera i kolizje nie blokują przejścia, a czas oraz skala są mierzalne.

## Etap 2 — zagrożenie i podstawowa próba

1. Dodać jednego bohatera z wartościami HP i prędkości z GDD oraz jedną działającą umiejętnością ataku; użyć tymczasowego modelu i animacji. Wybrać bohatera przed implementacją; warianty umiejętności i wartości pozostają testowe.
2. Dodać co najmniej jeden typ automatycznej wieży, stałe rozmieszczenie, wykrywanie, zasięg, cooldown, czytelny pocisk i obrażenia. Pocisk i ściana powinny mieć interakcję zgodną z GDD; jawnie opisać wszelkie uproszczenia. Zwykły atak Kael na wieże w 2D jest eksperymentem, nie przesądza o docelowych zasadach.
3. Dodać mgłę przesuwającą się po trasie, spowolnienie i licznik skażenia oraz obrażenia według GDD. Dla próby przetestować 5 s opóźnienia startu i dotarcie czoła po 120 s od początku; to hipoteza, nie nowa ostateczna reguła.
4. Dodać HP, czas, cooldown, stan mgły, wyraźny wynik wygranej/przegranej, powrót do menu i natychmiastowy restart. Nie kończyć próby wyłącznie dlatego, że mgła dotarła do mety.

**Gotowe, gdy:** gracz może rozpocząć próbę, otrzymać obrażenia, użyć zdolności, dotrzeć do mety albo zostać pokonany, zobaczyć wynik i powtórzyć ją bez restartu gry. Trasa pozostaje przechodnia. Wartości są łatwe do zmiany bez przebudowy całej logiki.

## Etap 3 — paczka do zewnętrznego testu

1. Dodać proste menu, podstawowe ustawienia głośności i czułości myszy, pauzę offline oraz identyfikator wersji widoczny w menu i na ekranie wyniku.
2. Przygotować i sprawdzić paczkę ZIP na Windows: uruchomienie na drugim komputerze bez edytora, działające zasoby, restart, zamknięcie gry, brak wymaganego połączenia z internetem. Sprawdzić wymagane biblioteki uruchomieniowe i poinstruować testerów, jeśli są potrzebne.
3. Przygotować krótką instrukcję: pobierz i rozpakuj całą paczkę, uruchom właściwy plik, sterowanie, wymagania sprzętowe i sposób zgłaszania błędów. Podawać wersję builda; nie oczekiwać obchodzenia ostrzeżeń systemowych.
4. Zapisać lokalny raport wyniku: wersja, bohater, czas, wynik, otrzymane obrażenia, użycia umiejętności i ewentualny powód niepowodzenia. Opcjonalny eksport do pliku do ręcznego przesłania; bez automatycznej wysyłki i bez danych osobowych w minimum.
5. Przeprowadzić krótki test kilku osób: czy rozumieją cel, widzą zagrożenia, trafiają na problemy z kamerą oraz czy chcą ponowić próbę. Zebrać informacje o sprzęcie, płynności, powtarzających się błędach i odczuciach.

**Gotowe, gdy:** niezależny tester potrafi rozpakować grę i ukończyć lub przegrać próbę offline, ponowić ją oraz przekazać wynik lub opis problemu. Sprawdzić przynajmniej jedną słabszą konfigurację; docelowe wymagania sprzętowe ustalić na podstawie pomiarów.

## Etap 4 — opcjonalni sojusznicy AI

Po działaniu samotnej próby dodać opcję jednego sojusznika AI, którą można wyłączyć. Bot porusza się po tej samej trasie, ma własne HP i skażenie, podejmuje proste decyzje o umiejętności i jest prawidłowym celem wież. Zdefiniować zachowanie po śmierci gracza, wynik przy dotarciu każdego z Runnerów oraz kolejność celowania. Potem sprawdzić większą liczbę botów, ich współpracę, poruszanie i wydajność. Najpierw proste reguły; bardziej zaawansowana nawigacja i podejmowanie decyzji dopiero w odpowiedzi na wyniki testów.

**Gotowe, gdy:** gracz może włączyć lub wyłączyć bota; ten potrafi ukończyć trasę lub zostać pokonany bez blokowania gracza; wieże i mgła działają na obie postacie, a wynik obejmuje obie. Testujemy także śmierć, wcześniejsze dotarcie do mety, restart i brak celu.

## Następne kroki po pre-PoC

Dopiero po uzyskaniu sensownych wyników testu: kolejne postacie i typy wież, ustawienie standard/custom, treningowe mastery oraz współpraca kilku osób. Oddzielny techniczny test sieciowy powinien sprawdzić co najmniej dwa klienty, stan utrzymywany przez serwer, ruch, obrażenia i podstawową widoczność; nie jest wymagany do pierwszego offline builda. Dotychczasowe PoC 2v2 i pełny mecz pozostają późniejszym zakresem opisanym w GDD.

## Decyzje przed rozpoczęciem

- **Silnik i wersja:** UE5 jako kandydat; potwierdzenie po teście instalacji, eksportu, licencji i sprzętu.
- **Pierwsza platforma:** proponowany Windows 64-bit; inne systemy po osobnej ocenie i testach.
- **Pierwszy bohater i umiejętność:** wybrać jeden zestaw; model zastępczy wystarczy.
- **Pierwsze wieże:** wybrać co najmniej jeden typ i parametry startowe; uniknąć zakładania gotowego balansu.
- **Dostęp do testów:** sposób przekazania paczki oraz miejsce zgłaszania uwag; w pierwszej wersji bez kont i automatycznej telemetrii.

Nie podajemy daty dostarczenia ani kosztu jako pewnika. Najpierw mierzymy czas instalacji, pierwszego eksportu i stworzenia grywalnej trasy; po tych krokach można oszacować resztę, uwzględniając sprzęt, dostępność narzędzi, liczbę osób oraz czas na poprawki.

## Źródła techniczne

- Epic, [Packaging Unreal Engine Projects](https://dev.epicgames.com/documentation/unreal-engine/packaging-your-project) — tworzenie samodzielnej paczki i konfiguracje builda.
- Epic, [Setting Up Dedicated Servers](https://dev.epicgames.com/documentation/unreal-engine/setting-up-dedicated-servers-in-unreal-engine) — osobny model dla późniejszych testów sieciowych.
