# Biblioteka grafik Riftorynu

Oryginały grafik, które trafiają do projektu, przechowujemy tutaj według rodzaju.

- **Bohaterowie i towarzysze** · [portrety](heroes/portraits/), [arkusze do modelowania](heroes/modeling-concepts/), [towarzysze](heroes/companions/).
- **Opowieści** · [ilustracje rozdziałów](heroes/stories/illustrations/), [komiksy](heroes/stories/comics/), [referencje rysunkowe](heroes/stories/references/).
- **Świat** · [bestiariusz](lore/bestiary/), [mapy](lore/cartography/), [oś czasu](lore/timeline/), [wozy](lore/wagons/), [bandyci](lore/outlaws/), [pieniądz](lore/economy/).
- **Interfejs** · [żywioły](ui/elements/), [klasy](ui/roles/), [statystyki](ui/stats/), [stany](ui/statuses/), [drzewo talentów](ui/talent-tree/).
- **Materiały robocze** · [zrzuty komiksów](references/screenshots/comics/), [pliki modeli i dokumentacja](characters/).

## Dodawanie i aktualizacja

1. Umieść oryginał w odpowiednim podfolderze. Przy portretach użyj [mapy portretów](heroes/portraits/portrait-map.json), a przy ilustracjach rozdziałów postępuj zgodnie z [instrukcją opowiadań](../opowiadania/README.md).
2. [Mapa grafik Unity](unity-artwork.json) określa, które pozostałe foldery i pojedyncze pliki trafiają do gry. Na przykład każdy PNG w `lore/bestiary/` trafia do `WorldArtifacts/`.
3. Uruchom z katalogu głównego `python scripts/sync_artwork.py --sync`. Polecenie aktualizuje kopie Unity i dane czytnika WWW.

Archiwum WWW czyta oryginały z `assets/`. Unity ładuje kopie z `riftoryn-unity/Assets/Resources/`, gdzie przechowuje też ustawienia importu `.meta`. Kopie są w Git, żeby po pobraniu repozytorium edytor mógł od razu wczytać zasoby. Zgodność plików sprawdza `python scripts/sync_artwork.py`.
