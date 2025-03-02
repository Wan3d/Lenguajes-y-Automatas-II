using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM
{
    public class Variable
    {
        public enum TipoDato
        {
            Char, Int, Float
        }
        private TipoDato tipo;
        private string nombre;
        private float valor;

        public Variable(TipoDato tipo, string nombre, float valor = 0)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.valor = valor;
        }
        public void setValor(float valor)
        {
            if (valorToTipoDato(valor) <= tipo)
            {
                this.valor = valor;
            }
            else
            {
                throw new Error("Semántico. No se puede asignar un " + valorToTipoDato(valor) + " a un " + tipo + " en la variable " + nombre, Lexico.log, Lexico.linea, Lexico.columna);
            }
        }
        public static TipoDato valorToTipoDato(float valor)
        {
            if (!float.IsInteger(valor))
            {
                return TipoDato.Float;
            }
            else if (valor <= 255)
            {
                return TipoDato.Char;
            }
            else if (valor <= 65535)
            {
                return TipoDato.Int;
            }
            else
            {
                return TipoDato.Float;
            }
        }
        public float Valor //getValor
        {
            get { return valor; }
            set { valor = value; }
        }
        public string Nombre //getNombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public TipoDato Tipo //getTipoDato
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}