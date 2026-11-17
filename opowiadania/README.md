# Opowiadania Riftoryn

Każdy bohater ma swój katalog, a każda część osobny podkatalog nazwany w formacie `<numer>_<english_title>`: numer części (`1_`, `2_`, `3_` itd.), a po nim angielski tytuł zapisany małymi literami i z podkreśleniami. W nim edytuj `historia.pl.md`. Pierwszy nagłówek `#` to tytuł, a `##` rozpoczyna rozdział. Zachowuj puste linie między akapitami.

Pod nagłówkiem każdego rozdziału podaj datę z rokiem i erą oraz miejsce akcji. Przed pisaniem lub redakcją przeczytaj [złote zasady pisania opowieści](DOCELOWY_FORMAT.md#złote-zasady-pisania-opowieści).

Sprawdzaj położenie i połączenia w [atlasie ASCII Riftorynu](../RIFTORYN_MAPA_ASCII.md). Przy dodawaniu nowej osady, gospody, drogi lub innego punktu na szlaku dopisz go do indeksu atlasu wraz z odnośnikiem do rozdziału.

Opcjonalne tłumaczenie zapisz obok jako `historia.en.md`, w tym samym układzie. Dopóki go nie ma, czytnik nie oferuje angielskiej wersji. Nie wpisuj polskiego tekstu do angielskiego pliku jako zastępnika.

Ilustrację wstaw w obu tekstach znacznikiem `<!-- ilustracja: 01 -->`. W `ilustracje.json` zapisz numer, opis alternatywny `alt` i opcjonalnie `altEn`. Znacznik z `pending: true` rezerwuje miejsce bez wyświetlania pustego obrazka. Gotowy plik `01.jpg` umieść w podfolderze `ilustracje` obok tekstu i usuń `pending: true`; skrypt skopiuje go do zasobów Unity. Ilustracja jest wspólna dla obu języków.

Kolejność części, statusy i powiązania z zasobami Unity są w `katalog.json`. Pole `draft: true` oznacza część do zatwierdzenia; po zatwierdzeniu ustaw `draft: false` dla konkretnej części i uruchom synchronizację. Oznaczenia w obu czytnikach korzystają z tego samego statusu. Relacje grafu są w `riftoryn-unity/Web/hero-relations-data.js`, każda para występuje raz.

Po każdej zmianie uruchom z głównego katalogu:

```text
python scripts/sync_stories.py
python -m unittest discover -s tests
```

Skrypt aktualizuje dane archiwum HTML i zasoby Unity. Istniejący build Unity trzeba przebudować, aby zawierał zmienione zasoby. Poprzednie rękopisy i ich notatki zachowano w `_archiwum`; nie są źródłem czytników i nie służą do bieżącej edycji.
