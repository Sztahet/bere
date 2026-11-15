# Grafiki bohaterów

Ten katalog jest miejscem, w którym szukamy materiałów graficznych bohaterów:

- [portraits/](portraits/) — portrety źródłowe. Tu znajdują się również [Azmir](portraits/azmir-darkness-support.png) i [Vaelis](portraits/vaelis-darkness-support.png).
- [modeling-concepts/](modeling-concepts/) — arkusze i referencje do modeli.
- [stories/](stories/) — materiały ilustracyjne i komiksowe.

## Dlaczego portrety są też w Unity?

Gra wczytuje pliki z `riftoryn-unity/Assets/Resources/Portraits/` pod krótkimi nazwami, np. `azmir.png`. Niektóre strony i dokumenty odwołują się natomiast do nazw opisowych w `assets/heroes/portraits/`. Obie lokalizacje są więc potrzebne; nie należy samodzielnie usuwać ani przenosić kopii używanych przez projekt.

Pliki w `portraits/` są źródłem, a kopie Unity mają mieć tę samą zawartość. [Mapa nazw](portraits/portrait-map.json) łączy identyfikator bohatera z nazwą źródła. Dotychczasowe nazwy plików pozostawiono, ponieważ prowadzą do nich istniejące odnośniki.

Po zmianie lub dodaniu portretu:

1. Umieść plik źródłowy w `portraits/` i dodaj lub zaktualizuj jego wpis w mapie. Dla nowych prac stosuj opisową nazwę, np. `imie-zywiol-rola.png`.
2. Z głównego katalogu repozytorium uruchom `python scripts/sync_hero_portraits.py --sync`. Skrypt kopiuje obrazy do Unity, nie usuwa pozostałych plików i nie zmienia plików `.meta`.
3. Uruchom `python scripts/sync_hero_portraits.py`, aby sprawdzić zgodność. Jeśli pojawił się plik poza mapą, najpierw ustal jego pochodzenie i uzupełnij mapę.

Nowe portrety powinny mieć przezroczyste tło. Tło dobiera interfejs; starsze portrety zachowują swój obecny wygląd. Arkusze modeli oraz ilustracje mogą mieć osobne kopie używane przez Unity i czytniki — przed ich przenoszeniem należy sprawdzić odnośniki.

