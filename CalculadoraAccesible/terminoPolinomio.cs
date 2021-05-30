using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraAccesible
{
    public class terminoPolinomio
    {
        public double coeficiente;
        public int potencia;

        public terminoPolinomio(double coef, int pot)
        {
            coeficiente = coef;
            potencia = pot;
        }
    }
}
