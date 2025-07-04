using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Sintaxis : Lexico
    {
        public Sintaxis(string nombre = "gram.txt") : base(nombre)
        {
            nextToken();
        }
        public void match(string contenido)
        {
            if (contenido == Contenido)
            {
                nextToken();
            }
            else
            {
                throw new Error("Sintaxis. Se espera un " + contenido, log, linea, columna);
            }
        }
        public void match(Tipos clasificacion)
        {
            if (clasificacion == Clasificacion)
            {
                nextToken();
            }
            else
            {
                throw new Error("Sintaxis. Se espera un " + clasificacion, log, linea, columna);
            }
        }
    }
}