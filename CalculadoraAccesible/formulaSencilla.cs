using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraAccesible
{
    class formulaSencilla
    {
        //double resultadoSinParentesis;
        public double resultado;
        public bool swEsFormulaValida = true;
        const double codigoError = 12345678.987654321385638;
        enum tipoOperacion { suma, resta, multiplicacion, division, ninguna };

        public formulaSencilla(string formula, bool redondear)
        {
            resultado = realizarOperacionesConParentesis(formula); //realizarOperaciones(formula);
            if (redondear) //si hay que redondear, se devuelven sólo dos dígitos si hay decimal
                resultado = Math.Round(resultado, 2);

            //if (resultadoComoFraccion)
            //{
            //    string fraccion = DecimalToFraction(resultado);
            //    double result = 0;
            //    bool swConversion = double.TryParse(, out result);
            //    if (swConversion)
            //        resultado = result;
            //}

            if (resultado == codigoError)
                swEsFormulaValida = false;
            else
                swEsFormulaValida = true;
        }

        public formulaSencilla() //constructor para poder usar la función DecimalToFraction
        {

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

        bool formulaConParentesis(string formula)
        {
            if (formula.IndexOf('(') < 0) return false;
            if (formula.IndexOf(')') < 0) return false;
            return true;
        }

        double realizarOperacionesConParentesis (string formula)
        {
            try
            {
                if (!formulaConParentesis(formula))
                    return realizarOperaciones(formula);
                
                int lugarPrimerCierraParentesis = formula.IndexOf(')');
                int lugarAbreParéntesis = -1;

                if (lugarPrimerCierraParentesis > 0) //si hay un cierra paréntesis
                {
                    for (int i = lugarPrimerCierraParentesis; i >= 0; i--)//se recorre hacia atrás la cadena desde el cierra paréntesis hasta encontrar el abre paréntesis
                    {
                        if (formula[i] == '(')
                        {
                            lugarAbreParéntesis = i;
                            break;
                        }    
                    }
                    if (lugarAbreParéntesis == -1)
                        return codigoError; //; //si había cierra paréntesis, pero no habre paréntesis
                    else
                        lugarAbreParéntesis++;

                    string formulaEntreParentesis = formula.Substring(lugarAbreParéntesis, lugarPrimerCierraParentesis - lugarAbreParéntesis);
                    double swBienResuelto = realizarOperaciones(formulaEntreParentesis); //se resulve el paréntesis

                    if (swBienResuelto == codigoError) return codigoError; //; //si hubo problema resolviendo el paréntesis, es no válido

                    //se arma la fórmula reemplazando el paréntesis por el resultado
                    string cadenaSinUnParentesis = formula.Substring(0, lugarAbreParéntesis-1);//desde el inicio hasta el abre paréntesis
                    cadenaSinUnParentesis += swBienResuelto; //el resultado del paréntesis que se resolvió
                    cadenaSinUnParentesis += formula.Substring(lugarPrimerCierraParentesis+1, formula.Length - lugarPrimerCierraParentesis-1); //desde el cierra paréntesis hasta el final de la fórmula

                    //------------ITERADOR-----------------
                    if (cadenaSinUnParentesis == "") //si no se procesó toda la fórmula, se itera de nuevo la función
                        return swBienResuelto;

                    //if (swBienResuelto != 0)
                            return realizarOperacionesConParentesis(cadenaSinUnParentesis);
                    //resultado = swBienResuelto;
                }
                return codigoError;
            }
            catch
            {
                return codigoError; //;
            }
        }

        double realizarOperaciones(string formula)
        {
            List<string> elementosFormula = new List<string>();
            string auxNumero = "";
            string auxLetras = "";
            char cAux;
            

            try
            {
                formula = realizarRaices(formula); //si tiene raíces
                formula = realizarPotencias(formula); //si tiene potencias, se resuelven primero
                

                int sumador = -1;
                bool swHaySignoNegativo = false;
                double posibleResult = 0;
                bool swYaHayResult = double.TryParse(formula, out posibleResult);
                if (swYaHayResult) return posibleResult;


                //----------separamos cada elemento de la fórmula -----------------
                foreach (char c in formula)
                {
                    sumador++;
                    cAux = c;
                    if (cAux == '.') cAux = ','; //se usan comas como separador decimal
                    if (new ValidadorCadenas().esCaracterValido(cAux, true))
                    {
                        if (cAux == '=' || cAux == ' ') continue; //se saltea si escribió espacio o igual

                        if (char.IsDigit(cAux) || cAux == ',')// || (c == '-') && sumador == 0)) //sólo se carga el menos en el número si es el primmero
                        {
                            if (swHaySignoNegativo)
                                auxNumero += cAux;//"-" + cAux;
                            else
                                auxNumero += cAux;

                            if (auxLetras != "")
                            {
                                elementosFormula.Add(auxLetras);
                                
                                //if (c != '-')
                                    auxLetras = "";
                                //else
                                //    auxLetras = "-";
                            }

                            if (sumador == formula.Length - 1)
                            {
                                elementosFormula.Add(auxNumero);
                                auxNumero = "";
                            }
                        }
                        else //si es letra
                        {
                            if (c == '-')
                            {
                                if (auxNumero == "-") //si el número ya es negativo
                                {
                                    //if (c == '-') //y viene otro negativo, negativo de un negativo es un positivo
                                    //{
                                        auxNumero = auxNumero.Substring(1, auxNumero.Length - 1);
                                        swHaySignoNegativo = false;
                                    //}
                                }
                                else
                                {

                                    //if (c == '-')
                                    //{
                                        cAux = '+';
                                    double numero = 0;
                                    bool swYaEsbuenNumero = double.TryParse(auxNumero, out numero);
                                    if (swYaEsbuenNumero)
                                    {
                                        elementosFormula.Add(numero.ToString());
                                        auxNumero = "-";
                                    }
                                    else
                                        auxNumero += "-";
                                        
                                    swHaySignoNegativo = true;
                                    //}
                                    //else
                                        
                                }
                            }
                            else
                                swHaySignoNegativo = false;


                            if (auxLetras == "" && sumador > 0 && cAux != '-')
                                auxLetras += cAux;
                            

                            if (auxNumero != "" && auxNumero != "-")
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
                        return codigoError; //false; //si hay un carácter no válido la fórmula está mal
                }


                //-------------------------------------------------------------------------------
                //-------------------se realizan las operaciones de la fórmula-------------------
                double auxResult = 0;
                bool convertirPrimerNumero = double.TryParse(elementosFormula[0], out auxResult);

                if (!convertirPrimerNumero) return codigoError; //false; //si no se pudo convertir el primer número es que se escribió mal la fórmula

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

                                if (!convertirPrimerNumero) return codigoError; //false;

                                switch (t)
                                {
                                    case tipoOperacion.suma:
                                        if (sumador == elementosFormula.Count - 1) //si está en el final de los elementos
                                        {
                                            if (tipoOpAnterior == tipoOperacion.suma || tipoOpAnterior == tipoOperacion.resta || tipoOpAnterior == tipoOperacion.ninguna)
                                                cadenaFormulaProcesada += auxResult + "+" + b;
                                            else
                                                cadenaFormulaProcesada += "+" + b;
                                        }
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



                                        if (sumador == elementosFormula.Count - 1) //si está en el final de los elementos
                                        {
                                            //cadenaFormulaProcesada += "-" + Math.Abs(b);

                                            if (tipoOpAnterior == tipoOperacion.suma || tipoOpAnterior == tipoOperacion.resta || tipoOpAnterior == tipoOperacion.ninguna)
                                                cadenaFormulaProcesada += auxResult + b; //"-" + Math.Abs(b);
                                            else
                                                cadenaFormulaProcesada += b;// "-" + Math.Abs(b);
                                        }
                                        else
                                        {
                                            if (tipoOpAnterior == tipoOperacion.suma || tipoOpAnterior == tipoOperacion.resta || tipoOpAnterior == tipoOperacion.ninguna)
                                                cadenaFormulaProcesada += auxResult + "-";
                                            else
                                                cadenaFormulaProcesada += "-";
                                            auxResult = b;//Math.Abs(b);
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
                                        return codigoError; //false;
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

                                if (!convertirPrimerNumero) return codigoError; //false;

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
                                        return codigoError;// false;
                                }
                            }

                        }
                        sumador++;
                    }

                }

                //------------ITERADOR-----------------
                if (cadenaFormulaProcesada == "") //si no se procesó toda la fórmula, se itera de nuevo la función
                    return auxResult;

                //if (resultadoSinParentesis == 0)
                //resultadoSinParentesis = auxResult;
                return realizarOperaciones(cadenaFormulaProcesada);

            }
            catch
            {
                return codigoError; //false; //si hay error la fórmula está mal
            }
            //return - true;
        }

        string realizarPotencias (string formula) //devuelve la fórmula igual pero con las pontecias realizadas
        {
            //string formulaLista = formula;
            int sumador = 0;
            int añadido = 0;
            foreach (char c in formula) //recorremos la fórmula para que no haya signo negativo sin antes un +
            {
                //if (>1)
                //{
                    if (c == '-' && sumador > 0)
                    {
                        if (char.IsDigit(formula[sumador + añadido - 1])) //si hay un número antes del -
                        {
                            formula = formula.Substring(0, sumador) + "+" + formula.Substring(sumador, formula.Length - sumador); //añadimos el +
                            añadido++;
                        }
                    }
                //}
                sumador++;
            }


            int lugarpotencia = formula.IndexOf('^');
            if (lugarpotencia < 0 || lugarpotencia == formula.Length || lugarpotencia == 0) return formula; //si no hay potencias, o están al principio, o al final, se devuelve todo como llegó

            //------------se busca la base, pueden ser números con coma o negativos
            int lugarEmpiezaNumero = 0;
            for (int i = lugarpotencia-1; i >= 0; i--)//se recorre hacia atrás para buscar la base de la potencia
            {
                if (!(char.IsDigit(formula[i]) || formula[i] == ',' || formula[i] == '-')) //si es número, o es una coma, o un signo negativo
                {
                    lugarEmpiezaNumero = i;
                    break;
                }
            }
            if (lugarEmpiezaNumero > 0)
                lugarEmpiezaNumero++;

            string numeroBase = formula.Substring(lugarEmpiezaNumero, lugarpotencia - lugarEmpiezaNumero);
            double basePotencia = 0;
            bool swBienResuelto = double.TryParse(numeroBase, out basePotencia); //se resulve el paréntesis

            if (!swBienResuelto) return formula; //hay algún problema para extraer la base, por las dudas se devuelve la formula como vino


            //----------se busca el exponente, puede ser número negativo----------------
            int lugarTerminaNumero = formula.Length;
            for (int i = lugarpotencia + 1; i <= formula.Length-1; i++)//se recorre hacia adelante después del signo de potencia para buscar su exponente de la potencia
            {
                if (!(char.IsDigit(formula[i]) || (formula[i] == '-' && i ==lugarpotencia + 1))) //si es número, o un signo negativo al principio
                {
                    lugarTerminaNumero = i;
                    break;
                }
            }

            string numeroExponente = formula.Substring(lugarpotencia +1, lugarTerminaNumero - lugarpotencia - 1);
            double exponentePotencia = 0;
            swBienResuelto = double.TryParse(numeroExponente, out exponentePotencia); //se resulve el paréntesis

            if (!swBienResuelto) return formula; //hay algún problema para extraer la base, por las dudas se devuelve la formula como vino

            double resultado = Math.Pow(basePotencia, exponentePotencia);

            //se arma la fórmula reemplazando el paréntesis por el resultado
            string cadenaResultado = formula.Substring(0, lugarEmpiezaNumero);//desde el inicio hasta el abre paréntesis
            cadenaResultado += resultado; //el resultado del paréntesis que se resolvió
            if (lugarTerminaNumero != formula.Length)
                cadenaResultado += formula.Substring(lugarTerminaNumero, formula.Length - lugarTerminaNumero); //desde el cierra paréntesis hasta el final de la fórmula

            if (!cadenaResultado.Contains('^'))
                return cadenaResultado;

            return realizarPotencias(cadenaResultado);
        }

        string realizarRaices(string formula) //devuelve la fórmula igual pero con las pontecias realizadas
        {
            //string formulaLista = formula;
            int sumador = 0;
            //foreach (char c in formula) //recorremos la fórmula para que no haya signo negativo sin antes un +
            //{
            //    if (c == '-' && sumador > 0)
            //    {
            //        if (char.IsDigit(formula[sumador - 1])) //si hay un número antes del -
            //        {
            //            formula = formula.Substring(0, sumador) + "+" + formula.Substring(sumador, formula.Length - sumador); //añadimos el +
            //        }
            //    }
            //    //}
            //    sumador++;
            //}

            sumador = 0;
            int añadido = 0;
            foreach (char c in formula) //recorremos la fórmula para poner un 2 antes de $ si es raíz cuadrada
            {
                if (c == '$')
                {
                    if (sumador == 0) //si el $ está al inicio
                    {
                        formula = "2" + formula;
                        añadido++;
                    }
                    else
                    {
                        if (!char.IsDigit(formula[sumador + añadido - 1])) //si NO hay un número antes del -
                        {
                            formula = formula.Substring(0, sumador + añadido) + "2" + formula.Substring(sumador + añadido, formula.Length - sumador - añadido); //añadimos el +
                            añadido++;
                        }
                    }
                }
                //}
                sumador++;
            }


            int lugarRaiz = formula.LastIndexOf('$');
            if (lugarRaiz < 0 || lugarRaiz == formula.Length) return formula; //si no hay raíces, o están al principio, o al final, se devuelve todo como llegó


            //------------se busca el índice, pueden ser números con coma o negativos
            int lugarEmpiezaNumero = 0;
            for (int i = lugarRaiz - 1; i >= 0; i--)//se recorre hacia atrás para buscar la base de la potencia
            {
                if (!(char.IsDigit(formula[i]) || formula[i] == ',' || (formula[i] == '-' && !char.IsDigit(formula[i - 1])))) //si es número, o es una coma, o un signo negativo
                {
                    //if (formula[i] == '-' && !char.IsDigit(formula[i-1]))
                    //{
                    //    lugarEmpiezaNumero = i + 1;
                    //    break;
                    //}
                    //else
                    //{
                        lugarEmpiezaNumero = i +1;
                        break;
                    //}
                }
            }
            //if (lugarEmpiezaNumero > 0)
            //    lugarEmpiezaNumero++;

            string numeroIndice = formula.Substring(lugarEmpiezaNumero, lugarRaiz - lugarEmpiezaNumero);
            double indicePotencia = int.Parse(numeroIndice);
            bool swBienResuelto = double.TryParse(numeroIndice, out indicePotencia); //se convierte el índice

            if (!swBienResuelto) return formula; //hay algún problema para extraer la base, por las dudas se devuelve la formula como vino


            //----------se busca la base, puede ser número negativa o decimal----------------
            int lugarTerminaNumero = formula.Length;
            for (int i = lugarRaiz + 1; i <= formula.Length - 1; i++)//se recorre hacia adelante después del signo de potencia para buscar su exponente de la potencia
            {
                if (!(char.IsDigit(formula[i]) || formula[i] == ',' || (formula[i] == '-' && i == lugarRaiz + 1))) //si es número, o un signo negativo al principio
                {
                        lugarTerminaNumero = i;
                    break;
                }
            }

            string numeroBase = formula.Substring(lugarRaiz + 1, lugarTerminaNumero - lugarRaiz - 1);
            double basePotencia = 0;
            swBienResuelto = double.TryParse(numeroBase, out basePotencia); //se resulve el paréntesis

            if (!swBienResuelto) return formula; //hay algún problema para extraer la base, por las dudas se devuelve la formula como vino

            if (indicePotencia%2 == 0 && basePotencia < 0) //si es par y es negativo
                return "ERROR";

            double resultado = Math.Pow(Math.Abs(basePotencia), 1/indicePotencia);

            if (basePotencia < 0) //si la base es negativa, se hace la potencia por el positivo y se añade el signo negativo
                resultado *= -1; 

            //se arma la fórmula reemplazando el paréntesis por el resultado
            string cadenaResultado = formula.Substring(0, lugarEmpiezaNumero);//desde el inicio hasta el abre paréntesis
            cadenaResultado += resultado; //el resultado del paréntesis que se resolvió
            if (lugarTerminaNumero != formula.Length)
                cadenaResultado += formula.Substring(lugarTerminaNumero, formula.Length - lugarTerminaNumero); //desde el cierra paréntesis hasta el final de la fórmula

            if (!cadenaResultado.Contains('$'))
                return cadenaResultado;

            return realizarRaices(cadenaResultado);
        }

        /// <summary>
        /// Converts Decimals into Fractions.
        /// </summary>
        /// <param name="value">Decimal value</param>
        /// <returns>Fraction in string type</returns>
        /// tomado desde acá: https://www.it-swarm.dev/es/c%23/algoritmo-para-simplificar-el-decimal-fracciones/971807271/ 
        /// Gracias a esta gran contribución! (pequeños arreglos para que funcione con números negativos y con enteros sin coma)
        public string DecimalToFraction(double value)
        {
            int parteEntera = (int)value;
            double parteDecimal = value - parteEntera;
            if (parteDecimal == 0) return value.ToString(); //si es un número entero, se devuelve así como viene
            bool swEsNegativo = false;
            if (value < 0) swEsNegativo = true;
            value = Math.Abs(value);
            string result;
            double numerator, realValue = value;
            int num, den, decimals, length;
            num = (int)value;
            value = value - num;
            value = Math.Round(value, 5);
            length = value.ToString().Length;
            decimals = length - 2;
            numerator = value;
            for (int i = 0; i < decimals; i++)
            {
                if (realValue < 1)
                {
                    numerator = numerator * 10;
                }
                else
                {
                    realValue = realValue * 10;
                    numerator = realValue;
                }
            }
            den = length - 2;
            string ten = "1";
            for (int i = 0; i < den; i++)
            {
                ten = ten + "0";
            }
            den = int.Parse(ten);
            num = (int)numerator;
            result = SimplifiedFractions(num, den);
            if (swEsNegativo) result = "-" + result;
            if (result == "0/1" || result == "-0/1") result = "0";
            if (result == "12345678,99") result = "ERROR";
            return result;
        }

        /// <summary>
        /// Converts Fractions into Simplest form.
        /// </summary>
        /// <param name="num">Numerator</param>
        /// <param name="den">Denominator</param>
        /// <returns>Simplest Fractions in string type</returns>
        string SimplifiedFractions(int num, int den)
        {
            int remNum, remDen, counter;
            if (num > den)
            {
                counter = den;
            }
            else
            {
                counter = num;
            }
            for (int i = 2; i <= counter; i++)
            {
                remNum = num % i;
                if (remNum == 0)
                {
                    remDen = den % i;
                    if (remDen == 0)
                    {
                        num = num / i;
                        den = den / i;
                        i--;
                    }
                }
            }
            return num.ToString() + "/" + den.ToString();
        }
    }

}

