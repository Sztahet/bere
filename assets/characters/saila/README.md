# Saila — model i baza kobieca do PoC

- `Riftoryn_Female_Base.blend` i `Riftoryn_Female_Base.fbx`: robocza baza kobiecej sylwetki ze szkieletem, o wzroście ok. 1,72 m. Bazuje na dostarczonej figurze BodyChan z pliku `BodyChan - BodyKun Posable Drawing Figures.blend`; oryginał nie był zmieniany.
- `Saila_PoC.blend` i `Saila_PoC.fbx`: uproszczony model Saily wzorowany na arkuszu postaci. Jedna skóra, osiem materiałów, ok. 17 tys. wierzchołków; stroje i włosy są uproszczone, a siatka zachowuje segmentowe stawy źródłowego manekina.
- `build_saila.py` to roboczy skrypt modelowania uruchamiany w Blenderze po wczytaniu bazy jako kolekcji `BodyChan` i armatury `Saila_Female_Rig`; łączenie siatek oraz końcowy eksport wykonano po jego uruchomieniu. Nie jest samodzielnym generatorem kompletnego modelu.

Unity ładuje `Assets/Resources/Characters/saila-poc.fbx`, kopię eksportu FBX. Po nowym eksporcie należy ją podmienić, zachowując istniejący plik `.meta`. Wybór Saily jest dostępny w drafcie próby. Q wyłącza najbliższą wieżę w zasięgu na 3 s, odnowienie 24 s; wartości i efekt są testowe. Ruch szkieletu realizuje prowizoryczna animacja proceduralna, nie autorskie klipy. Ostateczne animacje, siatka stawów, materiały i podobieństwo do arkusza wymagają dalszego dopracowania i oceny w grze.

Ponowny import do Unity, działanie Q oraz podstawowy ruch kości sprawdzono w Play Mode. Konieczne pozostają dalsze próby deformacji, kolizji i grywalności; nie jest to gotowy asset produkcyjny. Warunki licencyjne dostarczonej sylwetki BodyChan nie zostały zweryfikowane i należy je ustalić przed dystrybucją.
