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
        public Lenguaje(string nombre = "gram.txt") : base(nombre)
        {
        }
        // Genera -> Header { Producciones }
        public void Genera()
        {
            Header();
            match("{");
            Producciones();
            match("}");
        }
        // Header -> Lenguaje: ST | SNT;
        private void Header()
        {
            match("Lenguaje");
            match(":");

            string nombreProyecto = Contenido;

            if (Clasificacion == Tipos.ST)
            {
                match(Tipos.ST);
            }
            else
            {
                match(Tipos.SNT);
            }

            match(";");

            // public class Lenguaje
            Write($"namespace {nombreProyecto}");
            Write("{");
            Write("public class Lenguaje ", 1);
            Write("{", 1);

            Write("public Lenguaje()", 2);
            Write("{", 2);
            Write("}", 2);
            
            Write("}", 1);
            Write("}");
        }
        // Producciones -> Produccion FinProduccion Producciones?
        private void Producciones()
        {
            Produccion();

            if (Contenido != "}")
            {
                Producciones();
            }
        }
        // Produccion -> SNT Produce ListaSimbolos
        private void Produccion()
        {
            match(Tipos.SNT);
            match(Tipos.Produce);
            // ListaSimbolos();
            match(Tipos.FinProduccion);
        }
        private void Write(string text, int tabs = 0){
            for (int i = 0; i < tabs; i++)
            {
                lenguaje.Write("\t");
            }
            lenguaje.WriteLine(text);
        }
    }
}