# Archiwum Riftoryn

Archiwum HTML i zasoby opowieści Unity są generowane z tego samego katalogu `opowiadania`. Instrukcja edycji, języków i ilustracji: [opowiadania/README.md](../../opowiadania/README.md).

Po edycji uruchom z katalogu głównego `python scripts/sync_stories.py`, a następnie `python -m unittest discover -s tests`. Otwórz `archiwum-riftoryn.html`, aby sprawdzić stronę. Istniejący build Unity wymaga przebudowania po zmianie zasobów. Poprzednie generatory są zachowane jako zgodne punkty wejścia i uruchamiają ten sam skrypt.

Saila korzysta ze wspólnych obrazów w `Assets/Resources/SailaStory`, `SailaAwakening` i `SailaFinale`. Znaczniki i opisy ilustracji znajdują się w odpowiednich katalogach `opowiadania/saila`.
