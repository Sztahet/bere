# Riftoryn — audyt narracyjny historii

Data: 12 listopada 2026

## Zakres i werdykt

Ocena dotyczy polskiego kanonu w `world_history.md`, biografii w `RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md` oraz roboczego przeglądu motywacji. Punktem odniesienia jest stan dokumentów, nie pełny audyt implementacji Unity ani ocena gotowej kampanii. Wcześniejszy audyt spójności świata z tego samego dnia szczegółowo omawia instytucje i technikę; poniższy raport skupia się na dramacie, postaciach i doświadczeniu gracza. Biografie mają różne statusy zatwierdzenia; propozycje poniżej nie zmieniają kanonu.

Riftoryn ma silną podstawę: wielka polityka wyrasta z codziennej zależności od szlaków, a Noc pozostawia przestrzeń na śledztwo bez wskazywania winnego. Najważniejsze zadanie narracyjne to zaprojektowanie, jak gracz stopniowo poznaje świat i poszczególnych bohaterów, grając nimi w meczach i rozwijając mastery postaci. To dobry fundament dla prototypu narracyjnego, a nie dowód gotowości kompletnej kampanii.

Oceny są subiektywną oceną materiału projektowego, nie pomiarem jakości gotowej gry:

- Spójność świata: **82/100** — przyczyny, historia i ograniczenia są przemyślane, choć złożone do przekazania.
- Kierunek opowieści i rozgrywki: **67/100** — stawka Igrzysk istnieje, lecz docelowy sposób poznawania historii przez mastery nie jest jeszcze rozpisany.
- Oryginalność: **72/100** — infrastruktura i polityka jako źródło napięcia wyróżniają świat; część biografii ma powtarzalną konstrukcję.
- Angażowanie: **74/100** — mocne relacje i tajemnice, ale brakuje zaplanowanych odpowiedzi i interakcji.
- Logiczna spójność: **80/100** — dokument odróżnia fakty od hipotez; kilka połączeń między opowieścią a regułami wymaga doprecyzowania.

Orientacyjna średnia pięciu ocen: **75/100**. Nie oznacza oceny rynku ani gotowości produkcyjnej.

## Założenie sezonowego odkrywania historii

Doprecyzowanie autora: nie chcemy ujawniać graczom na początku tego, co wydarzyło się podczas Nocy Zgasłych Wież, ani ukrytych wątków wybranych bohaterów. Kolejne wątki będą dodawane w następnych sezonach. Jest to świadomy kierunek narracji, a nie brak wymagający naprawy. Nie zakłada on też, że istnieje już ustalona pełna odpowiedź na przyczynę Nocy: w kanonie pozostaje ona niepotwierdzona.

Poniższe propozycje dotyczą wewnętrznego planowania informacji i doświadczenia gracza, a nie obowiązku ujawnienia tajemnic w pierwszym sezonie. Materiał projektowy z sekretami powinien pozostać oddzielony od treści dostępnych graczowi. Tempo, kolejność i zakres ujawnień pozostają decyzją autora.

## Rola gracza i dostęp do historii

Doprecyzowanie autora: gracz w meczu wciela się w jednego wybranego bohatera, podobnie jak w grach Dota i League of Legends. Może grać różnymi bohaterami i stopniowo poznawać ich historie poprzez mastery postaci. Rozgrywka gracza nie czyni go osobną postacią fabuły ani nie oznacza, że przebieg meczu zmienia historię świata. Konkretne progi, nagrody i sposoby prezentacji pozostają do zaprojektowania.

Historie udostępniane teraz w PoC służą testom. Nie przesądza to, które informacje będą od razu dostępne wszystkim graczom w docelowej wersji. Dostępność historii w PoC nie oznacza też, że wszystkie sekrety i późniejsze wątki są jawne na starcie gry.

## Ustalenia i propozycje

### N1 — Należy zaprojektować odkrywanie historii przez mastery
**Priorytet: wysoki.** Historia świata przewiduje stopniowe poznawanie postaci przez grę nimi, lecz progi i sposób prezentacji pozostają otwarte (`world_history.md:322`). Wcześniejsza wersja audytu błędnie zakładała osobną rolę gracza w fabule, decyzje dotyczące transportu i fabularne następstwa meczu. Takie założenia nie wynikają z przyjętego modelu gry.

**Propozycja:** dla jednej wybranej postaci przygotować roboczy podział informacji na materiał początkowy i kolejne odblokowania mastery. Każdy fragment powinien działać samodzielnie, zachowywać ustalone sekrety i dać się rozszerzać w kolejnych sezonach. Następnie przetestować, czy gracz rozumie osobowość i motywację bohatera bez poznania całej biografii. Konkretne progi i format pozostawić do decyzji; testowa dostępność treści w PoC nie jest deklaracją dostępności w wydaniu. **Rezultat:** sprawdzalny sposób łączenia gry bohaterem z poznawaniem jego historii. Szacunek: 2–4 dni na prototyp treści i test; projektant narracji i UX.

### N2 — Obietnice bohaterów nie mają jeszcze przewidywalnego związku z wynikiem
**Priorytet: wysoki.** Świat opisuje, że wyniki wpływają na kolejność obietnic, ale ich wiążący zakres, finansowanie i przelicznik są otwarte (`world_history.md:282–300`). Kael, Pyrra czy Eron zabiegają o konkretne sprawy; bez tej więzi trudno wyjaśnić, dlaczego ich udział w fabularnych Igrzyskach miałby im pomóc. Nie zakładamy przy tym, że wyniki meczów graczy rozstrzygają fabułę. Analiza motywacji już wskazuje ograniczenia tej zgodności.

**Propozycja:** dla jednego wycinka ustalić jawne minimum: kto zobowiązuje się do projektu, z jakiej puli i co dokładnie oznacza przesunięcie jego kolejności. Przykładowo program może przewidywać dokumentację i termin rozpatrzenia naprawy stacji Zeta, bez gwarancji natychmiastowej budowy. Sprawdzić wariant wygranej i przegranej strony oraz remis. To propozycja mechanizmu, nie przyjęta obietnica. **Rezultat:** osobista stawka bohatera staje się sprawdzalna bez obiecywania rzeczy, których turniej nie gwarantuje. 1–3 dni: projektant narracji i systemów.

### N3 — Tajemnice wymagają wewnętrznego planu sezonowego
**Priorytet: wysoki.** Siedem raportów Nocy zawiera konkurujące hipotezy (`world_history.md:169–201`); Dara, Rian i Nocten mają osobne niewiadome. Dokument nie ustala jeszcze, jak gracz zdobywa i weryfikuje wskazówki. Ciągłe odkładanie odpowiedzi mogłoby osłabić zaufanie do śledztwa; sama nieznana przyczyna Nocy jest zamierzona i nie jest błędem.

**Propozycja:** wewnętrznie rozpisać, co gracz może wiedzieć na początku, jakie pytania i wątki można otwierać w późniejszych sezonach oraz które informacje pozostają sekretem. Dla każdej planowanej wskazówki określić źródło, wiarygodność i to, czego nie dowodzi; nie trzeba ujawniać jej w pierwszym sezonie ani wymyślać już teraz ostatecznego rozwiązania Nocy. Pierwszy sezon może zakończyć satysfakcjonująco lokalny wątek niezwiązany z rozwiązaniem głównej tajemnicy. **Rezultat:** sezony mogą rozwijać ciekawość bez zdradzania sekretów przed czasem. Szacunek: 2–4 dni na roboczą mapę informacji; projektant narracji.

### N4 — Część obsady powtarza ten sam przebieg przemiany
**Priorytet: średni.** Iskar uczy się planu awaryjnego, Zet sprawdzania ostrzeżeń, Kiran słuchania górniczki, a Miko testowania urządzenia (`RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md:139, 348, 364, 428`). Każda historia ma inny zawód, lecz kilka kończy się podobnym wnioskiem: po błędzie bohater słucha innych i działa ostrożniej. Ich głosy mogą zlać się w pamięci gracza. To obserwacja struktury, nie żądanie zmiany zatwierdzonych historii.

**Propozycja:** przed zatwierdzeniem pozostałych historii zestawić dla każdej scenę teraźniejszą, pragnienie, wadę i decyzję pod presją. Przykładowo Zet może spierać się o to, kiedy przerwać weryfikację, a Kiran o prawo pracowników do zatrzymania wyrobiska mimo utraty zarobków; nie muszą przechodzić ponownie tej samej lekcji. **Rezultat:** różne konflikty wynikają z już istniejących doświadczeń. 1–2 dni: scenarzysta, potem przegląd przez autora świata.

### N5 — Silne relacje potrzebują scen i przestrzeni na odmowę
**Priorytet: wysoki.** Taren i Lucen mają konkretną stratę, rozbieżne wspomnienia i poglądy (`historie:147–159, 510–524`), a siostry odnalazły się przed powodzią (`world_history.md:324`). To dobry materiał na dramat, ale obecny opis nie określa, jak ich relacje ujawniają się w grze. Łatwe pojednanie odebrałoby znaczenie latom rozłąki i prawu Lucena do odmowy.

**Propozycja:** przygotować dwie krótkie sceny Tarena i Lucena jako potencjalne fragmenty historii odblokowywane w odpowiednim czasie: pierwsze spotkanie przy liście kwalifikacji oraz późniejszą rozmowę o jednym zachowanym liście. W obu dopuszczać brak porozumienia; sprawa przyczyny awarii Miry pozostaje nierozstrzygnięta. Siostrom dać scenę o konsekwencjach powodzi, wykorzystując już zatwierdzoną historię bez jej przepisywania. **Rezultat:** relacje istnieją w teraźniejszości gry, a nie wyłącznie w biografiach. 2–3 dni: scenarzysta.

### N6 — Neris i Morrak potrzebują zasad ujawniania informacji
**Priorytet: średni.** Biografia Neris wprost zdradza jej rolę organizatorki napadów (`historie:500–508`); Morrak prowadzi własne dochodzenia i zabija tych, których uzna za winnych (`:185–197`). Jeśli te informacje pojawią się od razu w publicznym kodeksie, śledztwo i reakcje innych postaci mogą stracić napięcie. Dokumenty nie przesądzają jednak, że gracz dostanie pełne biografie na początku.

**Propozycja:** sporządzić matrycę „wie publiczność / wie bohater / może odkryć gracz” dla jednej sceny każdego z nich. Neris najpierw przedstawia wiarygodny raport; później gracz może porównać go z niezależnym świadectwem. W przypadku Morraka pokazać także ryzyko błędnego osądu bez ustanawiania nowej winy czy pochodzenia postaci. **Rezultat:** napięcie wynika z dostępnych dowodów, a nie z ukrywania przed graczem informacji arbitralnie. 1–2 dni: scenarzysta i projektant UX narracji.

### N7 — Początkowa prezentacja wymaga mniejszej dawki pojęć
**Priorytet: średni.** Historia wprowadza splotyt, siedem krain, Noc, rozejm, Traktat, Radę, Urząd, Próby, Wielkie Igrzyska i dwa warianty. Wszystkie mają funkcję, lecz jednoczesne wyłożenie ich przed pierwszą decyzją może utrudnić wejście w opowieść.

**Propozycja:** w początkowej prezentacji świata skupić się na kilku faktach potrzebnych do zrozumienia bohaterów i areny, a pozostałe informacje udostępniać stopniowo w odpowiednich materiałach i odblokowaniach. Sprawdzić z 3–5 osobami niezaznajomionymi z projektem, czy potrafią opisać podstawową sytuację bez znajomości sekretów. **Rezultat:** prostszy punkt wejścia bez usuwania głębi. 1 dzień na szkic i osobna krótka sesja testowa: scenarzysta i osoba prowadząca testy.

## Postacie: ocena wybranych wątków

Oceny odnoszą się do potencjału opisanej historii, nie do ostatecznego zatwierdzenia ani jakości implementacji. Nie oceniano tu liczbowo całej obsady.

- **Rian — 87/100:** świadoma wina, konkretne konsekwencje i brak wymuszonego przebaczenia. Następny krok: sprawdzić, jak jej przeszłość wybrzmiewa w scenach i dostępie do areny.
- **Taren i Lucen — 85/100 jako para:** osobisty konflikt przekłada się na dwie wizje nadzoru bez dowodzenia przyczyny awarii. Potrzebują scen z obu perspektyw i prawa do nierozwiązania sporu.
- **Dara — 81/100:** własna obawa łączy się z bezpieczeństwem górników, a podejrzenie nie rozstrzyga Nocy. Potrzebuje osiągalnego pierwszego tropu i decyzji, komu zaufać.
- **Saila — 80/100:** osobiste doświadczenie i dwa realne koszty pozwalają jej nie wybierać strony z góry. Potrzebuje sceny decyzji dotyczącej transportu i jej następstwa.
- **Nocten — 78/100:** pamiętnik i sprzeczne wspomnienia tworzą rozpoznawalną tajemnicę. Potrzebuje wewnętrznego planu ujawniania w kolejnych sezonach i wyraźnych granic wiedzy gracza; odpowiedź nie musi pojawić się na początku.
- **Neris — 77/100:** konkretne egoistyczne motywy i pozycja informatorki dają aktywną antagonistkę. Potrzebuje planu tropów i sposobu, w jaki jej działania mogą zostać podważone.
- **Siostry — 84/100 jako wspólny wątek:** rozłąka, ponowne spotkanie i powódź dają relacji własną historię; nie wymagają teraz rozszerzenia zatwierdzonych biografii. Warto sprawdzić ich obecną dynamikę w krótkiej scenie.

## Co zachować

- Konsekwentne rozróżnienie faktów, świadectw i hipotez wokół Nocy.
- Powiązanie polityki z transportem, żywnością, pracą i bezpieczeństwem zwykłych ludzi.
- Niejednorodność krain oraz możliwość współpracy bohaterów o różnych poglądach.
- Brak automatycznego przebaczenia Rian i brak obowiązku pojednania Tarena z Lucenem.
- Ciepło więzi z towarzyszami, przy zachowaniu ich granic i niezależności.

## Zalecana kolejność

1. Zaprojektować i przetestować stopniowe odkrywanie historii jednej postaci przez mastery (N1).
2. Ustalić jedną sprawdzalną obietnicę oraz jej los w obu wynikach (N2).
3. Przygotować wewnętrzną mapę wiedzy i potencjalnych ujawnień na kolejne sezony, bez obowiązku zdradzania odpowiedzi na początku (N3).
4. Opracować i przetestować prezentację relacji oraz ujawniania wiedzy w odpowiednich odblokowaniach i sezonach (N5–N7).
5. Porównać pozostałe niezatwierdzone biografie pod kątem odmiennych decyzji (N4).

Terminy są szacunkami pracy nad małym prototypem, nie harmonogramem całej gry. Wszystkie przykłady wymagają decyzji autora; żadna z propozycji nie ustanawia nowych faktów świata.
