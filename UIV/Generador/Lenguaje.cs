using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Lenguaje : Sintaxis
    {
        public Lenguaje(string nombre = "prueba.cpp") : base(nombre)
        {
            log.WriteLine("Constructor lenguaje");
        }
        public int Programa(){
            return 0;
        }
    }
}