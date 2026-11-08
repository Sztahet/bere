# Bonusy klas i pasywki bohaterów — propozycja do zatwierdzenia

Każda postać ma **bonus wspólny dla klasy** oraz **własną pasywkę bohatera**. Bonus klasy w menu powinien być osobną pozycją z ikoną roli (`assets/ui/roles/tank.png`, `support.png`, `specialist.png` lub `summoner.png`). Pasywka bohatera jest osobną pozycją i może mieć własną ikonę.

Wartości poniżej to propozycje do omówienia. Zachowane efekty z dotychczasowej specyfikacji są oznaczone jako istniejące; nowe efekty jako propozycje. Bonus klasowy nie zastępuje ani nie wlicza się do unikalnej pasywki bohatera.

## Bonusy klasowe

### Tank — ikona klasy `tank.png`
**Elementalna wytrzymałość:** otrzymuje dodatkowe +5% odporności na obrażenia każdego żywiołu.

### Wsparcie — ikona klasy `support.png`
**Pomocna dłoń:** leczenie i naprawy wykonywane przez bohatera są o 10% silniejsze. Nie zwiększa osłon ani efektów, które nie leczą i nie naprawiają.

### Specjalista — ikona klasy `specialist.png`
**Skupiony ostrzał:** wieża ręcznie pilotowana przez Specjalistę w fazie obrony otrzymuje +25% szybkości ataku. Premia działa podczas ręcznego sterowania i nie zwiększa szybkości automatycznego ostrzału.

### Przywoływacz — ikona klasy `summoner.png`
**Pet — towarzysz:** bohaterowi towarzyszy jego własny Pet z osobnym paskiem HP, który automatycznie podąża za nim. Przekierowanie części obrażeń na Peta jest osobną propozycją do ustalenia, nie częścią zatwierdzonego efektu tej premii.

## Pasywki bohaterów

### Tankowie

#### Brom — Kamienny pancerz *(istniejąca pasywka)*
Otrzymuje o 10% mniej obrażeń od bezpośrednich trafień. W obronie ta sama premia chroni jego ręcznie pilotowaną wieżę.

#### Thora — Żywotne korzenie *(istniejąca pasywka)*
Otrzymuje o 15% więcej leczenia. W obronie jej naprawa osłony jest silniejsza.

#### Morrak — Oczy mroku *(istniejąca pasywka)*
Efekty ograniczające jego widoczność trwają o 20% krócej. Dotyczy to również widoku wieży podczas jego ręcznego pilotażu.

#### Kael — Hartowanie *(istniejąca pasywka)*
Otrzymuje o 20% mniej obrażeń okresowych. W obronie ta sama premia dotyczy ręcznie pilotowanej przez nią wieży.

#### Iskar — Lodowa zbroja
Regeneruje 1 punkt osłony na sekundę, maksymalnie do 10 punktów z tej pasywki. Osłona sumuje się z innymi źródłami, ale gdy łączna osłona Iskara przekracza 10, regeneracja pasywki jest wyłączona.

#### Taren — Ostatni bastion
Gdy jego HP spadnie poniżej 50%, Taren staje się niewrażliwy na obrażenia przez 2 sekundy. Efekt ma 10-sekundowy cooldown i może ponownie zadziałać, gdy będzie gotowy, a HP Tarena ponownie spadnie poniżej progu.

#### Orien — Święta aura *(Holy Aura)*
Aura Oriena oślepia przeciwników i daje Orienowi oraz sojusznikom w promieniu 3 m 10% szansy na uniknięcie otrzymanego ataku. Zasięg uniku i działanie na obrażenia obszarowe wymagają doprecyzowania.

### Wsparcie

#### Liora — Troska *(istniejąca pasywka)*
Jej leczenie i naprawa są o 10% silniejsze; wartości jej zdolności uwzględniają tę premię. Po dodaniu bonusu klasy Wsparcia należy uzgodnić, czy te premie się łączą.

#### Elira — Jasny wzrok *(istniejąca pasywka)*
Obiekty wykryte przez sojuszników pozostają widoczne dla drużyny o 1 sekundę dłużej po utracie wykrycia. Pasywka nie odkrywa obiektów samodzielnie.

#### Varek — Resztkowy impuls *(istniejąca pasywka)*
Po użyciu Q porusza się o 10% szybciej przez 2 sekundy. W obronie przygotowanie do ręcznego pilotażu trwa u niego 1,8 sekundy zamiast roboczych 2 sekund.

#### Niva — Mrozoodporność *(istniejąca pasywka)*
Działające na nią spowolnienia ruchu są o 20% słabsze. Wyłączenia wieży, którą ręcznie pilotuje, są o 20% krótsze.

#### Solan — Ciepło wspólnoty *(propozycja)*
Po użyciu zdolności osłaniającej Solan zyskuje krótką premię do prędkości ruchu, by móc dotrzeć do kolejnego sojusznika. Czas i wysokość premii do ustalenia.

#### Dara — Pewna ręka *(propozycja)*
Po naprawieniu sojuszniczej wieży jej kolejna naprawa tej samej wieży w krótkim czasie jest silniejsza. Premia wygasa po użyciu; wartość i czas do ustalenia.

#### Nocten — Szept archiwisty *(propozycja)*
Gdy Nocten wykryje przeciwnika, drużyna otrzymuje krótką informację o jego kierunku ruchu nawet po utracie bezpośredniego widoku. Czas i warunki działania do ustalenia.

### Specjaliści

#### Saila — Zimny krok *(istniejąca pasywka)*
Spowolnienia ruchu działają na nią o 15% słabiej. W obronie ręcznie pilotowana przez nią wieża ma o 10% większy promień wykrywania.

#### Zet — Iskra manewru *(istniejąca pasywka)*
Po użyciu Q otrzymuje 15% prędkości na 2 sekundy. W obronie ręcznie pilotowana przez niego wieża ma o 10% większy promień wykrywania.

#### Rian — Żaroodporność *(istniejąca pasywka)*
Otrzymuje o 15% mniej obrażeń Ognia. W obronie ta sama premia chroni ręcznie pilotowaną przez nią wieżę przed zdolnościami Ognia.

#### Morven — Krok w cień *(istniejąca pasywka)*
Po użyciu Q promień wykrywania Morvena przez wrogie wieże jest o 20% mniejszy przez 2 sekundy. W obronie Runnerzy odkrywają jego ręcznie pilotowaną wieżę z odległości mniejszej o 2 m; pasywka nie ukrywa informacji już zdobytych.

#### Miko — Szczęście sprzyja zuchwałym *(propozycja)*
Po udanym sabotażu porusza się o 10% szybciej przez 2 sekundy. Pechowy wynik zdolności nie aktywuje premii.

#### Eron — Tropiciel szlaku *(propozycja)*
Po wejściu w pobliże nieodkrytej wieży otrzymuje krótką wskazówkę o jej położeniu. Pasywka nie ujawnia wieży całej drużynie; zasięg do ustalenia.

#### Kiran — Czytanie terenu *(propozycja)*
Po krótkim bezruchu wykrywa zagrożenia i pułapki w niewielkim obszarze przed sobą. Czas przygotowania, zasięg i rodzaje wykrywanych obiektów do ustalenia.

#### Adrien — Niezłomny operator *(propozycja)*
Wieża, którą ręcznie pilotuje, nie może zostać wyłączona ani zatrzymana przez zdolności przeciwnika. Wpływ tej ochrony na obrażenia i zniszczenie wieży wymaga osobnej decyzji.

### Przywoływacze

> Bonus klasy Przywoływacza opisuje obecność Peta i jego podstawowe zachowanie. Poniższe pasywki są indywidualnymi cechami bohaterów lub ich więzi z Petem.

#### Pyrra — Żar pod kontrolą *(propozycja)*
Po otrzymaniu obrażeń od Ognia zyskuje na krótko odporność na podpalenie. Czas i odnowienie do ustalenia.

#### Eira — Spokojny oddech *(propozycja)*
Spowolnienia ruchu działają na niego o 15% słabiej. W obronie jego wieża szybciej odzyskuje zwykłą szybkostrzelność po zakończeniu spowolnienia.

#### Volt — Rytm przekaźnika *(propozycja)*
Po użyciu Q porusza się o 10% szybciej przez 2 sekundy. Wartość i czas wymagają testów.

#### Ana — Opiekuńczy dotyk *(propozycja)*
Jej leczenie jest silniejsze wobec sojusznika, który ma najmniej HP w zasięgu. Premia i sposób wyboru celu do ustalenia.

#### Rokan — Twardy grunt *(propozycja)*
Pierwsze bezpośrednie trafienie po rozpoczęciu rundy zadaje mu o 10% mniej obrażeń. Odnowienie i działanie w obronie do ustalenia.

#### Lumen — Uważna obserwatorka *(propozycja)*
Wykryci przez nią przeciwnicy pozostają widoczni dla drużyny o 1 sekundę dłużej. Nie wykrywa ich samodzielnie.

#### Koneva — Zwinny unik *(propozycja)*
Po uniknięciu obrażeń zyskuje krótką premię do prędkości ruchu. Czas trwania i odnowienie do ustalenia.

#### Neris — Czujny lot *(istniejąca koncepcja do testów)*
Jej Pet, kruk Cień, automatycznie utrzymuje się około 3 m przed nią wzdłuż trasy. Każde trafienie skierowane w Cień ma 25% szansy na uniknięcie obrażeń.
