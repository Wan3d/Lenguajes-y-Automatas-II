/*  
    Requerimientos
    1.  La primera producción es pública, el resto son privadas. [DONE]
    2.  Agregar los métodos a una lista para validar que todos los métodos invocados (SNT),
        del lado derecho de la producción existan. [DONE]
    3.  Agregar a la estructura el símbolo con el que inicia la producción. [DONE]
    4.  Implementar la cerradura Epsilon en agrupaciones. [%]
    5.  Implementar el OR. [DONE]
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
            ListaSimbolos(function, true);
            match(Tipos.FinProduccion);

            Write("}", 2);
        }
        private void ListaSimbolos(string function = "", bool primeraVez = false, int tabs = 3)
        {
            if (primeraVez)
            {
                string simbolo = Contenido;
                functions.Add(function, simbolo);

                primeraVez = false;
            }

            if (Clasificacion == Tipos.SNT)
            {
                // En caso de venir un SNT pero una Clasificación
                if (esClasificacion(Contenido))
                {
                    string nombre = Contenido;
                    match(Tipos.SNT);

                    if (Clasificacion == Tipos.Optativo || Clasificacion == Tipos.OR)
                    {
                        Write($"if(Clasificacion == Tipos.{nombre})", tabs);
                        Write("{", tabs);
                        Write($"match(Tipos.{nombre});", tabs + 1);
                        Write("}", tabs);

                        if (Clasificacion == Tipos.OR)
                        {
                            match(Tipos.OR);
                            writeOr(Clasificacion, Contenido);
                        }
                        else
                        {
                            match(Tipos.Optativo);
                        }
                    }
                    else
                    {
                        Write($"match(Tipos.{nombre});", tabs);
                    }
                }
                // En caso de venir un SNT pero una Función
                else
                {
                    string nombre = Contenido;
                    string? primerSimbolo = obtenerSimbolo(nombre);

                    while (existeFuncion(primerSimbolo!))
                    {
                        primerSimbolo = obtenerSimbolo(primerSimbolo!);
                    }

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
                        }
                        else
                        {
                            throw new Error($"La función {nombre} no existe o ya fue agregada", log, linea, columna);
                        }
                    }
                }
            }
            // En caso de venir un ST, o sea, un match de un Contenido
            else if (Clasificacion == Tipos.ST)
            {
                string nombre = Contenido;

                match(Tipos.ST);

                if (Clasificacion == Tipos.Optativo || Clasificacion == Tipos.OR)
                {
                    Write($"if(Contenido == \"{nombre}\")", tabs);
                    Write("{", tabs);
                    Write($"match(\"{nombre}\");", tabs + 1);
                    Write("}", tabs);

                    if (Clasificacion == Tipos.OR)
                    {
                        match(Tipos.OR);
                        writeOr(Clasificacion, Contenido);
                    }
                    else
                    {
                        match(Tipos.Optativo);
                    }
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
                ListaSimbolos("", primeraVez, tabs + 1);
                match(Tipos.CierreAgrupacion);
                Write("}", tabs);
            }

            if (Clasificacion != Tipos.FinProduccion && Clasificacion != Tipos.CierreAgrupacion)
            {
                ListaSimbolos("", primeraVez, tabs);
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
        private void writeOr(Tipos clasificacion, string Contenido, int tabs = 3)
        {
            if (clasificacion == Tipos.SNT && existeFuncion(Contenido))
            {
                // Falta por agregar algunas cosas
                // Comprobar con ciclo el simbolo de una función (?)
                Write($"else", tabs);
                Write("{", tabs);
                Write($"{Contenido}();", tabs + 1);
                Write("}", tabs);
                nextToken();
            }
            else if (clasificacion == Tipos.SNT && esClasificacion(Contenido))
            {
                Write($"else", tabs);
                Write("{", tabs);
                Write($"match(Tipos.{Contenido})", tabs + 1);
                Write("}", tabs);
                nextToken();
            }
            else if (clasificacion == Tipos.ST)
            {
                Write($"else", tabs);
                Write("{", tabs);
                Write($"match(\"{Contenido}\");", tabs + 1);
                Write("}", tabs);
                nextToken();
            }
            else
            {
                throw new Error("No se ingresó una gramática válida", log, linea, columna);
            }
        }
    }
}