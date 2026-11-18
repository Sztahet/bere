# Grafiki bohaterów

Ten katalog jest miejscem, w którym szukamy materiałów graficznych bohaterów:

- [portraits/](portraits/) — portrety źródłowe. Tu znajdują się również [Azmir](portraits/azmir-darkness-support.png) i [Vaelis](portraits/vaelis-darkness-support.png).
- [modeling-concepts/](modeling-concepts/) — arkusze i referencje do modeli.
- [stories/](stories/) — ilustracje rozdziałów, komiksy i referencje rysunkowe.
- [companions/](companions/) — grafiki i podglądy towarzyszy.

## Dlaczego portrety są też w Unity?

`portraits/` zawiera oryginały. Gra wczytuje kopie z `riftoryn-unity/Assets/Resources/Portraits/` pod krótkimi nazwami, np. `azmir.png`. [Mapa nazw](portraits/portrait-map.json) łączy identyfikator bohatera z nazwą źródła.

Po zmianie lub dodaniu portretu:

1. Umieść plik źródłowy w `portraits/` i dodaj lub zaktualizuj jego wpis w mapie. Dla nowych prac stosuj opisową nazwę, np. `imie-zywiol-rola.png`.
2. Z głównego katalogu repozytorium uruchom `python scripts/sync_artwork.py --sync`, aby uaktualnić kopie Unity i dane czytnika WWW.

Nowe portrety powinny mieć przezroczyste tło. Pozostałe działy znajdziesz w [bibliotece grafik](../README.md).

