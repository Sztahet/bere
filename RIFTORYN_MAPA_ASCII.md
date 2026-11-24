# Mapa ASCII Riftorynu

[Ilustrowana plansza atlasu](assets/lore/cartography/riftoryn-mapa-atlas-v1.png) pokazuje główne ośrodki i wybrane oznaczenia. Przy śledzeniu tras korzystaj z mapy ASCII i indeksu poniżej.

Mapa robocza kontynentu dla opowieści, stan odniesienia 12 ET. Północ jest u góry. Linia brzegowa, jezioro, rzeki i układ głównych dróg pochodzą z przerysu [mapy świata](assets/lore/cartography/riftoryn-mapa-swiata-v1.png). Kierunki i nazwy głównych ośrodków pochodzą z [sekcji mapy w GDD](RIFTORYN_GDD.md#riftoryn--mapa-świata-i-ośrodki-prób--koncepcja-fabularna--v01). Odległości, dokładny przebieg tras i granice pozostają otwarte. Położenie Val Meris jako neutralnego węzła potwierdza [historia świata](world_history.md#val-meris--wspólny-węzeł).

## Kontynent

```text
                                                              N
                                                              ^
                                                              |
                                                         W <--+--> E
                                                              |
                                                              S
+----------------------------------------------------------------------------------------------------------------------------------------------------------+
|~                                                     ~   ~    ~     ~         ~                                ~         ~     ~    ~   ~                |
|   ~  ~                              ~        ~      ...            ~       .....                   ~        ~              ..              ~  ~          |
|                                ....... ~    ~     .. . .... ~     ......... ^^^ ....                 ~                   .. .....      ~                 |
|  ~          ..          .......       ...    .....  ^      .......^   ^     .  .   ^............  ~                    ..^   ^   ...      ~              |
|           ..  ....    ..       ^^^    ^  ....    ^ ^             ^  .   ^ ^   .  ^             ^.....           ~    ..             ..                   |
|         ..   ^^^  ....        ^   ^  ^   ^^       ^^       ^^   ^  ^   ^         ^    ^^^            ...          ...             ^   .........    ~     |
|      ... ^ V E S P E R                       ^ ^     .   .^   ^ N I V O R E N    ^   ^                  ... ~ ....      ^          ^   ^.   .  ...       |
|   ...  ^  ^  ^      .^                  ^   ^      .   ^           ^    ^   ^    ^           ^   .      ^  ...                 ^.      ^  ^  ^  ^ ..    ~|
|    ~..                     . ^   ^^      .      ^ .  . ^      .      ^^   ^ .                       ^    ^  ^      ^^       ^^      ^  ^    ^       ..  ~|
|       .......     ^   ^     . .     ^   ^                   ^ ^              ^      ^  ^  ^    . ^^ .    ^  ^  ^      ^              ^ ^              .  |
|~          ..  ^  o V01 Cienisty Port      ^ ^                   ^   ^     . .     ^   ^                   ^ ^           V A R E T H  ^  ^    . ^^ .  . ~ |
|     ~   ..          ==========^   ^^      .      ^ .  . ^      .      ^^   ^ .                       ^    ^  ^      ^^       ^^      ^  ^    ^      .    |
|        .   ^           ^    ^ ==^    ^           ^   .      ^   ^           o N01 Bialy Przesmyk ^  ^  ^  ^      .^                  ^   ^      . ..     |
|      ..            ^   ^       ^====      ^ [R02]  .     ^ ^               =                 ^ ^     .   .^   ^          ^       ^   ^          ..       |
|     ..          ^^^    ^         ^  ======  ^^       ^^       ^^   ^  ^   ^=        ^    ^^^                   ^                     ^         .      ~  |
|       .. ^             ^  .   ^ ^   .  ^  ====== *         *  *        ~    =                 .          .                    ^^^    ^     ^    ..       |
|      ..                  .  .         *     .  *===   * **              ~  =                          ^       .     ^  =@ F01 Zarowisko Vareth    ..~    |
|    ~..                                       *  *  = *  * .  *  *      ~ ==                                    .    ===   ^ ^    .     .           ^.    |
|      ~.                                           * =       *  *   [R03]=      .                             ^^^====     ^.             ^^^        .     |
|        .                           *        *    * * == .          ====                             . [R04] ====                                   .     |
|~        .                     *   .   ::::::::::: *    =======  ===  ~  ~~~~~~~~~                  =========                                      .      |
|         .  A U R E L       .      :::: * ~~*     ::::::     ::== .    ~~~~~  ~~~~~~~~~~~~~       ==       .            ^^^            ^   .   ^    ..    |
|        ..    .                  ::**   ~~       *[R08] ::::: *==    ~~~~~~~~~~~~~~~~~~~~~~~~    =   .            ^                  ^       ^  ^ .^  ..  |
|       .                       ::     ~~           *    *  *   =~~~~~~~WIELKIE JEZIORO ~~~~~~    =                          ^   ^                ^ ...  ~ |
|       .           .          :        ~           *    * *   =~~  ~~~~~~ ..~~~ ~~~~~  ~~~~~ ~~  =                 . . .   ^    ^       ^       ^ .~      |
|      .                  .   :          ~~.                  ==~~ ~ ~~~~~~. .....~~~~~~~~~~~~~~~ =                      ..    ^          .^      .        |
|   ~..                     ::       ========                ==  ~~  ~~~~~.wyspy.~~  ~~~~ ~~ ~~~  =                   ^Z E R A T H      ^ ^        ..      |
|   ..                    ::     ====~~~~~   =====            =~~~~ ~~ ~~....~..~~~~ ~~~~~~~~~~   ===            ^       ^                 .^   ^    ..~   |
|     ....         Latarnia Aurelu @ A01          ====        == ~~~~~~~..  ..~ ~ ~ ~ ~ ~~~      =   ==== [R09]           ^   ^    .       ^      ^  .     |
|        ..                 ::::        .          [R01]=     =        ~~~..~~~~~~ ~~~~~~      ==        =====       ^  ^   ^^ .   . ^^   ^  ^      .  ~   |
|~      ..                      ::::: .                  ==  ===             ~~~~~ ~~        ==            ~~ =====                 ^   ^          .       |
|      .                             ::::        .         =========      .  ~~~~~~      ==== .       .      ~~    ====.         ^       ^   ^     .  ~    |
|    ~...     .                          ::                    ============ ~        ====                    . ~   ..  ===     . ^ ^ .          ...  ~ ~   |
|    .    ^    ^ ^             ^      [R10]::                       ===========.   ==   .      .               [R05]=======@ Z01 Iglica Zerath    .        |
|    .^  ^  ^   ^^   ^  ^  ^         ^       :            .    .    .   =========O M01 VAL MERIS      .    .   =====                 ^         ^  .  ~     |
|   ...         ^^^   .^     ^     ^^         ::                  ===     ======         ===           .=======          ^ ^      ^ ^      ^ ^   .         |
|      ....^    ^ ^             ^      ^ ^      :               ==   =====~ .=            . ===       ==    ~~                 .       ^^^   .^..          |
|   ~      ..^  ^  ^  ^          ^               :         .====.  * **    ~=                  =======** ~~~.    .  ^   ^                  ^  .            |
|         ~ ..  ^  K H A R U M        ^           :     ====             ~~*=        *        *    *  ~~~         ^   ^             .         .    ~  ~ ~  |
|         ..           ^  ^ ^   .                 .:  ==              * ~  *== [R06]*           *  ~~~           ^ ^                ^      ^  .         ~  |
|~       .                       ^           ^      ==        *      .~~      ==     * . *               .      *   ^     ^    ^ ^           ^.            |
|        .   .    .    .   ^^^    ^         ^     ==        .        .  ~~~     ==     *    **            . * *.    .     ^                   .            |
|       .   ^          ^ ^                   [R07]                 *       ~~~    =*    *     **         *  *      ^          ^             ..    ~        |
|      . ^  ^      ^^      ^  ^         ^ ====.       *      *  *             ~~  *=o D01 Korzenna Przystan     *      ^ ^        .     ^ ..               |
|~    .  .^^   ^      ^      .      ^ ====            *      **      **      *  ~~  *     *     *     .*            ^   ^^.     .         .                |
|   ~  ..  ^     ^         ^       ===  ^  ^  ^        *  * *        *          ~    *     **    *    *                  *               ..  ~             |
|   ~    .   .              .    @ K01 Kamienny Prog   *.     *  *             ~   *   *   *   *   *             *  *     .*          ...    ~             |
|        .         ^              .   ^         .        *  *            *  *~~      .**           .*                   * * *       ..                     |
|        .        ^         ^    . ^  ^  ^  ^         *.  *              *    ~~                   *                                  ..            ~      |
|  ~     .. ^^                ^^      ^    .  ^             *      *  *         ~                             *  *      *               ........    ~ ~  ~ |
|          .^^^  ^      ^             ^   ^          *     .                .  ~  *V E R D A N*    **         *      * * *  ...      .     .  .        ~   |
|        ~ .                          ^    ^         *  * * *                 ~ *             *         *  *    *  *    *...   ..    .........    ~        |
|           .                     ^  .^       ^ .       *           .       * *~~  *   *   *   *   *  * * *       .     .        ....                      |
|     ~     .                . ^ ^                   .*  . *                  *~.  *.   **     *    *       .  *      ...  ~       ~           ~       ~   |
|           .            ^   .            .       .           *    *        * ~     .*           *        **      *...  ~            ~~                    |
|           .. .    ^      .. ... ..     ^^   ^           *      *  *          ~  *   *  .*   *.  *   *.           .             ~   MORZE             ~   |
|        ~   ~..   ........     ..  .   ^   ^       .    *  .       *  *      ..~ * .........         . .**        .          ~   ~               ~   ~    |
|               ...            ~     .      .                      *  *      .  ....         .       .   .       ..               ~                        |
| ~         ~                        ~.. ... ..       . ... ** ...   ....   .              ~  .......     ...  ..   ~    ~                 ~         ~     |
|        MORZE   ~  ~ ~                 ..     ...    ..   .... ~ ...    ... ~        ~       ~              ..        ~ ~  ~                             ~|
|     ~     ~    ~   ~                     ~      ....                    ujscia rzek     ~         ~                                ~         ~     ~    ~|
|                    ~        ~                                                     ~~                 ~~                                                  |
|     ~         ~          ~                                                                                        ~          ~         ~     ~         ~ |
+----------------------------------------------------------------------------------------------------------------------------------------------------------+
```

- `O` oznacza Val Meris, `@` miasto, `o` osadę, a `!` na szkicu lokalnym gospodę lub oberżę. Identyfikatory łączą mapę z indeksem poniżej.
- `=` oznacza główny korytarz drogowy. Wspólne odcinki kilku tras mogą korzystać z tej samej drogi. `:` oznacza orientacyjne łączniki, których dokładny przebieg pozostaje otwarty.
- `~` oznacza wodę, `^` góry i skaliste wyżyny, `*` las, `.` linię brzegową lub tło lądu. „Wielkie jezioro” to opis akwenu z mapy i GDD.
- W indeksie **[K]** oznacza punkt potwierdzony przez historię świata, **[O]** punkt z aktualnego opowiadania, **[G]** propozycję z GDD, a **[?]** wzmiankę wymagającą doprecyzowania. Znak może dotyczyć miejsca albo wskazanego wprost szczegółu, np. jego dokładnego położenia.

## Drogi i przejazdy

Numery R01–R10 odnoszą się do korytarzy na mapie kontynentu. Kolejne numery wskazują trasy, które da się odtworzyć z tekstów jako relacje między miejscami. Strzałki w szkicach lokalnych pokazują kolejność punktów lub odgałęzienie. Czasy przejazdów dotyczą konkretnej podróży bohatera.

- **R01 — zachodni korytarz** · Latarnia Aurelu (A01) ↔ okolice Val Meris (M01). Odcinek na ilustracji. [Mapa świata][mapa]
- **R02 — północno-zachodni korytarz** · Cienisty Port (V01) ↔ zachodnia strona jeziora ↔ Val Meris. Przejazd Azmira do punktu zapisów jest opisany. [Mapa świata][mapa] · [Imię na liście][morrak3]
- **R03 — północny korytarz** · Val Meris ↔ zachodnia strona jeziora ↔ okolice Białego Przesmyku (N01). Połączenie pokazuje przerys. Odcinki górskie mają własny szkic poniżej. [Mapa świata][mapa] · [Biały Przesmyk][przesmyk]
- **R04 — północno-wschodni korytarz** · Val Meris ↔ wschodnia strona jeziora ↔ Żarowisko Vareth (F01). [Ośrodki w GDD][gdd] · [Mapa świata][mapa]
- **R05 — wschodni korytarz** · Iglica Zerath (Z01) ↔ Val Meris. Taren jedzie wozem do Val Meris w maju 12 ET. [Po drugiej stronie][taren3]
- **R06 — południowy korytarz** · Korzenna Przystań (D01) ↔ Val Meris. Thora wyjeżdża w dniu wyboru i dociera pod koniec marca 12 ET. Eron, Liora i Ana z Bobem wyruszają w różnych terminach w maju. [Thora][thora2] · [Eron][eron] · [Liora][liora] · [Ana][ana]
- **R07 — południowo-zachodni korytarz** · Kamienny Próg (K01) ↔ okolice Val Meris. 24 maja 12 ET Ilda rusza z Korą wozem do przewozu bydła. Zaprzęg jest zmieniany po drodze i docierają 28 maja. Wóz towarowy Hedy wyjeżdża tego samego dnia, zabiera dodatkowy ładunek po drodze i dociera 1 czerwca 12 ET. Osobno potwierdzony jest przewóz ładunku Vedy z Kharum do składu pod wschodnią bramą Val Meris. [Mapa świata][mapa] · [Księga bez autora][piecarius1] · [Przysięga z księgi ojca][piecarius3] · [Jeden przechodzi pierwszy][rokan3] · [Tędy można wrócić][ilda3]
- **R08 — zachodnie odgałęzienie na północ** · Aurel → północny szlak → schronienie, przy którym Piecarius spotyka Ruvana (N08) → dolna droga pod przełęczą. Piecarius wyrusza z Aurelu 23 kwietnia 12 ET. [Ślady w popiele][piecarius2]
- **R09 — wschodnie odgałęzienie między korytarzami** · drogi na wschód od jeziora. W atlasie odcinek orientacyjny z ilustracji, bez ustalonej nazwy i etapów przejazdu między Vareth a Zerath. [Mapa świata][mapa]
- **R10 — połączenie Kharum z Aurelem** · położenie łącznika jest orientacyjne. Wędrówka Piecariusa jest potwierdzona w opowiadaniach. Jest w Kharum 8 kwietnia i w Aurelu 22 kwietnia 12 ET. [Księga bez autora][piecarius1] · [Ślady w popiele][piecarius2] · [To, co zostaje po drodze][veda1]
- **R11 — górska droga do schroniska przy zakręcie** · osada Saily (N02) → N04. Ponad dwie godziny szybkiego marszu. Przy schronisku rozchodzą się krótsze przejście między skałami i dłuższe obejście wzniesień, które zajmuje prawie pół dnia więcej. [Ciężar kryształu][saila1]
- **R12 — dolna droga w Nivoren** · dolny posterunek (N05) i dojazd wozów z doliny do osady. Ruvan porównuje warianty dostaw. Po pierwszej odwilży tą drogą wracają leki i zapasy. [To, czego nie nauczył mnie Ruvan][saila3]
- **R13 — dolna droga pod przełęczą** · zejście wskazane przez Ruvana → magazyn Tovina i okolica → gospoda (N09) → dolny trakt. Na tym trakcie Piecarius dołącza do transportu części na areny. Nocten także odwiedza gospodę 19 maja 12 ET. Tożsamość tej drogi z R12 pozostaje otwarta. [Ślady w popiele][piecarius2] · [Pierwsza lekcja][nocten3]
- **R14 — dolny trakt → Val Meris** · z okolicy N09 w stronę M01. Transport ma ruszyć rankiem po 19 maja. Piecarius dociera wieczorem 26 maja 12 ET. Pośrednie postoje pozostają nieustalone. [Ślady w popiele][piecarius2] · [Przysięga z księgi ojca][piecarius3]
- **R15 — kurs Vedy w Kharum** · oberża (K02) → chata Orena (K03) → droga i przeprawa na skraju lasu (K04) → górna osada z pompą (K05). [Księga bez autora][piecarius1]
- **R16 — wyjazd Lucena z Zerath** · Iglica Zerath → szlak i postój wozów → rozwidlenie przy składzie → brama Aurelu. Opowiadanie potwierdza przebieg w 4 PT w tej kolejności. [Z tego, co ocalało][lucen1]
- **R17 — szlak pod Aurelem** · odcinek z rozwidleniem, zamkniętym brodem, kładką i oznaczonym objazdem. Lucen i przewodnik zabezpieczają przejazd wozów jesienią 11 ET. [Nie przeciwko tobie][lucen3]
- **R18 — droga w dolinie Verdan** · przepust i objazd dla wozów, z osobną górną ścieżką dla pieszych za zgodą gospodarzy. To szlak lokalny, którego przebieg opisuje Eron. [Przez cudze pole][eron]
- **R19 — droga Cienistego Portu do kopalni** · Azmir jedzie zebrać odpisy świadectw, a po powrocie porównuje je w archiwum. Punkty pośrednie pozostają otwarte. [Świadkowie milczenia][morrak2]
- **R20 — przejazd Noctena do Val Meris** · wóz rusza rankiem 23 maja i dociera 27 maja 12 ET. Miejsce wyruszenia to okolice domu Noctena, a przebieg pośredni pozostaje otwarty. [Pierwsza lekcja][nocten3]

## Indeks miejsc i szkice lokalne

### Vesper — północny zachód

~~~text
o V01 CIENISTY PORT ===== [R02 -> VAL MERIS]
  |-- V02 Archiwum Cienistego Portu
  |-- V03 Dom Meret nad warsztatem szewca
  |-- V04 Magazyny oraz bezimienne gospody portowe
  +-- R19 ---------------- V05 Kopalnia z akt Morraka
~~~

- **V01–V02 [K/O]** — Cienisty Port i jego archiwum, miejsce pracy Azmira i Vaelis. **Brama Zmierzchu [G]** to miejscowy ośrodek Prób. [Vesper w historii świata][vesper] · [Pierwsze odpisy i akta][vaelis1] · [GDD][gdd]
- **V03 [O/?]** — dom Meret nad warsztatem. Opowieść wiąże wizytę z kwerendą Azmira. Dokładna ulica i położenie względem portu pozostają otwarte. [Imię, którego nie wypowiedział][morrak1]
- **V04 [K]** — portowe magazyny, gospody i schronienia dla załóg. Nazwy poszczególnych lokali są otwarte. **V05 [O]** — kopalnia i miejsca przesłuchań po zawaleniu. [Vesper w historii świata][vesper] · [Świadkowie milczenia][morrak2]

### Nivoren — północ

~~~text
                            ^ ^ ^ GORSKIE PRZEJSCIA ^ ^ ^

                      o N01 BIALY PRZESMYK
                            |
                      o N02 Osada Saily -- N03 chlodnia i magazyn
                            |
                [R11] ----- + ------- N04 Gorne schronisko
                            |              |-- waskie przejscie
                            |              +-- dluzsze obejscie wzniesien
                [R12] ----- + ------- N05 Dolny posterunek
                            |
                            +------- N06 Szlak pod wieza Blyskawic
                                     N07 Schronisko przy szlaku

   osobna relacja z podrozy Piecariusa:
   [R08] --- N08 schronienie Ruvana --- [R13] --- N10 okolica magazynu
                                               |
                                               +-- ! N09 Gospoda przy dolnej drodze
                                               +-- [R14 -> VAL MERIS]
~~~

- **N01 [K/G]** — Biały Przesmyk, osada u wejścia do przejścia górskiego. Kanon opisuje również rozleglejsze okolice i osady przy Przesmyku. **Lodowe Wrota [G]** to proponowany ośrodek Prób. [Biały Przesmyk][przesmyk] · [GDD][gdd]
- **N02–N05 [O]** — bezimienna osada pod Przesmykiem, chłodnia z magazynem i dziedzińcem, górne schronisko na rozwidleniu oraz dolny posterunek. W tekście pojawiają się też południowa osada obsadzająca trasę, dalsze osady korzystające z zapasów oraz strumień poniżej górnego schroniska. [Ciężar kryształu][saila1] · [To, czego nie nauczył mnie Ruvan][saila3]
- **N06–N07 [O]** — wieża Błyskawic nad górskim szlakiem, skalne przejście i schronisko, do którego ludzie z posterunku zanoszą Sailę. Tożsamość N07 z górnym schroniskiem N04 pozostaje otwarta. [Gdy wieża zamilkła][saila2]
- **N08 [O]** — schronienie na północnym szlaku, przy którym Piecarius spotyka Ruvana. **N09 [O]** — gospoda przy dolnej drodze pod przełęczą, miejsce rozmowy Piecariusa z Noctenem. **N10 [O]** — osada lub zabudowania przy poszukiwanym magazynie Tovina, miejsce pożaru magazynu i szopy. Nazwy i dokładne położenie pozostają otwarte. [Ślady w popiele][piecarius2] · [Pierwsza lekcja][nocten3]
- **N11 [O]** — miejsca odpoczynku i pojenia koni przy dolnym obejściu. **N12 [O/?]** — górskie schronienia dla podróżnych wspominane przez Sailę. Lista poszczególnych schronień pozostaje otwarta. [Gdy wieża zamilkła][saila2] · [To, czego nie nauczył mnie Ruvan][saila3]

### Vareth — północny wschód

- **F01 [K/G]** — Żarowisko Vareth, miasto warsztatów i pieców u skraju wulkanicznych wyżyn. **F02 [K]** — dzielnice warsztatowe, dziedzińce i bezimienne jadłodajnie dla rzemieślników i przewoźników. **F03 [K]** — wulkan i urządzenia wykorzystujące jego ciepło do ładowania splotytu. **Żelazna Brama [G]** to odbudowana strażnica i proponowany ośrodek Prób. [Vareth w historii świata][vareth] · [GDD][gdd]

### Zerath — wschód

~~~text
@ Z01 IGLICA ZERATH ===== [R05 -> VAL MERIS]
  |-- ! Z02 Oberza pod brama
  |-- Z03 Dom Tarena, brama i nabrzeze
  |-- Z04 Glowny rozdzielnik i boczne stacje
  +-- Z05 Stacja graniczna / Z06 warsztat i stanowisko prob
~~~

- **Z01 [K/G]** — Iglica Zerath, ośrodek przekaźników przy skalistym wybrzeżu. **Dziedziniec Impulsu [G]** to proponowany ośrodek Prób. [Zerath w historii świata][zerath] · [GDD][gdd]
- **Z02 [O]** — oberża pod bramą w Zerath. Rozmówca Tarena zna jej nazwę, lecz tekst jej nie podaje. **Z03 [O]** — dom Tarena i Lucena, brama z rejestrem wozów, nabrzeże oraz stacja w Iglicy. [To, co pozostało][taren1] · [Po drugiej stronie][taren3]
- **Z04 [O]** — główny rozdzielnik, boczne stacje z pompami, warsztat ślusarki przy najdłuższym odcinku i skład części po drugiej stronie granicy. **Z05–Z06 [O]** — stacja graniczna, na której brygada Varek podaje znaki z okien, oraz pracownia i stanowisko prób Miko. Numer i położenie poszczególnych stacji pozostają otwarte. [Cena lojalności][taren2] · [Jeszcze jeden impuls][miko2]
- **Z08 [O]** — dolny posterunek i zjazd z dwiema podporami; górny posterunek oraz rozwidlenie, skąd Zet sprawdza uszkodzony odcinek drogi dla konwoju. Ich dokładne położenie względem innych stacji pozostaje otwarte. [Drugi sygnał][varek1]
- **Z09 [O]** — w szkicu: dom i schronienie opiekunki zwierząt w Iglicy Zerath; okolica wypuszczenia Groma pozostaje nieumiejscowiona. [Skrzydło][volt1]
- **Z07 [?]** — pierwsza karczma wspomniana przy rozważaniu trasy z Zerath do Aurelu. To wzmianka o możliwym miejscu wymiany kurtki. Brak danych pozwalających umieścić ją na konkretnym odcinku R16. [To, co pozostało][taren1]

### Verdan — południe

~~~text
o D01 KORZENNA PRZYSTAN ===== [R06 -> VAL MERIS]
  |-- D02 Targ, studnia, warsztat i miejsce posiedzenia starszyzny
  |-- D03 Lecznica, domy pod walem, pola i kanaly
  |-- D04 Most, gorny wal, sad i miejsce sadzonki
  +-- D05 Przepust
      |-- [R18] gorna sciezka dla pieszych
      +-- [R18] objazd dla wozow ---> D06 dalsze osady i gospodarstwa
~~~

- **D01 [K/G]** — Korzenna Przystań, osada w dolinie rzeki. **Krąg Korzeni [G]** to arena w pobliżu szlaku do centrum. [Verdan w historii świata][verdan] · [GDD][gdd]
- **D02 [K/O]** — rynek i wspólne stoły przy ogrodach, studnia, warsztat mijany przez Anę i Boba oraz miejsce wyboru czworga kandydatów. [Verdan w historii świata][verdan] · [Kiedy obeschnie ziemia][ana] · [Dzień, który miał być spokojny][thora2]
- **D03 [O]** — lecznica, domy odwiedzane przez Liorę, trzy pola pod wałem i wloty rowów. **D04 [O]** — most przy górnym wale, sad i miejsce zasadzenia drzewka. [Zanim zamknę lecznicę][liora] · [Ostatni korzeń][thora1]
- **D05–D06 [O]** — przepust z dwoma wylotami, gospodarstwo na krótszej drodze, ścieżka za płotem, ogród używany przez Anę przy objazdowej drodze i trzy bezimienne osady czekające na deski. Osad tych nie da się jeszcze przypisać do konkretnych punktów na mapie. [Przez cudze pole][eron] · [Kiedy obeschnie ziemia][ana]

### Kharum — południowy zachód

~~~text
@ K01 KAMIENNY PROG ===== [R07 -> VAL MERIS]
  +-- K06 Wyrobiska i place warsztatowe

[R15]  ! K02 Oberza --- K03 Chata Orena --- K04 Przeprawa --- o K05 Gorna osada
~~~

- **K01 [K/G]** — Kamienny Próg, miasto górnicze. **K06 [K/G]** obejmuje wyrobiska i place warsztatowe. **Kamienny Krąg [G]** to arena w pobliżu wyrobisk. [Kharum w historii świata][kharum] · [GDD][gdd]
- **Dojazd do K06 [O]** · krótszy odcinek pod skalną półką ma południowy wjazd i północny wylot. Górna droga prowadzi do tego samego zespołu wyrobisk i służy za objazd podczas zamknięcia dolnego odcinka. [Dłuższa droga][rokan2]
- **Droga przy sadzie [O]** · miejscowy odcinek dojazdu w Kamiennym Progu, przy dawnym podwórzu koniuszym Ildy. Przy dolnym zakręcie są sad i przepust. Miedza po zewnętrznej stronie sadu prowadzi od przepustu do lasu. [Od strony sadu][ilda1]
- **K02 [O]** — oberża w Kharum, w której Piecarius poznaje Vedę. Osada i nazwa lokalu pozostają otwarte. **K03–K05 [O]** — chata Orena na skraju lasu, przeprawa i trudny odcinek z koleiną oraz bezimienna górna osada z pompą. Tekst pozwala ustalić kolejność na trasie, a położenie względem Kamiennego Progu pozostaje otwarte. [Księga bez autora][piecarius1]
- **K07 [?]** — szlak kopalniany pod Kamiennym Progiem w rękopisie Broma. Przejście prowadzi pod górą w stronę osad, a jego przebieg pozostaje do zatwierdzenia. [Kamień, którego nie oddaliśmy](opowiadania/brom/1_the_stone_we_kept/historia.pl.md#ostatni-wóz)
- **K08 [?]** — skład południowy w Kamiennym Progu w rękopisie Broma. Obsługuje miejscowych przewoźników oraz wspólne konwoje. [Jedna kraina, wiele głosów](opowiadania/brom/2_one_land_many_voices/historia.pl.md#dwa-dni-w-tygodniu)

### Aurel — zachód

- **A01 [K/G]** — Latarnia Aurelu, zachodni ośrodek z GDD. Rękopisy często mówią po prostu o Aurelu. Atlas przypisuje punkty z tych scen do zachodniego obszaru bez nanoszenia planu miasta. **Plac Siedmiu Świateł [G]** to proponowany ośrodek Prób. [Aurel w historii świata][aurel] · [GDD][gdd]
- **A02 [O]** — uniwersytet, pracownia Adriena, stanowiska Leny i Miko, warsztaty i hala prób. **A03 [O]** — obserwatorium, dom przy schodach i latarnia Oriena. **A04 [O]** — miejsce odpisów u Lumen i piekarnia Demy. [Pierwszy impuls Miko][miko1] · [Z tego, co ocalało][lucen1] · [Ślady w popiele][piecarius2] · [Znam kogoś, kto potrafi][lena3]
- **A08 [O]** — w szkicu: skład przyjmujący złączki przewożone przez Vedę oraz stajnia, z której korzysta podczas wizyty w Aurelu; dokładne położenie względem innych punktów pozostaje otwarte. [To, co zostaje po drodze][veda1]
- **A09 [K/?]** — Birch, mała osada przy bocznej drodze w okolicach Aurelu; dokładne położenie i odległość od miasta pozostają do ustalenia. [Siedem dni](opowiadania/lumen/1_seven_days/historia.pl.md#siedem-domów).
- **A05 [O]** — brama Aurelu i rozwidlenie prowadzące do składu na trasie Lucena. **A06 [O]** — zachodnie punkty oznakowania, bród, kładka powyżej brodu i pobliskie gospodarstwo na szlaku Lucena. **A07 [O]** — prowizoryczne stanowisko sygnalizatora i rozwidlenie na zboczu, przy którym ginie przewodnik w 4 PT. [Z tego, co ocalało][lucen1] · [Nie przeciwko tobie][lucen3] · [Światło, które wskazało drogę][adrien1]

### Val Meris — neutralny węzeł

~~~text
         drogi obok wielkiego jeziora
                       |
              O M01 VAL MERIS
                |-- M02 Wschodnia brama i sklad Vedy
                |-- M03 Urzad zapisow i dziedziniec
                |-- M04 Szkola Magow / M05 archiwum
                |-- M06 Place zawodow i wieze cwiczebne
                |-- ! M07 Gospoda Piecariusa
                +-- M08 Sklady, kwatery i maly dziedziniec
~~~

- **M01 [K]** — neutralne miasto przy skrzyżowaniu szlaków i gospodarz Wielkich Igrzysk. **Serce Labiryntu [G]** to propozycja areny. [Val Meris w historii świata][val] · [GDD][gdd]
- **M02–M03 [O]** — wschodnia brama i skład Vedy, urząd zapisów, tablica kandydatów oraz dziedziniec. **M04–M05 [K/O]** — Szkoła Magów z pracowniami i publiczne archiwum powojennych świadectw. [Przysięga z księgi ojca][piecarius3] · [Nauka kontroli][lucen2] · [Po drugiej stronie][taren3] · [Historia świata][val]
- **M06 [O]** — plac miejscowych zawodów, plac przed areną, wieże ćwiczebne, stanowisko z modelem śluzy i teren właściwych Igrzysk. Zawody Piecariusa i Thory odbywają się 28 maja. Próba Miko i Thory przypada na pierwsze dni kwalifikacji rozpoczętych 29 maja 12 ET. [Dzień, który miał być spokojny][thora2] · [Wystarczy][thora3] · [Jeszcze jeden piorun][miko3] · [Kiedy obeschnie ziemia][ana]
- **M07 [O]** — gospoda, w której Piecarius nocuje i ćwiczy powitanie przed pokrywką garnka. Nazwa i ulica pozostają otwarte. **M08 [O]** — składy, kwatera przyjezdnych, mały dziedziniec z poidłem. **M09 [K/O]** — brama sabotowana przez Rian podczas wojny. Identyfikacja jej z konkretną współczesną bramą pozostaje otwarta. [Przysięga z księgi ojca][piecarius3] · [Po drugiej stronie][taren3] · [Jedenaście minut][rian1]

### Punkty bez przypisanej pozycji

- **U01 [O/?]** — osada, w której mieszka Nocten podczas zapisków. W jej okolicy są piekarnia, magazyn i archiwa odwiedzane przez Noctena. Jego dokumenty wskazują północną osadę jako miejsce urodzenia. Tożsamość obu osad pozostaje otwarta. Powrót Noctena z N09 przed zmrokiem 19 maja sugeruje dostęp z okolic domu, ale brak danych do wyznaczenia trasy. [Droga, której nie było][nocten1] · [Tym razem nie wierz Noctenowi][nocten2] · [Pierwsza lekcja][nocten3]
- **U02 [?]** — miasto za białymi murami ze wspomnień Noctena. Bohater znajduje rozbieżności w dokumentach, więc atlas pozostawia ten punkt bez współrzędnych. [Droga, której nie było][nocten1]
- **U03 [O/?]** — wczesny warsztat Rian, pobliska osada z unieruchomioną wieżą, miejsce zapłaty poza Val Meris oraz sala komisji. Rękopisy pozwalają przypisać im funkcję, lecz nie podają pełnych adresów. [Jedenaście minut][rian1] · [Drugi ołówek][rian2] · [Kobieta, która wiedziała][adrien2]
- **U04 [O/?]** — inne bezimienne postoje przewoźników na północnym szlaku i gospody wspomniane ogólnie przy drodze z Verdan. Teksty nie rozróżniają poszczególnych lokali, poza nazwanymi opisowo punktami w indeksie. [Ślady w popiele][piecarius2] · [Dzień, który miał być spokojny][thora2]

## Aktualizowanie atlasu

1. Dodaj punkt z wolnym identyfikatorem danej krainy. Zapisz nazwę albo opis, rodzaj miejsca, najbliższy potwierdzony punkt lub trasę i link do rozdziału. Bezimienna gospoda może pozostać bezimienna w indeksie.
2. Uzupełnij szkic lokalny. Jeśli źródło ustala kolejność miejsc, wpisz ją przy trasie R. Przy podanej dacie lub czasie podróży dopisz bohatera i rok, żeby można było porównać przejazdy.
3. Dodaj znacznik do mapy kontynentu, gdy znane jest jego położenie na tyle dobrze, żeby umieścić go względem istniejących punktów. Przy niejasnej geografii użyj istniejącego opisu krainy lub rejestru U i zachowaj odnośnik do źródła.
4. Przy wspólnej scenie, jak rozmowa Noctena z Piecariusem w N09, użyj jednego identyfikatora i podlinkuj oba opowiadania.

[mapa]: assets/lore/cartography/riftoryn-mapa-swiata-v1.png
[gdd]: RIFTORYN_GDD.md
[val]: world_history.md#val-meris--wspólny-węzeł
[przesmyk]: world_history.md#jeden-szlak--biały-przesmyk
[vesper]: world_history.md#vesper--tradycje-ciemności
[vareth]: world_history.md#vareth--tradycje-ognia
[zerath]: world_history.md#zerath--tradycje-błyskawic
[verdan]: world_history.md#verdan--tradycje-natury
[kharum]: world_history.md#kharum--tradycje-ziemi
[aurel]: world_history.md#aurel--tradycje-światła
[adrien1]: opowiadania/adrien/1_the_light_that_showed_the_way/historia.pl.md
[adrien2]: opowiadania/adrien/2_the_woman_who_knew/historia.pl.md
[ana]: opowiadania/ana/1_when_the_ground_dries/historia.pl.md
[eron]: opowiadania/eron/1_through_someones_field/historia.pl.md
[lena3]: opowiadania/lena/3_i_know_someone_who_can/historia.pl.md
[liora]: opowiadania/liora/1_the_handover/historia.pl.md
[lucen1]: opowiadania/lucen/1_from_what_was_saved/historia.pl.md
[lucen2]: opowiadania/lucen/2_learning_control/historia.pl.md
[lucen3]: opowiadania/lucen/3_not_against_you/historia.pl.md
[miko1]: opowiadania/miko/1_that_should_work/historia.pl.md
[miko2]: opowiadania/miko/2_one_more_pulse/historia.pl.md
[miko3]: opowiadania/miko/3_one_more_bolt/historia.pl.md
[morrak1]: opowiadania/morrak/1_the_name_he_didnt_speak/historia.pl.md
[morrak2]: opowiadania/morrak/2_witnesses_to_silence/historia.pl.md
[morrak3]: opowiadania/morrak/3_a_name_on_the_list/historia.pl.md
[nocten1]: opowiadania/nocten/1_the_road_that_wasnt_there/historia.pl.md
[nocten2]: opowiadania/nocten/2_dont_believe_nocten_this_time/historia.pl.md
[nocten3]: opowiadania/nocten/3_the_first_lesson/historia.pl.md
[piecarius1]: opowiadania/piecarius/1_the_book_without_an_author/historia.pl.md
[piecarius2]: opowiadania/piecarius/2_traces_in_the_ashes/historia.pl.md
[piecarius3]: opowiadania/piecarius/3_a_vow_from_my_fathers_book/historia.pl.md
[rokan2]: opowiadania/rokan/2_the_longer_road/historia.pl.md#pod-półką
[ilda1]: opowiadania/ilda/1_from_the_orchard/historia.pl.md
[ilda3]: opowiadania/ilda/3_a_way_back/historia.pl.md
[rokan3]: opowiadania/rokan/3_one_goes_first/historia.pl.md#miejsce-na-wozie
[rian1]: opowiadania/rian/1_eleven_minutes/historia.pl.md
[rian2]: opowiadania/rian/2_the_second_pencil/historia.pl.md
[saila1]: opowiadania/saila/1_the_burden_of_the_crystal/historia.pl.md
[saila2]: opowiadania/saila/2_when_the_tower_fell_silent/historia.pl.md
[saila3]: opowiadania/saila/3_what_ruvan_didnt_teach_me/historia.pl.md
[taren1]: opowiadania/taren/1_what_remained/historia.pl.md
[taren2]: opowiadania/taren/2_the_cost_of_loyalty/historia.pl.md
[taren3]: opowiadania/taren/3_on_the_other_side/historia.pl.md
[thora1]: opowiadania/thora/1_the_last_root/historia.pl.md
[thora2]: opowiadania/thora/2_the_day_that_should_have_been_quiet/historia.pl.md
[thora3]: opowiadania/thora/3_enough/historia.pl.md
[vaelis1]: opowiadania/vaelis/1_the_sealed_copy/historia.pl.md

[varek1]: opowiadania/varek/1_the_second_signal/historia.pl.md

[veda1]: opowiadania/veda/1_what_the_road_keeps/historia.pl.md

[volt1]: opowiadania/volt/1_the_wing/historia.pl.md
