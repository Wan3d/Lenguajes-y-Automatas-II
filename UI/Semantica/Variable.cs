using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semantica
{
    public class Variable 
    {
        public enum TipoDato
        {
            Char, Int, Float
        }
        TipoDato tipo;
        string nombre;
        float valor;

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
                throw new Error("Semántico. No se puede asignar un " + valorToTipoDato(valor) + " a un " + tipo + " en la variable " + nombre);
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
            else if (valor <= Math.Pow(2, 16))
            {
                return TipoDato.Int;
            }
            else
            {
                return TipoDato.Float;
            }
        }
        public float getValor()
        {
            return valor;
        }
        public string getNombre()
        {
            return nombre;
        }
        public TipoDato getTipoDato()
        {
            return tipo;
        }
    }
}