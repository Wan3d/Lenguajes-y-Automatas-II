using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Program : Token
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje l = new())
                {
                    l.Genera();
                    /*while (!l.finArchivo())
                    {
                        l.nextToken();
                    }*/
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}