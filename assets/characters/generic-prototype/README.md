# Prototyp generycznej postaci

Punkt wyjścia to widoczna w dostarczonym pliku `BodyChan - BodyKun Posable Drawing Figures.blend` figura BodyKun. Oryginalny plik pozostał bez zmian. To lokalna kopia robocza do oceny, a nie ukończona postać produkcyjna.

- `Riftoryn_Generic_Prototype.blend` — edytowalna kopia z oryginalnym szkieletem i segmentami ciała; ukryta jest domyślna kolekcja startowa Blendera.
- `Riftoryn_Generic_Prototype.fbx` — próbny eksport szkieletu i 12 części siatki bez animacji; wysokość w przestrzeni sceny ok. 1,80 m, stopy przy poziomie podłoża.
- W kopii usunięto puste modyfikatory armatury, zachowując działające przypisania do szkieletu. Siatki mają łącznie 17 230 wierzchołków i 18 286 ścian przed triangulacją. Wszystkie wierzchołki należą do co najmniej jednej grupy.
- Test ponownego importu FBX do Blendera potwierdził 12 siatek, armaturę, przypisania grup, modyfikatory armatury oraz wysokość 1,80 m. Dodatkowo próbny obrót kości ramienia poruszył 648 wierzchołków siatki ramion. Nie jest to pełny test animacji ani poprawnego importu do Unity.

Model zachowuje przeguby i segmentację manekina oraz istniejący rig; wymaga oceny i dalszego opracowania geometrii stawów, deformacji, animacji, materiałów, optymalizacji i importu do Unity. Warunki licencyjne oryginalnego pobranego modelu nie zostały zweryfikowane; przed użyciem w dystrybuowanej grze należy je ustalić.
