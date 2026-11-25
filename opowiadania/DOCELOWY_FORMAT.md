# Format opowiadań i relacji

Bieżący format i polecenie synchronizacji opisuje [README](README.md). Rękopisy przechowujemy jako Markdown PL i opcjonalnie EN, a dane ilustracji jako JSON w katalogu każdej części. `katalog.json` określa kolejność i foldery zasobów Unity. Wszystkie relacje grafu znajdują się w `riftoryn-unity/Web/hero-relations-data.js`.

## Złote zasady pisania opowieści

1. **Każda opowieść zaczyna się od daty i miejsca akcji.** Pod nagłówkiem pierwszego rozdziału wpisz w osobnym akapicie `dzień miesiąca, rok ET/PT · miejsce`. Powtórz taki zapis pod każdym kolejnym nagłówkiem rozdziału. W obrębie rozdziału doprecyzuj czas lub miejsce, gdy nie wynikają jasno z narracji albo są istotne dla ciągłości; naturalne „rano” po scenie wieczornej w tym samym miejscu nie wymaga kolejnego nagłówka z datą. Podaj krainę, miasto lub konkretne miejsce. Jeśli sama nazwa budynku albo osady nie wystarcza do zorientowania się, dopisz miasto lub krainę.
2. **Dokładność daty wynika z fabuły.** Jeśli dzień ma znaczenie dla podróży, zlecenia, spotkania albo wspólnej sceny, ustal i podaj konkretną datę. Gdy dzień jest nieustalony i nie ma znaczenia dla chronologii, podaj miesiąc, porę roku lub zakres dat z wyraźnym rokiem i erą, np. `Koniec marca, 12 ET · Korzenna Przystań, Verdan`. Jeśli chronologia ustala jedynie rok lub przedział lat, użyj tej dokładności. Zwroty typu „później” i „przed Igrzyskami” wymagają dopisania roku i ery. Daty i nazwy miejsc sprawdzaj w [kanonie świata](../world_history.md) i w opowiadaniach powiązanych z daną sceną.
3. **Sprawdź, czy bohater mógł tam być.** Jeśli jest w dwóch miejscach w krótkim odstępie, sprawdź trasę, czas podróży i wcześniejsze zobowiązania. Przy wspólnej scenie uzgodnij datę, miejsce, uczestników i przebieg wydarzeń we wszystkich opowiadaniach, w których się pojawia.
4. **Trzymaj się perspektywy bohatera.** Bohater opowiada własnymi słowami, co zobaczył, zrobił lub czego się dowiedział. Jeśli przypisuje komuś zamiar, czytelnik musi rozumieć, skąd bohater to wie albo na czym opiera przypuszczenie.
5. **Pokaż przyczynę i skutek.** Czytelnik ma rozumieć, co bohater próbuje zrobić, co się zmieniło i dlaczego podjął kolejną decyzję. Przy magii i urządzeniach pokaż działanie na przykładzie i ujawnij ograniczenie wtedy, gdy ma skutek dla sceny. Objaśniaj realia świata w chwili, gdy są potrzebne do zrozumienia akcji.
6. **Nazwiska są zakazane bez wyjątków.** Zasada obejmuje szkice, tłumaczenia, tytuły, narrację, dialogi i dokumenty przytaczane w opowieści. Używaj imion. Gdy potrzebujesz kogoś rozróżnić, dodaj funkcję, miejsce pochodzenia lub ustalony przydomek, np. „Ana z Korzennej Przystani”. W formularzach i spisach używaj pola „imię”.

### Przykładowe otwarcie rozdziału

```md
## Imię na formularzu

28 maja, 12 ET · Val Meris

Przed poranną odprawą poszedłem do składu.
```

W angielskiej wersji stosuj oznaczenia `AT` i `BT` oraz zachowaj te same daty i miejsca.

Przed oddaniem tekstu przeczytaj go jak ktoś, kto widzi Riftoryn po raz pierwszy. Sprawdź kolejno sens zdań, ciągłość scen i naturalność języka według [audytu sensu prozy](../.cursor/skills/riftoryn-prose-logic-audit/SKILL.md).

Poprzednie rękopisy i towarzyszące im notatki są zachowane w `_archiwum`; nie należy ich używać do bieżącej edycji. Polski pozostaje dostępny, gdy nie ma pliku tłumaczenia. Miejsca z `pending: true` nie są wyświetlane.
