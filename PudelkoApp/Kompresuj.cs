using System;
using System.Collections.Generic;
using System.Linq;
using PudelkoLibrary;
using System.Text;
using System.Threading.Tasks;

namespace PudelkoLibrary
{
    public static class PudelkoKompresuj
    {
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            double bok = Math.Pow(pudelko.Objetosc, 1.0 / 3.0);
            return new Pudelko(bok, bok, bok, pudelko.Unit);
        }
    }
}