# Instrukcje pracy z Unity CLI

Przeczytaj ten plik na początku pracy z tym projektem, także po restarcie komputera lub nowej rozmowie. Traktuj poniższe wyniki jako historię, a aktualny stan sprawdzaj na podstawie bieżącej odpowiedzi edytora.

## Znane środowisko i ograniczenie dostępu

8 listopada 2026 użytkownik potwierdził w zwykłym PowerShellu Unity CLI `1.0.0-beta.17`, edytor `6000.6.8f1` i działające połączenie Pipeline z tym projektem. Edytor odpowiedział `ready`; pakiet projektu to `com.unity.pipeline` `0.10.0-exp.1`. Te wersje i status mogą się zmienić.

W tym samym czasie agent w sandboxie nie mógł uruchomić aliasu `C:\Users\adria\AppData\Local\Microsoft\WindowsApps\unity.exe`: Windows zwrócił „System nie może uzyskać dostępu do pliku”. Ten błąd oznacza ograniczenie dostępu w środowisku agenta; sam w sobie nie dowodzi braku instalacji CLI, niedziałającego edytora ani braku pakietu Pipeline. Nie proponuj reinstalacji ani nie każ użytkownikowi ponownie diagnozować instalacji tylko na tej podstawie.

Potwierdzone obejście: właściwy program pakietu znajduje się w `C:\Program Files\WindowsApps\UnityTechnologies.UnityCLI_1.0.0.20017_x64__2vrhnee42bhxm\app\unity.exe`. Skopiowanie go do `D:\github\bere\riftoryn-unity\Temp\codex-unity-cli\unity.exe` pozwoliło uruchomić CLI. Dostęp do uruchomionego edytora wymagał uruchomienia polecenia poza sandboxem (`require_escalated`): w sandboxie `status` pokazywał `unreachable`, poza nim `ready`, a `command editor status` i `recompile` działały. Po restarcie `Temp` może zostać wyczyszczony, a wersja pakietu może się zmienić: znajdź aktualny katalog `UnityTechnologies.UnityCLI_*` w `C:\Program Files\WindowsApps`, skopiuj `app\unity.exe` ponownie i sprawdź `--version`. Następnie użyj pełnej ścieżki kopii oraz pełnej ścieżki projektu. Nie traktuj błędu sandboxa jako dowodu, że edytor nie działa. Jeśli eskalacja zostanie odrzucona lub połączenie nadal nie działa, podaj użytkownikowi dokładny błąd i potrzebną komendę do jego PowerShella.

## Połączenie z lokalnym edytorem

- Zawsze używaj pełnej ścieżki projektu `D:\github\bere\riftoryn-unity` we wszystkich poleceniach Unity CLI, które sterują edytorem.
- Na początku sprawdź `unity --version`, a następnie:
  `unity status --project-path "D:\github\bere\riftoryn-unity"`
- Jeśli edytor ma status `ready`, sprawdź odpowiedź edytora poleceniem:
  `unity command editor status --project-path "D:\github\bere\riftoryn-unity" --caller plugin --skill unity-cli`
- Jeśli polecenia CLI nie da się uruchomić z tego środowiska (np. błąd dostępu do `C:\Users\adria\AppData\Local\Microsoft\WindowsApps\unity.exe`), nie powtarzaj bez końca tej samej próby. Powiedz użytkownikowi wprost, że lokalny PowerShell potrafi uruchomić CLI, ale bieżące środowisko agenta nie ma do niego dostępu. Poproś o uruchomienie dokładnych, potrzebnych poleceń w PowerShellu i wklejenie wyników. Nie twierdź, że połączenie lub zmiany zostały sprawdzone, dopóki wynik tego nie potwierdzi.
- Jeśli alias jest niedostępny, zastosuj najpierw opisane wyżej sprawdzone obejście z kopią CLI i dostępem poza sandboxem. O pomoc użytkownika poproś dopiero, jeśli obejście jest niedostępne lub nie działa.
- Jeśli Unity zgłasza Safe Mode, sprawdź i zgłoś błędy kompilacji. Gdy nie można połączyć się z edytorem, jasno podaj dokładny błąd i nie zakładaj, że projekt jest w Safe Mode.

## Sprawdzona procedura builda Windows

Istniejący katalog bieżącego builda to `D:\github\bere\riftoryn-unity\Build\current_version`, a plik wykonywalny to `Riftoryn.exe`. 8 listopada 2026 poniższe wywołanie Pipeline zakończyło się wynikiem `Succeeded`, 0 błędów i 0 ostrzeżeń. Jeśli agent może użyć CLI, wykonaj je sam. Jeśli dostęp do aliasu jest ograniczony jak opisano wyżej, poproś użytkownika o uruchomienie tego jednego polecenia w jego PowerShellu i podanie odpowiedzi:

```powershell
unity command build run --project-path "D:\github\bere\riftoryn-unity" --caller plugin --skill unity-cli --target StandaloneWindows64 --output-path "D:\github\bere\riftoryn-unity\Build\current_version\Riftoryn.exe" --confirm
```

Nie dodawaj `--options`: Pipeline domyślnie używa `DetailedBuildReport`. W tym środowisku `unity build ... --target StandaloneWindows64` oraz ręczne `--options DetailedBuildReport` zostały odrzucone błędem `--options expects String[]`; ręczne przekazanie tablicy JSON przez PowerShell również nie zadziałało z powodu cudzysłowów. Powyższa komenda bez `--options` jest potwierdzonym obejściem.

Odpowiedź `status: queued` oznacza wyłącznie przyjęcie zadania. Zapamiętaj `buildId` i samodzielnie odczytuj `D:\github\bere\riftoryn-unity\Temp\pipeline_build_status.json`, jeśli plik jest dostępny. Czekaj na `status: completed` dla tego samego `buildId`; sprawdź `result`, `outputPath`, `totalErrors` i `totalWarnings`. `Succeeded` potwierdza build. `idle` nie oznacza powodzenia. Gdy plik jest niedostępny, poproś użytkownika o wynik:

```powershell
unity command build status --project-path "D:\github\bere\riftoryn-unity" --caller plugin --skill unity-cli
```

Nie uruchamiaj builda ponownie, gdy poprzedni jest w kolejce lub trwa. Nie uznawaj starego `Riftoryn.provenance.json` za wynik builda uruchomionego bezpośrednio przez Pipeline: potwierdź wynik bieżącym raportem i identyfikatorem zadania.

## Sprawdzanie gry w Play Mode

- Przed sprawdzeniem zmian w działającej grze najpierw skompiluj projekt, uruchom Play Mode, potwierdź, że gra faktycznie działa, i sprawdź konsolę.
- Po sprawdzeniu zatrzymaj Play Mode i opisz wynik. Sam status `playMode: playing` nie potwierdza, że gra działa ani że klatki postępują.
