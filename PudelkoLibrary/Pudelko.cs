using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PudelkoLibrary
{
    // Zadanie 2 ENUM
    public enum UnitOfMeasure
    {
        milimeter,
        centimeter,
        meter
    }


    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        // Zadanie 1: Ograniczenia wymiarów
        private const double DefaultDimension = 0.1; 
        private const double MaxDimension = 10.0; 

        private readonly double a;
        private readonly double b;
        private readonly double c;


        // Zadanie 3: Właściwości A, B, C zwracające wymiary w metrach
        public double A => Math.Round(a, 3);
        public double B => Math.Round(b, 3);
        public double C => Math.Round(c, 3);
        public UnitOfMeasure Unit { get; }

        // Zadanie 2: Konstruktor z obsługą jednostek miary i domyślnych wartości
        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            this.a = Math.Round(ConvertToMeters(a ?? DefaultDimension, unit), 3);
            this.b = Math.Round(ConvertToMeters(b ?? DefaultDimension, unit), 3);
            this.c = Math.Round(ConvertToMeters(c ?? DefaultDimension, unit), 3);

            ValidateDimensions(this.a, this.b, this.c);
            Unit = unit;
        }

        // Zadanie 2: Konwersja jednostek miary na metry
        private double ConvertToMeters(double value, UnitOfMeasure unit)
        {
            return unit switch
            {
                UnitOfMeasure.milimeter => value / 1000,
                UnitOfMeasure.centimeter => value / 100,
                UnitOfMeasure.meter => value,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Nieprawidłowa jednostka miary")
            };
        }

        // Zadanie 1: Walidacja wymiarów
        private void ValidateDimensions(params double[] dimensions)
        {
            foreach (var dimension in dimensions)
            {
                if (dimension <= 0 || dimension > MaxDimension)
                    throw new ArgumentOutOfRangeException(nameof(dimensions), "Wymiary muszą być dodatnie i nie większe niż 10 metrów");
            }
        }

        // Zadanie 4: ToString
        public override string ToString()
        {
            return ToString("m", null);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "m";

            return format switch
            {
                "m" => $"{A:F3} m × {B:F3} m × {C:F3} m",
                "cm" => $"{(A * 100):F1} cm × {(B * 100):F1} cm × {(C * 100):F1} cm",
                "mm" => $"{(A * 1000):F0} mm × {(B * 1000):F0} mm × {(C * 1000):F0} mm",
                _ => throw new FormatException($"Nieobsługiwany format: '{format}'.")
            };
        }
        public string? ToString(string format)
        {
            throw new NotImplementedException();
        }

        // Zadanie 5: Właściwość Objetosc
        public double Objetosc => Math.Round(a * b * c, 9);

        // Zadanie 6: Właściwość Pole
        public double Pole => Math.Round(2 * (a * b + b * c + a * c), 6);

        // Zadanie 7: Implementacja Equals
        public bool Equals(Pudelko other)
        {
            if (other == null)
            return false;

            var dimensions = new[] { A, B, C };
            var otherDimensions = new[] { other.A, other.B, other.C };

            Array.Sort(dimensions);
            Array.Sort(otherDimensions);

            return dimensions.SequenceEqual(otherDimensions);
        }

        public override bool Equals(object obj)
        {
            if (obj is Pudelko other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var dimensions = new[] { A, B, C };
            Array.Sort(dimensions);
            return HashCode.Combine(dimensions[0], dimensions[1], dimensions[2]);
        }
        public static bool operator ==(Pudelko left, Pudelko right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Pudelko left, Pudelko right)
        {
            return !(left == right);
        }

        // Zadanie 8: Operator łączenia pudełek (+)
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double newA = Math.Max(p1.A, p2.A);
            double newB = Math.Max(p1.B, p2.B);
            double newC = Math.Max(p1.C, p2.C);

            return new Pudelko(newA, newB, newC, UnitOfMeasure.meter);
        }

        // Zadanie 9: Konwersje jawne(explicit) i niejawne(implicit)
        public static explicit operator double[](Pudelko p)
        {
            return new[] { p.A, p.B, p.C };
        }

        public static implicit operator Pudelko((int a, int b, int c) dimensions)
        {
            return new Pudelko(dimensions.a / 1000.0, dimensions.b / 1000.0, dimensions.c / 1000.0, UnitOfMeasure.meter);
        }

        // Zadanie 10: Indexer
        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => A,
                    1 => B,
                    2 => C,
                    _ => throw new IndexOutOfRangeException("Nieprawidłowy indeks")
                };
            }
        }

        // Zadanie 11: Iteracja foreach
        public IEnumerator<double> GetEnumerator()
        {

            double[] wymiary = { A, B, C };
            return ((IEnumerable<double>)wymiary).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        { 
            return GetEnumerator();
        }

        // Zadanie 12: Metoda Parse
        public static Pudelko Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input), "Ciąg wejściowy nie może być pusty");

            var parts = input.Split('×', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");

            double[] dimensions = new double[3];
            UnitOfMeasure unit = UnitOfMeasure.meter;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i].Trim();
                var valueUnit = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (valueUnit.Length != 2)
                    throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");

                if (!double.TryParse(valueUnit[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");

                unit = valueUnit[1] switch
                {
                    "m" => UnitOfMeasure.meter,
                    "cm" => UnitOfMeasure.centimeter,
                    "mm" => UnitOfMeasure.milimeter,
                    _ => throw new FormatException("Ciąg wejściowy ma nieprawidłowy format")
                };

                dimensions[i] = unit switch
                {
                    UnitOfMeasure.meter => value,
                    UnitOfMeasure.centimeter => value / 100,
                    UnitOfMeasure.milimeter => value / 1000,
                    _ => throw new FormatException("Ciąg wejściowy ma nieprawidłowy format")
                };
            }

            return new Pudelko(dimensions[0], dimensions[1], dimensions[2], UnitOfMeasure.meter);
        }
    }
}