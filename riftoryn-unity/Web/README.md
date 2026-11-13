# Opowieść Saily — wspólne źródło

Tekst, kolejność rozdziałów i ilustracji oraz opisy alternatywne są w `Assets/Resources/SailaStory/story.json`. Ilustracje `01.jpg`–`08.jpg` w tym samym katalogu są wspólne dla gry i strony. Edytuj te pliki, aby zmienić opublikowaną opowieść; `saila-opowiesc.html` jest tylko szkieletem czytnika. Kod i style strony są w `riftoryn-unity/Web/`. Plik `RIFTORYN_SAILA_MINIKSIAZKA_PROPOZYCJA.md` jest literackim szkicem, nie źródłem czytników — po zmianach tekstu należy świadomie zadbać o jego aktualność.

Po zmianie uruchom `python -m unittest discover -s tests`, sprawdź Unity i opublikuj stronę. Strona po wdrożeniu pobiera aktualny JSON; istniejący build Unity zawiera zasoby z chwili jego zbudowania, więc po zmianie zasobów potrzebuje nowego builda.

Wspólne źródło obejmuje opowieść Saily. Pozostałe sekcje starszego prototypu HTML mają osobne dane i zasoby; nie zakładaj ich synchronizacji z Unity.

## Gdy wieża zamilkła

Druga opowieść korzysta z `Assets/Resources/SailaAwakening/story.json` i ilustracji z tego samego katalogu. HTML `saila-gdy-wieza-zamilkla.html` i czytnik w Unity pobierają te same rozdziały oraz obrazy. Wersja robocza zawiera trzy zatwierdzone ilustracje (01, 05 i 07); pozostałe trzy zostaną dodane po ich przygotowaniu. Tekst roboczy znajduje się także w `RIFTORYN_SAILA_ODBLOKOWANIE_OPOWIESC_DRAFT.md`.
