# Komiksy bohaterów w Unity

Komiksy i podpisy wyświetla kodeks po wybraniu przycisku historii na karcie bohatera. Oryginały plansz są w [bibliotece komiksów](../../assets/heroes/stories/comics/), a tytuły i napisy w `Assets/Resources/comic_stories.json`.

W czytniku przyciski − / + zmieniają powiększenie od 100% do 200%; po powiększeniu przewijanie poziome służy do zmiany kolumny. W małych oknach nawigacja przełącza się na większe przyciski, a dodatkowe strzałki przesuwają powiększoną planszę na boki.

Każdy wpis `comics` zawiera `heroId`, ścieżkę `resource` bez rozszerzenia, `titlePl`, `titleEn`, pole `titleRect` oraz listę `panels` z tekstami `pl`, `en` i położeniem. Współrzędne `x`, `y`, `width`, `height` są ułamkiem wymiarów oryginalnej planszy; początek układu znajduje się w lewym górnym rogu. Kolejność paneli odpowiada kolejności czytania. Jeden bohater może mieć kilka wpisów — pojawią się na liście w kolejności z pliku.

Dodając planszę, umieść obraz w `assets/heroes/stories/comics/`. Dodaj wpis z oboma językami, ustaw pola wewnątrz pustych ramek i uruchom z katalogu głównego `python scripts/sync_artwork.py --sync`. Sprawdź całą planszę w obu językach w grze. Gdy komiksu nie ma, widok pokazuje opisową historię albo informację o przygotowaniach.
