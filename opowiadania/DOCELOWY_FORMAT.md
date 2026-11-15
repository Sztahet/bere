# Jeden system opowiadań i relacji — projekt migracji

Ten dokument opisuje proponowany system docelowy. Istniejące czytniki nie obsługują go jeszcze: dopóki nie zostaną dostosowane i przetestowane, obowiązują instrukcje w `README.md`.

## Jeden katalog części

Każda część każdej historii ma jeden katalog, na przykład:

```text
opowiadania/piecarius/01-ksiega-bez-autora/
  historia.pl.md
  historia.en.md
  ilustracje.json
  ilustracje/
    01.jpg
    02.jpg
```

Obie wersje językowe są zwykłymi plikami tekstowymi, czytelnymi w edytorze. Każda zawiera tytuł, nagłówki rozdziałów oraz tekst w swoim języku. Pliku angielskiego nie tworzymy z polskim tekstem udającym tłumaczenie: jeśli tłumaczenia nie ma, jest ono oznaczone jako niedostępne.

W miejscu ilustracji w obu językach umieszczamy ten sam znacznik, na osobnej linii:

```text
<!-- ilustracja: 01 -->
```

Ten sam identyfikator łączy miejsce w tekście z jednym plikiem obrazu. Obrazy nie zawierają napisów i są wspólne dla obu języków. W `ilustracje.json` przechowujemy tylko informacje potrzebne do obrazu: identyfikator, nazwę pliku, opisy alternatywne PL i EN oraz status `pending` lub `ready`. Dla `pending` sam obraz nie musi jeszcze istnieć; czytnik pomija puste miejsce. Opisy alternatywne są osobne dla obu języków.

## Jeden katalog historii

Jeden plik `opowiadania/katalog.json` zawiera identyfikatory bohaterów i części, kolejność, ścieżki do katalogów części, status redakcyjny i dostępność języków. Tytuły rozdziałów i treść należą do plików językowych; katalog ich nie kopiuje. Status szkicu lub zatwierdzenia dotyczy tekstu, nie oznacza automatycznego zatwierdzenia tłumaczenia ani ilustracji.

## Jeden zbiór relacji

Jeden plik `opowiadania/relacje.json` przechowuje relacje wszystkich bohaterów jako listę par identyfikatorów, typu, opisu PL i EN, statusu oraz odnośnika do źródła. Relację wpisujemy tylko raz; graf pokazuje ją z obu stron. Rozróżniamy fakty, szkice i informacje tajne, żeby nie przedstawiać pomysłu jako zatwierdzonego kanonu ani nie ujawniać go bez oznaczenia. Teksty opowiadań nadal pokazują spotkania, ale nie są osobną bazą danych grafu.

## Publikacja i kontrola

Jeden skrypt synchronizacji waliduje katalog, języki, znaczniki, ilustracje, identyfikatory i relacje, a następnie generuje dane HTML oraz zasoby Unity. Wygenerowanych plików nie edytujemy ręcznie. Brakujące tłumaczenia i grafiki są jawnie oznaczone; nie blokują polskiej wersji. Błędne ścieżki, podwójne identyfikatory i brak znaczników dla gotowych ilustracji są błędami. Wybór języka w czytniku musi informować o brakującym tłumaczeniu, a nie udawać, że pokazuje wersję angielską.

Migracja musi zachować istniejące teksty, statusy, kolejność scen i grafiki, opisy alternatywne oraz dostępne adresy opowieści. Najpierw należy porównać Markdown z JSON-em Saily, Adriena i Leny, a ilustracje przypiąć do właściwych miejsc; nie zakładać, że stary Markdown zawsze jest aktualny. Następnie uruchomić równoległe generowanie i testy porównujące każdy tekst i obraz przed przełączeniem czytników. Na koniec usunąć stare generatory dopiero po sprawdzeniu HTML, Unity, przełączania języków i brakujących tłumaczeń. Istniejący build Unity wymaga przebudowania.
