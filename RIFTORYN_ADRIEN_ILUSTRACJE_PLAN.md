# Adrien — miejsca na ilustracje

Każda część ma osiem ponumerowanych miejsc w `story.json`. Są oznaczone jako `pending`, więc czytnik gry pomija je do czasu dodania gotowych grafik. Nazwy plików w odpowiednim katalogu zasobów to `01.jpg`–`08.jpg`. Przy dodawaniu obrazu usuń `pending: true` z właściwego bloku, sprawdź opis alternatywny i proporcje obrazu. Plan jest wskazówką do przyszłych ilustracji, nie gotowym promptem ani ustaleniem wyglądu scen.

## Światło, które wskazało drogę — `AdrienStory1`

1. Adrien sprawdza sygnalizator na placu.
2. Praca nad piecem zasilanym kryształem.
3. Młoda Lena ostrzega przed wadą mocowania.
4. Adrien otrzymuje wiadomość o awarii.
5. Badanie sygnalizatora przy drodze.
6. Raport po wypadku.
7. Lena i Adrien sprawdzają mocowania w warsztacie.
8. Imię przewodnika w notatniku.

## Kobieta, która wiedziała — `AdrienStory2`

1. Rian zeznaje przed komisją.
2. Adrien szkicuje mechanizm podczas zeznań.
3. Pierwsza rozmowa nad projektem zabezpieczenia.
4. Adrien przygotowuje wniosek.
5. Rian sprawdza model pod nadzorem.
6. Młody Lucen ładuje kryształ w warsztacie.
7. Lucen i Adrien badają konstrukcję.
8. Rian uczy rzemieślników.

## Wynalazek, którego nie przewidziałem — `AdrienStory3`

1. Miko przynosi prototyp.
2. Adrien i Miko porównują notatki.
3. Próby regulatora w warsztacie.
4. Rian analizuje urządzenia obu wynalazców.
5. Adrien i Rian badają odchylenie wskazówki.
6. Adrien szkicuje urządzenia na arenę.
7. Adrien i Lena sprawdzają prototyp.
8. Adrien wraca do projektu i zapisuje plan prób.

Źródłem tekstu są odpowiednie pliki Markdown. Po ich redakcji można zaktualizować dane czytnika poleceniem `python scripts/build_adrien_stories.py`; zapisane bloki obrazów zostaną zachowane. Następnie uruchom `python -m unittest discover -s tests` i zbuduj nową wersję gry.
