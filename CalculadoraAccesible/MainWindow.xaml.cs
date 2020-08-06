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

namespace CalculadoraAccesible
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> voces = voz.listarVocesPorIdioma("Español");
        bool swSeManejoElEventoEnWindow = false;
        bool swRedondear = true;
        //bool swResultadosFracciones = false;


        public MainWindow()
        {
            InitializeComponent();

            if (voz.listarVocesPorIdioma("Español").Count <= 0) //si no hay voces en español instaladas
            {
                MessageBox.Show("Este programa necesita que en la computadora haya instalada una voz en Español, por favor instale una.", "Error");
                this.Close();
                return;
            }

            try { 
                WebClient webClient = new WebClient();
                if (!webClient.DownloadString("https://pastebin.com/raw/jkG8NyGM").Contains("1.0"))
                {
                    if (MessageBox.Show("Hay una actualización de la Calculadora. Desea descargarla?", "Actualización", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://guilletoscani.weebly.com/uploads/1/3/3/2/133298324/calculadora_accesible.exe");
                    }
                }
                //else
                //{
                //    MessageBox.Show("Todo actualizado");

                //}
            }
            catch
            { }
            finally
            {
                voz.cambiarVoz(voz.listarVocesPorIdioma("Español")[0]);

                //txtFracciones.Foreground = Brushes.Black;
                //txtFracciones.Text = "FRACCIONES: NO (F3)";

                txtRedondear.Foreground = Brushes.Red;
                txtRedondear.Text = "REDONDEAR: SÍ (F2)";


                txtInfo.Text = "F1: lee esta ayuda. Enter: Da el resultado de la cuenta. F2: cambia el redondeo del resultado." +
                    " F5, F6 y F7 modifican la voz. Flechas: leen lo escrito. Control: callar la voz." +
                    "\nSe pueden hacer sumas (+), restas (-), multiplicaciones (*) y divisiones (/). También usar paréntesis, tanto simples como anidados, " +
                    "lo que reemplaza el uso de corchetes y llaves." +
                    "\nPara hacer potencias se usa el signo circunflejo. Por ejemplo 2^3 sería 2 al cubo." +
                    "\nPara realizar raíces se usa el signo pesos. Por ejemplo 3$8 sería raíz cúbica de 8. Si se usa $9 sería raíz cuadrada de 9." +
                    "\nTrabaja con números enteros, positivos y negativos, y con decimales. Al usar enter, la cuenta y el resultado se copian automáticamente" +
                    "\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
                txtPedido.Text = "Escribí tu cuenta";
                txtFormula.Focus();
                voz.hablarAsync("Abriendo la calculadora. Escribí el cálculo que quieras hacer y apretá enter para saber el resultado. Si apretás efe uno vas escuchar la ayuda");
            }
        
        }

        private void txtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            

            if (e.Key == Key.F1) //F1 leer la ayuda
            {
                string ayuda = "efe uno: lee esta ayuda. Enter: Da el resultado de la cuenta. efe dos: cambia si el resultado se redondea o no. efe cinco, efe seis y efe siete modifican la voz. Flechas: leen lo escrito. Control: callar la voz." +
                "\nSe pueden hacer sumas usando el signo más, restas usando el guión, multiplicaciones usando asterisco y divisiones la barra diagonal. También usar paréntesis, tanto simples como anidados, " +
                "lo que reemplaza el uso de corchetes y llaves." +
                "\nPara hacer potencias se usa el signo circunflejo. Por ejemplo dos circunflejo tres sería dos al cubo." +
                "\nPara realizar raíces se usa el signo pesos. Por ejemplo tres pesos ocho, sería raíz cúbica de ocho. Si se usa pesos nueve, sería raíz cuadrada de nueve." +
                "\nTrabaja con números enteros, positivos y negativos, y con decimales. Al usar enter, la cuenta y el resultado se copian automáticamente, lo que permite pegarlo en otro programa, por ejemplo Word" +
                "\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
                voz.hablarAsync(ayuda);
                return;
            }

            

            if (e.Key == Key.Down) //flecha abajo lee lo que ya está escrito en el cuadro de texto
            {
                if (txtFormula.Text.Trim() != "")
                    voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text, false));
                else
                    voz.hablarAsync("No hay nada escrito");

                return;
            }

            if (e.Key == Key.Up) //flecha abajo lee lo que ya está escrito en el cuadro de texto
            {
                if (txtFormula.Text.Trim() != "")
                    voz.hablarAsync(new ValidadorCadenas().separarCadenaconEspacios(txtFormula.Text));
                else
                    voz.hablarAsync("No hay nada escrito");

                return;
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) //control calla la voz
            {
                voz.callar();
                return;
            }

            if (e.Key == Key.F2) //F2 cambia redondeo del resultado
            {
                swRedondear = !swRedondear;
                if (swRedondear)
                {
                    txtRedondear.Foreground = Brushes.Red;
                    txtRedondear.Text = "REDONDEAR: SÍ (F2)";
                    voz.hablarAsync("redondear el resultado activado");
                }
                else
                {
                    txtRedondear.Foreground = Brushes.Black;
                    txtRedondear.Text = "REDONDEAR: NO (F2)";
                    voz.hablarAsync("redondear el resultado desactivado");
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

            if (e.Key == Key.F5) //F5 más lento
            {
                voz.cambiarVelocidad(voz.velocidadVozActual() - 1);
                voz.hablarAsync("más lento");
                return;
            }

            if (e.Key == Key.F6) //F6 más rápido
            {
                voz.cambiarVelocidad(voz.velocidadVozActual() + 1);
                voz.hablarAsync("más rápido");
                return;
            }

            if (e.Key == Key.Left)
            {
                if (txtFormula.Text.Trim() == "") //si el cuadro está vacío
                    voz.hablarAsync("No hay nada escrito");
                else
                {
                    if (txtFormula.SelectionStart == 0) //si está al principio del cuadro
                        voz.hablarAsync("Estás en el comienzo del cuadro para escribir tu ejercicio");
                    //else if (txtFormula.SelectionStart == txtFormula.Text.Length)
                    //    Voz.hablarAsync("")
                    else
                    {
                        voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true));
                    }
                }
                return;
            }

            if (e.Key == Key.Right)
            {
                if (txtFormula.Text.Trim() == "") //si el cuadro está vacío
                    voz.hablarAsync("No hay nada escrito");
                else
                {
                    if (txtFormula.SelectionStart == txtFormula.Text.Length) //si está al principio del cuadro
                        voz.hablarAsync("Último carácter: " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true));
                    //else if (txtFormula.SelectionStart == txtFormula.Text.Length)
                    //    Voz.hablarAsync("")
                    else
                    {
                        //if ()
                        voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true));
                    }
                }
                return;
            }

            if (e.Key == Key.F7)
            {
                if (voces.Count == 1)
                    voz.hablarAsync("no puedo cambiar mi voz porque tenés una sola voz en español instalada en esta computadora. Si querés cambiarme, por favor instalá otra voz");
                else
                {
                    int pos = voces.IndexOf(voz.vozActual());
                    if (pos >= voces.Count - 1) pos = -1;
                    pos++;
                    voz.cambiarVoz(voces[pos]);
                    voz.hablarAsync("elegiste mi voz para hablarte");
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
                        txtResultado.Text = f.resultado.ToString();
                    
                }
                
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;
                if (txtFormula.Text.Length != 0)
                    voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true));
            }

            swSeManejoElEventoEnWindow = false; //se resetea

        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                voz.hablarAsync("no se puede usar la tecla suprimir");
                e.Handled = true;
            }

            if (e.Key == Key.Back)
            {
                swSeManejoElEventoEnWindow = true;
                string cadena = "";
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;

                if (txtFormula.Text == "")
                    cadena = "Borraste todo";
                else
                    cadena = "Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true);

                txtResultado.Text = "";
                voz.hablarAsync(cadena);
                return;
            }

            if (e.Key == Key.Enter)
            {
                swSeManejoElEventoEnWindow = true;

                if (txtFormula.Text.Trim() == "")
                    voz.hablarAsync("no hay una cuenta escrita. Para apretar enter, primero escribí el cálculo");
                else
                {
                    formulaSencilla f = new formulaSencilla(txtFormula.Text.Trim(), swRedondear);

                    if (!f.swEsFormulaValida)
                    {
                        voz.hablarAsync("la cuenta no está bien escrita. Por favor corregila y después apretá enter para saber el resultado");
                        txtResultado.Text = "ERROR";
                    }
                    else
                    {
                        //if (swResultadosFracciones)
                        //    txtResultado.Text = new formulaSencilla().DecimalToFraction(f.resultado);
                        //else
                            txtResultado.Text = f.resultado.ToString();

                        voz.hablarAsync("El resultado es " + new ValidadorCadenas().traducirCadenaParaLeer(txtResultado.Text, false) + ". copiado");
                    }
                }

                Clipboard.SetText(txtFormula.Text + "=" + txtResultado.Text);

                return;
            }
        }
    }
}
