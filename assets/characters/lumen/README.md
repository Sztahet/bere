# Lumen i Blask — modele PoC

Uproszczone modele na podstawie [arkusza Lumen i jej towarzysza](../../heroes/modeling-concepts/lumen-glimmer.png). Polski projekt używa imienia Blask; arkusz podpisuje go Glimmer. Są to modele do prób gry, nie gotowe modele produkcyjne.

Lumen używa wspólnej bazy szkieletu i proceduralnego ruchu. Blask ma osobne nogi poruszane prostą animacją proceduralną podczas biegu, ale nie ma pełnego szkieletu i wymaga dopracowania animacji, proporcji oraz materiałów. W projekcie Unity używane są kopie eksportów w `Assets/Resources/Characters/lumen-poc.fbx` i `Assets/Resources/Pets/blask-poc.fbx`.

Pozycja Blasku pozostaje uproszczona: podąża za Lumen, utrzymując pozycję względem niej; nie wyznacza samodzielnie trasy ani nie korzysta jeszcze z własnej prędkości z pliku statystyk.

Model Lumen tworzy `assets/characters/build_sarna_lumen.py` po otwarciu `assets/characters/saila/Riftoryn_Female_Base.blend` i podaniu `-- Lumen`. Skrypt `build_blask.py` należy uruchamiać wyłącznie w nowej scenie: usuwa jej obiekty.
