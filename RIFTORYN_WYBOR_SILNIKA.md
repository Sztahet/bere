# Riftoryn — wybór silnika: Godot, Unreal Engine czy Unity

**Analiza na 7 listopada 2026 · rekomendacja, nie zatwierdzona zmiana GDD.** Przeanalizowano główne zasady i zakres pre-PoC, późniejszy PoC, specyfikacje bohaterów, wież, żywiołów, technologii, świata i treningu w `RIFTORYN_GDD.md`. Szczegółowe wartości nadal podlegają decyzjom i testom. Porównanie oparto na dokumentacji producentów; nie wykonano porównywalnych buildów ani pomiarów wszystkich trzech silników.

## Rekomendacja

**Wybrać Godot 4 jako pierwszy silnik projektu i zbudować w nim pre-PoC 3D, z obowiązkową wczesną weryfikacją sieci przed uznaniem wyboru za trwały.** Nie rekomenduję już automatycznego wyboru UE5. Riftoryn potrzebuje czytelnej stylizowanej gry TPP i szybkiego rozwoju, a nie fotorealistycznego renderingu. Godot jest już dostępny w naszym środowisku (wykryto 4.6.1) i nie wymaga opłat licencyjnych od gry. Jego większe ryzyko to ilość pracy nad responsywną, autorytatywną grą sieciową oraz utrzymanie takiej infrastruktury małym zespołem. Obecność programu nie dowodzi jeszcze, że eksport klienta, eksport serwera lub działanie na docelowym sprzęcie zostały sprawdzone.

**Unreal jest silniejszą alternatywą dla docelowego 5v5:** ma rozbudowane systemy replikacji, ruchu postaci i umiejętności sieciowych, ale ma bardziej złożony proces pracy i większy próg opanowania. Nie zakładajmy, że bez pomiaru jego build będzie za ciężki lub że narzędzia sieciowe automatycznie rozwiążą nasze mechaniki. **Unity** jest sensownym środkiem: C#, 3D i kilka ścieżek multiplayer, lecz wymaga wyboru i integracji stosu sieciowego oraz śledzenia warunków licencji. Nie ma podstaw do uznania go za wykluczony.

Warunek: jeżeli rzeczywisty test Godot pokaże, że nie umiemy osiągnąć płynnego ruchu i poprawnej widoczności przy docelowych warunkach, pierwszą alternatywą do porównania jest Unreal. Nie przenosimy produkcji automatycznie po pierwszym błędzie: najpierw identyfikujemy koszt i przyczynę. Wybór 3D nie zmienia wymagań samej gry.

## Aktualizacja: większość prac wykonuje agent AI

[WERYFIKACJA W TOKU] Wcześniejsza rekomendacja Godot nie uwzględniała wystarczająco tego, że większość implementacji i pracy w edytorze wykona Codex. Nie wybieramy więc jeszcze docelowego silnika: **porównujemy w praktyce Unity i Godot, a Unreal pozostaje mocnym kandydatem ze względu na oficjalny MCP i narzędzia sieciowe**.

- W bieżącej sesji nie ma połączonego MCP żadnego z tych trzech silników. Godot 4.6.1 działa z wiersza poleceń; wykrycie Unreal i Unity w PATH nie powiodło się i nie dowodzi, że nie są zainstalowane.
- Unity oferuje [oficjalny plugin dla Codex, CLI i MCP](https://unity.com/features/ai), a Epic dokumentuje [Unreal MCP z konfiguracją dla Codex](https://dev.epicgames.com/documentation/unreal-engine/unreal-mcp-in-unreal-editor). Integracji nie uruchomiono ani nie porównano w tym środowisku; dokumentacja Unreal ostrzega, że część funkcji jest niekompletna. Unity opisuje swoje narzędzia AI jako beta.
- W Godot można pracować na tekstowych plikach scen i skryptach oraz uruchamiać polecenia edytora; istnieją też MCP społeczności. Konkretny dodatek trzeba osobno ocenić pod kątem wymagań, wersji, uprawnień i tego, czy przesyła dane poza komputer. Sam katalog narzędzi nie dowodzi jakości czy bezpiecznej integracji.
- Test porównawczy ma obejmować: założenie projektu, scenę 3D TPP, automatyczną wieżę, umiejętność, mgłę, wykonanie i uruchomienie builda, odczyt błędów, poprawkę i ponowny test. Mierzymy faktyczną liczbę wymaganych działań użytkownika, czas pracy agenta, niezawodność i jakość podglądu. Następnie weryfikujemy mały test sieciowy z ukrytymi danymi. Deklaracje producentów nie są wynikiem tego testu.

## Co naprawdę wymaga GDD

1. **Pre-PoC i trening:** klient Windows offline, sterowanie postacią i kamera TPP, jedna spiralna trasa 3D, automatyczne wieże, pociski i kolizje, HP, zdolność, mgła, licznik, wynik i restart. Bot jest poza obowiązkowym minimum; docelowo standard/custom, wspólna gra i mastery.
2. **Docelowy mecz:** 10 graczy w 5v5, oddzielne identyczne labirynty, naprzemienne ataki, fazy budowy i inwestycji, draft, stała obrona, automatyczny ostrzał i ręczny pilotaż. Czasy i wynik muszą wynikać z wiarygodnego stanu meczu, w tym obsługi pauzy/reconnectu.
3. **Najtrudniejszy wymóg sieciowy:** serwer rozstrzyga ruch i interakcje, obrażenia, strzały, budowę, rezerwacje i ekonomię; klienci dostają jedynie uprawnione informacje. Odkryta wieża pozostaje w pamięci z ostatnim znanym stanem, ale nie ujawnia zdalnych zmian. Defender nie powinien dostawać pozycji niewykrytego Runnera; obserwator podlega tym samym zasadom. Wizualne ukrycie obiektu, który klient już otrzymał, nie wystarczy.
4. **Akcja i wiele systemów:** płynny ruch TPP i pilotaż przy opóźnieniu, pociski blokowane ścianami, mgła wzdłuż trasy i licznik skażenia, efekty statusów, interakcje żywiołów, umiejętności, moby, pety, boty i dziesiątki bohaterów. Wartości i część zasad są wciąż otwarte.
5. **Produkcja:** stylizowane, czytelne 3D; postacie, animacje, efekty, UI, dźwięki i możliwość łatwego strojenia danych. Nie zakładamy konieczności fotorealizmu. Czas pracy i kompetencje zespołu, wydajność na komputerach graczy oraz wielkość klienta i serwera muszą zostać zmierzone.
6. **Usługi poza silnikiem:** konta, matchmaking, znajomi, przechowywanie mastery i wyników, hosting, monitoring i aktualizacje. Żaden z trzech silników nie zapewni całego tego produktu automatycznie; dostępne usługi i ich koszty wymagają osobnej oceny. Offline trening i boty nie wymagają serwera.

## Porównanie praktyczne

### Godot 4

- **Mocne strony:** gotowy do pracy w naszym środowisku; otwarty kod na licencji MIT, bez tantiem silnika; skryptowy workflow i eksport 3D sprzyjają iteracji. Obsługuje sieć wysokiego poziomu, filtry widoczności synchronizacji oraz eksport serwera headless. Dokumentacja wyraźnie zaznacza, że sama sieć nie zabezpiecza logiki gry. [Sieć Godot](https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html), [serwer](https://docs.godotengine.org/en/stable/tutorials/export/exporting_for_dedicated_servers.html).
- **Ryzyka:** więcej pracy projektowej i integracyjnej przy przewidywaniu ruchu, korygowaniu rozbieżności, replikacji skomplikowanych umiejętności i ograniczaniu danych o przeciwniku. Nie twierdzimy, że te funkcje są niemożliwe ani że sam silnik przesądza wydajność 5v5. Konsolowy eksport nie jest udostępniany jako oficjalny otwarty eksport; wymaga dalszego rozeznania rozwiązań podmiotów trzecich i warunków platform. [FAQ Godot](https://docs.godotengine.org/en/stable/about/faq.html).
- **Najlepszy powód wyboru dla nas:** możemy szybko sprawdzić 3D i codzienny workflow bez kupowania licencji; równolegle wcześnie zbadamy ryzyko sieci.

### Unreal Engine 5

- **Mocne strony:** silnie rozwinięte narzędzia dla akcji 3D oraz model klient–serwer. Relevancy pozwala ograniczać replikację aktorów zależnie od połączenia; Gameplay Ability System oferuje m.in. atrybuty, efekty, cooldowny i wsparcie predykcji sieciowej. To punkty wyjścia dla naszej widoczności i bohaterów, a nie kompletna implementacja GDD. [Relevancy](https://dev.epicgames.com/documentation/unreal-engine/actor-relevancy-in-unreal-engine), [Gameplay Ability System](https://dev.epicgames.com/documentation/unreal-engine/understanding-the-unreal-engine-gameplay-ability-system).
- **Ryzyka:** nauka silnika, Blueprintów/C++ oraz narzędzi sieciowych może obciążyć mały zespół. Przy dedykowanym serwerze trzeba zweryfikować dla wybranej wersji wymagania narzędzi i budowania ze źródeł. Trzeba zmierzyć wymagania sprzętu i wielkość builda; nie mamy pomiarów. Potężne narzędzia nie są konieczne, by osiągnąć stylizowany wygląd.
- **Kiedy wybrać:** gdy priorytetem jest minimalizacja ryzyka systemów sieciowych, mamy zasoby na opanowanie narzędzi, a test 3D i dwuklientowy działa dla naszego zespołu lepiej niż w Godot.

### Unity

- **Mocne strony:** C#, dojrzałe narzędzia 3D, budowanie klienta i serwera oraz rozwiązania multiplayer. Można wybrać właściwą bibliotekę do architektury gry; nie trzeba przyjmować, że usługa chmurowa jest konieczna do treningu offline. [Opcje sesji Unity](https://docs.unity3d.com/6000.0/Documentation/Manual/building-blocks-multiplayer-sessions.html).
- **Ryzyka:** wcześniej trzeba wybrać konkretny stos sieciowy i sprawdzić, co rzeczywiście dostarcza w zakresie przewidywania, widoczności i serwera dla naszych potrzeb. Pakiety, usługi, ich kompatybilność i opłaty mogą zmieniać się niezależnie; porównujemy konkretne wersje, a nie obietnicę „Unity ma multiplayer”.
- **Kiedy wybrać:** jeśli zespół preferuje C#, a porównywalny test i koszty okażą się korzystniejsze. Brak danych o naszych kompetencjach i wydajności projektu uniemożliwia uczciwe ogłoszenie tego dziś.

## Licencja i dystrybucja — stan do ponownego sprawdzenia

- **Godot:** licencja MIT, bez tantiem od gry; trzeba spełnić wymogi informacji licencyjnej, także dla komponentów zewnętrznych i zasobów. [Licencje](https://docs.godotengine.org/en/stable/about/complying_with_licenses.html).
- **Unreal:** przy standardowych warunkach gry obowiązuje co do zasady 5% royalty od kwalifikującego się przychodu po pierwszym 1 mln USD przychodu brutto produktu w całym okresie; istnieje m.in. wyłączenie dla kwartałów poniżej 10 tys. USD oraz dodatkowe wyjątki i warunki. To nie jest procent z zysku ani z każdej instalacji. [Warunki Epic](https://www.unrealengine.com/eula/unreal), [raportowanie](https://www.unrealengine.com/news/unreal-engines-improved-royalty-reporting-system).
- **Unity:** Personal jest darmowy dla uprawnionych osób i małych organizacji poniżej progu 200 tys. USD przychodów i pozyskanego finansowania w ostatnich 12 miesiącach, według zasad Unity; powyżej obowiązują odpowiednie warunki płatnych planów. Opłatę Runtime Fee dla gier anulowano. Sprawdzić aktualną ofertę i warunki w chwili zakupu. [Unity Personal](https://unity.com/products/unity-personal), [komunikat Unity](https://unity.com/blog/unity-is-canceling-the-runtime-fee).

Koszty serwerów, usług, sklepów, assetów, portowania i pracy są odrębne od licencji silnika. Dystrybucja offline ZIP nie wymaga własnego serwera gry; wymagania systemowe, pliki zależne i ostrzeżenia systemu sprawdzamy na osobnym komputerze. Te porównania nie zastępują analizy umowy obowiązującej przy komercyjnym wydaniu.

## Próba potwierdzająca wybór

**Bramka A — 3D offline:** w Godot zbudować jedną drogę, postać TPP, automatyczną wieżę z pociskiem, umiejętność, mgłę i wynik; wyeksportować na Windows i uruchomić na osobnym PC bez edytora oraz internetu. Zmierzyć czas iteracji, FPS i płynność kamery na wybranych urządzeniach, rozmiar paczki oraz błędy. Wartości porównywać na tym samym sprzęcie, przy zbliżonej jakości i tych samych ustawieniach; nie obiecywać FPS przed pomiarem.

**Bramka B — najtrudniejsze ryzyko sieciowe:** jak najwcześniej po A uruchomić proces dedykowanego serwera i dwa oddzielne klienty. Przetestować na serwerze ruch i trafienia, przejęcie wieży, wykrycie, utratę widoczności i pamięć ostatniego stanu; zweryfikować rzeczywiste dane sieciowe, a nie sam obraz. Uwzględnić zasymulowane opóźnienie i utratę pakietów, reconnect oraz pomiar obciążenia. Ten mały test nie jest pełnym 5v5 ani dowodem gotowości produkcyjnej.

**Bramka C — rozszerzenie obciążenia:** stopniowo sprawdzić 10 połączeń, dwie kopie mapy, większą liczbę wież i pocisków, moby oraz koszty na kliencie i serwerze. Z góry zapisać sprzęt, warunki sieci, liczbę obiektów oraz wymagane progi płynności i opóźnienia; określić je przed oceną wyników. Zmierzyć również koszt oraz trudność wdrożenia i utrzymania.

Jeśli B lub C nie spełnią wspólnie ustalonych progów po rozsądnej próbie poprawy, wykonać możliwie ten sam ograniczony test w UE5; porównać także Unity, jeśli C# i jego stos sieciowy będą nam pasować. Dopiero wtedy potwierdzić docelowy silnik. Żaden obecny wynik nie potwierdza gotowości sieciowej, grafiki ani wydajności któregokolwiek silnika dla Riftoryn.
