using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CalculadoraAccesible
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> voces = voz.listarVocesPorIdioma("Español");
        bool swSeManejoElEventoEnWindow = false;


        public MainWindow()
        {
            InitializeComponent();

            if (voz.listarVocesPorIdioma("Español").Count <= 0) //si no hay voces en español instaladas
            {
                MessageBox.Show("Este programa necesita que en la computadora haya instalada una voz en Español, por favor instale una.", "Error");
                this.Close();
                return;
            }

            voz.cambiarVoz(voz.listarVocesPorIdioma("Español")[0]);


            txtInfo.Text = "F1: lee esta ayuda. Enter: Da el resultado de la cuenta. F5, F6 y F7 modifican la voz. Flechas: leen lo escrito. Control: callar la voz.\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
            txtPedido.Text = "Escribí tu cuenta";
            txtFormula.Focus();
            voz.hablarAsync("Abriendo la calculadora. Escribí el cálculo que quieras hacer y apretá enter para saber el resultado. Si apretás efe uno vas escuchar la ayuda");
        
        }

        private void txtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            

            if (e.Key == Key.F1) //F1 leer la ayuda
            {
                voz.hablarAsync(txtInfo.Text);
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


            if (!swSeManejoElEventoEnWindow)
            {
                txtResultado.Text = "";
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
                    formulaSencilla f = new formulaSencilla(txtFormula.Text.Trim());
                    if (!f.swEsFormulaValida)
                        voz.hablarAsync("la cuenta no está bien escrita. Por favor corregila y después apretá enter para saber el resultado");
                    else
                    {
                        voz.hablarAsync("El resultado es " + new ValidadorCadenas().traducirCadenaParaLeer(f.resultado.ToString(), false));
                        txtResultado.Text = f.resultado.ToString();
                    }
                }
                return;
            }
        }
    }
}
