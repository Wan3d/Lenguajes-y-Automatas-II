using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Generador
{
    public class Error : Exception
    {
        public Error(string message) : base("" + message) {}
        public Error(string message, StreamWriter log) : base(message)
        {
            log.WriteLine("Error: " + message);
        }
        public Error(string message, StreamWriter log, int linea, int columna) : base(message + " en la linea [" + linea + "] en la columna [" + columna + "]")
        {
            log.WriteLine("Error: " + message + " en la linea [" + linea + "] en la columna [" + columna + "]");
        }
    }
}