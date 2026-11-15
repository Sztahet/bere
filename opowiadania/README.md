# Opowiadania Riftoryn

Każdy bohater ma swój katalog, a każda część osobny podkatalog. W nim edytuj `historia.pl.md`. Pierwszy nagłówek `#` to tytuł, a `##` rozpoczyna rozdział. Zachowuj puste linie między akapitami.

Opcjonalne tłumaczenie zapisz obok jako `historia.en.md`, w tym samym układzie. Dopóki go nie ma, czytnik nie oferuje angielskiej wersji. Nie wpisuj polskiego tekstu do angielskiego pliku jako zastępnika.

Ilustrację wstaw w obu tekstach znacznikiem `<!-- ilustracja: 01 -->`. W `ilustracje.json` zapisz numer, opis alternatywny `alt` i opcjonalnie `altEn`. Znacznik z `pending: true` rezerwuje miejsce bez wyświetlania pustego obrazka. Gotowy plik `01.jpg` umieść w podfolderze `ilustracje` obok tekstu i usuń `pending: true`; skrypt skopiuje go do zasobów Unity. Ilustracja jest wspólna dla obu języków.

Kolejność części, statusy i powiązania z zasobami Unity są w `katalog.json`. Relacje grafu są w `riftoryn-unity/Web/hero-relations-data.js`, każda para występuje raz.

Po każdej zmianie uruchom z głównego katalogu:

```text
python scripts/sync_stories.py
python -m unittest discover -s tests
```

Skrypt aktualizuje dane archiwum HTML i zasoby Unity. Istniejący build Unity trzeba przebudować, aby zawierał zmienione zasoby. Poprzednie rękopisy i ich notatki zachowano w `_archiwum`; nie są źródłem czytników i nie służą do bieżącej edycji.
