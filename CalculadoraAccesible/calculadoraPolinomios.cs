using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CalculadoraAccesible
{
    class calculadoraPolinomios
    {
        public bool swcalculoPolinomiosBienEscrito = true;
        public bool swHayUnSoloPolinomio = false;
        public Polinomio polResult;
        public Polinomio polResto;



        //double[] coef = new double[] { 10, 22, 3, 0, -7, 0, -1 };
        //double[] coef2 = new double[] { 1, -1, 3, 5, -6 };
        List<terminoPolinomio> ts = new List<terminoPolinomio>();
        List<terminoPolinomio> ts2 = new List<terminoPolinomio>();
        //terminoPolinomio t1 = new terminoPolinomio(10, 5);
        //terminoPolinomio t2 = new terminoPolinomio(7, 2);
        //terminoPolinomio t3 = new terminoPolinomio(3, 2);
        //terminoPolinomio t4 = new terminoPolinomio(22, 8);
        //------------------------------------------------------------------------------------------- 
        //Constructores 

        //Constructor sin argumentos; 
        public calculadoraPolinomios(string formula)
        {
            try
            { 
                formula = new ValidadorCadenas().quitarEspaciosEncadena(formula);
                formula = realizarPotencias(formula);
                formula = reemplazarRestas(formula);
                formula = realizarMultiplicacionesYDivisiones(formula);
                //------------------------------
                //se realizan las sumas y restas
                formula = ValidadorCadenas.quitarParentesis(formula);
                formula = new formulaSencilla().simplificarTodosLosSignosRepetidos(formula);
                bool swRes = separarTerminos(formula, out ts2);
                polResult = Polinomio.normalizar(ts2); //se hacen las últimas sumas y restas de términos
                swcalculoPolinomiosBienEscrito = true;
                //borrar
                formula = polResult.ACadena(false);

            }
            catch
            {
                swcalculoPolinomiosBienEscrito = false; 
            }
        }

        bool separarTerminos(String formula, out List<terminoPolinomio> terminosSeparados)
        {
            terminosSeparados = new List<terminoPolinomio>();

            try
            {
                string coeficiente = "";
                string potencia = "";

                string termino = "";
                int lugarsigno;

                while (formula != "")
                {
                    lugarsigno = -1;

                    if (formula.Length > 0)
                    {
                        for (int i = 1; i < formula.Length; i++)
                        {
                            if (formula[i] == '+' || formula[i] == '-')
                            {
                                lugarsigno = i;
                                break;
                            }
                        }
                    }

                    if (lugarsigno > -1) //si hay signo
                    {
                        if (lugarsigno == 0) //pero no es el primero                        
                            lugarsigno++;

                        termino = formula.Substring(0, lugarsigno);
                    }
                    else
                        termino = formula;

                    formula = formula.Substring(termino.Length, formula.Length - termino.Length);

                    int lugarX = termino.IndexOf('x');

                    if (lugarX == -1) //si no tiene x es término independiente
                    {
                        coeficiente = termino;
                        potencia = "0";
                    }
                    else //si no es término independiente
                    {
                        //se busca el coeficiente
                        coeficiente = termino.Substring(0, lugarX);
                        if (coeficiente == "" || coeficiente == "+" || coeficiente == "-") 
                            coeficiente = "1";

                        //se busca la potencia
                        int lugarPotencia = termino.IndexOf('^');
                        if (lugarPotencia == -1)
                            potencia = "1";
                        else
                        {
                            potencia = termino.Substring(lugarPotencia + 1, termino.Length - lugarPotencia - 1);
                        }
                    }
                    terminosSeparados.Add(new terminoPolinomio(Convert.ToDouble(coeficiente), Convert.ToInt32(potencia)));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        string reemplazarRestas(string formula)
        {
            int lugarResta = formula.LastIndexOf("-(");
            if (lugarResta == -1)
            {
                return formula;
            }
            else
            {
                
                int lugarCierraParéntesis = -1;
                string formulaARestar = "";
                //string potencia = "";

                if (lugarResta > -1) //si hay un menos frente a un paréntesis
                {
                    for (int i = lugarResta; i < formula.Length; i++)//se recorre hacia adelante la cadena desde el menos hasta encontrar el cierra paréntesis
                    {
                        if (formula[i] == ')')
                        {
                            lugarCierraParéntesis = i;
                            break;
                        }
                    }
                    if (lugarCierraParéntesis == -1)
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }
                    else
                        lugarCierraParéntesis++;

                    formulaARestar = formula.Substring(lugarResta+2, lugarCierraParéntesis - lugarResta-3);

                    formula = formula.Substring(0, lugarResta) + "+(-1*(" + formulaARestar + "))" + formula.Substring(lugarCierraParéntesis, formula.Length-lugarCierraParéntesis);
                    
                    //formula = formula.Replace("-(", "+(-1*(");
                }

                if (formula.Contains("-("))
                    return reemplazarRestas(formula);

                return formula;
            }
        }

        string realizarMultiplicaciones (string formula)
        {
            //--------------------------------------------
            //buscar la multiplicación
            //--------------------------------------------
            int lugarMultiplicacion = formula.LastIndexOf("*");
            
            if (lugarMultiplicacion == -1)
            {
                return formula;
            }
            else
            {
                bool swfactor1Polinomio = false;
                bool swfactor2Polinomio = false;
                int lugarCierraParéntesis = -1;
                int lugarAbreParéntesis = -1;
                string factor1 = "";
                string factor2 = "";
                int lugarComienzoMultiplicación = 0;
                int lugarFinMultiplicación = 0;
                int cantidadParentesisAnidados = 0;
                int aux=0;
                string cadenaInicioFormula;// = formula.Substring(0, lugarComienzoMultiplicación);
                string cadenaFinFormula;// = formula.Substring(lugarFinMultiplicación, formula.Length - lugarFinMultiplicación - aux);


                if (lugarMultiplicacion == 0 || lugarMultiplicacion == formula.Length-1) //si hay un * al inicio o al final la formula está mal escrita
                {
                    swcalculoPolinomiosBienEscrito = false;
                    return "";
                }

                //--------------------------------------------
                //buscar los factores, ver si alguno es escalar
                //---------------------------------------------

                //--------> corregir: que sepa que es polinomio aunque sea monomio sin paréntesis x^4*
                if (formula.Substring(lugarMultiplicacion-1, 1) == ")") //chequeamos si el factor 1 es escalar o polinomio
                    swfactor1Polinomio = true;

                if (formula.Substring(lugarMultiplicacion + 1, 1) == "(" || formula.Substring(lugarMultiplicacion + 1, 1) == "+" || formula.Substring(lugarMultiplicacion + 1, 1) == "-") //chequeamos si el factor 2 es escalar o polinomio
                {
                    if (formula.Substring(lugarMultiplicacion + 1, 1) == "(")
                    {
                        swfactor2Polinomio = true;
                        aux = 0; //para poder sacar el signo que sobra frente al paréntesis en buscar Factor2
                    }
                    else
                    {
                        if (formula.Substring(lugarMultiplicacion + 2, 1) == "(")
                        {
                            swfactor2Polinomio = true;
                            aux = 1; //para poder sacar el signo que sobra frente al paréntesis en buscar Factor2
                        }
                    }
                }

                //se busca el factor1
                if (swfactor1Polinomio)
                {
                    for (int i = lugarMultiplicacion-2; i >= 0; i--)//se recorre hacia atrás la cadena desde el menos hasta encontrar el abre paréntesis
                    {
                        if (formula[i] == ')')
                            cantidadParentesisAnidados++;

                        if (formula[i] == '(')
                        {
                            if (cantidadParentesisAnidados == 0)
                            {
                                lugarAbreParéntesis = i;
                                break;
                            }
                            else
                                cantidadParentesisAnidados--;
                        }
                    }

                    if (lugarAbreParéntesis == -1)
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había cierra paréntesis, pero no abre paréntesis
                        return "";
                    }
                    //else
                    //    lugarAbreParéntesis++;

                    factor1 = formula.Substring(lugarAbreParéntesis + 1, lugarMultiplicacion - lugarAbreParéntesis - 2);
                    //lugarComienzoMultiplicación = lugarAbreParéntesis;
                    cadenaInicioFormula = formula.Substring(0, lugarAbreParéntesis);
                }
                else //si el factor1 es un escalar
                {
                    bool swPasóSigno = false;

                    for (int i = lugarMultiplicacion - 1; i >= 0; i--)//se recorre hacia atrás la cadena 
                    {
                        if (swPasóSigno)
                            break;

                        if (char.IsDigit(formula[i]) || !swPasóSigno)
                        {
                            if (formula[i] == '+' || formula[i] == '-' || char.IsDigit(formula[i]))
                            {
                                factor1 = formula[i] + factor1;
                                lugarComienzoMultiplicación = i;
                            }

                            if (!char.IsDigit(formula[i]))
                            {
                                if (formula[i] != '^' && formula[i] != 'x') //se chequea que el signo no sea en realidad los signos de potencia o la x
                                    swPasóSigno = true;
                                else
                                {
                                    factor1 = formula[i] + factor1;
                                    swfactor1Polinomio = true; //si parece escalar pero en realidad es monomio
                                }
                            }
                        }
                        else
                        {
                            if (i < lugarMultiplicacion - 1)
                                swPasóSigno = true;
                        }
                    }

                    if (factor1 == "")
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }

                    cadenaInicioFormula = formula.Substring(0, lugarComienzoMultiplicación);
                }


                //se busca el factor2
                if (swfactor2Polinomio) //si el factor 2 es polinomio
                {
                    cantidadParentesisAnidados = 0;
                    for (int i = lugarMultiplicacion + 2 + aux; i < formula.Length; i++)//se recorre hacia adelante la cadena desde el menos hasta encontrar el cierra paréntesis
                    {
                        if (formula[i] == '(')
                            cantidadParentesisAnidados++;

                        if (formula[i] == ')')
                        {
                            if (cantidadParentesisAnidados == 0)
                            {
                                lugarCierraParéntesis = i;
                                break;
                            }
                            else
                                cantidadParentesisAnidados--;
                        }
                    }

                    if (lugarCierraParéntesis == -1)
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }
                    else
                        lugarCierraParéntesis++;

                    factor2 = formula.Substring(lugarMultiplicacion +2 + aux, lugarCierraParéntesis - lugarMultiplicacion-3 - aux);
                    //lugarFinMultiplicación = lugarCierraParéntesis;
                    cadenaFinFormula = formula.Substring(lugarCierraParéntesis, formula.Length - lugarCierraParéntesis);

                }
                else //si el factor2 es un escalar
                {
                    bool swPasódigito = false;

                    for (int i = lugarMultiplicacion+1; i < formula.Length; i++)//se recorre hacia adelante la cadena
                    {
                        if (swPasódigito)
                            break;

                        if (char.IsDigit(formula[i]) || i == lugarMultiplicacion + 1)
                        {
                            if (char.IsDigit(formula[i]) || formula[i] == '+' || formula[i] == '-')
                            {
                                factor2 += formula[i];
                                lugarFinMultiplicación = i;
                                lugarFinMultiplicación++;
                            }
                        }
                        else
                        {
                            if (i > lugarMultiplicacion + 1)
                             //   swPasódigito = true;
                            {
                                if (formula[i] != '^' && formula[i] != 'x') //se chequea que el signo no sea en realidad los signos de potencia o la x
                                    swPasódigito = true;
                                else
                                {
                                    factor2 += formula[i];
                                    swfactor2Polinomio = true; //si parece escalar pero en realidad es monomio
                                }
                            }
                        }
                    }

                    if (factor2 == "")
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }
                    cadenaFinFormula = formula.Substring(lugarFinMultiplicación, formula.Length - lugarFinMultiplicación);
                }


                //string cadenaInicioFormula = formula.Substring(0, lugarComienzoMultiplicación);
                //string cadenaFinFormula = formula.Substring(lugarFinMultiplicación, formula.Length - lugarFinMultiplicación - aux);

                //-------------------------------------------
                //si algún factor es otra multiplicación --------> iterar
                //-------------------------------------------
                if (factor1.Contains('*'))
                    factor1 = realizarMultiplicaciones(factor1);

                if (factor2.Contains('*'))
                    factor2 = realizarMultiplicaciones(factor2);

                //--------------------------------------------
                //multiplicar los factores
                //--------------------------------------------
                List<terminoPolinomio> listaFactor1 = new List<terminoPolinomio>();
                bool swSepararFactor1 = separarTerminos(factor1, out listaFactor1);

                if (!swSepararFactor1)
                {
                    swcalculoPolinomiosBienEscrito = false;
                    return "";
                }

                List<terminoPolinomio> listaFactor2 = new List<terminoPolinomio>();
                bool swSepararFactor2 = separarTerminos(factor2, out listaFactor2);

                if (!swSepararFactor2)
                {
                    swcalculoPolinomiosBienEscrito = false;
                    return "";
                }


                Polinomio polfactor1 = Polinomio.normalizar(listaFactor1);
                Polinomio polfactor2 = Polinomio.normalizar(listaFactor2);
                if (swfactor1Polinomio && swfactor2Polinomio)
                    polResult = polfactor1 * polfactor2;

                if (!swfactor1Polinomio)
                    polResult = Int32.Parse(factor1) * polfactor2;

                if (!swfactor2Polinomio)
                    polResult = polfactor1 * Int32.Parse(factor2);


                //--------------------------------------------
                //reemplazar en formula el resultado
                //--------------------------------------------
                formula = cadenaInicioFormula + polResult.ACadena(false) + cadenaFinFormula;

                //--------------------------------------------
                //chequear otras multiplicaciones
                //--------------------------------------------
                if (formula.Contains('*'))
                    formula = realizarMultiplicaciones(formula);

                //--------------------------------------------
                //devolver formula sin multiplicaciones
                //--------------------------------------------
                return formula;
            }
        }


        string realizarMultiplicacionesYDivisiones(string formula)
        {
            //--------------------------------------------
            //buscar la primer multiplicación o división
            //--------------------------------------------
            char[] signos = new char[2] { '*', '/' };
            int lugarOperación = formula.IndexOfAny(signos);//-1;
            
            //for (int i = 0; i< formula.Length; i++)
            //{
            //    if (formula[i] == '*' || formula[i] == '/')
            //        lugarOperación = i;
            //}

            if (lugarOperación == -1)
            {
                return formula;
            }
            else
            {
                bool swfactor1Polinomio = false;
                bool swfactor2Polinomio = false;
                int lugarCierraParéntesis = -1;
                int lugarAbreParéntesis = -1;
                string factor1 = "";
                string factor2 = "";
                int lugarComienzoMultiplicación = 0;
                int lugarFinMultiplicación = 0;
                int cantidadParentesisAnidados = 0;
                int aux = 0;
                string cadenaInicioFormula;// = formula.Substring(0, lugarComienzoMultiplicación);
                string cadenaFinFormula;// = formula.Substring(lugarFinMultiplicación, formula.Length - lugarFinMultiplicación - aux);
                bool swOperaciónMultiplicación = false; //para saber si multiplicar o dividir


                if (lugarOperación == 0 || lugarOperación == formula.Length - 1) //si hay un * al inicio o al final la formula está mal escrita
                {
                    swcalculoPolinomiosBienEscrito = false;
                    return "";
                }

                //--------------------------------------------
                //saber si es multiplicación o división
                //--------------------------------------------
                if (formula[lugarOperación] == '*')
                    swOperaciónMultiplicación = true; //por defecto división, si entra acá es multiplicación

                //--------------------------------------------
                //buscar los factores, ver si alguno es escalar
                //---------------------------------------------

                //--------> corregir: que sepa que es polinomio aunque sea monomio sin paréntesis x^4*
                if (formula.Substring(lugarOperación - 1, 1) == ")") //chequeamos si el factor 1 es escalar o polinomio
                    swfactor1Polinomio = true;

                if (formula.Substring(lugarOperación + 1, 1) == "(" || formula.Substring(lugarOperación + 1, 1) == "+" || formula.Substring(lugarOperación + 1, 1) == "-") //chequeamos si el factor 2 es escalar o polinomio
                {
                    if (formula.Substring(lugarOperación + 1, 1) == "(")
                    {
                        swfactor2Polinomio = true;
                        aux = 0; //para poder sacar el signo que sobra frente al paréntesis en buscar Factor2
                    }
                    else
                    {
                        if (formula.Substring(lugarOperación + 2, 1) == "(")
                        {
                            swfactor2Polinomio = true;
                            aux = 1; //para poder sacar el signo que sobra frente al paréntesis en buscar Factor2
                        }
                    }
                }

                //se busca el factor1
                if (swfactor1Polinomio)
                {
                    for (int i = lugarOperación - 2; i >= 0; i--)//se recorre hacia atrás la cadena desde el menos hasta encontrar el abre paréntesis
                    {
                        if (formula[i] == ')')
                            cantidadParentesisAnidados++;

                        if (formula[i] == '(')
                        {
                            if (cantidadParentesisAnidados == 0)
                            {
                                lugarAbreParéntesis = i;
                                break;
                            }
                            else
                                cantidadParentesisAnidados--;
                        }
                    }

                    if (lugarAbreParéntesis == -1)
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había cierra paréntesis, pero no abre paréntesis
                        return "";
                    }
                    //else
                    //    lugarAbreParéntesis++;

                    factor1 = formula.Substring(lugarAbreParéntesis + 1, lugarOperación - lugarAbreParéntesis - 2);
                    //lugarComienzoMultiplicación = lugarAbreParéntesis;
                    cadenaInicioFormula = formula.Substring(0, lugarAbreParéntesis);
                }
                else //si el factor1 es un escalar
                {
                    bool swPasóSigno = false;

                    for (int i = lugarOperación - 1; i >= 0; i--)//se recorre hacia atrás la cadena 
                    {
                        if (swPasóSigno)
                            break;

                        if (char.IsDigit(formula[i]) || !swPasóSigno)
                        {
                            if (formula[i] == '+' || formula[i] == '-' || char.IsDigit(formula[i]))
                            {
                                factor1 = formula[i] + factor1;
                                lugarComienzoMultiplicación = i;
                            }

                            if (!char.IsDigit(formula[i]))
                            {
                                if (formula[i] != '^' && formula[i] != 'x') //se chequea que el signo no sea en realidad los signos de potencia o la x
                                    swPasóSigno = true;
                                else
                                {
                                    factor1 = formula[i] + factor1;
                                    swfactor1Polinomio = true; //si parece escalar pero en realidad es monomio
                                }
                            }
                        }
                        else
                        {
                            if (i < lugarOperación - 1)
                                swPasóSigno = true;
                        }
                    }

                    if (factor1 == "")
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }

                    cadenaInicioFormula = formula.Substring(0, lugarComienzoMultiplicación);
                }


                //se busca el factor2
                if (swfactor2Polinomio) //si el factor 2 es polinomio
                {
                    cantidadParentesisAnidados = 0;
                    for (int i = lugarOperación + 2 + aux; i < formula.Length; i++)//se recorre hacia adelante la cadena desde el menos hasta encontrar el cierra paréntesis
                    {
                        if (formula[i] == '(')
                            cantidadParentesisAnidados++;

                        if (formula[i] == ')')
                        {
                            if (cantidadParentesisAnidados == 0)
                            {
                                lugarCierraParéntesis = i;
                                break;
                            }
                            else
                                cantidadParentesisAnidados--;
                        }
                    }

                    if (lugarCierraParéntesis == -1)
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }
                    else
                        lugarCierraParéntesis++;

                    factor2 = formula.Substring(lugarOperación + 2 + aux, lugarCierraParéntesis - lugarOperación - 3 - aux);
                    //lugarFinMultiplicación = lugarCierraParéntesis;
                    cadenaFinFormula = formula.Substring(lugarCierraParéntesis, formula.Length - lugarCierraParéntesis);

                }
                else //si el factor2 es un escalar
                {
                    bool swPasódigito = false;

                    for (int i = lugarOperación + 1; i < formula.Length; i++)//se recorre hacia adelante la cadena
                    {
                        if (swPasódigito)
                            break;

                        if (char.IsDigit(formula[i]) || i == lugarOperación + 1)
                        {
                            if (char.IsDigit(formula[i]) || formula[i] == '+' || formula[i] == '-')
                            {
                                factor2 += formula[i];
                                lugarFinMultiplicación = i;
                                lugarFinMultiplicación++;
                            }
                        }
                        else
                        {
                            if (i > lugarOperación + 1)
                            //   swPasódigito = true;
                            {
                                if (formula[i] != '^' && formula[i] != 'x') //se chequea que el signo no sea en realidad los signos de potencia o la x
                                    swPasódigito = true;
                                else
                                {
                                    factor2 += formula[i];
                                    swfactor2Polinomio = true; //si parece escalar pero en realidad es monomio
                                }
                            }
                        }
                    }

                    if (factor2 == "")
                    {
                        swcalculoPolinomiosBienEscrito = false; //; //si había abre paréntesis, pero no cierra paréntesis
                        return "";
                    }
                    cadenaFinFormula = formula.Substring(lugarFinMultiplicación, formula.Length - lugarFinMultiplicación);
                }

                //-------------------------------------------
                //si algún factor es otra multiplicación o división--------> iterar
                //-------------------------------------------
                if (factor1.Contains('*') || factor1.Contains('/'))
                    factor1 = realizarMultiplicacionesYDivisiones(factor1);

                if (factor2.Contains('*') || factor2.Contains('/'))
                    factor2 = realizarMultiplicacionesYDivisiones(factor2);

                //--------------------------------------------
                //multiplicar o dividir los factores
                //--------------------------------------------
                List<terminoPolinomio> listaFactor1 = new List<terminoPolinomio>();
                bool swSepararFactor1 = separarTerminos(factor1, out listaFactor1);

                if (!swSepararFactor1)
                {
                    swcalculoPolinomiosBienEscrito = false;
                    return "";
                }

                List<terminoPolinomio> listaFactor2 = new List<terminoPolinomio>();
                bool swSepararFactor2 = separarTerminos(factor2, out listaFactor2);

                if (!swSepararFactor2)
                {
                    swcalculoPolinomiosBienEscrito = false;
                    return "";
                }


                Polinomio polfactor1 = Polinomio.normalizar(listaFactor1);
                Polinomio polfactor2 = Polinomio.normalizar(listaFactor2);
                bool swDivisiónSinErrores = false;

                if (swfactor1Polinomio && swfactor2Polinomio)//polinomio con polinomio
                {
                    if (swOperaciónMultiplicación)
                        polResult = polfactor1 * polfactor2;
                    else
                        swDivisiónSinErrores = Polinomio.extendedSyntheticDivision(polfactor1, polfactor2, out polResult, out polResto);

                    if (!swDivisiónSinErrores)
                    {
                        swcalculoPolinomiosBienEscrito = false;
                        return "";
                    }    
                }

                if (!swfactor1Polinomio) //escalar con polinomio
                {
                    if (swOperaciónMultiplicación)
                        polResult = Int32.Parse(factor1) * polfactor2;
                    else
                    {
                        swcalculoPolinomiosBienEscrito = false;
                        return "";
                    }
                }

                if (!swfactor2Polinomio) //polinomio con escalar
                {
                    if (swOperaciónMultiplicación)
                        polResult = polfactor1 * Int32.Parse(factor2);
                    else
                    {
                        swDivisiónSinErrores = Polinomio.dividir(polfactor1, Int32.Parse(factor2), out polResult, out polResto);

                        if (!swDivisiónSinErrores)
                        {
                            swcalculoPolinomiosBienEscrito = false;
                            return "";
                        }
                    }
                }
                //--------------------------------------------
                //reemplazar en formula el resultado
                //--------------------------------------------
                formula = cadenaInicioFormula + polResult.ACadena(false) + cadenaFinFormula;

                //--------------------------------------------
                //chequear otras multiplicaciones
                //--------------------------------------------
                if (formula.Contains('*'))
                    formula = realizarMultiplicacionesYDivisiones(formula);

                //--------------------------------------------
                //devolver formula sin multiplicaciones
                //--------------------------------------------
                return formula;
            }
        }


        //por ahora no se hacen potencias, complejo lo de cuadrado de binomio, cubo binomio
        string realizarPotencias(string formula)
        {
            //separar paréntesis
            int lugarPotencia = formula.IndexOf(")^");
            if (lugarPotencia > -1)
            {
                swcalculoPolinomiosBienEscrito = false;
                return "";
            }
            else
                return formula;


            ////int lugarAbreParéntesis = -1;
            ////string formulaAPotenciar = "";
            ////string potencia="";

            ////if (lugarPotencia > -1) //si hay un cierra paréntesis elevado
            ////{
            ////    for (int i = lugarPotencia; i >= 0; i--)//se recorre hacia atrás la cadena desde el cierra paréntesis hasta encontrar el abre paréntesis
            ////    {
            ////        if (formula[i] == '(')
            ////        {
            ////            lugarAbreParéntesis = i;
            ////            break;
            ////        }
            ////    }
            ////    if (lugarAbreParéntesis == -1)
            ////    {
            ////        swcalculoPolinomiosBienEscrito = false; //; //si había cierra paréntesis, pero no habre paréntesis
            ////        return "";
            ////    }
            ////    else
            ////        lugarAbreParéntesis++;

            ////    formulaAPotenciar = formula.Substring(lugarAbreParéntesis, lugarPotencia - lugarAbreParéntesis);
                
            ////    for (int i=lugarPotencia; i<formula.Length; i++)
            ////    {
            ////        if (formula[i] == '+' || formula[i] == '-' || char.IsDigit(formula[i]))
            ////            potencia += formula[i];
            ////        else
            ////            break;
            ////    }

            ////    int tmp=0;
            ////    if (!Int32.TryParse(potencia, out tmp))
            ////    {
            ////        swcalculoPolinomiosBienEscrito = false;
            ////        return "";
            ////    }


            ////    //chequear si hay potencias --> recursividad
            ////    if (formulaAPotenciar.Contains(")^"))
            ////        return realizarPotencias(formulaAPotenciar);


            ////    //resolver potencia
            ////    bool swSeparar = separarTerminos(formulaAPotenciar, out ts);

            ////    if (swSeparar)
            ////    {
            ///         if (ts.Count() > 2) //chequea si es mayor a binomio
            ///         
            ///         if (tmp > 2) //chequea si no es binomio
            ///         
            ////        Polinomio polRes = Polinomio.normalizar(ts);
            ////        polRes = Polinomio.potencia(tmp);
            ////    }
            ////    else
            ////    {
            ////        swcalculoPolinomiosBienEscrito = false;
            ////        return "";
            ////    }

            ////    //reemplazar en formula la potencia resuelta


            ////    //devolver la nueva formula
            ////}
            ////else //si no hay potencia
            ////{
            ////    return formula;
            ////}
        }
    }
}
