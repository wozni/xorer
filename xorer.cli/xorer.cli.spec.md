## Specyfikacja linii komend xorer.cli

Wariant1: Wypisujemy rezultat na konsolę
- -inputKeyA <kluczA>
- -inputKeyB <kluczB>

Przykład:
    xorer.cli --inputKeyA ABC --inputKeyB DEF 


Wariant2: Wypisujemy do pliku
- -inputKeyA <kluczA>
- -inputKeyB <kluczB>
- -outputFile <ścieżka do pliku gdzie zapisany zostanie wynik>



Przykład:
    xorer.cli --inputKeyA ABC --inputKeyB DEF --outputFile wynik.txt


Wariant3: klucze i wynik w plikach
- -inputFileA <ścieżka do pliku z kluczem A>
- -inputFileB <ścieżka do pliku z kluczem B>
- -outputFile <ścieżka do pliku gdzie zapisany zostanie wynik>

Przykład:
    xorer.cli --inputFileA ./kluczA.txt --inputFileB ./kluczB.txt --outputFile wynik.txt



### Założenia
Tekst jest w formacie ASCII czyli jeden znak na bajt