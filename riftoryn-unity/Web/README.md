# Opowieść Saily — wspólne źródło

Tekst, kolejność rozdziałów i ilustracji oraz opisy alternatywne są w `Assets/Resources/SailaStory/story.json`. Ilustracje `01.jpg`–`08.jpg` w tym samym katalogu są wspólne dla gry i strony. Edytuj te pliki, aby zmienić opublikowaną opowieść; `saila-opowiesc.html` jest tylko szkieletem czytnika. Kod i style strony są w `riftoryn-unity/Web/`. Plik `RIFTORYN_SAILA_MINIKSIAZKA_PROPOZYCJA.md` jest literackim szkicem, nie źródłem czytników — po zmianach tekstu należy świadomie zadbać o jego aktualność.

W „Ciężarze kryształu” kadr 07 jest w rozdziale III, a nowe kadry 06 i 08 są odpowiednio w rozdziałach VI i VII. [Prompty i nazwy plików](../../RIFTORYN_SAILA_PROMPTY_AKTUALIZACJA.md).

Po zmianie uruchom `python -m unittest discover -s tests`, sprawdź Unity i opublikuj stronę. Strona po wdrożeniu pobiera aktualny JSON; istniejący build Unity zawiera zasoby z chwili jego zbudowania, więc po zmianie zasobów potrzebuje nowego builda.

Wspólne źródło obejmuje opowieść Saily. Pozostałe sekcje starszego prototypu HTML mają osobne dane i zasoby; nie zakładaj ich synchronizacji z Unity.

## Gdy wieża zamilkła

Druga opowieść korzysta z `Assets/Resources/SailaAwakening/story.json` i ilustracji z tego samego katalogu. HTML `saila-gdy-wieza-zamilkla.html` i czytnik w Unity pobierają te same rozdziały oraz obrazy. Wersja robocza zawiera sześć ilustracji (01, 02, 04, 05, 06 i 07); numery zachowują oznaczenia scen z pierwotnego planu. Tekst roboczy znajduje się także w `RIFTORYN_SAILA_ODBLOKOWANIE_OPOWIESC_DRAFT.md`.

## To, czego nie nauczył mnie Ruvan

Finał korzysta z `Assets/Resources/SailaFinale/story.json`. Wszystkie osiem kadrów jest w tekście, a kadr 04 jest również okładką strony. [Opisy scen i miejsca w tekście](../../RIFTORYN_SAILA_FINAL_PROMPTY_ILUSTRACJE.md).

## Opowieści Adriena w grze

Trzy opowieści są dostępne z karty Adriena w kodeksie Unity i korzystają odpowiednio z `Assets/Resources/AdrienStory1/story.json`, `AdrienStory2/story.json` i `AdrienStory3/story.json`. Są w języku polskim. Każda zawiera osiem oznaczonych jako `pending` miejsc na przyszłe ilustracje; czytnik je pomija, dopóki nie zostaną uzupełnione. [Plan miejsc i instrukcja dodawania obrazów](../../RIFTORYN_ADRIEN_ILUSTRACJE_PLAN.md). Teksty JSON można odtworzyć ze szkiców poleceniem `python scripts/build_adrien_stories.py`; istniejące bloki ilustracji zostają zachowane. Opowieści Adriena nie mają jeszcze osobnych stron HTML.
