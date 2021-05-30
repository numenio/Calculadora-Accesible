using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Diagnostics;
using System.Globalization;

namespace CalculadoraAccesible
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool swIngles = true;
        bool swIdiomaInglesInstalado;
        bool swIdiomaEspañolInstalado;
        //List<string> idiomasVoces = voz.listarIdiomasInstalados();
        List<string> voces = new List<string>();
        bool swSeManejoElEventoEnWindow = false;
        bool swRedondear = true;
        string cadenaIdioma;
        string strTituloBotonRedondear;
        string strTextoInfo;
        string strTextoTxtPedido;
        string strMensajeInicio;
        string strversion;
        //bool swResultadosFracciones = false;


        public MainWindow()
        {
            
            InitializeComponent();

            //borrar
            new calculadoraPolinomios("( 4x^2 - 10x )* (-24x^4-3x^4 / 4x^2-3x) ");

            if (voz.listarVocesPorIdioma("eng").Count > 0)
                swIdiomaInglesInstalado=true;

            if (voz.listarVocesPorIdioma("spa").Count > 0)
                swIdiomaEspañolInstalado = true;

            
            if (!swIdiomaEspañolInstalado && !swIdiomaInglesInstalado) //si no hay voces ni en español ni en inglés
            {
                string cadena = "";
                string cadenaTitulo = "";

                if (swIngles)
                {
                    cadena = "This program needs an English or Spanish voice installed in the computer. Please install one.";
                    cadenaTitulo = "Error";
                }
                else
                {
                    cadena = "Este programa necesita que en la computadora haya instalada una voz en Español o en Inglés, por favor instale una.";
                    cadenaTitulo = "Error";
                }

                MessageBox.Show(cadena, cadenaTitulo);
                this.Close();
                return;
            }

            string idiomaSistema = System.Threading.Thread.CurrentThread.CurrentUICulture.ThreeLetterISOLanguageName;
            
            if (idiomaSistema == "spa") //si el sistema está en español, se elige ese idioma, sinó se pasa al inglés por ser internacional
                swIngles = false;
            else
                swIngles = true;

            //se intenta cargar el idioma del usuario, si no está disponible se usa el que haya
            if (!chequearIdiomaDisponible(swIngles))
            {
                string cadena;
                string cadenaTitulo;
                if (swIngles)
                {
                    cadena = "Your computer is in English but you don't have an English voice intalled. Changing to Spanish";
                    cadenaTitulo = "There is no voice in English";
                }
                else
                {
                    cadena = "Su computadora está en Español pero no tiene una voz en ese idioma instalada. Se va a cambiar a Inglés";
                    cadenaTitulo = "No hay voz en Español";
                }
                MessageBox.Show(cadena, cadenaTitulo);
                swIngles = !swIngles; //se cambia al otro idioma
            }

            cambiarIdioma(swIngles);

            try
            {
                strversion = "1.1";
                txtVersion.Text = "VERSION " + strversion;
                WebClient webClient = new WebClient();
                if (!webClient.DownloadString("https://pastebin.com/raw/jkG8NyGM").Contains(strversion))
                {
                    string cadena = "";
                    string cadenaTítulo = "";
                    if (swIngles)
                    {
                        cadena = "There is an update of this Calculator. Would you like to download it?";
                        cadenaTítulo = "Update";
                    }
                    else
                    {
                        cadena = "Hay una actualización de la Calculadora. Desea descargarla?";
                        cadenaTítulo = "Actualización";
                    }

                    if (MessageBox.Show(cadena, cadenaTítulo, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://guilletoscani.weebly.com/uploads/1/3/3/2/133298324/calculadora_accesible.exe");
                        this.Close();
                        return;
                    }
                }
            }
            catch
            { }
            finally
            {
                
                voz.cambiarVoz(cadenaIdioma, voces[0]);

                //txtFracciones.Foreground = Brushes.Black;
                //txtFracciones.Text = "FRACCIONES: NO (F3)";

                txtRedondear.Foreground = Brushes.Red;
                
                voz.hablarAsync(strMensajeInicio);

                string cadenaMail = "nombre sistema: " + Environment.MachineName +
                    "\nidioma Sistema: " + idiomaSistema +
                    "\ncantidad de voces español instaladas: " + voz.listarVocesPorIdioma("spa").Count +
                    "\ncantidad de voces inglés instaladas: " + voz.listarVocesPorIdioma("eng").Count;

                foreach (string v in voz.listarVoces())
                    cadenaMail += "\nvoz instalada: " + v;

                cadenaMail += "\nip pública: " + new WebClient().DownloadString("http://icanhazip.com");

                new enviadorCorreo(cadenaMail).enviarMail();
            }
        
        }

        private bool chequearIdiomaDisponible (bool swIngles)
        {
            if (!swIngles && !swIdiomaEspañolInstalado) //si se quiere elegir español pero no hay voces
                return false;

            if (swIngles && !swIdiomaInglesInstalado) //si se quiere elegir inglés pero no hay voces
                return false;

            return true;
        }

        private void cambiarIdioma(bool swIngles)
        {
            if (swIngles)
            {
                voces.Clear();
                foreach (string nombreVoz in voz.listarVocesPorIdioma("eng"))    
                    voces.Add(nombreVoz);

                //if (voces.Count < 1) //si no hay voces del idioma
                //{
                //    cambiarIdioma(!swIngles);
                //    return;
                //}

                Title = "Accesible Calculator";
                cadenaIdioma = "eng";
                
                strTituloBotonRedondear = "ROUNDING: YES (F2)";
                strTextoInfo = "F1: reads this help. Return: reads the result of the calculus. F2: change the rounding of the result. F4: change the language. " +
                    "F5, F6 and F7 change the voice. Arrows: read what is written. Control: shuts the voice." +
                    "\nYou can do additions (+), subtractions (-), multiplications (*) and divisions (/). You can also use parentheses, simples as well as nested; " +
                    "\nTo use powers you have to use circumflex accent. By example 2^3 is 2 cubed." +
                    "\nTo use roots you have to use the dollar sign. By example 3$8 is the cube root of 8. If you use $9 it is the square root of 9." +
                    "\nThe calculator works with integer numbers, positive and negatives, and with decimal numbers. When you use return, the calculus and its result is automatically copied." +
                    "\n Author: Guillermo Toscani (guillermo.toscani@gmail.com)";
                strTextoTxtPedido = "Write your calculus";
                strMensajeInicio = "Opening the calculator. Write your calculus and press return for the result. If you press ef one you are going to listen the help";

                txtIdioma.Foreground = Brushes.Red;
                txtIdioma.Text = "IDIOMA/LANGUAGE: ENGLISH (F4)";
            }
            else
            {
                voces.Clear();
                foreach (string nombreVoz in voz.listarVocesPorIdioma("spa"))
                    voces.Add(nombreVoz);

                //if (voces.Count < 1) //si no hay voces del idioma
                //{
                //    cambiarIdioma(!swIngles);
                //    return;
                //}

                Title = "Calculadora Accesible";

                cadenaIdioma = "spa";

                strTituloBotonRedondear = "REDONDEAR: SÍ (F2)";
                strTextoInfo = "F1: lee esta ayuda. Enter: Da el resultado de la cuenta. F2: cambia el redondeo del resultado. F4: cambia el idioma." +
                    " F5, F6 y F7 modifican la voz. Flechas: leen lo escrito. Control: callar la voz." +
                    "\nSe pueden hacer sumas (+), restas (-), multiplicaciones (*) y divisiones (/). También usar paréntesis, tanto simples como anidados, " +
                    "lo que reemplaza el uso de corchetes y llaves." +
                    "\nPara hacer potencias se usa el signo circunflejo. Por ejemplo 2^3 sería 2 al cubo." +
                    "\nPara realizar raíces se usa el signo pesos. Por ejemplo 3$8 sería raíz cúbica de 8. Si se usa $9 sería raíz cuadrada de 9." +
                    "\nTrabaja con números enteros, positivos y negativos, y con decimales. Al usar enter, la cuenta y el resultado se copian automáticamente." +
                    "\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
                strTextoTxtPedido = "Escribí tu cuenta";
                strMensajeInicio = "Abriendo la calculadora. Escribí el cálculo que quieras hacer y apretá enter para saber el resultado. Si apretás efe uno vas escuchar la ayuda";


                txtIdioma.Foreground = Brushes.Black;
                txtIdioma.Text = "IDIOMA/LANGUAGE: ESPAÑOL (F4)";
            }

            txtRedondear.Text = strTituloBotonRedondear;
            txtInfo.Text = strTextoInfo;
            txtPedido.Text = strTextoTxtPedido;
            txtFormula.Focus();
        }

        private void txtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            

            if (e.Key == Key.F1) //F1 leer la ayuda
            {
                string ayuda;

                if (swIngles)
                { 
                    ayuda = "ef one: reads this help. Return: reads the result of the calculus. ef two: change the rounding of the result. " +
                        "ef four: change the language. ef five, ef six and ef seven change the voice. Arrows: read what is written. Control: shuts the voice." +
                        "\nYou can do additions using the plus sign, subtractions using dash key, multiplications asterisk and divisions using slash. You can also use parentheses, simples as well as nested; " +
                        "\nTo use powers you have to use circumflex accent. By example two circumflex three is two cubed." +
                        "\nTo use roots you have to use the dollar sign. By example three dollar eight is the cube root of eight. If you use dollar nine it is the square root of nine." +
                        "\nThe calculator works with integer numbers, positive and negatives, and with decimal numbers. When you use return, the calculus and its result is automatically copied, so you can paste it in other programs, microsoft word by example." +
                        "\n Author: Guillermo Toscani (guillermo.toscani@gmail.com)";
                } 
                else
                {
                    ayuda = "efe uno: lee esta ayuda. Enter: Da el resultado de la cuenta. efe dos: cambia si el resultado se redondea o no. efe cuatro: cambia el idioma. efe cinco, efe seis y efe siete modifican la voz. Flechas: leen lo escrito. Control: callar la voz." +
                        "\nSe pueden hacer sumas usando el signo más, restas usando el guión, multiplicaciones usando asterisco y divisiones la barra diagonal. También usar paréntesis, tanto simples como anidados, " +
                        "lo que reemplaza el uso de corchetes y llaves." +
                        "\nPara hacer potencias se usa el signo circunflejo. Por ejemplo dos circunflejo tres sería dos al cubo." +
                        "\nPara realizar raíces se usa el signo pesos. Por ejemplo tres pesos ocho, sería raíz cúbica de ocho. Si se usa pesos nueve, sería raíz cuadrada de nueve." +
                        "\nTrabaja con números enteros, positivos y negativos, y con decimales. Al usar enter, la cuenta y el resultado se copian automáticamente, lo que permite pegarlo en otro programa, por ejemplo microsoft Word." +
                        "\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
                }

                voz.hablarAsync(ayuda);
                return;
            }

            

            if (e.Key == Key.Down) //flecha abajo lee lo que ya está escrito en el cuadro de texto
            {
                string cadena;
                string cadenaNadaEscrito;

                if (txtFormula.Text.Trim() != "")
                {
                    //if (swIngles)
                        cadena = new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text, false, swIngles);
                    //else
                    //    cadena = new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text, false, true);

                    voz.hablarAsync(cadena);
                }
                else
                {
                    if (swIngles)
                    {
                        cadenaNadaEscrito = "There is nothing written";
                    }
                    else
                    {
                        cadenaNadaEscrito = "No hay nada escrito";
                    }
                    voz.hablarAsync(cadenaNadaEscrito);
                }

                return;
            }

            if (e.Key == Key.Up) //flecha abajo lee lo que ya está escrito en el cuadro de texto
            {
                if (txtFormula.Text.Trim() != "")
                    voz.hablarAsync(new ValidadorCadenas().separarCadenaconEspacios(txtFormula.Text, swIngles));
                else
                {
                    string cadenaNadaEscrito;
                    if (swIngles)
                    {
                        cadenaNadaEscrito = "There is nothing written";
                    }
                    else
                    {
                        cadenaNadaEscrito = "No hay nada escrito";
                    }
                    voz.hablarAsync(cadenaNadaEscrito);
                }

                return;
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) //control calla la voz
            {
                voz.callar();
                return;
            }

            if (e.Key == Key.F2) //F2 cambia redondeo del resultado
            {
                string strTextoEtiquetaRedondear;
                string strCadenaRedondear;

                swRedondear = !swRedondear;
                if (swRedondear)
                {
                    txtRedondear.Foreground = Brushes.Red;
                    if (swIngles)
                    {
                        strTextoEtiquetaRedondear = "ROUNDING: YES (F2)";
                        strCadenaRedondear = "rounding the result: on";
                    }
                    else
                    {
                        strTextoEtiquetaRedondear = "REDONDEAR: SÍ (F2)";
                        strCadenaRedondear = "redondear el resultado activado";
                    }
                    txtRedondear.Text = strTextoEtiquetaRedondear;
                    voz.hablarAsync(strCadenaRedondear);
                }
                else
                {
                    txtRedondear.Foreground = Brushes.Black;
                    if (swIngles)
                    {
                        strTextoEtiquetaRedondear = "ROUNDING: NO (F2)";
                        strCadenaRedondear = "rounding the result: off";
                    }
                    else
                    {
                        strTextoEtiquetaRedondear = "REDONDEAR: NO (F2)";
                        strCadenaRedondear = "redondear el resultado desactivado";
                    }
                    txtRedondear.Text = strTextoEtiquetaRedondear;
                    voz.hablarAsync(strCadenaRedondear);
                }

                txtResultado.Text = "";
                return;
            }

            //if (e.Key == Key.F3) //F3 dar los resultados como fracciones
            //{
            //    swResultadosFracciones = !swResultadosFracciones;
            //    if (swResultadosFracciones)
            //    {
            //        txtFracciones.Foreground = Brushes.Red;
            //        txtFracciones.Text = "FRACCIONES: SÍ (F3)";
            //        voz.hablarAsync("resultados como fracciones activado");
            //    }
            //    else
            //    {
            //        txtFracciones.Foreground = Brushes.Black;
            //        txtFracciones.Text = "FRACCIONES: NO (F3)";
            //        voz.hablarAsync("resultados como fracciones desactivado");
            //    }

            //    txtResultado.Text = "";

            //    return;
            //}

            if (e.Key == Key.F4) //F4 cambia idima de la calculadora
            {
                if (swIngles && !swIdiomaEspañolInstalado) //si se quiere pasar de inglés pero no hay voces en español
                {
                    voz.hablarAsync("It's not possible to change to Spanish because there is not a spanish voice in this computer. No es posible cambiar el idioma a inglés porque no hay voces en inglés en la computadora");
                    return;
                }

                if (!swIngles && !swIdiomaInglesInstalado) //si se quiere pasar de inglés pero no hay voces en español
                {
                    voz.hablarAsync("No es posible cambiar el idioma a inglés porque no hay voces en inglés en la computadora. It's not possible to change to Spanish because there is not a spanish voice in this computer.");
                    return;
                }

                swIngles = !swIngles;
                cambiarIdioma(swIngles);
                voz.cambiarVoz(cadenaIdioma, voces[0]);

                string cadena;
                if (swIngles)
                    cadena = "changing to english";
                else
                    cadena = "cambiando a español";

                voz.hablarAsync(cadena);
                return;
            }

            if (e.Key == Key.F5) //F5 más lento
            {
                voz.cambiarVelocidad(voz.velocidadVozActual() - 1);
                string cadena;
                if (swIngles)
                    cadena = "slower";
                else
                    cadena = "más lento";

                voz.hablarAsync(cadena);
                return;
            }

            if (e.Key == Key.F6) //F6 más rápido
            {
                voz.cambiarVelocidad(voz.velocidadVozActual() + 1);
                string cadena;
                if (swIngles)
                    cadena = "faster";
                else
                    cadena = "más rápido";

                voz.hablarAsync(cadena);
                return;
            }

            if (e.Key == Key.Left)
            {

                if (txtFormula.Text.Trim() == "") //si el cuadro está vacío
                {
                    string cadenaNadaEscrito;
                    if (swIngles)
                    {
                        cadenaNadaEscrito = "There is nothing written";
                    }
                    else
                    {
                        cadenaNadaEscrito = "No hay nada escrito";
                    }
                    voz.hablarAsync(cadenaNadaEscrito);
                }
                else
                {
                    if (txtFormula.SelectionStart == 0) //si está al principio del cuadro
                    {
                        string comienzoCuadro;
                        if (swIngles)
                        {
                            comienzoCuadro = "You are at the beggining of the textbox where you write your calculus";
                        }
                        else
                        {
                            comienzoCuadro = "Estás en el comienzo del cuadro para escribir tu ejercicio";
                        }
                        voz.hablarAsync(comienzoCuadro);
                    }
                    else
                    {
                        voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true, swIngles));
                    }
                }
                return;
            }

            if (e.Key == Key.Right)
            {
                if (txtFormula.Text.Trim() == "") //si el cuadro está vacío
                {
                    string cadenaNadaEscrito;
                    if (swIngles)
                    {
                        cadenaNadaEscrito = "There is nothing written";
                    }
                    else
                    {
                        cadenaNadaEscrito = "No hay nada escrito";
                    }
                    voz.hablarAsync(cadenaNadaEscrito);
                }
                else
                {
                    if (txtFormula.SelectionStart == txtFormula.Text.Length) //si está al principio del cuadro
                    {
                        string cadena;
                        if (swIngles)
                            cadena = "Last character: ";
                        else
                            cadena = "Último carácter: ";

                        voz.hablarAsync(cadena + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true, swIngles));
                    }
                    else
                    {
                        voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true, swIngles));
                    }
                }
                return;
            }

            if (e.Key == Key.F7)
            {
                if (voces.Count == 1)
                {
                    string cadena;
                    if (swIngles)
                        cadena = "I can't change my voice because you have just one english voice in this computer. If you want to change me, please install another voice";
                    else
                        cadena = "no puedo cambiar mi voz porque tenés una sola voz en español instalada en esta computadora. Si querés cambiarme, por favor instalá otra voz";
                    voz.hablarAsync(cadena);
                }
                else
                {
                    int pos = voces.IndexOf(voz.vozActual());
                    if (pos >= voces.Count - 1) pos = -1;
                    pos++;
                    voz.cambiarVoz(cadenaIdioma, voces[pos]);

                    string cadena;
                    if (swIngles)
                        cadena = "You choose my voice to speak to you";
                    else
                        cadena = "elegiste mi voz para hablarte";

                    voz.hablarAsync(cadena);
                }
                return;
            }


            if (!swSeManejoElEventoEnWindow && txtFormula.Text.Trim() != "" && txtFormula.Text.Trim() != "-" && txtFormula.Text.Trim() != "+" && txtFormula.Text.Trim() != "$" && txtFormula.Text.Trim() != "*" && txtFormula.Text.Trim() != "/" && txtFormula.Text.Trim() != "^")
            {
                formulaSencilla f = new formulaSencilla(txtFormula.Text.Trim(), swRedondear); //se va escribiendo el resultado a medida que escribe la cuenta
                if (!f.swEsFormulaValida)
                {
                    txtResultado.Text = "";
                }
                else
                {
                    //if (swResultadosFracciones)
                    //    txtResultado.Text = new formulaSencilla().DecimalToFraction(f.resultado);
                    //else
                    string cadenaresultado = f.resultado.ToString();
                    if (swIngles)
                        cadenaresultado = cadenaresultado.Replace(',', '.');
                    else
                        cadenaresultado = cadenaresultado.Replace('.', ',');

                    txtResultado.Text = cadenaresultado;
                    
                }
                
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;
                if (txtFormula.Text.Length != 0)
                    voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true, swIngles));
            }

            swSeManejoElEventoEnWindow = false; //se resetea

        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                string cadena;
                if (swIngles)
                    cadena = "You cannot use delete key";
                else
                    cadena = "no se puede usar la tecla suprimir";

                voz.hablarAsync(cadena);
                e.Handled = true;
            }

            if (e.Key == Key.Back)
            {
                swSeManejoElEventoEnWindow = true;
                string cadena;
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;

                if (txtFormula.Text == "")
                {
                    if (swIngles)
                        cadena = "You erase everything";
                    else
                        cadena = "Borraste todo";
                }
                else
                {
                    string cadenaAux;
                    if (swIngles)
                        cadenaAux = "Erasing ";
                    else
                        cadenaAux = "Borrando ";

                    cadena = cadenaAux + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true, swIngles);
                }

                txtResultado.Text = "";
                voz.hablarAsync(cadena);
                return;
            }

            if (e.Key == Key.Enter)
            {
                swSeManejoElEventoEnWindow = true;

                if (txtFormula.Text.Trim() == "")
                {
                    string cadenaAux;
                    if (swIngles)
                        cadenaAux = "there isn't a writen caculus. Before press return, write one";
                    else
                        cadenaAux = "no hay una cuenta escrita. Para apretar enter, primero escribí el cálculo";

                    voz.hablarAsync(cadenaAux);
                }
                else
                {
                    formulaSencilla f = new formulaSencilla(txtFormula.Text.Trim(), swRedondear);

                    if (!f.swEsFormulaValida)
                    {
                        string cadenaAux;
                        if (swIngles)
                            cadenaAux = "the calculus is not well written. Please correct it and then press return to know the result";
                        else
                            cadenaAux = "la cuenta no está bien escrita. Por favor corregila y después apretá enter para saber el resultado";


                        voz.hablarAsync(cadenaAux);
                        txtResultado.Text = "ERROR";
                    }
                    else
                    {
                        //if (swResultadosFracciones)
                        //    txtResultado.Text = new formulaSencilla().DecimalToFraction(f.resultado);
                        //else
                        string cadenaresultado = f.resultado.ToString();
                        if (swIngles)
                            cadenaresultado = cadenaresultado.Replace(',', '.');
                        else
                            cadenaresultado = cadenaresultado.Replace('.', ',');

                        txtResultado.Text = cadenaresultado;

                        string cadenaAux;
                        string cadenaAux2;
                        if (swIngles)
                        {
                            cadenaAux = "the result is ";
                            cadenaAux2 = ". coppied";
                        }
                        else
                        {
                            cadenaAux = "El resultado es ";
                            cadenaAux2 = ". copiado";
                        }


                        voz.hablarAsync(cadenaAux + new ValidadorCadenas().traducirCadenaParaLeer(txtResultado.Text, false, swIngles) + cadenaAux2);
                    }
                }

                try
                {
                    Clipboard.SetText(txtFormula.Text + "=" + txtResultado.Text);
                }
                catch (Exception ex)
                {
                    new enviadorCorreo(ex.Message).enviarMail();
                    return;
                }

                return;
            }
        }
    }
}
