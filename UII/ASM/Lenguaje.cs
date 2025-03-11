/*
REQUERIMIENTOS:
1. Declarar las variables con su tipo correspondiente en Ensamblador. O sea, declararlas con su tipo de dato correspondiente (Como Char, Byte, Int) [DONE]
2. En Asignación, generar código en Ensamblador para ++(inc) & --(dec) [DONE]
3. En Asignación, generar código en Ensamblador para +=, -=, *=, /=, %= [DONE]
4. Generar código en Ensamblador para Console.Write y Console.WriteLine [DONE]
5. Generar código en Ensamblador para Console.Read y Console.ReadLine [DONE]
6. Programar el do While [DONE]
7. Programar el While [DONE]
8. Programar el For [DONE]
9. Condicionar todos los setValor en Asignación | ListaIdentificadores (If (ejecuta){}) [DONE]
10. Programar el Else [DONE]
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
        private int strCounter = 1;
        private bool ejecutarIf, ejecutarElse;
        private List<string> cadenasGeneradas = new List<string>();  // Lista para almacenar las cadenas generadas
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
                asm.WriteLine($"{elemento.Nombre} DD 0");
            }

            foreach (string cadenaCompleta in cadenasGeneradas)
            {
                string[] partes = cadenaCompleta.Split('|');
                string nombreStr = partes[0];
                string cadena = partes[1];

                asm.WriteLine($"{nombreStr} DB '{cadena}', 0");  // Definimos las cadenas correctamente
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
                        match("(");
                        int r = Console.Read();
                        asm.WriteLine($"\tGET_DEC 4, {v.Nombre}");
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
                        match("(");
                        string? r = Console.ReadLine();
                        asm.WriteLine($"\tGET_DEC 4, {v.Nombre}");
                        asm.WriteLine("\tNEWLINE");
                        if (float.TryParse(r, out float valor))
                        {
                            //asm.WriteLine("\tPUSH");
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
                    asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX");
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
        private void Asignacion(bool ejecuta, Variable? v = null)
        {
            // Se iniciliaza cada vez que hagamos una expresión matemática
            maximoTipo = Variable.TipoDato.Char;
            float r;
            v = l.Find(variable => variable.Nombre == Contenido);
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
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]");
                asm.WriteLine("\tINC EAX");
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX");
                v.setValor(r);
            }
            else if (Contenido == "--")
            {
                match("--");
                r = v.Valor - 1;
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]");
                asm.WriteLine("\tDEC EAX");
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX");
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
                        asm.WriteLine($"\tGET_DEC 4, {v.Nombre}");
                        if (ejecuta)
                        {
                            if (maximoTipo > Variable.valorToTipoDato(valorLeido))
                            {
                                throw new Error("Tipo dato. No está permitido asignar un valor " + maximoTipo + " a una variable " + Variable.valorToTipoDato(valorLeido), log, linea, columna);
                            }
                            v.setValor(valorLeido);
                        }
                    }
                    else
                    {
                        match("ReadLine");
                        match("(");
                        string? line = Console.ReadLine();
                        asm.WriteLine($"\tGET_DEC 4, {v.Nombre}");
                        asm.WriteLine("\tNEWLINE");

                        if (float.TryParse(line, out float numero))
                        {
                            if (ejecuta)
                            {
                                if (maximoTipo > Variable.valorToTipoDato(numero))
                                {
                                    throw new Error("Tipo dato. No está permitido asignar un valor " + maximoTipo + " a una variable " + Variable.valorToTipoDato(numero), log, linea, columna);
                                }
                                v.setValor(numero);
                            }
                        }
                    }
                    match(")");
                }
                else
                {
                    //asm.WriteLine($"; Asignación de {v.Nombre}");
                    Expresion();
                    r = s.Pop();
                    asm.WriteLine("\tPOP EAX");
                    asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX"); //CORREGIR
                    if (ejecuta)
                    {
                        /*if (maximoTipo > Variable.valorToTipoDato(r))
                        {
                            throw new Error("Tipo dato. No está permitido asignar un valor " + maximoTipo + " a una variable " + Variable.valorToTipoDato(r), log, linea, columna);
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
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]");
                asm.WriteLine($"\tADD EAX, {valorSuma}");
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX");
                //asm.WriteLine("\tPOP EAX");
                v.setValor(r);
            }
            else if (Contenido == "-=")
            {
                match("-=");
                Expresion();
                float valorResta = s.Pop();
                r = v.Valor - valorResta;
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]");
                asm.WriteLine($"\tSUB EAX, {valorResta}");
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX");
                //asm.WriteLine("\tPOP EAX");
                v.setValor(r);
            }
            else if (Contenido == "*=")
            {
                //CORREGIR
                match("*=");
                Expresion();
                float valorMul = s.Pop();
                r = v.Valor * valorMul;
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]"); // Cargar el valor a la variable
                asm.WriteLine($"\tMOV EBX, {valorMul}"); // Se mueve el valor del multiplicador
                asm.WriteLine($"\tMUL EBX"); // Se multiplica el multiplicador por el registro anterior
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX"); // Se guarda el resultado en la variable
                //asm.WriteLine("\tPOP EAX");
                v.setValor(r);
            }
            else if (Contenido == "/=")
            {
                match("/=");
                Expresion();
                float valorDiv = s.Pop();
                r = v.Valor / valorDiv;
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]");
                asm.WriteLine($"\tMOV ECX, {valorDiv}");
                asm.WriteLine($"\tXOR EDX, EDX");
                asm.WriteLine($"\tDIV ECX");
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EAX");
                //asm.WriteLine("\tPOP EAX");
                v.setValor(r);
            }
            else if (Contenido == "%=")
            {
                match("%=");
                Expresion();
                int valorRes = (int)s.Pop(); // Se asegura que valorRes sea un entero para no causar problemas en Ensamblador
                r = (int)v.Valor % valorRes;
                asm.WriteLine($"\tMOV EAX, DWORD[{v.Nombre}]");
                asm.WriteLine($"\tMOV EBX, {valorRes}");
                asm.WriteLine("\tXOR EDX, EDX");
                asm.WriteLine("\tDIV EBX");
                asm.WriteLine($"\tMOV DWORD[{v.Nombre}], EDX");
                //asm.WriteLine("\tPOP EAX");
                v.setValor(r);
            }
            //displayStack();
        }
        /*If -> if (Condicion) bloqueInstrucciones | instruccion
        (else bloqueInstrucciones | instruccion)?*/
        private void If(bool ejecuta2)
        {
            string labelInicioIf = $"jump_if_{ifCounter}:";
            string labelInicioElse = $"jump_else_{ifCounter}:";
            string labelFinCondicion = $"fin_Condicion_{ifCounter}:";

            match("if");
            match("(");

            asm.WriteLine($"; Inicio If/Else {ifCounter}");
            asm.WriteLine(labelInicioIf);

            ejecutarIf = Condicion(labelInicioElse) && ejecuta2;

            match(")");

            if (Contenido == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }

            asm.WriteLine($"\tJMP {labelFinCondicion.Replace(":", string.Empty)}");

            asm.WriteLine(labelInicioElse);

            if (Contenido == "else")
            {
                match("else");

                ejecutarElse = !ejecutarIf && ejecuta2;  // Solo ejecutar el else si el if no se ejecutó y ejecuta2 es true

                if (Contenido == "{")
                {
                    BloqueInstrucciones(true);
                }
                else
                {
                    Instruccion(true);
                }
            }

            asm.WriteLine(labelFinCondicion);  // Etiqueta de fin de la condición
            asm.WriteLine($"; Fin If/Else {ifCounter}");

            ifCounter++;
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
            string labelInicio = $"jump_While_{whileCounter}:";
            string labelFin = $"end_While_{whileCounter}:";

            whileCounter++;

            asm.WriteLine(labelInicio); // Indica a partir de donde hará los saltos de instrucción
            match("while");
            match("(");
            Condicion(labelFin); // Indica si la condición es falsa. Si lo es, salta a la etiqueta que da fin al ciclo
            match(")");
            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }
            asm.WriteLine($"\tJMP {labelInicio.Replace(":", string.Empty)}"); // Si la condición es verdadera, salta al inicio del bucle para repetirse
            asm.WriteLine(labelFin); // En caso de haber llegado aquí significa que la condición del bucle es falsa y llegamos al fin del bucle
        }
        /*Do -> do bloqueInstrucciones | intruccion 
        while(Condicion);*/
        private void Do(bool ejecuta)
        {
            match("do");
            asm.WriteLine("; Do");
            string label = $"jump_do_{doWhileCounter}:";
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
            doWhileCounter++;
        }
        /*For -> for(Asignacion; Condicion; Asignacion) 
        BloqueInstrucciones | Intruccion*/
        private void For(bool ejecuta)
        {
            string labelInicio = $"jump_For_{forCounter}:";
            string labelFin = $"end_For_{forCounter}:";

            forCounter++; // Se incrementa uno más por si hay un ciclo anidado, así generamos etiquetas únicas para cada for

            match("for");
            match("(");
            Asignacion(ejecuta);
            match(";");

            asm.WriteLine($"; Inicio de For {forCounter}");
            asm.WriteLine(labelInicio);

            Condicion(labelFin);

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

            asm.WriteLine($"\tJMP {labelInicio.Replace(":", string.Empty)}");
            asm.WriteLine(labelFin);
            asm.WriteLine($"; Fin de For {forCounter}");

            //forCounter++;
        }
        //Console -> Console.(WriteLine|Write) (cadena? concatenaciones?);
        private void console(bool ejecuta)
        {
            bool isWriteLine = false;
            string concatenaciones = "";
            string nombreVariable = "";
            bool isVariable = false;

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
                string nombreStr = $"str{strCounter}"; // Guardar la cadena junto con el nombre generado

                cadenasGeneradas.Add($"{nombreStr}|{concatenaciones}"); // Agregar el nombre junto a la cadena
                strCounter++;
                match(Tipos.Cadena);
            }
            else
            {
                Variable? v = l.Find(var => var.Nombre == Contenido);
                if (v == null)
                {
                    throw new Error("Sintaxis: La variable " + Contenido + " no está definida", log, linea, columna);
                }
                else
                {
                    concatenaciones = v.Valor.ToString();
                    nombreVariable = v.Nombre;
                    match(Tipos.Identificador);
                    isVariable = true;
                }
            }

            if (Contenido == "+")
            {
                concatenaciones += Concatenaciones();
            }

            match(")");
            match(";");

            if (ejecuta)
            {
                if (isWriteLine)
                {
                    Console.WriteLine(concatenaciones);
                    if (!isVariable)
                    {
                        string nombreStr = $"str{strCounter - 1}";
                        asm.WriteLine($"\tPRINT_STRING {nombreStr}");
                    }
                    else
                    {
                        asm.WriteLine($"\tPRINT_DEC 4, {nombreVariable}");
                    }
                    asm.WriteLine("\tNEWLINE");
                }
                else
                {
                    Console.Write(concatenaciones);
                    if (!isVariable)
                    {
                        string nombreStr = $"str{strCounter - 1}";
                        asm.WriteLine($"\tPRINT_STRING {nombreStr}");
                    }
                    else
                    {
                        asm.WriteLine($"\tPRINT_DEC 4, {nombreVariable}");
                    }
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
                        asm.WriteLine("\tXOR EDX, EDX");
                        asm.WriteLine("\tDIV EBX");
                        s.Push(n2 % n1);
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
                asm.WriteLine($"\tMOV EAX, {Contenido}");
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
                asm.WriteLine($"\tMOV EAX, DWORD[{Contenido}]");
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
                    asm.WriteLine("\tPOP EAX");
                    switch (tipoCasteo)
                    {
                        case Variable.TipoDato.Int: r = r % 65536; break;
                        case Variable.TipoDato.Char: r = r % 256; break;
                    }
                    s.Push(r);
                    asm.WriteLine("\tPUSH EAX");
                }
                match(")");
            }
        }
        /*SNT = Producciones = Invocar el metodo
        ST  = Tokens (Contenido | Classification) = Invocar match Variables -> tipo_dato Lista_identificadores; Variables?*/
    }
}