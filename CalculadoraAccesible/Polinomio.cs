using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraAccesible
{
    //-----------------------------------------------
    //  CÓDIGO TOMAD DESDE X Geek
    //  http://xgeeker.blogspot.com/2012/11/ejemplo-de-una-calculadora-de.html
    //-----------------------------------------------

    public class Polinomio
    {
        //variables públicas 
        public int grado;
        public double[] coeficiente;

        //------------------------------------------------------------------------------------------- 
        //Constructores 

        //Constructor sin argumentos; 
        public Polinomio()
        {
            double[] gpdefec = new double[0];//gpdefec variable que guarda el grado del polinomio  por defecto  
            grado = 0;
            coeficiente = gpdefec;
        }

        //Constructor que especifica el grado del polinomio 
        public Polinomio(int numgrado)
        {
            grado = numgrado;
            coeficiente = new double[grado + 1];

        }

        //Constructor de la clase Polinomio que permite especificar los coeficientes 
        public Polinomio(double[] nuevocoefs)
        {
            coeficiente = nuevocoefs;
            grado = nuevocoefs.Length - 1;


            for (int i = nuevocoefs.Length - 1; i > 0; i--)
            {
                if (nuevocoefs[i] == 0)
                {
                    grado--;
                }
                else
                {
                    break;
                }
            }

        }



        //---------------------------------------------------------------------------------------------- 
        //Operadores 

        //Operador que permite multiplicar polinomios 
        public static Polinomio operator *(Polinomio pol1, Polinomio pol2)
        {
            double[] a = new double[(pol1.grado + pol2.grado) + 1];
            for (int i = 0; i <= pol1.grado; i++)
            {
                for (int j = 0; j <= pol2.grado; j++)
                {
                    a[i + j] += pol1.coeficiente[i] * pol2.coeficiente[j];
                }
            }
            Polinomio nuevo = new Polinomio(a);
            return nuevo;
        }


        //Operador que permite multiplicar un escalar por un polinomio 
        public static Polinomio operator *(double escalar, Polinomio poli)
        {
            double[] temp = new double[poli.coeficiente.Length];
            for (int i = 0; i < poli.coeficiente.Length; i++)
            {
                temp[i] = escalar * poli.coeficiente[i];
            }
            return new Polinomio(temp);
        }


        //Operador que permite multiplicar un polinomio por un escalar 
        public static Polinomio operator *(Polinomio poli, double escalar)
        {
            return escalar * poli;
        }

        //Operador que permite sumar un polinomio y un escalar 
        public static Polinomio operator +(double escalar, Polinomio poli)
        {
            double[] temp = new double[1];
            temp[0] = escalar;
            return new Polinomio(temp) + poli;
        }

        //Operador que permite sumar un escalar y un polinomio 
        public static Polinomio operator +(Polinomio poli, double escalar)
        {
            return escalar + poli;
        }


        //Operador que permite sumar polinomios 
        public static Polinomio operator +(Polinomio p1, Polinomio p2)
        {
            Polinomio s;
            Polinomio t = new Polinomio(p1.grado);
            Polinomio q = new Polinomio(p2.grado);
            if (p1.grado >= p2.grado)
            {

                for (int i = 0; i <= p1.grado; i++)
                {
                    if (p2.grado == 0)
                    {
                        double[] arr = new double[p1.coeficiente.Length];

                        for (i = 0; i < p1.coeficiente.Length; i++)
                        {
                            arr[i] = p1.coeficiente[i];
                        }
                        arr[0] += p2.coeficiente[0];

                        t.coeficiente = arr;
                    }
                    else

                    if (i < p2.coeficiente.Length)//= p2.coeficiente.Length)
                    {
                        t.coeficiente[i] = (p1.coeficiente[i] + p2.coeficiente[i]);

                    }
                    else
                    {
                    t.coeficiente[i] = p1.coeficiente[i];
                    }

                }
                s = new Polinomio(t.coeficiente);
            }
            else
            {

                for (int i = 0; i <= p2.grado; i++)
                {
                    //////////////// 
                    if (p1.grado == 0)
                    {
                        double[] arr = new double[p2.coeficiente.Length];

                        for (i = 0; i < p2.coeficiente.Length; i++)
                        {
                            arr[i] = p2.coeficiente[i];
                        }
                        arr[0] += p1.coeficiente[0];
                        q.coeficiente = arr;
                    }
                    else
                    //////////////// 
                    if (i < p1.coeficiente.Length)
                    {
                        q.coeficiente[i] = (p2.coeficiente[i] + p1.coeficiente[i]);
                    }
                    else
                    {
                        q.coeficiente[i] = p2.coeficiente[i];
                    }
            }

                s = new Polinomio(q.coeficiente);
            }
            return s;
        }


        //Operador que permite restar dos polinomios 
        public static Polinomio operator -(Polinomio pol1, Polinomio pol2)
        {
            Polinomio temp = new Polinomio(pol2.grado);
            for (int i = 0; i <= pol2.grado; i++)
            {
                temp.coeficiente[i] = pol2.coeficiente[i] * -1;
            }
            return pol1 + temp;
        }

        //Operador que permite restar un escalar y un polinomio 
        public static Polinomio operator -(double escalar, Polinomio poli)
        {
            double[] temp = new double[poli.coeficiente.Length];
            temp[0] = escalar;
            Polinomio aux1 = new Polinomio(temp);
            return aux1 - poli;
        }

        //Operador que permite restar un polinomio  y un escalar 
        public static Polinomio operator -(Polinomio poli, double escalar)
        {
            double[] temp = new double[poli.coeficiente.Length];
            temp[0] = escalar;
            Polinomio aux1 = new Polinomio(temp);

            return poli - aux1;
        }



        //----------------------------------------------------------------------------------------------       
        //Validaciones para escribirlos en la consola  
        public string ACadena(bool swMostrarTerminosNulos)
        {
            string polinomio = "";
            if (this.grado > 0)
            {
                for (int i = this.grado; i >= 0; i--)
                {
                    string grado = i.ToString();
                    string signo = "+";
                    string x = "x^";
                    string coeficiente = this.coeficiente[i].ToString();

                    ////Validacion para 0x^n 
                    if (!swMostrarTerminosNulos)
                    {
                        if (this.coeficiente[i] == 0)
                        {
                            signo = "";
                            x = "";
                            grado = "";
                            coeficiente = "";
                        }
                    }


                    //Validacion para 1x 
                    if (this.coeficiente[i] == 1)
                    {
                        coeficiente = "";
                    }

                    //Validacion para el -x; 
                    if (this.coeficiente[i] == -1)
                    {
                        //coeficiente = "";  
                        x = "x^";
                    }


                    //Validacion para el +-x 
                    if (this.coeficiente[i] < 0)
                    {
                        signo = "";
                    }


                    //Validacion para x^2-1 
                    if (i == this.grado)
                    {
                        signo = "";
                    }

                    //Validacion para  x^1 
                    if ((i == 1) && (this.coeficiente[i] != 0))
                    {
                        x = "x";
                        grado = "";
                    }
                    //Validacion para los terminos independientes 
                    if (i == 0)
                    {
                        grado = "";
                        x = "";
                    }

                    if ((i == 0) && (this.coeficiente[i] == 1))
                    {
                        grado = "";
                        x = "1";
                    }


                    polinomio += signo + coeficiente + x + grado;
                }
                if (polinomio == "")
                {
                    return "0";
                }
                else
                {

                    return polinomio;
                }
            }
            else
                return "0";
        }

        //-----------------------------------------------------------
        //-----------------------------------------------------------
        //-----------------------------------------------------------
        //-----------------------------------------------------------
        //-----------------------------------------------------------


        ////////MODELO MATEMÁTICO DE RUFFINI
        ////////NOMBRE:  LUIS
        ////////Operador que permite dividir dos polinomios por método de Horner
        //////public static Polinomio operator /(Polinomio p1, Polinomio p2)
        //////{
        //////    Polinomio s;
        //////    Polinomio t = new Polinomio(p1.grado);
        //////    Polinomio q = new Polinomio(p2.grado);

        //////    int i;
        //////    int grado = p1.grado;

        //////    float x;
        //////    float[] residuo;
        //////    float[] coeficiente;
        //////    Console.WriteLine("ALGORITMO DE DIVISIÓN SINTÉTICA");
        //////    Console.WriteLine();

        //////    Console.WriteLine("INGRESE EL GRADO DE LA ECUACIÓN");
        //////    grado = int.Parse(Console.ReadLine());
        //////    coeficiente = new float[grado + 1];
        //////    residuo = new float[grado + 1];
        //////    for (i = 0; i <= grado; i++)
        //////    {
        //////        Console.WriteLine("INGRESE EL COEFICIENTE->{0}  ", i);
        //////        coeficiente = float.Parse(Console.ReadLine());
        //////    }
        //////    Console.WriteLine();
        //////    Console.WriteLine();
        //////    Console.WriteLine("INGRESE EL VALOR DE X-> ");
        //////    x = float.Parse(Console.ReadLine());
        //////    residuo[0] = coeficiente[0];

        //////    Console.WriteLine();
        //////    Console.WriteLine();

        //////    //Procesando datos
        //////    for (i = 1; i <= grado; i++)
        //////    {
        //////        residuo = (residuo[i - 1] * x) + coeficiente;
        //////    }

        //////    Console.WriteLine("ESCRIBIR RESIDUO  {0}", residuo[grado]);
        //////    Console.ReadLine();

        //    MODELO MATEMÁTICO DE HORNER

        //MODELO MATEMÁTICO DE HORNER
        //NOMBRE:
        public static bool dividir(Polinomio p1, Polinomio p2, out Polinomio pResult, out Polinomio pResto)
        {
            //////Polinomio s;
            //////Polinomio t = new Polinomio(p1.grado);
            //////Polinomio q = new Polinomio(p2.grado);

            //////int i;
            //////int grado;

            //////float x;
            //////float h;
            //////float[] coeficiente;
            //////Console.WriteLine("MODELO MATEMÁTICO DE HORNER");
            //////Console.WriteLine();
            //////Console.WriteLine("ALGORITMO DE HORNER");
            //////Console.WriteLine();
            //////Console.WriteLine("INGRESE EL GRADO DE LA ECUACIÓN");
            //////grado = int.Parse(Console.ReadLine());
            //////coeficiente = new float[grado + 1];
            //////Console.WriteLine();
            //////for (i = 0; i <= grado; i++)
            //////{
            //////    Console.WriteLine("INGRESE EL COEFICIENTE-> {0}  ", i);
            //////    coeficiente = float.Parse(Console.ReadLine());
            //////}
            //////Console.WriteLine();
            //////Console.WriteLine();
            //////Console.WriteLine("INGRESE EL VALOR DE X-> ");
            //////x = float.Parse(Console.ReadLine());
            //////h = coeficiente[0];

            //////Console.WriteLine();
            //////Console.WriteLine();

            ////////Procesando datos
            //////for (i = 1; i <= grado; i++)
            //////{
            //////    h = (h * x) + coeficiente[i];
            //////    Console.WriteLine("ESCRIBIR RESIDUO  {0}", h);
            //////    Console.ReadLine();
            //////}
            //////Console.WriteLine();
            //////Console.WriteLine("RESULTADO ES->  {0}", h);
            //////Console.ReadLine();


            try
            {
                double[] coefficientsNum = p1.coeficiente;//new int[] { 10, 22, 3, 0, -7, 0, -1 };
                double[] coefficientsDem = p2.coeficiente;// new int[] { 1, -1, 3, 5, -6 };

                int degree = coefficientsNum.Length + 1;
                int degree2 = coefficientsDem.Length + 1;

                if (degree < degree2) throw new Exception(); //si el dividor es menor que el dividendo

                int currentDegree = degree;
                List<double> solutionCoefficients = new List<double>();
                List<double> tempCoefficients = new List<double>();
                tempCoefficients.AddRange(coefficientsNum);

                for (int i = 0; i <= coefficientsNum.Length - coefficientsDem.Length; i++)
                {
                    if (currentDegree >= 0)
                    {
                        double aux = tempCoefficients[i] / coefficientsDem[0];
                        if (Double.IsNaN(aux)) aux = 0;

                        solutionCoefficients.Add(aux);// tempCoefficients[i] / coefficientsDem[0]);
                        for (int e = 0; e < coefficientsDem.Length; e++)
                        {
                            tempCoefficients[i + e] = tempCoefficients[i + e] - (solutionCoefficients[i] * coefficientsDem[e]);
                        }
                        currentDegree--;
                    }
                    else
                    {
                        double aux = tempCoefficients[i] / coefficientsDem[0];
                        if (Double.IsNaN(aux)) aux = 0;

                        solutionCoefficients.Add(aux);// tempCoefficients[i] / coefficientsDem[0]);
                    }
                }

                double[] resultados = solutionCoefficients.ToArray();
                double[] restos = tempCoefficients.ToArray();
                Array.Reverse(resultados);
                Array.Reverse(restos);
                pResult = new Polinomio(resultados);// resultados);
                pResto = new Polinomio(restos);

                return true;
            }
            catch
            {
                pResult = new Polinomio();
                pResto = new Polinomio();
                return false;
            }
        }

        public static bool extendedSyntheticDivision(Polinomio p1, Polinomio p2, out Polinomio pResult, out Polinomio pResto)
        {
            //double[] coefficientsNum = p1.coeficiente;
            //double[] coefficientsDem = p2.coeficiente;

            List<double> output = p1.coeficiente.ToList();//dividend.ToList();
            double normalizer = p2.coeficiente[0];// divisor[0];

            List<double> dividend = p1.coeficiente.ToList();
            List<double> divisor = p2.coeficiente.ToList();

            for (int i = 0; i < dividend.Count() - (divisor.Count() - 1); i++)
            {
                output[i] /= normalizer;

                double coef = output[i];
                if (coef != 0)
                {
                    for (int j = 1; j < divisor.Count(); j++)
                        output[i + j] += -divisor[j] * coef;
                }
            }

            int separator = output.Count() - (divisor.Count() - 1);

            double[] resultados = output.GetRange(0, separator).ToArray();
            double[] restos = output.GetRange(separator, output.Count() - separator).ToArray();
            Array.Reverse(resultados);
            Array.Reverse(restos);

            pResult = new Polinomio(resultados);// resultados);
            pResto = new Polinomio(restos);

            return true;
            //    (
            //    output.GetRange(0, separator),
            //    output.GetRange(separator, output.Count() - separator)
            //);
        }

        public static bool dividir(Polinomio p1, int escalar, out Polinomio pResult, out Polinomio pResto)
        {
            try
            {
                double[] temp = new double[p1.coeficiente.Length];
                for (int i = 0; i < p1.coeficiente.Length; i++)
                {
                    temp[i] = Math.Round(p1.coeficiente[i] / escalar);
                }
                pResult = new Polinomio(temp);
                pResto = new Polinomio();

                return true;
            }
            catch
            {
                pResult = new Polinomio();
                pResto = new Polinomio();
                return false;
            }
        }

            public static Polinomio normalizar (List<terminoPolinomio> terminos)
        {
            try
            {
                int grado = 0;
                foreach (terminoPolinomio t in terminos)
                    if (t.potencia > grado) grado = t.potencia;

                double[] coeficientes = new double[grado+1];

                //for (int i = 0; i < grado - 1; i++)
                //    coeficientes[i] = 0;

                foreach (terminoPolinomio t in terminos)
                {
                    if (coeficientes[t.potencia] == 0) //si el lugar de la potenia está vacío, osea no hay otro término del mismo orden
                        coeficientes[t.potencia] = t.coeficiente;
                    else
                        coeficientes[t.potencia] = coeficientes[t.potencia] + t.coeficiente; //si no está vacío se suman los coeficientes de ambos términos
                }

                return new Polinomio(coeficientes);
            }
            catch
            {
                return new Polinomio();
            }
        }


        //HACER - potencia de polimonio, cubo de binomio, cuadrado de binomio, ver si hay otros tipos de potencias
        public static Polinomio potencia (int potenciaAElevar)
        {
            return new Polinomio();
        }

    }
}
