/*
REQUERIMIENTOS:

1) Excepción en el Console.Read() [DONE]
2) Programar todas las funciones matemáticas que están en Léxico en la función matemática de Lenguaje [DONE]
3) Programar el método For. La segunda asignación del for (incremento) debe de ejecutarse después del bloque de instrucciones | instrucción
4) Programar el método While [DONE]
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Emulador
{
    public class Lenguaje : Sintaxis
    {
        Stack<float> s;
        List<Variable> l;
        Variable.TipoDato maximoTipo;

        public Lenguaje(string nombre = "prueba.cpp") : base(nombre)
        {
            s = new Stack<float>();
            l = new List<Variable>();
            log.WriteLine("Constructor lenguaje");
            maximoTipo = Variable.TipoDato.Char;
        }
        private void displayLista()
        {
            log.WriteLine("Lista de variables: ");
            foreach (Variable elemento in l)
            {
                log.WriteLine($"{elemento.Nombre} {elemento.Tipo} {elemento.Valor}");
            }
        }
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (Contenido == "using")
            {
                Librerias();
            }
            if (Clasificacion == Tipos.TipoDato)
            {
                Variables();
            }
            Main();
            displayLista();
        }
        //Librerias -> using ListaLibrerias; Librerias?
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");
            if (Contenido == "using")
            {
                Librerias();
            }
        }
        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato t = Variable.TipoDato.Char;
            switch (Contenido)
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(t);
            match(";");
            if (Clasificacion == Tipos.TipoDato)
            {
                Variables();
            }
        }
        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);
            if (Contenido == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }
        //ListaIdentificadores -> identificador (= Expresion)? (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(variable => variable.Nombre == Contenido) != null)
            {
                throw new Error($"La variable {Contenido} ya existe", log, linea, columna);
            }
            Variable v = new Variable(t, Contenido);
            l.Add(v);
            match(Tipos.Identificador);
            if (Contenido == "=")
            {
                match("=");
                if (Contenido == "Console")
                {
                    match("Console");
                    match(".");
                    if (Contenido == "Read")
                    {
                        match("Read");
                        int r = Console.Read();
                        if (r >= 48 && r <= 57)
                        {
                            r = convertirAscii(r);
                            s.Push(r);
                            v.setValor(r, maximoTipo);
                        }
                        else
                        {
                            throw new Error("Semántico. No se ingresó un carácter válido ", log, linea, columna);
                        }
                    }
                    else
                    {
                        match("ReadLine");
                        string? r = Console.ReadLine();
                        if (float.TryParse(r, out float valor))
                        {
                            v.setValor(valor);
                        }
                        else
                        {
                            throw new Error("Semántico. No se ingresó un número ", log, linea, columna);
                        }
                    }
                    match("(");
                    match(")");
                }
                else
                {
                    // Como no se ingresó un número desde el Console, entonces viene de una expresión matemática
                    //Console.WriteLine("Antes: " + maximoTipo);
                    Expresion();
                    //Console.WriteLine("Despues: " + maximoTipo);
                    float r = s.Pop();
                    v.setValor(r);
                }
            }
            if (Contenido == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        //BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool ejecuta)
        {
            match("{");
            if (Contenido != "}")
            {
                ListaInstrucciones(ejecuta);
            }
            else
            {
                match("}");
            }
        }
        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta)
        {
            Instruccion(ejecuta);
            if (Contenido != "}")
            {
                ListaInstrucciones(ejecuta);
            }
            else
            {
                match("}");
            }
        }
        //Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool ejecuta)
        {
            if (Contenido == "Console")
            {
                console(ejecuta);
            }
            else if (Contenido == "if")
            {
                If(ejecuta);
            }
            else if (Contenido == "while")
            {
                While(ejecuta);
            }
            else if (Contenido == "do")
            {
                Do(ejecuta);
            }
            else if (Contenido == "for")
            {
                For(ejecuta);
            }
            else if (Clasificacion == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
                match(";");
            }
        }
        //Asignacion -> Identificador = Expresion; (DONE)
        /*
        Id++ (DONE)
        Id-- (DONE)
        Id IncrementoTermino Expresion (DONE)
        Id IncrementoFactor Expresion (DONE)
        Id = Console.Read() (DONE)
        Id = Console.ReadLine() (DONE)
        */
        private void Asignacion()
        {
            // Se iniciliaza cada vez que hagamos una expresión matemática
            maximoTipo = Variable.TipoDato.Char;
            float r;
            Variable? v = l.Find(variable => variable.Nombre == Contenido);
            if (v == null)
            {
                throw new Error("Sintaxis: La variable " + Contenido + " no está definida", log, linea, columna);
            }
            //Console.WriteLine($"Contenido en Asignación = {Contenido}");
            match(Tipos.Identificador);
            if (Contenido == "++")
            {
                //Console.WriteLine("Antes de match '++', Contenido = " + Contenido);
                match("++");
                //Console.WriteLine("Después de match '++', Contenido = " + Contenido);
                r = v.Valor + 1;
                v.setValor(r);
                //Console.WriteLine($"Actualización: {v.Nombre} = {v.Valor}");
            }
            else if (Contenido == "--")
            {
                match("--");
                r = v.Valor - 1;
                v.setValor(r);
            }
            else if (Contenido == "=")
            {
                match("=");
                if (Contenido == "Console")
                {
                    match("Console");
                    match(".");
                    if (Contenido == "Read")
                    {
                        match("Read");
                        match("(");
                        int valorLeido = Console.Read();
                        if (valorLeido >= 48 && valorLeido <= 57)
                        {
                            valorLeido = convertirAscii(valorLeido);
                            s.Push(valorLeido);
                            v.setValor(valorLeido, maximoTipo);
                        }
                        else
                        {
                            throw new Error("Semántico. No se ingresó un carácter válido ", log, linea, columna);
                        }
                    }
                    else
                    {
                        match("ReadLine");
                        match("(");
                        string? line = Console.ReadLine();
                        if (float.TryParse(line, out float valor))
                        {
                            s.Push(valor);
                            v.setValor(valor, maximoTipo);
                        }
                        else
                        {
                            throw new Error("Semántico. No se ingresó un número ", log, linea, columna);
                        }
                    }
                    match(")");
                }
                else
                {
                    Expresion();
                    r = s.Pop();
                    v.setValor(r, maximoTipo);
                }
            }
            else if (Contenido == "+=")
            {
                match("+=");
                Expresion();
                r = v.Valor + s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "-=")
            {
                match("-=");
                Expresion();
                r = v.Valor - s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "*=")
            {
                match("*=");
                Expresion();
                r = v.Valor * s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "/=")
            {
                match("/=");
                Expresion();
                r = v.Valor / s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "%=")
            {
                match("%=");
                Expresion();
                r = v.Valor % s.Pop();
                v.setValor(r);
            }
            //displayStack();
        }
        /*If -> if (Condicion) bloqueInstrucciones | instruccion
        (else bloqueInstrucciones | instruccion)?*/
        private void If(bool ejecuta2)
        {
            match("if");
            match("(");
            bool ejecuta = Condicion() && ejecuta2;
            //Console.WriteLine(ejecuta);
            match(")");
            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }

            if (Contenido == "else")
            {
                match("else");
                bool ejecutarElse = !ejecuta && ejecuta2; // Solo se ejecuta el else si el if no se ejecutó
                if (Contenido == "{")
                {
                    BloqueInstrucciones(ejecutarElse);
                }
                else
                {
                    Instruccion(ejecutarElse);
                }
            }
        }
        //Condicion -> Expresion operadorRelacional Expresion
        private bool Condicion()
        {
            maximoTipo = Variable.TipoDato.Char;
            Expresion();
            float valor1 = s.Pop();
            string operador = Contenido;
            match(Tipos.OperadorRelacional);
            maximoTipo = Variable.TipoDato.Char;
            Expresion();
            float valor2 = s.Pop();
            switch (operador)
            {
                case ">": return valor1 > valor2;
                case ">=": return valor1 >= valor2;
                case "<": return valor1 < valor2;
                case "<=": return valor1 <= valor2;
                case "==": return valor1 == valor2;
                default: return valor1 != valor2;
            }
        }
        //While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While(bool ejecuta)
        {
            int tempChar = contadorCaracteres - 6;
            int tempLine = Lexico.linea;

            match("while");
            match("(");

            bool ejecutarWhile = Condicion() && ejecuta;

            match(")");

            // Esta variable servirá para verificar y controlar la ejecución de los bloques
            // Si es true, se ejecutará como de costumbre
            // Si es false, significa que ya no se debe ejecutar el while
            // Así que, el parámetro que pasa es false y ya no se ejecutan las instrucciones
            // Se hace de esta forma para que, en caso de ser false, matcheará la llave cerrada y abierta
            // Y seguirá con el código res
            bool ejecutarBloque = ejecutarWhile;

            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecutarBloque);
            }
            else
            {
                Instruccion(ejecutarBloque);
            }

            // Si la condición sigue siendo true, se reajusta nuevamente la posición del apuntador y los caracteres
            // Luego se consume el token para que empiece nuevamente a matchear el while y repita el procedimiento...
            if (ejecutarWhile)
            {
                setPosicion(tempChar, tempLine);
            }
        }
        /*Do -> do bloqueInstrucciones | intruccion 
        while(Condicion);*/
        private void Do(bool ejecuta)
        {
            bool ejecutarDo;
            int tempChar = contadorCaracteres - 3;
            int tempLine = Lexico.linea;

            /*Método Seek
            public override long Seek(long offset, System.IO.SeekOrigin origin);
            Offset = Int64 -> Indica la cantidad de bytes (posiciones) que va a recorrer desde el origen
            Origin = SeekOrigin -> Indica desde donde se empezará a buscar
            Y devuelve la nueva posición del cursor
            */

            do
            {
                match("do");
                if (Contenido == "{")
                {
                    BloqueInstrucciones(ejecuta);
                }
                else
                {
                    Instruccion(ejecuta);
                }

                match("while");
                match("(");
                ejecutarDo = Condicion() && ejecuta;
                match(")");
                match(";");

                if (ejecutarDo)
                {
                    setPosicion(tempChar, tempLine);
                }
            } while (ejecutarDo);
        }
        /*For -> for(Asignacion; Condicion; Asignacion) 
        BloqueInstrucciones | Intruccion*/
        private void For(bool ejecuta)
        {
            int inicioFor = contadorCaracteres - 4;  // Guarda la posición para volver después al principio
            int lineaInicioFor = Lexico.linea;

            match("for");
            match("(");

            Asignacion();

            int inicioSegundoBloqueFor = contadorCaracteres - 1;
            int lineaSegundoBloqueFor = Lexico.linea;
            match(";");

            //Console.WriteLine($"Contenido (Antes de condición) = {Contenido}");
            bool ejecutarFor = Condicion() && ejecuta;
            //Console.WriteLine($"Contenido (Después de condición) = {Contenido}");

            // Se guarda la posición antes de consumir la tercera parte del For
            int inicioTercerBloqueFor = contadorCaracteres - 1;
            int lineaTercerBloqueFor = Lexico.linea;
            //Console.WriteLine($"Contenido (Antes de guardar la posición) = {Contenido} // Número de caracter = {lineaTercerBloque}");
            match(";");
            //Asignacion();

            while (Contenido != ")")
            {
                nextToken();
            }

            match(")");

            int inicioBloqueInstrucciones = contadorCaracteres - 2;
            int lineaBloqueInstrucciones = Lexico.linea;
            //Console.WriteLine($"Probando caracter = {Contenido}");

            while (Contenido != "}")
            {
                nextToken();
            }

            int finalBloqueInstrucciones = contadorCaracteres - 2;
            int lineaFinalBloqueInstrucciones = Lexico.linea;

            while (ejecutarFor)
            {
                setPosicion(inicioBloqueInstrucciones, lineaBloqueInstrucciones);
                if (Contenido == "{") BloqueInstrucciones(ejecuta);
                else Instruccion(ejecuta);

                setPosicion(inicioFor, lineaInicioFor);

                setPosicion(inicioSegundoBloqueFor, lineaSegundoBloqueFor);
                ejecutarFor = Condicion() && ejecuta;

                setPosicion(inicioTercerBloqueFor, lineaTercerBloqueFor);
                Asignacion();

                Console.WriteLine($"Contenido (Después de Asignación) = {Contenido}");

                if (Contenido == ")")
                {
                    nextToken();
                    if (Contenido == "{") BloqueInstrucciones(false);
                    else Instruccion(false);
                }
            }
        }
        private void setPosicion(int posicion, int linea)
        {
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
            contadorCaracteres = posicion;
            Lexico.linea = linea;
            nextToken();
        }
        //Console -> Console.(WriteLine|Write) (cadena? concatenaciones?);
        private void console(bool ejecuta)
        {
            bool isWriteLine = false;
            string concatenaciones = "";
            bool isEmpty = false;

            match("Console");
            match(".");

            if (Contenido == "WriteLine")
            {
                match("WriteLine");
                isWriteLine = true;
            }
            else
            {
                match("Write");
            }

            match("(");

            if (Clasificacion == Tipos.Cadena)
            {
                concatenaciones = Contenido.Trim('"');
                match(Tipos.Cadena);
            }
            else if (Clasificacion == Tipos.Identificador)
            {
                Variable? v = l.Find(var => var.Nombre == Contenido);

                if (v == null)
                {
                    throw new Error("Sintaxis. La variable " + Contenido + " no está definida", log, linea, columna);
                }
                else
                {
                    concatenaciones = v.Valor.ToString();
                    match(Tipos.Identificador);
                }
            }
            else
            {
                isEmpty = true;
            }

            if (Contenido == "+")
            {
                //match("+");
                concatenaciones += Concatenaciones();  // Se acumula el resultado de las concatenaciones
            }

            match(")");
            match(";");

            if (ejecuta)
            {
                if (isWriteLine)
                {
                    if (!isEmpty)
                    {
                        Console.WriteLine(concatenaciones);
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.Write(concatenaciones);
                }
            }
        }
        // Concatenaciones -> Identificador|Cadena ( + concatenaciones )?
        private string Concatenaciones()
        {
            string resultado = "";

            if (Clasificacion == Tipos.Identificador)
            {
                Variable? v = l.Find(variable => variable.Nombre == Contenido);
                if (v != null)
                {
                    resultado = v.Valor.ToString(); // Obtener el valor de la variable y convertirla
                }
                else
                {
                    throw new Error("La variable " + Contenido + " no está definida", log, linea, columna);
                }
                match(Tipos.Identificador);
            }
            else if (Clasificacion == Tipos.Cadena)
            {
                resultado = Contenido.Trim('"');
                match(Tipos.Cadena);
            }
            if (Contenido == "+")
            {
                match("+");
                resultado += Concatenaciones();  // Acumula el siguiente fragmento de concatenación
            }
            return resultado;
        }
        //Main -> static void Main(string[] args) BloqueInstrucciones 
        private void Main()
        {
            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            BloqueInstrucciones(true);
        }
        // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (Clasificacion == Tipos.OperadorTermino)
            {
                string operador = Contenido;
                match(Tipos.OperadorTermino);
                Termino();
                //Console.Write(operador + " ");
                float n1 = s.Pop();
                float n2 = s.Pop();
                switch (operador)
                {
                    case "+": s.Push(n2 + n1); break;
                    case "-": s.Push(n2 - n1); break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (Clasificacion == Tipos.OperadorFactor)
            {
                string operador = Contenido;
                match(Tipos.OperadorFactor);
                Factor();

                //Console.Write(operador + " ");
                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "*": s.Push(n2 * n1); break;
                    case "/": s.Push(n2 / n1); break;
                    case "%": s.Push(n2 % n1); break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (Clasificacion == Tipos.Numero)
            {
                //Variable.valorToTipoDato(float.Parse(Contenido)); 
                if (maximoTipo < Variable.valorToTipoDato(float.Parse(Contenido)))
                {
                    maximoTipo = Variable.valorToTipoDato(float.Parse(Contenido));
                }
                s.Push(float.Parse(Contenido));
                //Console.Write(Contenido + " ");
                match(Tipos.Numero);
            }
            else if (Clasificacion == Tipos.Identificador)
            {
                Variable? v = l.Find(variable => variable.Nombre == Contenido);
                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + Contenido + " no está definida", log, linea, columna);
                }
                if (maximoTipo < v.Tipo)
                {
                    maximoTipo = v.Tipo;
                }
                s.Push(v.Valor);
                //Console.Write(Contenido + " ");
                match(Tipos.Identificador);
            }
            else if (Clasificacion == Tipos.FuncionMatematica)
            {
                string nombreResultado = Contenido;
                match(Tipos.FuncionMatematica);

                match("(");
                Expresion();
                match(")");

                float resultado = s.Pop();

                float resultadoFuncion = funcionMatematica(resultado, nombreResultado);
                s.Push(resultadoFuncion);
            }
            else
            {
                match("(");
                Variable.TipoDato tipoCasteo = Variable.TipoDato.Char;
                bool huboCasteo = false;
                if (Clasificacion == Tipos.TipoDato)
                {
                    switch (Contenido)
                    {
                        case "int": tipoCasteo = Variable.TipoDato.Int; break;
                        case "float": tipoCasteo = Variable.TipoDato.Float; break;
                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                    huboCasteo = true;
                }
                Expresion();
                if (huboCasteo)
                {
                    maximoTipo = tipoCasteo;
                    float r = s.Pop();
                    switch (tipoCasteo)
                    {
                        case Variable.TipoDato.Int: r = (r % 65536); break;
                        case Variable.TipoDato.Char: r = (r % 256); break;
                    }
                    s.Push(r);
                }
                match(")");
            }
        }
        private float funcionMatematica(float resultado, string nombreExpresion)
        {
            switch (nombreExpresion)
            {
                case "ceil": return (float)Math.Ceiling(resultado);
                case "pow": return (int)Math.Pow(resultado, 2);
                case "sqrt": return (float)Math.Sqrt(resultado);
                case "exp": return (float)Math.Exp(resultado);
                case "floor": return (float)Math.Floor(resultado);
                case "max":
                    int maxTipo = 0;
                    Variable.TipoDato tipo = Variable.valorToTipoDato(resultado);
                    switch (tipo)
                    {
                        case Variable.TipoDato.Int: maxTipo = 65535; break;
                        case Variable.TipoDato.Char: maxTipo = 255; break;
                    }
                    return Math.Max(maxTipo, resultado);
                case "abs": return Math.Abs(resultado);
                case "log10": return (float)Math.Log10(resultado);
                case "log2": return (float)Math.Log2(resultado);
                case "rand": Random random = new Random(); return random.Next(0, (int)(resultado));
                case "trunc": return (float)Math.Truncate(resultado);
                case "round": return (float)Math.Round(resultado);
            }
            return resultado;
        }
        private int convertirAscii(int valor)
        {
            return valor - 48;
        }
        /*SNT = Producciones = Invocar el metodo
        ST  = Tokens (Contenido | Classification) = Invocar match Variables -> tipo_dato Lista_identificadores; Variables?*/
    }
}