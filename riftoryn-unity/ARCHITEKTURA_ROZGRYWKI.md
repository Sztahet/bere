# Rozgrywka — aktualna architektura

Główne menu ma jedno wejście do gry. Lokalna sesja używa `RiftorynGameSettings`: liczba bohaterów i wież, druga drużyna i sposób ustawiania obrony są parametrami tej samej rozgrywki. Ustawienia określają długość i kolejność draftu, ale nie tworzą osobnych zasad ruchu ani walki.

`RiftorynTrial` zarządza stanem próby. Lista `runners` jest wspólnym źródłem uczestników dla ruchu, obrażeń, pocisków, widoczności, HUD-u i warunków zakończenia. `player` i `ally` pozostają referencjami do pierwszych dwóch uczestników, wykorzystywanymi m.in. przez starszy protokół LAN; nie są osobnymi listami drużyn. W lokalnej grze boty i gracz korzystają ze wspólnego ruchu, obrażeń, wykrywania wież i zdolności. Przy umiejętnościach działających na jedną osobę wybierany jest najbliższy żywy sojusznik. Patka zachowuje wybrany cel ochrony nawet wtedy, gdy inny sojusznik podejdzie bliżej.

Faza ręcznego ustawiania korzysta z tego samego edytora pól co wcześniejszy test obrony; w lokalnej grze limit wynosi od jednej do siedmiu wież (domyślnie pięć), a typy można powtarzać. Ta sama lista zapisuje układ do odpowiedniego przebiegu. Obie strony meczu używają tej samej wybranej mapy.

## Świadomie pozostawione różnice

- **LAN:** z ekranu ustawień nadal dostępny jest poprzedni test dwóch osób. Ma własny protokół, ekran połączenia i ograniczony zestaw synchronizowanych zdolności; nie jest jeszcze sieciową wersją nowego meczu ani nie obsługuje jego parametrów. Nie należy przedstawiać go jako pełnego wspólnego multiplayera. Ruch gościa korzysta już z tej samej funkcji ruchu co pozostałe postacie.
- **Stare tryby i ranking:** starsze punkty wejścia i implementacja wyników pozostają w kodzie na czas migracji, lecz nie są osobnymi grami w menu. Dotychczasowy ranking można obejrzeć jako poprzednie wyniki; nowa gra nie dopisuje do niego wyników, bo jej konfiguracje i mapy nie są porównywalne ze starym duetem.
- Szesnaście wybranych postaci ma Q/E/R w lokalnym prototypie. Niva, Liora, Rian, tankowie i nowi przywoływacze korzystają częściowo z parametrów w `Resources/HeroDesign`; wcześniejsze zestawy nadal wymagają migracji do wspólnych danych. Pasywki Any, Lumen i Morraka wymagają dalszej pracy. Ana, Bob, Sarna, Lumen i Blask mają uproszczone modele PoC. Wspólny kontrakt towarzysza obsługuje celowanie i trafienia Patki, Iskry, Boba i Blasku.
- Pnącza Natury zatruwają także żywych towarzyszy: 4 obrażenia na sekundę przez 3 sekundy od wejścia, z pochłanianiem przez dostępną osłonę. Pozostałe efekty statusowe i spowolnienie ruchu towarzyszy wymagają odrębnej implementacji i testów.

## Kontrola zmian

Powtarzalne testy reguł konfiguracji są w `../tests/unity-game-settings/`. Uruchom `dotnet run --project ../tests/unity-game-settings/GameSettings.Tests.csproj` z katalogu projektu Unity (.NET SDK 10). Test obejmuje wielkości drużyny 1–5, limity wież 1–7, grę z przeciwnikiem i bez, poprawność naprzemiennego draftu oraz odrzucanie nieprawidłowych wartości. Ruch, UI, scenę i sieć trzeba dodatkowo sprawdzać w Unity; ten test nie zastępuje rozgrywki ani testu dwóch komputerów.
