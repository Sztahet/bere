# Opowiadania Riftoryn

Rękopisy są podzielone na katalogi według imienia bohatera. Pliki o numerach 01–03 to kolejne części. Można je edytować dowolnym edytorem tekstu obsługującym UTF-8. Nagłówki i numerację rozdziałów należy zachować: skrypty używają ich do podziału opowieści.

HTML i Unity nie pobierają tych plików automatycznie. Po edycji należy uruchomić odpowiednie polecenia z głównego katalogu repozytorium:

- **Piecarius, Nocten, Morrak:** `python scripts/build_draft_stories.py`, a następnie `python scripts/build_story_archive.py`. Powstają dane dla Unity i archiwum HTML.
- **Adrien:** `python scripts/build_adrien_stories.py`, a następnie `python scripts/build_story_archive.py`. Skrypt zachowuje istniejące bloki ilustracji.
- **Lena:** `python scripts/build_lena_stories.py`, a następnie `python scripts/build_story_archive.py`.
- **Rian, Lucen, Taren:** `python scripts/build_story_archive.py` odświeża archiwum HTML. Te pełne opowieści nie mają obecnie osobnych czytników w Unity.
- **Saila:** rękopisy są w katalogu `saila`, ale źródłem tekstu i ilustracji czytników są obecnie pliki `riftoryn-unity/Assets/Resources/SailaStory/story.json`, `SailaAwakening/story.json` i `SailaFinale/story.json`. Zmiany tekstu trzeba uzgodnić w obu wersjach, zachowując ilustracje i ich opisy, a następnie uruchomić `python scripts/build_story_archive.py`. Testy sprawdzają zgodność tekstów.

Po aktualizacji uruchom `python -m unittest discover -s tests`. Gotowy build Unity zawiera zasoby z chwili budowania, więc po zmianach wymaga nowego builda. Pliki koncepcji, biografii i materiały ilustracyjne pozostają na razie poza tym katalogiem.
