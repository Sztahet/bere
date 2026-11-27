# Opowiadania Riftoryn

To główne źródło informacji o postaciach i wydarzeniach pokazanych w ich historiach. Czytaj wszystkie części wskazane w `katalog.json` oraz istotne sceny w opowiadaniach innych bohaterów. Rozróżniaj ukończone części od oznaczonych `draft: true`: szkice mogą się zmienić. Zasady świata i chronologię opisuje [historia świata](../world_history.md); rozbieżności między aktualnymi tekstami wymagają sprawdzenia, a nie automatycznego nadpisania jednej wersji. GDD opisuje projekt gry, nie zastępuje opowiadań ani nie jest podstawą oceny postaci.

Każdy bohater ma swój katalog, a każda część osobny podkatalog nazwany w formacie `<numer>_<english_title>`: numer części (`1_`, `2_`, `3_` itd.), a po nim angielski tytuł zapisany małymi literami i z podkreśleniami. W nim edytuj `historia.pl.md`. Pierwszy nagłówek `#` to tytuł, a `##` rozpoczyna rozdział. Zachowuj puste linie między akapitami.

Pod nagłówkiem każdego rozdziału podaj datę z rokiem i erą oraz miejsce akcji. Przed pisaniem lub redakcją przeczytaj [złote zasady pisania opowieści](DOCELOWY_FORMAT.md#złote-zasady-pisania-opowieści).

Sprawdzaj położenie i połączenia w [atlasie ASCII Riftorynu](../RIFTORYN_MAPA_ASCII.md). Przy dodawaniu nowej osady, gospody, drogi lub innego punktu na szlaku dopisz go do indeksu atlasu wraz z odnośnikiem do rozdziału.

Opcjonalne tłumaczenie zapisz obok jako `historia.en.md`, w tym samym układzie. Dopóki go nie ma, czytnik nie oferuje angielskiej wersji. Nie wpisuj polskiego tekstu do angielskiego pliku jako zastępnika.

Ilustrację wstaw w obu tekstach znacznikiem `<!-- ilustracja: 01 -->`. W `ilustracje.json` obok rękopisu zapisz numer, opis alternatywny `alt` i opcjonalnie `altEn`. Znacznik z `pending: true` rezerwuje miejsce. Gotowy plik `01.jpg` umieść w `assets/heroes/stories/illustrations/<bohater>/<część>/` i usuń `pending: true`. Na przykład dla `opowiadania/rian/1_eleven_minutes/` oryginał należy do [ilustracji „Jedenastu minut”](../assets/heroes/stories/illustrations/rian/1_eleven_minutes/). Oryginał służy obu wersjom językowym.

Kolejność części, statusy i powiązania z zasobami Unity są w `katalog.json`. Pole `draft: true` oznacza część do zatwierdzenia; po zatwierdzeniu ustaw `draft: false` dla konkretnej części i uruchom synchronizację. Oznaczenia w obu czytnikach korzystają z tego samego statusu. Relacje grafu są w `riftoryn-unity/Web/hero-relations-data.js`, każda para występuje raz.

Po każdej zmianie uruchom z głównego katalogu:

```text
python scripts/sync_artwork.py --sync
python -m unittest discover -s tests
```

Skrypt aktualizuje dane archiwum HTML i zasoby Unity. Istniejący build Unity trzeba przebudować, aby zawierał zmienione zasoby. Poprzednie rękopisy i ich notatki zachowano w `_archiwum`; nie są źródłem czytników i nie służą do bieżącej edycji.
