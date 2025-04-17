using System;
using System.Collections.Generic;
using PudelkoLibrary;

class Program
{
    static void Main()
    {
        // Tworzenie listy pudełek przy użyciu różnych konstruktorów
        List<Pudelko> pudelka = new List<Pudelko>
        {
            new Pudelko(2.5, 3.0, 4.0), // Wymiary w metrach
            new Pudelko(250, 300, 400, UnitOfMeasure.centimeter), // Wymiary w centymetrach
            new Pudelko(2500, 3000, 4000, UnitOfMeasure.milimeter), // Wymiary w milimetrach
            new Pudelko(1.0, 1.0, 1.0), // Sześcian
            new Pudelko() // Domyślne wymiary
        };

        // Wyświetlenie listy pudełek przed sortowaniem
        Console.WriteLine("Nieposortowane pudełka:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine(pudelko);
        }

        // Definiowanie kryteriów sortowania jako delegat Comparison<Pudelko>
        Comparison<Pudelko> kryteriaSortowania = (p1, p2) =>
        {
            // Porównanie według objętości
            int porownanieObjetosci = p1.Objetosc.CompareTo(p2.Objetosc);
            if (porownanieObjetosci != 0) return porownanieObjetosci;

            // Jeśli objętości są równe, porównanie według pola powierzchni
            int porownaniePola = p1.Pole.CompareTo(p2.Pole);
            if (porownaniePola != 0) return porownaniePola;

            // Jeśli pola powierzchni są równe, porównanie według sumy długości krawędzi
            double sumaKrawedzi1 = p1.A + p1.B + p1.C;
            double sumaKrawedzi2 = p2.A + p2.B + p2.C;
            return sumaKrawedzi1.CompareTo(sumaKrawedzi2);
        };

        // Sortowanie listy pudełek według zdefiniowanych kryteriów
        pudelka.Sort(kryteriaSortowania);

        // Wyświetlenie listy pudełek po sortowaniu
        Console.WriteLine("\nPosortowane pudełka:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine(pudelko);
        }
        // Wyświetlenie informacji o testowaniu poprawności implementacji
        Console.WriteLine("\nTestowanie poprawności implementacji:");

        // Test 1: Wyświetlenie objętości każdego pudełka
        Console.WriteLine("Objętości pudełek:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine($"Pudełko: {pudelko}, Objętość: {pudelko.Objetosc} m³");
        }

        // Test 2: Wyświetlenie pola powierzchni każdego pudełka
        Console.WriteLine("\nPola powierzchni pudełek:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine($"Pudełko: {pudelko}, Pole: {pudelko.Pole} m²");
        }

        // Test 3: Wyświetlenie sumy długości krawędzi każdego pudełka
        Console.WriteLine("\nSumy długości krawędzi pudełek:");
        foreach (var pudelko in pudelka)
        {
            double sumaKrawedzi = pudelko.A + pudelko.B + pudelko.C;
            Console.WriteLine($"Pudełko: {pudelko}, Suma krawędzi: {sumaKrawedzi} m");
        }

        // Test 4: Sprawdzenie, czy lista pudełek jest poprawnie posortowana
        Console.WriteLine("\nSprawdzanie poprawności sortowania:");
        bool poprawneSortowanie = true;
        for (int i = 0; i < pudelka.Count - 1; i++)
        {
            // Jeśli kryteria sortowania są naruszone, oznacz błąd
            if (kryteriaSortowania(pudelka[i], pudelka[i + 1]) > 0)
            {
                poprawneSortowanie = false;
                Console.WriteLine($"Błąd sortowania między: {pudelka[i]} a {pudelka[i + 1]}");
            }
        }

        // Wyświetlenie wyniku testu sortowania
        Console.WriteLine(poprawneSortowanie ? "Sortowanie poprawne." : "Sortowanie niepoprawne.");
    }
}