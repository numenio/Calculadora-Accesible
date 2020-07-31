using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraAccesible
{
    class formulaSencilla
    {
        public double resultado;
        public bool swEsFormulaValida = true;
        enum tipoOperacion { suma, resta, multiplicacion, division, ninguna };

        public formulaSencilla(string formula)
        {
            swEsFormulaValida = realizarOperaciones(formula);
        }

        double sumar (double a, double b)
        {
            return a + b;
        }

        double restar(double a, double b)
        {
            return a - b;
        }

        double multiplicar(double a, double b)
        {
            return a * b;
        }

        double dividir(double a, double b)
        {
            return a / b;
        }

        bool realizarOperaciones(string formula)
        {
            List<string> elementosFormula = new List<string>();
            string auxNumero = "";
            string auxLetras = "";
            char cAux;
            

            try
            {
                int sumador = -1;
                //----------separamos cada elemento de la fórmula -----------------
                foreach (char c in formula)
                {
                    sumador++;
                    cAux = c;
                    if (cAux == '.') cAux = ','; //se usan comas como separador decimal
                    if (new ValidadorCadenas().esCaracterValido(cAux, true))
                    {
                        if (cAux == '=' || cAux == ' ') continue; //se saltea si escribió espacio o igual

                        if (char.IsDigit(cAux) || cAux == ',' || (c == '-' && sumador == 0)) //sólo se carga el menos en el número si es el primmero
                        {
                            auxNumero += cAux;

                            if (auxLetras != "")
                            {
                                elementosFormula.Add(auxLetras);
                                auxLetras = "";
                            }

                            if (sumador == formula.Length - 1)
                            {
                                elementosFormula.Add(auxNumero);
                                auxNumero = "";
                            }
                        }
                        else //si es letra
                        {
                            auxLetras += cAux;

                            if (auxNumero != "")
                            {
                                elementosFormula.Add(auxNumero);
                                auxNumero = "";
                            }

                            if (sumador == formula.Length-1)
                            {
                                elementosFormula.Add(auxLetras);
                                auxLetras = "";
                            }
                        }
                    }
                    else
                        return false; //si hay un carácter no válido la fórmula está mal
                }


                //-------------------------------------------------------------------------------
                //-------------------se realizan las operaciones de la fórmula-------------------
                double auxResult = 0;
                bool convertirPrimerNumero = double.TryParse(elementosFormula[0], out auxResult);

                if (!convertirPrimerNumero) return false; //si no se pudo convertir el primer número es que se escribió mal la fórmula

                sumador = 0;
                double b = 0;
                tipoOperacion t = tipoOperacion.ninguna;
                string cadenaFormulaProcesada = ""; //para las distintas iteraciones del procesamiento de la fórmula
                tipoOperacion tipoOpAnterior = tipoOperacion.ninguna;

                //----------PRIMERA ITERACIÓN: MULTIPLICACIONES Y DIVISIONES------------------
                if (elementosFormula.Contains("*") || elementosFormula.Contains("/")) //si tiene * ó /, se realizan primero
                {
                    //se realizan sólo las multiplic y divisiones, lo demás se pasa a cadenaFormulaProcesada
                    foreach (string cadena in elementosFormula) //para que esté bien hecha, una fórmula tiene que empezar por número
                    {
                        if (sumador > 0) //se saltea el primer elemento
                        {
                            //impares operaciones, pares números
                            if (sumador % 2 == 0) //si el objeto agarrado es par, osea que es número
                            {
                                convertirPrimerNumero = double.TryParse(cadena, out b);

                                if (!convertirPrimerNumero) return false;

                                switch (t)
                                {
                                    case tipoOperacion.suma:
                                        if (sumador == elementosFormula.Count - 1) //si no está en el final de los elementos
                                            cadenaFormulaProcesada += "+" + b;
                                        else
                                        {
                                            if (tipoOpAnterior == tipoOperacion.suma || tipoOpAnterior == tipoOperacion.resta || tipoOpAnterior == tipoOperacion.ninguna)
                                                cadenaFormulaProcesada += auxResult + "+";
                                            else
                                                cadenaFormulaProcesada += "+";
                                            auxResult = b;
                                        }
                                        tipoOpAnterior = tipoOperacion.suma;
                                        break;
                                    case tipoOperacion.resta:
                                        //cadenaFormulaProcesada += auxResult + "+";
                                        //auxResult = b;



                                        if (sumador == elementosFormula.Count - 1) //si no está en el final de los elementos
                                            cadenaFormulaProcesada += "-" + Math.Abs(b);
                                        else
                                        {
                                            if (tipoOpAnterior == tipoOperacion.suma || tipoOpAnterior == tipoOperacion.resta || tipoOpAnterior == tipoOperacion.ninguna)
                                                cadenaFormulaProcesada += auxResult + "-";
                                            else
                                                cadenaFormulaProcesada += "-";
                                            auxResult = Math.Abs(b);
                                        }
                                        tipoOpAnterior = tipoOperacion.resta;
                                        break;
                                    case tipoOperacion.multiplicacion:
                                        auxResult *= b;
                                        if (sumador == elementosFormula.Count - 1) //si no está en el final de los elementos
                                        {
                                            cadenaFormulaProcesada += auxResult;

                                            
                                        }
                                        else
                                        {
                                            if (elementosFormula[sumador + 1] != "*") //si el elemento siguiente no es de nuevo *
                                                if (elementosFormula[sumador + 1] != "/") //o /
                                                    cadenaFormulaProcesada += auxResult;
                                        }
                                        tipoOpAnterior = tipoOperacion.multiplicacion;
                                        break;
                                    case tipoOperacion.division:
                                        auxResult /= b;
                                        //cadenaFormulaProcesada += auxResult;
                                        //



                                        if (sumador == elementosFormula.Count - 1) //si no está en el final de los elementos
                                        {
                                            cadenaFormulaProcesada += auxResult;


                                        }
                                        else
                                        {
                                            if (elementosFormula[sumador + 1] != "*") //si el elemento siguiente no es de nuevo *
                                                if (elementosFormula[sumador + 1] != "/") //o /
                                                    cadenaFormulaProcesada += auxResult;
                                        }
                                        tipoOpAnterior = tipoOperacion.division;
                                        break;
                                }



                            }
                            else
                            {
                                switch (cadena)
                                {
                                    case "+":
                                        t = tipoOperacion.suma;
                                        break;
                                    case "-":
                                        t = tipoOperacion.resta;
                                        break;
                                    case "*":
                                        t = tipoOperacion.multiplicacion;
                                        break;
                                    case "/":
                                        t = tipoOperacion.division;
                                        break;
                                    default:
                                        return false;
                                }
                            }

                        }
                        sumador++;
                    }
                }

                //----------SEGUNDA ITERACIÓN: SUMAS Y RESTAS------------------
                else //si sólo son + y -
                {
                    //se realizan las operaciones
                    foreach (string cadena in elementosFormula) //para que esté bien hecha, una fórmula tiene que empezar por número
                    {
                        if (sumador > 0) //se saltea el primer elemento
                        {
                            //impares operaciones, pares números
                            if (sumador % 2 == 0) //si el objeto agarrado es par, osea que es número
                            {
                                convertirPrimerNumero = double.TryParse(cadena, out b);

                                if (!convertirPrimerNumero) return false;

                                switch (t)
                                {
                                    case tipoOperacion.suma:
                                        auxResult += b;
                                        break;
                                    case tipoOperacion.resta:
                                        auxResult -= b;
                                        break;
                                    case tipoOperacion.multiplicacion:
                                        auxResult *= b;
                                        break;
                                    case tipoOperacion.division:
                                        auxResult /= b;
                                        break;
                                }



                            }
                            else
                            {
                                switch (cadena)
                                {
                                    case "+":
                                        t = tipoOperacion.suma;
                                        break;
                                    case "-":
                                        t = tipoOperacion.resta;
                                        break;
                                    case "*":
                                        t = tipoOperacion.multiplicacion;
                                        break;
                                    case "/":
                                        t = tipoOperacion.division;
                                        break;
                                    default:
                                        return false;
                                }
                            }

                        }
                        sumador++;
                    }

                }

                //------------ITERADOR-----------------
                if (cadenaFormulaProcesada != "") //si no se procesó toda la fórmula, se itera de nuevo la función
                    realizarOperaciones(cadenaFormulaProcesada);

                if (resultado == 0)
                    resultado = auxResult;
            }
            catch
            {
                return false; //si hay error la fórmula está mal
            }
            return true;
        }

        
    }
}
