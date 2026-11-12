# Riftoryn — krytyczny audyt historii i zasad świata

**Data:** 12 listopada 2026. **Zakres:** kanon `world_history.md`, skrót i reguły w `RIFTORYN_GDD.md`, wybrane biografie, oś czasu kodeksu Unity i najnowsze ustalenia o splotycie. To analiza dokumentów, nie badanie graczy ani test gry. Ustalenia poniżej rozróżniają sprzeczność, lukę i ryzyko; propozycje nie zmieniają kanonu. Nie przeanalizowano szczegółowo każdej z 31 biografii, wszystkich grafik ani wszystkich treści Unity.

## Werdykt i oceny

Łańcuch przyczynowy jest wyraźny: rosnące potrzeby prowadzą do rozbudowy infrastruktury, ta zwiększa popyt na splotyt i znaczenie szlaków; niedobór i nieufność przyczyniają się do wojny, a jej koszty do Traktatu i Prób. Nowsze źródła energii, koszt przesyłu oraz niedawna konwersja żywiołów wzmacniają ten szkielet. Najsłabsze miejsca dotyczą sposobu podejmowania i egzekwowania decyzji, granic infrastruktury i tego, jak gracz pozna stawkę. Nie wykazano, że większość poniższych luk jest bezpośrednią sprzecznością.

- **Spójność świata — 76/100:** główne zdarzenia łączą się, lecz codzienne funkcjonowanie siedmiu krain i sieci jest nierówno opisane.
- **Sens rozgrywki i kierunek opowieści — 70/100:** mecz i stawka finału mają odrębne zasady, ale sposób przejścia od meczu gracza do wydarzeń świata pozostaje otwarty.
- **Oryginalność — 73/100:** ścieranie się polityki dostaw, sieci i rywalizacji daje własny charakter; samo zastosowanie żywiołów i turnieju jest znane.
- **Potencjał zaangażowania — 70/100:** osobiste relacje oraz sporne świadectwa są mocne, ale sposoby ich odkrywania w grze nie zostały potwierdzone.
- **Logika zasad — 72/100:** ograniczenia energii są lepsze niż wcześniej; część skutków sieci, ekonomii i klasyfikacji nadal wymaga operacyjnych zasad.

**Średnia: 72/100**, zaokrąglona; subiektywna ocena dokumentacji, nie procent ukończenia, wynik testu czy porównanie rynkowe. Poprzedni audyt używał innych akcentów, więc zmiana punktów nie jest miarą regresu. Brak danych o odbiorcach i konkurencji nie pozwala ocenić pozycji rynkowej ani gotowości wydawniczej.

## Ustalenia i propozycje

### H1 — Publiczne reguły finału są jeszcze puste

**Waga: wysoka; typ: jawna luka.** GDD 2 i `world_history.md`, „Seria meczów i wynik Igrzysk”, mówią, że reguły mają być ogłoszone przed finałem, lecz lista drużyn, punktacja i podział punktów składów mieszanych pozostają otwarte. W szczególności finał obsadza każda z dwóch stron, a tekst równocześnie dopuszcza mieszane reprezentacje: bez przykładu nie wiadomo, kiedy drużyna mieszana gra, jak przydzielić jej wynik i uniknąć korzystnych manipulacji lub niekończących się remisów. Nie jest to dowód sprzeczności, bo format formalnie nie został zaprojektowany.

**Przyczyna:** odłożono projekt klasyfikacji, zachowując deklarację jej jawności w świecie.

**Propozycja:** przygotować roboczy przykład z czterema drużynami, w tym jedną mieszaną: skład, przedmeczowa reprezentacja, dwa wyniki, punkty przy zwycięstwie/remisie, pełne rozliczenie i wybór dogrywki. Sprawdzić brak podwójnego naliczania oraz sytuację wielu remisów; oddzielić zasady sportowego meczu od rozstrzygnięcia politycznego. Nie ustanawiać modelu bez decyzji twórcy. **Właściciel:** projektant systemów i narracji; **rezultat:** przykład oraz 2–3 przypadki graniczne; **szacunek:** 1–2 dni robocze, bez implementacji.

### H2 — Dlaczego obie strony naprawdę wykonają przegrany wynik?

**Waga: wysoka; typ: wiarygodność i siła motywacji.** Kanon opisuje podpisy, arbitrów, nacisk społeczny i utratę przywilejów — podstawy już istnieją. Brakuje konkretnego przypadku, w którym wpływowa strona uznaje przegraną mimo kosztu, albo narusza wynik i spotykają ją proporcjonalne skutki. Przy stawce dotyczącej kontroli wydobycia sankcja utraty przyszłych preferencji może dla niektórych nie wystarczyć.

**Przyczyna:** procedury i zabezpieczenia opisano szczegółowo, ale zachowanie konkretnych stron pod presją nie zostało pokazane.

**Propozycja:** opisać dwa krótkie scenariusze przed finałem: wpływowy sygnatariusz przegrywa i wybiera wykonanie umowy z konkretnych powodów; inny kwestionuje wykonanie i trafia na istniejące procedury odwołania. Pokazać wpływ załóg, umów i opinii mieszkańców, bez gwarancji posłuszeństwa oraz bez pozbawiania podstawowych dostaw. **Właściciel:** narracja; **rezultat:** dwie sceny lub notatki polityczne; **szacunek:** 1–2 dni.

### H3 — Sieć nie ma jeszcze granic ani kosztów przedstawionych przez przykład

**Waga: wysoka; typ: luka systemowa.** Historia mówi, że więcej bliskich przekaźników może ograniczyć straty, a daleki przesył bywa drogi. Nie znamy granic odcinka, dostępu do stacji ani warunków, kiedy dodatkowy przekaźnik pomaga mimo własnego kosztu. Jeżeli dodatkowe stacje zawsze poprawiają sprawność, gracz może pytać, dlaczego nie zbudowano ich wszędzie. Kanon już wymienia koszt budowy i utrzymania, lecz nie pokazuje tego na konkretnym przypadku.

**Przyczyna:** jakość, konfiguracja i koszty są opisane jakościowo; nie sprawdzono jednej trasy z liczbą stacji i decyzją o opłacalności.

**Propozycja:** rozpisać przykład dwóch miast i odległej osady: istniejące przekaźniki, źródło energii, różnicę między kosztem przesyłu i kosztami ochrony nowej stacji oraz powód, dla którego inwestycja nie nastąpiła. Liczby mogą być relacyjne, bez ustanawiania wzoru czy automatycznej korzyści z każdej stacji. **Właściciel:** projektant świata/systemów; **rezultat:** jeden schemat i porównanie dwóch wariantów; **szacunek:** 1 dzień.

### H4 — Własność i dostęp do sieci pozostają niejasne

**Waga: wysoka; typ: luka polityczno-ekonomiczna.** Zerath jest pierwszym wynalazcą; Traktat obejmuje wspólne szlaki, wkłady i przydziały, ale opis nie określa, kto posiada połączenia, ustala opłaty i może odmówić przesyłu między różnymi właścicielami. To istotne dla nowego sporu o samodzielność i wspólny nadzór. Nie wynika z tego automatycznie, że Zerath kontroluje cały kontynent.

**Przyczyna:** kanon rozwinął zasady kopalń i dostaw wcześniej niż technologię dalekiego przesyłu.

**Propozycja:** dla jednej trasy prześledzić właściciela źródła, dwóch węzłów i stacji odbiorczej; zaznaczyć, co wynika z istniejących umów, kto płaci za straty oraz co obejmuje gwarancja ratunkowa w obu wariantach Porozumienia. Nie zakładać, że wszystkie stacje mają jednego właściciela ani że Rada może odciąć chronione usługi. **Właściciel:** narracja/systemy; **rezultat:** przykład umowy i procedura awaryjna; **szacunek:** 1–2 dni.

### H5 — Łączenie żywiołów jest kierunkowe, lecz interpretacja kierunku nie została ustalona

**Waga: wysoka; typ: jawna luka reguł.** `world_history.md` stwierdza, że skuteczność konwersji zależy od kierunku i odpowiada relacjom przewag/odporności; GDD 116 podaje proponowaną macierz ataku. Nie mówi jednak, czy mocniejszy atak oznacza lepsze czy gorsze przyjmowanie energii, a bez tego nie da się wskazać kierunku konwersji. Neutralne pary też wymagają opisu. Procenty obrażeń nie są ustalonymi procentami ładowania.

**Przyczyna:** w opisie świata powiązano dwa systemy przed wyborem interpretacji i liczb.

**Propozycja:** decyzja twórcy na przykładzie Błyskawice → Ogień oraz Ogień → Błyskawice; zapisać, która konwersja jest sprawniejsza i jak zachowują się pary neutralne, bez wprowadzania wartości do balansu walki. Oba kierunki nadal mogą mieć znaczące straty. **Właściciel:** twórca i projektant mechanik; **rezultat:** krótka reguła i przykład; **szacunek:** do 1 dnia po decyzji.

### H6 — Bezpieczeństwo aren i petów nie opowiada jeszcze całej historii

**Waga: wysoka; typ: luka między fabułą i mechaniką.** GDD opisuje pieczęcie wycofujące Runnerów i chroniące Defenderów, podczas gdy mgła zadaje obrażenia, wieże strzelają, a pety mogą być celami. W `world_history.md` pieczęcie rozwinięto w 1 PT. Nie ustalono, co przeżywa wyeliminowany pet, kto go odzyskuje ani co się dzieje przy uszkodzeniu pieczęci lub ataku w trakcie wycofania. Szczegóły technologii są jawnie otwarte; nie twierdzimy, że w kanonie giną zwierzęta.

**Przyczyna:** reguły punktacji, eliminacji i osobnych towarzyszy powstawały na różnych poziomach szczegółu.

**Propozycja:** jeden przykład ataku z Runnerem, petem, mgłą i wieżą; wskazać, kiedy zatrzymuje się obrażenia, kto odzyskuje uczestników oraz jakie procedury obsługują awarię. Nie zmieniać istniejącej reguły, że wycofany Runner nie wraca w tym samym ataku. **Właściciel:** projektant mechanik i narracji; **rezultat:** scenariusz zdarzeń i procedura bezpieczeństwa; **szacunek:** 1–2 dni.

### H7 — Wojna i Traktat potrzebują konkretnych kosztów dla jednego szlaku

**Waga: średnia; typ: niewykorzystany łącznik.** Historia wskazuje spadek łatwo dostępnych złóż w 41 PT, starcia o konwoje w 35 PT, wojnę od 30 PT i Pojedynek Bram w 1 PT. Struktura jest logiczna, lecz czytelnik otrzymuje dużo ogólnych decyzji i mało ciągłego obrazu jednego szlaku: kogo brak kryształu dotknął, kto zablokował dostawę i co zmienił rozejm.

**Przyczyna:** chronologia opisuje wydarzenia z perspektywy całych krain.

**Propozycja:** przygotować jedną linię losów fikcyjnego odcinka: przed kryzysem, ograniczenie dostaw, wojna, Noc, rozejm, 12 ET. Każdemu etapowi przypisać człowieka i materialną konsekwencję. Oznaczyć jako propozycję, dopóki nie zostanie zatwierdzona; nie rozstrzygać przyczyny Nocy. **Właściciel:** narracja; **rezultat:** jedna strona chronologii i scena; **szacunek:** 1–2 dni.

### H8 — Podwójne śledztwo: co wiadomo publicznie, a co wie gracz?

**Waga: średnia; typ: ryzyko ujawnienia tajemnicy.** Siedem raportów o Nocy ma konkretne przesłanki i ograniczenia, co jest zaletą. Biografia Neris ujawnia czytelnikowi jej przestępczy proceder, podczas gdy w świecie uchodzi za wiarygodną osobę. Jeśli identyczny tekst trafi do publicznego kodeksu, gracz pozna odpowiedź natychmiast, a postacie nadal będą jej ufać. Nie jest to samo w sobie sprzeczność: gracz może wiedzieć więcej niż bohaterowie.

**Przyczyna:** nie oznaczono konsekwentnie, które materiały są publiczną wiedzą świata, wiedzą autora albo informacją do odblokowania.

**Propozycja:** oznaczyć informacje etykietami „publiczne”, „prywatne postaci” i „ujawniane graczowi” dla Neris i raportów; wybrać świadomie, czy gra ujawnia jej prawdę od początku. Nie zmieniać biografii ani nie dopisywać powiązania z Nocą. **Właściciel:** narracja; **rezultat:** mapa informacji dla dwóch wątków; **szacunek:** 1 dzień.

### H9 — Zasady splotytu wymagają spójnego modelu strat

**Waga: średnia; typ: ograniczenie danych.** W doświadczeniu 8 PT pierwszy kryształ opróżniono, a drugi, podobny, osiągnął około 10% pojemności. Podobieństwo pozwala orientacyjnie mówić o dużych stratach, ale nie daje dokładnego bilansu; nie wiadomo, ile dodatkowej energii zużyły stacje ani jaka była początkowa zawartość drugiego kryształu. Obecny kanon zastrzega, że podział kosztów jest nieznany i wynik nie jest uniwersalny. Sformułowanie „około 90% straty” pozostaje uproszczeniem.

**Przyczyna:** opis opiera się na stanie dwóch podobnych magazynów, a nie na pełnym pomiarze energii całej sieci.

**Propozycja:** dla przyszłych opowieści zdecydować, czy odbiornik był pusty i czy zasilanie stacji wliczono do bilansu; zachować obecne „około”, dopóki nie ma tych danych. Dla gracza wystarczy komunikat: „po opróżnieniu podobnego kryształu odbiorczy miał około 10% pojemności”. **Właściciel:** projektant świata; **rezultat:** notatka pomiarowa eksperymentu; **szacunek:** kilka godzin.

### H10 — Wieże w mieście i na arenie wymagają odrębnych zabezpieczeń

**Waga: średnia; typ: luka przyczynowości.** W świecie ochrona dróg obejmuje podróżnych, w meczu wieże wybierają wrogie cele zgodnie z GDD. Obecny kanon przyznaje, że cywilne wieże nie muszą rozpoznawać celów bezbłędnie. Nie określa jednak, jak na ruchliwym szlaku unika się ostrzelania konwoju, zwierząt i ekip naprawczych ani kto ponosi odpowiedzialność za pomyłkę.

**Przyczyna:** urządzenie, które wiarygodnie stwarza zagrożenie w grze, wymaga innych warunków użycia jako infrastruktura cywilna.

**Propozycja:** dla jednej stacji opisać strefę ostrzegania, zatwierdzanie ognia i tryb awaryjny oraz sposób odwołania alarmu. To propozycja fabularnego użycia; nie zmienia automatycznego celowania na arenie. **Właściciel:** narracja i projektant mechanik; **rezultat:** karta stacji z jednym przypadkiem pomyłki; **szacunek:** 1 dzień.

### H11 — Bohaterowie i zwierzęta potrzebują historii, nie tylko funkcji

**Waga: średnia; typ: zakres analizy.** Taren i Lucen mają konkretny konflikt i odmienne stanowiska; Rian ma udokumentowaną odpowiedzialność za sabotaż, ale nie przyczynę całej Nocy; Neris ma motyw i ukryte działania. Zarazem materiał wielu postaci należy sprawdzić pod kątem tego, czy ich cele zmieniają sceny, zamiast wyłącznie wyjaśniać wybór na ekranie. Przywoływacze mają towarzyszy o własnym charakterze, a system walki nadaje im funkcję ochronną: potrzebna jest spójna prezentacja tej relacji i bezpieczeństwa.

**Przyczyna:** rozwinięto obsadę szerzej, niż obecnie określono plan prezentacji historii.

**Propozycja:** przejrzeć najpierw po dwie sceny Tarena/Lucena, Rian i Neris: cel, wybór, koszt, wiedza gracza i wynik niezależny od tego, po której stronie trafiają w zwykłym meczu. Następnie przeprowadzić analogiczny przegląd całej obsady, nie dopisując każdemu traumy ani przeciwnika jako obowiązkowej recepty. **Właściciel:** narracja; **rezultat:** cztery karty i plan audytu reszty; **szacunek:** 2–4 dni dla próbki.

**Ocena próbki (wyłącznie dokumenty):** Taren/Lucen 84/100 za konkretny konflikt i niejednoznaczny wybór; Rian 81/100 za sprawczość i konsekwencje, z ryzykiem uproszczenia odpowiedzialności; Neris 79/100 za klarowny interes i silną sprzeczność między wizerunkiem a czynami, z kwestią momentu ujawnienia; Morrak 78/100 za wyrazisty kodeks, wymagający przedstawienia przypadków niepewnego dowodu. Oceny nie przesądzają zatwierdzenia produkcyjnego.

### H12 — Kanon i treści gry nie są zsynchronizowane

**Waga: średnia; typ: rzeczywista luka publikacji.** `world_history.md` i GDD zawierają niedawne zasady źródeł, przesyłu, bestii i eksperymentu 8 PT; `Assets/Resources/codex_timeline.json` ma starsze ogólne wpisy 154 i 91 PT i nie zawiera tej nowej wiedzy w osi czasu. Starszy tekst nie musi być sprzeczny, ale gracz nie otrzymuje pełnego obrazu. Wcześniejszy audyt z 11 listopada zawiera również propozycje niezgodne z aktualnym kanonem, np. utożsamienie sabotażu Rian z przyczyną całej Nocy.

**Przyczyna:** kanon, biografie, kodeks i historyczne audyty służą różnym celom i były aktualizowane w różnym czasie.

**Propozycja:** stworzyć mapę synchronizacji: źródło, polski i angielski wpis Unity, status publiczny/spoiler i kontrola zgodności; oznaczyć wcześniejszy audyt jako historyczny, a treści gry aktualizować po uzgodnieniu sposobu ujawniania. Osobno sprawdzić układ, tłumaczenia i działanie w Unity. **Właściciel:** narracja/lokalizacja; **rezultat:** lista zmian i test obu języków; **szacunek:** 1–3 dni, zależnie od zakresu.

## Czego nie uznawać za dziurę bez dalszych dowodów

- Pochodzenie żywiołów, przyczyna Nocy, tożsamość Morraka i wspomnienia Noctena są świadomie otwarte. Problemem byłoby dopiero obiecanie odpowiedzi i brak planu, jak podawać sprawdzalne tropy.
- Pierwsze udokumentowane zastosowanie splotytu w 154 PT nie jest pierwszym znalezieniem minerału; 8 PT wypada pięć lat przed 3 PT. Eksperyment nie dowodzi przyczyny Nocy.
- Mieszane drużyny i różne poglądy postaci są zgodne z kanonem; otwarty jest sposób rozliczenia ich punktów w finałach.
- Prawidłowo użytkowany splotyt może być stabilny, a jednocześnie naładowany przyciągać niektóre bestie: to dwa różne zjawiska. Rzadkość spotkań nie wyklucza strachu ani politycznych nadużyć.

## Kolejność pracy

1. **Decyzje twórcy:** H5 — kierunek konwersji; H1 — choćby próbny sposób przypisywania punktów i reprezentacji.
2. **Test wiarygodności systemu:** H2–H4 — jedna przegrana polityczna i jeden szlak z dostępem do stacji, kosztami oraz procedurą ratunkową. Szacunek 3–5 dni pracy projektowej, nie termin wdrożenia.
3. **Most między światem a grą:** H6, H10 i H12 — scenariusz bezpieczeństwa ludzi i petów, jedna wieża cywilna i plan synchronizacji kodeksu; 3–5 dni plus test w Unity.
4. **Odbiór opowieści:** H7–H8, H11 — jeden szlak, mapa wiedzy o Neris i Nocy, próbka scen bohaterów. Ocenić z czytelnikiem, który nie zna dokumentów; nie traktować małej próby jako badania rynku.

Największym zagrożeniem dla projektu nie jest pojedyncza niewyjaśniona tajemnica, lecz obietnica świata, którego zależności zostaną opisane tylko w dokumentach. Najbardziej wartościowe kolejne sprawdzenie to mały, konkretny przypadek pokazujący jednocześnie koszt energii, ludzi zależnych od szlaku i konsekwencję politycznej decyzji.
