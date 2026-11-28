# Ana i Bob — modele PoC

Modele są uproszczoną interpretacją [arkusza Any i Boba](../../heroes/modeling-concepts/ana-bob.png), przeznaczoną do testu sylwetek i rozgrywki. Nie są modelami produkcyjnymi.

- `Ana_PoC.blend` i `Ana_PoC.fbx`: Ana oparta na tej samej bazie i szkielecie co Saila, Pyrra i Koneva. W grze używa istniejącej animacji proceduralnej.
- `Bob_PoC.blend` i `Bob_PoC.fbx`: model Boba w pozie na czterech łapach, z osobnymi łapami. W grze biegnie przed Aną, a prosta animacja proceduralna porusza łapami podczas ruchu. Model wymaga pełnego szkieletu, animacji i dopracowania sylwetki.
- `podglad-unity.png`: kontrolny widok modeli zaimportowanych do Unity; nie jest zrzutem z właściwej kamery rozgrywki.

Skrypt `build_ana.py` uruchamia się w Blenderze po otwarciu `assets/characters/saila/Riftoryn_Female_Base.blend` i korzysta ze wspólnych pomocników w `assets/characters/pyrra/build_pyrra.py`. `build_bob.py` uruchamia się w osobnej, nowej scenie Blendera; usuwa obiekty ze sceny, w której jest uruchomiony. Eksporty są kopiowane odpowiednio do `Assets/Resources/Characters/ana-poc.fbx` oraz `Assets/Resources/Pets/bob-poc.fbx` w projekcie Unity.

Ruch Boba to nadal uproszczona pozycja względem Any, nie samodzielne wyszukiwanie trasy ani symulacja prędkości z pliku statystyk. Przy ścianie wraca do pozycji Any. Proporcje, oświetlenie, tekstury, twarz, deformacje i czytelność w ruchu wymagają dalszego sprawdzenia i dopracowania.
