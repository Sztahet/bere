# Archiwum Riftoryn

Otwórz [archiwum-riftoryn.html](../../archiwum-riftoryn.html), aby wybrać bohatera i czytać kolejne części. Strona działa również po otwarciu pliku z dysku. Korzysta z danych przygotowanych poleceniem `python scripts/build_story_archive.py`. Uruchom je po każdej zmianie opowieści. Saila i Adrien korzystają z plików JSON gry. Lucen i Taren korzystają ze szkiców Markdown w głównym katalogu repozytorium. Katalog opowieści i kolejność części znajdują się w skrypcie.

## Opowieść Saily — wspólne źródło

Tekst, kolejność rozdziałów i ilustracji oraz opisy alternatywne są w `Assets/Resources/SailaStory/story.json`. Ilustracje `01.jpg`–`08.jpg` w tym samym katalogu są wspólne dla gry i strony. Edytuj te pliki, aby zmienić opublikowaną opowieść; `saila-opowiesc.html` jest tylko szkieletem czytnika. Kod i style strony są w `riftoryn-unity/Web/`. Plik `RIFTORYN_SAILA_MINIKSIAZKA_PROPOZYCJA.md` jest literackim szkicem, nie źródłem czytników — po zmianach tekstu należy świadomie zadbać o jego aktualność.

W „Ciężarze kryształu” kadr 07 jest w rozdziale III, a nowe kadry 06 i 08 są odpowiednio w rozdziałach VI i VII. [Prompty i nazwy plików](../../RIFTORYN_SAILA_PROMPTY_AKTUALIZACJA.md).

Po zmianie uruchom `python -m unittest discover -s tests`, sprawdź Unity i opublikuj stronę. Strona po wdrożeniu pobiera aktualny JSON; istniejący build Unity zawiera zasoby z chwili jego zbudowania, więc po zmianie zasobów potrzebuje nowego builda.

Wspólne źródło obejmuje opowieść Saily. Pozostałe sekcje starszego prototypu HTML mają osobne dane i zasoby; nie zakładaj ich synchronizacji z Unity.

## Gdy wieża zamilkła

Druga opowieść korzysta z `Assets/Resources/SailaAwakening/story.json` i ilustracji z tego samego katalogu. HTML `saila-gdy-wieza-zamilkla.html` i czytnik w Unity pobierają te same rozdziały oraz obrazy. Wersja robocza zawiera sześć ilustracji (01, 02, 04, 05, 06 i 07); numery zachowują oznaczenia scen z pierwotnego planu. Tekst roboczy znajduje się także w `RIFTORYN_SAILA_ODBLOKOWANIE_OPOWIESC_DRAFT.md`.

## To, czego nie nauczył mnie Ruvan

Finał korzysta z `Assets/Resources/SailaFinale/story.json`. Wszystkie osiem kadrów jest w tekście, a kadr 04 jest również okładką strony. [Opisy scen i miejsca w tekście](../../RIFTORYN_SAILA_FINAL_PROMPTY_ILUSTRACJE.md).

## Opowieści Adriena w grze

Trzy opowieści są dostępne z karty Adriena w kodeksie Unity oraz w Archiwum Riftoryn. Korzystają odpowiednio z `Assets/Resources/AdrienStory1/story.json`, `AdrienStory2/story.json` i `AdrienStory3/story.json`. Są w języku polskim. Czytniki pomijają miejsca na ilustracje oznaczone jako `pending`. [Plan miejsc i instrukcja dodawania obrazów](../../RIFTORYN_ADRIEN_ILUSTRACJE_PLAN.md). Teksty JSON można odtworzyć ze szkiców poleceniem `python scripts/build_adrien_stories.py`, które zachowuje istniejące bloki ilustracji. Następnie uruchom skrypt archiwum.

## Opowieści Leny w grze

Trzy kanoniczne opowieści są dostępne z karty Leny w kodeksie i w Archiwum Riftoryn. Po zmianie rękopisów odtwórz zasoby `LenaStory1`–`LenaStory3` poleceniem `python scripts/build_lena_stories.py`, a następnie uruchom `python scripts/build_story_archive.py`. Opowieści nie mają jeszcze ilustracji. Sama bohaterka nadal jest w przygotowaniu.

## Splotyt w grze

Przycisk **Splotyt** w kodeksie Unity otwiera poradnik w języku polskim, zapisany w `Assets/Resources/Splotyt/story.json`. Jego tekst pochodzi ze strony `splotyt.html`; po edycji strony odśwież zasób gry poleceniem `python scripts/build_splotyt_story.py`, sprawdź `python -m unittest discover -s tests` i zbuduj grę ponownie.
