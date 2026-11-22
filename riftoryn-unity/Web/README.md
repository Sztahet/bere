# Archiwum Riftoryn

Archiwum HTML i zasoby opowieści Unity korzystają z rękopisów w `opowiadania` i oryginalnych grafik w `assets`. [Instrukcja opowiadań](../../opowiadania/README.md) · [biblioteka grafik](../../assets/README.md).

Po edycji uruchom z katalogu głównego `python scripts/sync_artwork.py --sync`, `python scripts/build_timeline_web.py`, a następnie `python -m unittest discover -s tests`. Otwórz `archiwum-riftoryn.html`, aby sprawdzić stronę. Istniejący build Unity wymaga przebudowania po zmianie zasobów.

Generator osi czasu łączy datowane sceny z katalogu opowiadań z wydarzeniami z sekcji „Pełna oś czasu” w `world_history.md`. Wydarzenia historyczne nie przypisują bohaterów, a filtr postaci pozostawia je jako kontekst chronologii.

Oryginały [ilustracji Saily](../../assets/heroes/stories/illustrations/saila/) są wspólne dla obu czytników. Znaczniki i opisy ilustracji pozostają przy rękopisach w `opowiadania/saila`.
