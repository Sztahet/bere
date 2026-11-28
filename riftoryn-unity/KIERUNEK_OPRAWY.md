# Riftoryn — kierunek oprawy 3D

**Kierunek przyjęty:** stylizowane fantasy 3D. Najpierw dopracowujemy jeden grywalny fragment, a rozwiązania dotyczące czytelności, skali i wykonania przenosimy na następne regiony. Siedem żywiołów powinno być rozpoznawalnych przez materiały, sylwetki i akcenty, a nie jedynie przez zmianę koloru.

## Pierwsza próba: Vareth

- **Styl:** kamienna arena kuźni, czytelne bryły, kute metalowe detale i rytmiczne znaki, oglądane z kamery za bohaterem.
- **Paleta:** bazalt ścian `#402B2B`, ciepła posadzka `#5C453B`, ciemne żelazo `#1F1B1F`, brąz `#9E6130`, złoto `#DB9C4A`, żar `#FF4D12`.
- **Światło:** ciepłe światło dzienne i lokalne paleniska; zagrożenia, bohaterowie, wskaźniki i pociski muszą pozostać czytelne na ich tle.

Obecna implementacja to **pierwsze studium oprawy**, nie finalna mapa: mury i większość dekoracji nadal są prostą geometrią. Detale są bez kolizji, więc nie zmieniają przechodniości ani linii ostrzału. W dalszej pracy potrzebne są spójne modele otoczenia, materiały, animowane źródła światła, efekty, dźwięk i pomiary wydajności. Ikony umiejętności Saily korzystają tymczasowo z istniejącego atlasu i wymagają własnej oprawy.

## Kontrola kolejnych zmian

- Oceniamy widok podczas ruchu z kamery TPP oraz z widoku budowy; mapa musi pozostać czytelna w obu.
- Weryfikujemy widoczność wież, pocisków, stanu HP i znaczników, a nie tylko nieruchomy kadr.
- Dekoracje nie mogą zawężać przejścia ani zasłaniać pocisków przez fizyczne kolizje; ewentualne przyszłe przeszkody wymagają osobnej decyzji projektowej.
- Kolorystyka, architektura i oświetlenie pozostałych sześciu krain wymagają osobnego dopracowania i sprawdzenia w tej samej grze.
