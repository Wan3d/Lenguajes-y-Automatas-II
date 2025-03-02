/*
REQUERIMIENTOS:
1. Declarar las variables con su tipo correspondiente en Ensamblador. O sea, declararlas con su tipo de dato correspondiente (Como Char, Byte, Int) [DONE]
2. En Asignación, generar código en Ensamblador para ++(inc) & --(dec) [DONE]
3. En Asignación, generar código en Ensamblador para +=, -=, *=, /=, %= [DONE]
4. Generar código en Ensamblador para Console.Write y Console.WriteLine
5. Generar código en Ensamblador para Console.Read y Console.ReadLine
6. Programar el do While [DONE]
7. Programar el While
8. Programar el For
9. Condicionar todos los setValor en Asignación | ListaIdentificadores (If (ejecuta){}) [DONE]
10. Programar el Else
11. Usar set y get en Variable [DONE]
12. Ajustar todos los constructores con parámetros con default [DONE]
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ASM
{
    public class Lenguaje : Sintaxis
    {
        private int ifCounter, whileCounter, doWhileCounter, forCounter;
        Stack<float> s;
        List<Variable> l;
        Variable.TipoDato maximoTipo;
        public Lenguaje(string nombre = "prueba.cpp") : base(nombre)
        {
            s = new Stack<float>();
            l = new List<Variable>();
            log.WriteLine("Constructor lenguaje");
            maximoTipo = Variable.TipoDato.Char;
            ifCounter = whileCounter = doWhileCounter = forCounter = 1;
        }
        private void displayStack()
        {
            Console.WriteLine("Contenido del stack: ");
            foreach (float elemento in s)
            {
                Console.WriteLine(elemento);
            }
        }
        private void displayLista()
        {
            asm.WriteLine("section .data");
            log.WriteLine("Lista de variables: ");
            foreach (Variable elemento in l)
            {
                log.WriteLine($"{elemento.Nombre} {elemento.Tipo} {elemento.Valor}");
                if (elemento.Tipo == Variable.TipoDato.Char)
                {
                    asm.WriteLine($"{elemento.Nombre} DB {elemento.Valor}");
                }
                else if (elemento.Tipo == Variable.TipoDato.Int)
                {
                    asm.WriteLine($"{elemento.Nombre} DW {elemento.Valor}");
                }
                else
                {
                    asm.WriteLine($"{elemento.Nombre} DD {elemento.Valor}");
                }
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
                Variables(true);
            }
            Main();
            asm.WriteLine("\tRET");
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
        private void Variables(bool ejecuta)
        {
            Variable.TipoDato t = Variable.TipoDato.Char;
            switch (Contenido)
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(ejecuta, t);
            match(";");
            if (Clasificacion == Tipos.TipoDato)
            {
                Variables(true);
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
        private void ListaIdentificadores(bool ejecuta, Variable.TipoDato t)
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
                        if (ejecuta)
                        {
                            if (maximoTipo > Variable.valorToTipoDato(r))
                            {
                                throw new Error("Tipo dato. No está permitido asignar un valor " + maximoTipo + " a una variable " + Variable.valorToTipoDato(r), log, linea, columna);
                            }
                            v.setValor(r);
                        }
                    }
                    else
                    {
                        match("ReadLine");
                        string? r = Console.ReadLine();
                        if (float.TryParse(r, out float valor))
                        {
                            if (ejecuta)
                            {
                                if (maximoTipo > Variable.valorToTipoDato(valor))
                                {
                                    throw new Error("Tipo dato. No está permitido asignar un valor " + maximoTipo + " a una variable " + Variable.valorToTipoDato(valor), log, linea, columna);
                                }
                                v.setValor(valor);
                            }
                        }
                        else
                        {
                            throw new Error("Sintaxis. No se ingresó un número ", log, linea, columna);
                        }
                    }
                    match("(");
                    match(")");
                }
                else
                {
                    // Como no se ingresó un número desde el Console, entonces viene de una expresión matemática
                    //Console.WriteLine("Antes: " + maximoTipo);
                    asm.WriteLine($"; Asignación de {v.Nombre}");
                    Expresion();
                    //Console.WriteLine("Despues: " + maximoTipo);
                    float r = s.Pop();
                    asm.WriteLine("\tPOP EAX");
                    asm.WriteLine($"\tMOV [{v.Nombre}], EAX");
                    if (ejecuta)
                    {
                        v.setValor(r);
                    }
                }
            }
            if (Contenido == ",")
            {
                match(",");
                ListaIdentificadores(ejecuta, t);
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
                Variables(ejecuta);
            }
            else
            {
                Asignacion(ejecuta);
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
        private void Asignacion(bool ejecuta)
        {
            // Se iniciliaza cada vez que hagamos una expresión matemática
            maximoTipo = Variable.TipoDato.Char;
            float r;
            Variable? v = l.Find(variable => variable.Nombre == Contenido);
            if (v == null)
            {
                throw new Error("Sintaxis: La variable " + Contenido + " no está definida", log, linea, columna);
            }
            //Console.Write(Contenido + " = ");
            match(Tipos.Identificador);
            if (Contenido == "++")
            {
                match("++");
                r = v.Valor + 1;
                asm.WriteLine("\tMOV AL, [" + v.Nombre + "]");
                asm.WriteLine("\tINC AL");
                asm.WriteLine("\tMOV [" + v.Nombre + "], AL");
                v.setValor(r);
            }
            else if (Contenido == "--")
            {
                match("--");
                r = v.Valor - 1;
                asm.WriteLine("\tMOV AL, [" + v.Nombre + "]");
                asm.WriteLine("\tDEC AL");
                asm.WriteLine("\tMOV [" + v.Nombre + "], AL");
                v.setValor(r);
            }
            else if (Contenido == "=")
            {
                match("=");
                if (Contenido == "Console")
                {
                    ListaIdentificadores(ejecuta, v.Tipo); // Ya se hace este procedimiento arriba así que simplemente obtenemos a través del método lo que necesitamos
                }
                else
                {
                    //asm.WriteLine($"; Asignación de {v.Nombre}");
                    Expresion();
                    r = s.Pop();
                    asm.WriteLine("\tPOP EAX");
                    asm.WriteLine($"\tMOV [{v.Nombre}], EAX");
                    if (ejecuta)
                    {
                        /*if (maximoTipo > Variable.valorToTipoDato(r))
                        {
                            throw new Error("Tipo dato. 5 No está permitido asignar un valor " + maximoTipo + " a una variable " + Variable.valorToTipoDato(r), log, linea, columna);
                        }*/
                        v.setValor(r);
                    }
                }
            }
            else if (Contenido == "+=")
            {
                match("+=");
                Expresion();
                float valorSuma = s.Pop();
                r = v.Valor + valorSuma;
                asm.WriteLine("\tMOV EAX, [" + v.Nombre + "]");
                asm.WriteLine("\tADD EAX, [" + valorSuma + "]");
                asm.WriteLine("\tMOV [" + v.Nombre + "], EAX");
                v.setValor(r);
            }
            else if (Contenido == "-=")
            {
                match("-=");
                Expresion();
                float valorResta = s.Pop();
                r = v.Valor - valorResta;
                asm.WriteLine("\tMOV EAX, [" + v.Nombre + "]");
                asm.WriteLine("\tSUB EAX, [" + valorResta + "]");
                asm.WriteLine("\tMOV [" + v.Nombre + "], EAX");
                v.setValor(r);
            }
            else if (Contenido == "*=")
            {
                match("*=");
                Expresion();
                float valorMul = s.Pop();
                r = v.Valor * valorMul;
                asm.WriteLine("\tMOV EAX, [" + v.Nombre + "]");
                asm.WriteLine("\tMUL EAX, [" + valorMul + "]");
                asm.WriteLine("\tMOV [" + v.Nombre + "], EAX");
                v.setValor(r);
            }
            else if (Contenido == "/=")
            {
                match("/=");
                Expresion();
                float valorDiv = s.Pop();
                r = v.Valor / valorDiv;
                asm.WriteLine("\tMOV EAX, [" + v.Nombre + "]");
                asm.WriteLine("\tIMUL EAX, [" + valorDiv + "]");
                asm.WriteLine("\tMOV [" + v.Nombre + "], EAX");
                //asm.WriteLine("    POP");
                v.setValor(r);
            }
            else if (Contenido == "%=")
            {
                match("%=");
                Expresion();
                float valorRes = s.Pop();
                r = v.Valor % valorRes;
                asm.WriteLine("\tMOV EAX, [" + v.Nombre + "]");
                asm.WriteLine("\tMOV EBX, [" + valorRes + "]");
                asm.WriteLine("\tXOR EDX, EDX");
                asm.WriteLine("\tDIV EBX");
                asm.WriteLine("\tMOV [" + v.Nombre + "], EDX");
                //asm.WriteLine("    POP");
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
            asm.WriteLine("; If");
            string label = $"jump_if_{ifCounter++}:";
            bool ejecuta = Condicion(label) && ejecuta2;
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

            asm.WriteLine(label);

            if (Contenido == "else")
            {
                match("else");
                bool ejecutarElse = !ejecuta; // Solo se ejecuta el else si el if no se ejecutó
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
        private bool Condicion(string label, bool isDoWhile = false)
        {
            asm.WriteLine("; Expresión 1");
            maximoTipo = Variable.TipoDato.Char;
            Expresion();
            float valor1 = s.Pop();
            string operador = Contenido;
            match(Tipos.OperadorRelacional);
            maximoTipo = Variable.TipoDato.Char;
            asm.WriteLine("; Expresión 2");
            Expresion();
            float valor2 = s.Pop();
            asm.WriteLine("\tPOP EBX");
            asm.WriteLine("\tPOP EAX");
            asm.WriteLine("\tCMP EAX, EBX");
            if (!isDoWhile)
            {
                switch (operador)
                {
                    case ">":
                        asm.WriteLine($"\tJNA {label.Replace(":", string.Empty)}");
                        return valor1 > valor2;
                    case ">=":
                        asm.WriteLine($"\tJB {label.Replace(":", string.Empty)}");
                        return valor1 >= valor2;
                    case "<":
                        asm.WriteLine($"\tJAE {label.Replace(":", string.Empty)}");
                        return valor1 < valor2;
                    case "<=":
                        asm.WriteLine($"\tJA {label.Replace(":", string.Empty)}");
                        return valor1 <= valor2;
                    case "==":
                        asm.WriteLine($"\tJNE {label.Replace(":", string.Empty)}");
                        return valor1 == valor2;
                    default:
                        asm.WriteLine($"\tJE {label.Replace(":", string.Empty)}");
                        return valor1 != valor2;
                }
            }
            else
            {
                switch (operador)
                {
                    case ">":
                        asm.WriteLine($"\tJA {label.Replace(":", string.Empty)}");
                        return valor1 > valor2;
                    case ">=":
                        asm.WriteLine($"\tJAE {label.Replace(":", string.Empty)}");
                        return valor1 >= valor2;
                    case "<":
                        asm.WriteLine($"\tJB {label.Replace(":", string.Empty)}");
                        return valor1 < valor2;
                    case "<=":
                        asm.WriteLine($"\tJBE {label.Replace(":", string.Empty)}");
                        return valor1 <= valor2;
                    case "==":
                        asm.WriteLine($"\tJE {label.Replace(":", string.Empty)}");
                        return valor1 == valor2;
                    default:
                        asm.WriteLine($"\tJNE {label.Replace(":", string.Empty)}");
                        return valor1 != valor2;
                }
            }
        }
        //While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While(bool ejecuta)
        {
            match("while");
            match("(");
            Condicion("");
            match(")");
            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }
        }
        /*Do -> do bloqueInstrucciones | intruccion 
        while(Condicion);*/
        private void Do(bool ejecuta)
        {
            match("do");
            asm.WriteLine("; Do");
            string label = $"jump_do_{doWhileCounter++}:";
            asm.WriteLine(label);
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
            Condicion(label, true);
            match(")");
            match(";");
        }
        /*For -> for(Asignacion; Condicion; Asignacion) 
        BloqueInstrucciones | Intruccion*/
        private void For(bool ejecuta)
        {
            match("for");
            match("(");
            Asignacion(ejecuta);
            match(";");
            Condicion("");
            match(";");
            Asignacion(ejecuta);
            match(")");
            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }
        }
        //Console -> Console.(WriteLine|Write) (cadena? concatenaciones?);
        private void console(bool ejecuta)
        {
            bool isWriteLine = false;
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
            string concatenaciones = "";
            if (Clasificacion == Tipos.Cadena)
            {
                concatenaciones = Contenido.Trim('"');
                match(Tipos.Cadena);
            }
            if (Contenido == "+")
            {
                match("+");
                concatenaciones += Concatenaciones();  // Se acumula el resultado de las concatenaciones
            }
            match(")");
            match(";");
            if (ejecuta)
            {
                if (isWriteLine)
                {
                    Console.WriteLine(concatenaciones);
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
                asm.WriteLine("\tPOP EBX");
                float n2 = s.Pop();
                asm.WriteLine("\tPOP EAX");
                switch (operador)
                {
                    case "+":
                        s.Push(n2 + n1);
                        asm.WriteLine("\tADD EAX, EBX");
                        asm.WriteLine("\tPUSH EAX");
                        break;
                    case "-":
                        s.Push(n2 - n1);
                        asm.WriteLine("\tSUB EAX, EBX");
                        asm.WriteLine("\tPUSH EAX");
                        break;
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
                asm.WriteLine("\tPOP EBX");
                float n2 = s.Pop();
                asm.WriteLine("\tPOP EAX");
                switch (operador)
                {
                    case "*":
                        s.Push(n2 * n1);
                        asm.WriteLine("\tMUL EBX");
                        asm.WriteLine("\tPUSH EAX");
                        break;
                    case "/":
                        s.Push(n2 / n1);
                        asm.WriteLine("\tDIV EBX");
                        asm.WriteLine("\tPUSH EAX");
                        break;
                    case "%":
                        s.Push(n2 % n1);
                        asm.WriteLine("\tDIV EBX");
                        asm.WriteLine("\tPUSH EDX");
                        break;
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
                asm.WriteLine("\tMOV EAX, " + "[" + Contenido + "]");
                asm.WriteLine("\tPUSH EAX");
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
                asm.WriteLine("\tMOV EAX, " + "[" + Contenido + "]");
                asm.WriteLine("\tPUSH EAX");
                //Console.Write(Contenido + " ");
                match(Tipos.Identificador);
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
                    asm.WriteLine("\tPOP");
                    switch (tipoCasteo)
                    {
                        case Variable.TipoDato.Int: r = (r % 65536); break;
                        case Variable.TipoDato.Char: r = (r % 256); break;
                    }
                    s.Push(r);
                    asm.WriteLine("\tPUSH");
                }
                match(")");
            }
        }
        /*SNT = Producciones = Invocar el metodo
        ST  = Tokens (Contenido | Classification) = Invocar match Variables -> tipo_dato Lista_identificadores; Variables?*/
    }
}