using System;

namespace Semantica
{
    public class Token
    {
        public enum Tipos
        {
            Identificador, Numero, Caracter, FinSentencia,
            InicioBloque, FinBloque, OperadorTernario,
            OperadorTermino, OperadorFactor, IncrementoTermino,
            IncrementoFactor, Puntero, Asignacion,
            OperadorRelacional, OperadorLogico, Moneda,
            Cadena, TipoDato, PalabraReservada
        }

        private string Contenido { get; set; }
        private Tipos Clasificacion { get; set; }

        public Token()
        {
            Contenido = "";
            Clasificacion = Tipos.Identificador;
        }
        public void setContenido(string contenido) => Contenido = contenido; // Se asigna el valor contenido a la propiedad Contenido

        public string getContenido() => Contenido; // Simplemente devuelve el valor actual de la propiedad Contenido

        public void setClasificacion(Tipos clasificacion) => Clasificacion = clasificacion;

        public Tipos getClasificacion() => Clasificacion;
    }
}
