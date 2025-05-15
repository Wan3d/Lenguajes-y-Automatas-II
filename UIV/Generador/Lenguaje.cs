/*  
    Requerimientos
    1.  La primera producción es pública, el resto son privadas. [DONE]
    2.  Agregar los métodos a una lista para validar que todos los métodos invocados (SNT),
        del lado derecho de la producción existan. [DONE]
    3.  Agregar a la estructura el símbolo con el que inicia la producción. [DONE]
    4.  Implementar la cerradura Epsilon en agrupaciones.
    5.  Implementar el OR. [%]
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Generador
{
    public class Lenguaje : Sintaxis
    {
        Dictionary<string, string> functions = new Dictionary<string, string>();
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

            Write("}", 1);
            Write("}");
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
            switch (Contenido)
            {
                case "Programa": Write($"public void {Contenido}()", 2); break;
                default: Write($"private void {Contenido}()", 2); break;
            }

            if (existeFuncion(Contenido))
            {
                throw new Error($"La función {Contenido} ya existe o ya fue declarada", log, linea, columna);
            }

            string function = Contenido;

            match(Tipos.SNT);
            Write("{", 2);

            match(Tipos.Produce);
            ListaSimbolos(function);
            match(Tipos.FinProduccion);

            Write("}", 2);
        }
        private void ListaSimbolos(string function = "", int tabs = 3)
        {
            try
            {
                string primerSimbolo = Contenido;
                functions.Add(function, primerSimbolo);
            }
            catch (Exception)
            {
                Console.WriteLine("No se pudo agregar la función");
            }

            if (Clasificacion == Tipos.SNT)
            {
                if (esClasificacion(Contenido))
                {
                    string nombre = Contenido;
                    match(Tipos.SNT);

                    if (Clasificacion == Tipos.Optativo)
                    {
                        Write($"if(Clasificacion == Tipos.{nombre})", tabs);
                        Write("{", tabs);
                        Write($"match(Tipos.{nombre});", tabs + 1);
                        Write("}", tabs);
                        match(Tipos.Optativo);
                    }
                    else
                    {
                        Write($"match(Tipos.{nombre});", tabs);
                    }
                }
                else
                {
                    string nombre = Contenido;
                    string? primerSimbolo = obtenerSimbolo(nombre);
                    //char index = primerSimbolo![0];

                    match(Tipos.SNT);

                    if (Clasificacion == Tipos.Optativo && esClasificacion(primerSimbolo!))
                    {
                        Write($"if(Clasificacion == Tipos.{primerSimbolo})", tabs);
                        Write("{", tabs);
                        Write(nombre + "();", tabs + 1);
                        Write("}", tabs);
                        match(Tipos.Optativo);
                    }
                    else if (Clasificacion == Tipos.Optativo && !esClasificacion(primerSimbolo!))
                    {
                        Write($"if(Contenido == \"{primerSimbolo}\")", tabs);
                        Write("{", tabs);
                        Write(nombre + "();", tabs + 1);
                        Write("}", tabs);
                        match(Tipos.Optativo);
                    }
                    else
                    {
                        if (existeFuncion(nombre))
                        {
                            Write(nombre + "();", tabs);
                            Console.WriteLine($"Nombre {nombre}");
                        }
                        else
                        {
                            throw new Error($"La función {nombre} no existe o ya fue agregada", log, linea, columna);
                        }
                    }
                }
            }
            else if (Clasificacion == Tipos.ST)
            {
                string nombre = Contenido;
                match(Tipos.ST);

                if (Clasificacion == Tipos.Optativo)
                {
                    Write($"if(Contenido == \"{nombre}\")", tabs);
                    Write("{", tabs);
                    Write($"match(\"{nombre}\");", tabs + 1);
                    Write("}", tabs);
                    match(Tipos.Optativo);
                }
                else
                {
                    Write($"match(\"{nombre}\");", tabs);
                }
            }
            else
            {
                match(Tipos.InicioAgrupacion);
                Write("{", tabs);
                ListaSimbolos("", tabs + 1);
                match(Tipos.CierreAgrupacion);
                Write("}", tabs);
            }

            if (Clasificacion != Tipos.FinProduccion && Clasificacion != Tipos.CierreAgrupacion)
            {
                ListaSimbolos("", tabs);
            }
        }
        private void Write(string texto, int tabs = 0)
        {
            for (int i = 0; i < tabs; i++)
            {
                lenguaje.Write("\t");
            }
            lenguaje.WriteLine(texto);
        }
        private bool existeFuncion(string nombre)
        {
            return functions.ContainsKey(nombre);
        }
        private string? obtenerSimbolo(string funcion)
        {
            if (functions.TryGetValue(funcion, out string? simbolo))
            {
                return simbolo;
            }
            return null;
        }
    }
}