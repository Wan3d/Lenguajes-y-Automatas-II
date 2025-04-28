/*
Significado de los símbolos:
    SNT (Símbolo no terminal) = Producciones = Invocar el metodo
    ST (Símbolo terminal) = Tokens (Contenido | Classification) = Invocar match

    -> Produce
    \; Fin de producción
    \? Optativo
    \) Cierre de agrupación
    \( Inicio de agrupación
    \| OR
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Token
    {
        public enum Tipos
        {
            SNT,                    // SNT -> L+ (una o más letras) --Empieza con mayúscula 
            ST,                     // ST -> L+ (una o más letras)  --No empieza con mayúscula
                                    // ST -> Palabras reservadas de Tokens
                                    // ST -> Lambda
            Produce,                // Produce -> -> 
            FinProduccion,          // FinProduccion -> \;
            Optativo,               // Optativo -> \?
            CierreAgrupacion,       // CierreAgrupacion -> \)
            InicioAgrupacion,       // InicioAgrupacion -> \(
            OR                      // OR -> \|
        }
        private string contenido;
        private Tipos clasificacion;
        public Token()
        {
            contenido = "";
            //clasificacion = Tipos.Identificador;
        }
        public string Contenido
        {
            get { return contenido; }
            set { contenido = value; }
        }
        public Tipos Clasificacion
        {
            get { return clasificacion; }
            set { clasificacion = value; }
        }
    }
}