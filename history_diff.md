# Riftoryn — przegląd spójności historii świata

## Zakres i wniosek

Przejrzałem historię i oś czasu w [RIFTORYN_GDD.md](./RIFTORYN_GDD.md), jej prezentację w [riftoryn-lore.js](./riftoryn-lore.js), opisy miejsc w [riftoryn-swiat.js](./riftoryn-swiat.js) oraz powiązane historie bohaterów.

W repozytorium świat występuje jako **Riftoryn**; wyszukiwanie pisowni „Ryfrorin” nie zwróciło trafień. Główna opowieść jest spójna: Eryndor tworzy siedem krain o odmiennych tradycjach, lecz wspólnych szlakach i żywiołach; niedobór splotytu prowadzi do wojny, katastrofa w Val Meris wymusza rozejm, a ograniczone spory o zasoby przechodzą pod system Prób. Oś czasu w GDD i jej skrócona wersja w kronice prototypu pokrywają się. Mapa pokazuje siedem krain oraz Val Meris jako centralny węzeł — to osiem ośrodków, nie osiem krain.

Historia świata jest jednak wyraźnie oznaczona w GDD jako **propozycja do przeglądu, nie zatwierdzony kanon**. Poniższe punkty to propozycje doprecyzowania lub odnotowane zmiany. Pięć dostarczonych ilustracji zostało podłączonych do kroniki, a ewolucję składów od championów krain do mieszanych drużyn dopisano do GDD i osi czasu.

## Propozycje i zauważone rozbieżności

Wnioski z tego przeglądu zostały wdrożone jako doprecyzowania wciąż roboczej koncepcji. Nie oznaczają zatwierdzenia świata jako kanonu.

### 1. Ujednolicić oznaczenie wersji koncepcji — wykonano

Przed zmianą kronika i mapa używały odmiennych etykiet roboczych („WERSJA ROBOCZA” oraz „KONCEPCJA · V0.1”).

**Wykonano:** kronika i mapa używają etykiety „KONCEPCJA FABULARNA · v0.1”. GDD wskazuje tę samą wersję i zachowuje informację, że to propozycja do przeglądu.

Źródła: [GDD — historia świata](./RIFTORYN_GDD.md#L555), [GDD — mapa świata](./RIFTORYN_GDD.md#L687), [widok kroniki](./riftoryn-lore.js#L11), [widok mapy](./riftoryn-swiat.js#L12).

### 2. Ilustracje kroniki — podłączone

Pięć ilustracji dostarczonych do kroniki zapisano w `assets/lore/` i podłączono w `riftoryn-lore.js` pod nazwami `01-siedem-szlakow.png`–`05-era-prob.png`. Prompty poniżej pozostają zapisem pierwotnego kierunku dla scen; gotowe ilustracje zastępują placeholdery w widoku kroniki.

**Wykonano:** pliki przypisano kolejno do narodzin krain, rozwoju sieci splotu, wojny, drogi do traktatu i ery Prób.

#### Wspólne wskazówki wizualne

Zachować spójność między pięcioma scenami: panoramiczna ilustracja fantasy do gry, szeroki kadr 16:9, ręcznie malowane światło, delikatna geometryczna faktura materiałów i architektury, czytelne sylwetki oraz bogate, ale kontrolowane detale. Klimat poważny i przygodowy, bez fotorealizmu, nowoczesnych przedmiotów, tekstu, liter, logotypów ani znaków wodnych. Przedstawiać różne tradycje i żywioły jako część wspólnego świata, a nie odrębne, jednorodne krainy żywiołów. Każdy prompt poniżej jest samodzielny i powinien zachować tę samą oprawę.

#### `01-siedem-szlakow.png` — Narodziny krain

> Panoramic fantasy game illustration, wide 16:9 composition. At dawn, a busy ancient trade road connects several distinct settlements across one continuous, diverse landscape: a northern mountain pass, fertile southern river valley, western coastal archives and eastern workshops are visible in the distance, not separated into elemental zones. In the foreground, merchants, craftspeople and travelers of varied backgrounds meet around a simple waystone and exchange goods beneath colorful caravan awnings. Subtle hints of different elemental crafts appear naturally in daily life: a warm forge, an irrigated field, a lantern and a small arcane crystal being studied. No defensive towers yet. The mood is hopeful, communal and grounded, emphasizing cooperation built through trade. Hand-painted stylized fantasy art, soft atmospheric depth, refined geometric details in stone, cloth and wood, readable silhouettes, no text, no letters, no logos, no watermark.

#### `02-sieci-splotu.png` — Rozkwit splotu

> Panoramic fantasy game illustration, wide 16:9 composition. Show the first connected network of crystal-powered defensive towers protecting a prosperous trade city and its road. Several elegant stone-and-metal towers stand along the walls, each holding a softly glowing, faceted splotyt crystal; fine threads of elemental light visibly link the towers and converge toward a shared relay. In the foreground, an experienced operator places a hand on one tower's control surface as the network responds, while researchers and workers observe from a safe platform. Keep the technology handcrafted and magical, not industrial or modern. The scene should communicate discovery, shared infrastructure and wonder, not battle. Hand-painted stylized fantasy art, consistent soft light and subtle geometric material patterns, restrained crystal glow, clear composition, no text, no letters, no logos, no watermark.

#### `03-wojna-bram.png` — Wojna Siedmiu Bram

> Panoramic fantasy game illustration, wide 16:9 composition. During the siege of Val Meris in the War of the Seven Gates, two opposing fortified lines face each other across a crowded trade crossing. Crystal-powered towers on both sides have overloaded: some are dark, others flare unpredictably with fractured elemental light. Their interwoven energy network is breaking apart above the city as civilians and a supply convoy try to escape through a blocked road; rescue teams guide people away from the danger. Show the human cost and confusion of the disaster, not graphic injury. The seven gates are suggested by distant fortified passes along the horizon rather than seven literal doors in one wall. Dramatic smoky dusk, sparks and fading crystal light, hand-painted stylized fantasy game art, strong readable silhouettes, detailed stone and metalwork, no text, no letters, no logos, no watermark.

#### `04-traktat.png` — Droga do pokoju

> Panoramic fantasy game illustration, wide 16:9 composition. In the uneasy years after the war, representatives of two rival cities meet at a neutral arena to settle a dispute over a supply convoy without another siege. In the center, two small teams take opposite roles around matching, mirrored labyrinth defenses: one team prepares its towers while the other studies the route, then their positions visibly suggest that the roles will be reversed. Nearby, medics and craftspeople inspect protective arena seals, and city delegates witness the first measured trial before later signing a simple peace accord at a shared table. Preserve tension and distrust, but let cautious hope come through. Hand-painted stylized fantasy game art, consistent warm-cool lighting and geometric architectural details, no modern objects, no readable writing, no text, no letters, no logos, no watermark.

#### `05-era-prob.png` — Era Prób

> Panoramic fantasy game illustration, wide 16:9 composition. A celebrated public Trial unfolds in a grand but practical arena with a clear labyrinth route and defensive towers. A coordinated team of champions from different cities, guilds and traditions works together: a sturdy earth defender protects an ally, a swift shadow scout moves ahead, a nature healer supports the group, and an elemental specialist studies the tower network. Their clothing, features and magical effects are varied; no one elemental tradition dominates the team. Spectators from many communities watch from safe terraces as neutral officials record the result. The image should express earned trust, tactical cooperation and a fragile peace, while leaving room for unresolved political tensions. Hand-painted stylized fantasy game art, consistent soft atmospheric light, refined geometric details, readable silhouettes, no text, no letters, no logos, no watermark.

Źródła: [obrazy i etapy kroniki](./riftoryn-lore.js#L3), [obsługa braku ilustracji](./riftoryn-lore.js#L10), [dostępne materiały mapy](./assets/lore/).

### 3. Rozróżnić nazwy dwóch porozumień — doprecyzowano

W historii występują **Umowa Siedmiu Szlaków** (128 PT) i późniejsza **Rada Siedmiu Szlaków** (powołana w 0 ET). Ich role są różne, więc nie jest to sprzeczność, ale podobne nazwy mogą utrudniać zapamiętanie, że pierwsza była umową między miastami, a druga instytucją traktatową.

**Wykonano:** zachowano obie nazwy, określając Umowę jako dawne wielostronne porozumienie handlowe, a Radę jako organ nadzoru powołany przez Traktat. Kronika wyjaśnia różnicę przy pierwszym wystąpieniu Umowy.

Źródła: [GDD — Umowa](./RIFTORYN_GDD.md#L565), [GDD — Traktat i Rada](./RIFTORYN_GDD.md#L579), [oś czasu](./RIFTORYN_GDD.md#L589).

### 4. Doprecyzować, jak obowiązkowe Próby działają obok zawodów — wykonano

Traktat kieruje do Prób tylko określone spory i wiąże odmowę z możliwą utratą dostępu do wspólnych zasobów. Później Próby stają się także widowiskiem i drogą do sławy. Te funkcje mogą współistnieć, ale warto odróżnić formalne rozstrzygnięcia traktatowe od dobrowolnych zawodów, aby nie sugerować, że każdy bohater lub mieszkaniec musi uczestniczyć w turniejach.

**Wykonano:** rozdzielono wiążące rozstrzygnięcia traktatowe od dobrowolnych zawodów publicznych. Strony zgłaszają objęty Traktatem spór i samodzielnie wskazują własne składy; Urząd sprawdza ich zgodność z zasadami udziału. Miasta, gildie i stowarzyszenia mogą prowadzić nabór lub sponsorować skład. Wynik publicznych zawodów nie zmienia praw do zasobów. Szczegóły procedury odwoławczej pozostają otwarte.

Źródła: [GDD — zakres obowiązku](./RIFTORYN_GDD.md#L581), [GDD — rozwój Prób w widowisko](./RIFTORYN_GDD.md#L583), [otwarte decyzje](./RIFTORYN_GDD.md#L675).

### 5. Wyjaśnić, dlaczego bohaterowie z różnych krain walczą w jednej drużynie — dopisano do GDD i kroniki

Dodano wyjaśnienie historycznego przejścia od championów wystawianych przez krainy do składów dobieranych przez miasta, gildie i stowarzyszenia. Wczesne, powtarzalne przewagi jednych żywiołów nad innymi — przykładowo Ognia nad Lodem — szybko ujawniły ograniczenia drużyn opartych na jednej tradycji. Współpraca bohaterów z różnych krain wynika teraz ze wspólnej sprawy lub reprezentowanej organizacji, nie z nagłego zaniku politycznych sporów.

**Wprowadzony fragment w „Opowieści o świecie” umieszczono po opisie obowiązku traktatowego, przed przejściem do popularności Prób:**

> Pierwsze Próby traktatowe nadal przypominały dawne zmagania krain: każda z siedmiu wysyłała championów wychowanych we własnej tradycji żywiołu. Nie trzeba było wielu sezonów, by zauważyć, że taki układ sprzyjał przewidywalnym starciom. W bezpośrednich pojedynkach Ognia z Lodem zwycięstwo niemal zawsze przypadało drużynie Ognia; inne zestawienia również ujawniały mocne i słabe strony poszczególnych żywiołów. Nie oznaczało to, że każdy wynik był z góry przesądzony, ale sama doskonałość jednej tradycji nie wystarczała już do odpowiedzi na każdą sytuację.
>
> Wkrótce miasta, gildie i stowarzyszenia zaczęły wystawiać składy dobierane nie według pochodzenia championów, lecz według potrzeb konkretnej sprawy i uzupełniających się umiejętności. Bohaterowie z różnych krain mogli więc walczyć ramię w ramię, gdy reprezentowali tę samą społeczność, organizację albo roszczenie do szlaku czy zasobu. Nie oznaczało to końca sporów ani stałego sojuszu między ich ojczystymi krainami: wspólnicy jednej Próby mogli spotkać się po przeciwnych stronach następnej.
>
> Mieszane drużyny szybciej adaptowały się do przeciwnika, a ich sukces przekonał uczestników, że różnorodność jest siłą. Z czasem miasta, gildie i stowarzyszenia stały się głównymi organizatorami reprezentacji.

**Oś czasu w GDD i kronice prototypu:** zmianę umieszczono w **1 ET** — zaraz po wejściu Traktatu w życie, przed ujednoliceniem labiryntów w 3 ET i otwarciem wspólnej szkoły w 12 ET. Pozostałe daty zachowano.

**Dlaczego to spina świat:** bohaterowie z różnych krain mogą wiarygodnie walczyć ramię w ramię, ponieważ reprezentują wspólną sprawę lub organizację, a nie obowiązkowo własną krainę. Jednocześnie rywalizacja między odmiennymi interesami i frakcjami pozostaje możliwa; Próby nie kasują politycznych konfliktów.

Źródła: [GDD — ewolucja składów](./RIFTORYN_GDD.md#L583), [oś czasu GDD — zmiana w 1 ET](./RIFTORYN_GDD.md#L628), [kronika prototypu — zmiana w 1 ET](./riftoryn-lore.js#L7), [mapa — zasady spójności krain](./RIFTORYN_GDD.md#L704).

### 6. Ugruntować niepewność wokół Nocy Zgasłych Wież — wdrożono jako trop fabularny

Nieustalona przyczyna katastrofy w Val Meris oraz sprzeczne rejestry są użytecznymi zalążkami dalszej fabuły. Obecnie wiadomo, że istnieją rozbieżne źródła, ale nie wiadomo, kto je sporządził ani dlaczego ich relacje się różnią.

**Wykonano:** GDD i kronika przedstawiają trzy niezgodne świadectwa: powojenne opracowanie Urzędu przypisuje awarię równoczesnemu przeciążeniu obu sieci, ocalała wskazuje na wieżę strony nieostrzeliwującej mostu, a urwany zapis przekaźnika sugeruje wcześniejszy spadek zasilania. Żadne źródło nie rozstrzyga przyczyny ani liczby ofiar.

Źródła: [GDD — katastrofa i rozbieżne relacje](./RIFTORYN_GDD.md#L573), [konflikty i otwarte decyzje](./RIFTORYN_GDD.md#L667).

## Ustalenia robocze wprowadzone po przeglądzie

1. **Reprezentacja:** każda strona sporu samodzielnie wskazuje własny skład, a Urząd sprawdza go względem wspólnych zasad; miasta, gildie i stowarzyszenia mogą organizować nabór lub sponsorować bohaterów, ale sponsor nie staje się stroną sporu.
2. **Dwa rodzaje wydarzeń:** Próba traktatowa wiąże strony tylko wtedy, gdy Urząd formalnie ją przyjął jako rozstrzygnięcie. Sezony i zawody publiczne są dobrowolne, a ich wyniki dają nagrody i sławę, nie prawa do szlaków.
3. **Przewagi żywiołów:** relacja Ognia i Lodu została opisana jako historyczna seria wyników w dawnym formacie, nie uniwersalna reguła walki ani automatyczna przewaga mechaniczną. Szczegóły reakcji żywiołów nadal wymagają projektu gry.
4. **Val Meris:** sprzeczne świadectwa tworzą trop do dalszej fabuły; przyczyna katastrofy i pełna liczba ofiar pozostają nierozstrzygnięte.

**Pozostają do decyzji:** procedura odwoławcza od wyniku Próby, sposób kontroli akceptacji reprezentantów przez obie strony oraz to, które zawody publiczne organizuje Urząd, a które niezależne podmioty. Wprowadzony model jest spójną propozycją roboczą, nie zatwierdzonym kanonem.

## Co jest spójne

- Chronologia od pierwszych porozumień (180 PT) przez wojnę, rozejm i Traktat (0 ET) do początku gry (34 ET) jest zachowana w GDD i prototypowej kronice.
- Noc Zgasłych Wież przypada na 17 PT, a Wojna Siedmiu Bram zaczyna się w 30 PT; późniejszy Pojedynek Bram i Traktat wynikają z nierozwiązanego problemu dostępu do zasobów.
- Traktat nie zobowiązuje wszystkich mieszkańców do udziału w Próbach; dotyczy stron wskazanych sporów. To jest zgodne z opisem codziennego życia i mapą.
- Val Meris jest miejscem katastrofy i neutralnym węzłem Prób; nie jest ósmą krainą.
- Tradycje żywiołów nie ograniczają pochodzenia bohaterów ani składu drużyn, co jest zgodne z założeniem mieszanych drużyn.
- Niewyjaśnione przyczyny katastrofy i niezgodności w rejestrach są w GDD przedstawione jako otwarte wątki, a nie ujawnione fakty.

## Granice tego przeglądu

Nie weryfikowałem prawdziwości wydarzeń poza repozytorium ani nie oceniałem, czy propozycje fabularne powinny zostać zatwierdzone. Ilustracje kroniki zostały dostarczone przez użytkownika i podłączone do prototypu; nie zmieniałem ich treści. Ewolucja składów jest nadal propozycją fabularną, nie zatwierdzonym kanonem.
