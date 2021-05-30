using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraAccesible
{
    class ValidadorCadenas
    {
        public bool esCaracterValido(char c, bool swIncluirEspacios)
        {
            string caracteresValidos = "1234567890+-*/,()^$";  //"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+=";
            if (swIncluirEspacios)
                caracteresValidos += " ";

            if (caracteresValidos.Contains(c))
                return true;
            else
                return false;

        }

        public bool chequearFormulaValidaPorOperaciones (string formula)
        {
            formula = quitarEspaciosEncadena(formula);
            if (formula.Contains("-*")) return false;
            if (formula.Contains("+*")) return false;
            if (formula.Contains("-/")) return false;
            if (formula.Contains("+/")) return false;
            if (formula.Contains("-^")) return false;
            if (formula.Contains("+^")) return false;
            if (formula.Contains("//")) return false;
            if (formula.Contains("**")) return false;
            if (formula.Contains("^^")) return false;
            if (formula.Contains("$$")) return false;
            if (formula.Contains("^/")) return false;
            if (formula.Contains("^*")) return false;
            if (formula.Contains("/*")) return false;
            if (formula.Contains("*/")) return false;




            return true;
        }

        public bool esCadenaValida(string cadenaValida, bool swIncluirEspacios)
        {
            //bool swEsCadenaValida = true;

            foreach (char c in cadenaValida) //se quitan todos los caracteres que no sean válidos
                if (!new ValidadorCadenas().esCaracterValido(c, swIncluirEspacios))
                    return false;

            return true;
        }

        public string validarCadena(string cadenaAvalidar, bool swIncluirEspacios)
        {
            string auxcadenaValidada = "";

            foreach (char c in cadenaAvalidar) //se quitan todos los caracteres que no sean válidos
                if (new ValidadorCadenas().esCaracterValido(c, swIncluirEspacios))
                    auxcadenaValidada += c;

            return auxcadenaValidada;
        }

        public string quitarEspaciosEncadena(string cadena)
        {
            string auxcadenaValidada = "";

            foreach (char c in cadena) //se quitan todos los caracteres que no sean válidos
                if (!char.IsWhiteSpace(c))
                    auxcadenaValidada += c;

            return auxcadenaValidada;
        }

        public string OrdenarCadena(string cadenaParaOrdenar)
        {
            return String.Concat(cadenaParaOrdenar.OrderBy(c => c));
        }


        public string separarCadenaconEspacios(string cadenaParaSeparar, bool swIngles)
        {
            string aux = "";

            foreach (char c in cadenaParaSeparar)
                aux += c.ToString() + ' ';

            return traducirCadenaParaLeer(aux, false, swIngles);
        }

        public string traducirCadenaParaLeer(string cadenaParaTraducir, bool swLeerEspacio, bool swIngles)
        {
            string aux = "";
            foreach (char c in cadenaParaTraducir)
            {
                if (!char.IsWhiteSpace(c))
                    aux += traducirCaracterParaLeer(c, swIngles);
                else if (char.IsDigit(c))
                    aux += c;
                else
                {
                    if (swLeerEspacio)
                        aux += traducirCaracterParaLeer(c, swIngles);
                    else
                        aux += ' ';
                }

                if (esEnMayúscula(c))
                {
                    string cadena;
                    if (swIngles)
                    {
                        cadena = " capital ";
                        aux = cadena + aux;
                    }
                    else
                    {
                        cadena = " mayúsculas ";
                        aux += cadena;
                    }
                }
            }

            return aux;
        }

        public static string quitarParentesis(string formula)
        {
            string tmp = "";
            foreach (char c in formula)
            {
                if (c != '(' && c != ')')
                    tmp += c;
            }

            if (tmp != "")
                return tmp;
            else
                return formula;
        }

        private string traducirCaracterParaLeer(char c, bool swIngles)
        {
            string aux = c.ToString();

            if (!swIngles)
            {
                switch (c)
                {
                    case '´':
                        aux = " acento ";
                        break;
                    case '+':
                        aux = " mas ";
                        break;
                    case ' ':
                        aux = " espacio ";
                        break;
                    case '{':
                        aux = " abre llave ";
                        break;
                    case '}':
                        aux = " ciera llave ";
                        break;
                    case '(':
                        aux = " abre paréntesis ";
                        break;
                    case ')':
                        aux = " ciera paréntesis ";
                        break;
                    case '¨':
                        aux = " diéresis ";
                        break;
                    case '[':
                        aux = " abre corchetes ";
                        break;
                    case ']':
                        aux = " cierra corchetes ";
                        break;
                    case '*':
                        aux = " por "; //asterisco
                        break;
                    case ',':
                        aux = " coma ";
                        break;
                    case '.':
                        aux = " punto ";
                        break;
                    case '-':
                        aux = " menos "; //"guión";
                        break;
                    case '^':
                        aux = " elevado a la "; //"acentro circunflejo";
                        break;
                    case 'ç':
                        aux = " ce cerillas ";
                        break;
                    case '`':
                        aux = " acentro grave ";
                        break;
                    case ';':
                        aux = " punto y coma ";
                        break;
                    case ':':
                        aux = " dos puntos ";
                        break;
                    case '_':
                        aux = " guión bajo ";
                        break;
                    case 'º':
                        aux = " grados ";
                        break;
                    case 'ª':
                        aux = " a volada ";
                        break;
                    case '!':
                        aux = " cierra admiración ";
                        break;
                    case '¿':
                        aux = " abre pregunta ";
                        break;
                    case '?':
                        aux = " cierra pregunta ";
                        break;
                    case '|':
                        aux = " barra vertical ";
                        break;
                    case '¬':
                        aux = " signo especial ";
                        break;
                    case '\'':
                        aux = " comilla simple ";
                        break;
                    case '"':
                        aux = " comillas ";
                        break;
                    case '¡':
                        aux = " abre admiración ";
                        break;
                    case '\\':
                        aux = " barra inclinada a la izquierda ";
                        break;
                    case '~':
                        aux = " tilde ";
                        break;
                    case '$':
                        aux = " raíz de ";//"pesos";
                        break;
                    case '%':
                        aux = " porcentaje ";
                        break;
                    case '#':
                        aux = " numeral ";
                        break;
                    case '&':
                        aux = " unión ";
                        break;
                    case '/':
                        aux = " dividido por "; //"barra inclinada";
                        break;
                    case '@':
                        aux = " arroba ";
                        break;
                    case 'a':
                    case 'A':
                        aux = "a";
                        break;
                    case 'b':
                    case 'B':
                        aux = "be ";
                        break;
                    case 'c':
                    case 'C':
                        aux = "se ";
                        break;
                    case 'd':
                    case 'D':
                        aux = "de ";
                        break;
                    case 'e':
                    case 'E':
                        aux = "e ";
                        break;
                    case 'f':
                    case 'F':
                        aux = "efe ";
                        break;
                    case 'g':
                    case 'G':
                        aux = "je ";
                        break;
                    case 'h':
                    case 'H':
                        aux = "ache ";
                        break;
                    case 'i':
                    case 'I':
                        aux = "i ";
                        break;
                    case 'j':
                    case 'J':
                        aux = "jota ";
                        break;
                    case 'k':
                    case 'K':
                        aux = "ka ";
                        break;
                    case 'l':
                    case 'L':
                        aux = "ele ";
                        break;
                    case 'm':
                    case 'M':
                        aux = "eme ";
                        break;
                    case 'n':
                    case 'N':
                        aux = "ene ";
                        break;
                    case 'ñ':
                    case 'Ñ':
                        aux = "eñe ";
                        break;
                    case 'o':
                    case 'O':
                        aux = "o ";
                        break;
                    case 'p':
                    case 'P':
                        aux = "pe ";
                        break;
                    case 'q':
                    case 'Q':
                        aux = "ku ";
                        break;
                    case 'r':
                    case 'R':
                        aux = "erre ";
                        break;
                    case 's':
                    case 'S':
                        aux = "ese ";
                        break;
                    case 't':
                    case 'T':
                        aux = "te ";
                        break;
                    case 'u':
                    case 'U':
                        aux = "u ";
                        break;
                    case 'v':
                    case 'V':
                        aux = "be corta ";
                        break;
                    case 'w':
                    case 'W':
                        aux = "doble be ";
                        break;
                    case 'x':
                    case 'X':
                        aux = "equis ";
                        break;
                    case 'y':
                    case 'Y':
                        aux = "i griega ";
                        break;
                    case 'z':
                    case 'Z':
                        aux = "seta ";
                        break;
                }
            }
            else
            {
                switch (c)
                {
                    case '´':
                        aux = " accent ";
                        break;
                    case '+':
                        aux = " plus ";
                        break;
                    case ' ':
                        aux = " space ";
                        break;
                    case '{':
                        aux = " open curly bracket ";
                        break;
                    case '}':
                        aux = " close curly bracket ";
                        break;
                    case '(':
                        aux = " open parentheses ";
                        break;
                    case ')':
                        aux = " close parentheses ";
                        break;
                    case '¨':
                        aux = " umlauts ";
                        break;
                    case '[':
                        aux = " open square bracket ";
                        break;
                    case ']':
                        aux = " close square bracket ";
                        break;
                    case '*':
                        aux = " times "; //asterisco
                        break;
                    case ',':
                        aux = " comma ";
                        break;
                    case '.':
                        aux = " point ";
                        break;
                    case '-':
                        aux = " minus ";//"guión";
                        break;
                    case '^':
                        aux = " raised to the power of ";//"acentro circunflejo";
                        break;
                    case 'ç':
                        aux = " ce cerillas ";
                        break;
                    case '`':
                        aux = " accent grave ";
                        break;
                    case ';':
                        aux = " semicolon ";
                        break;
                    case ':':
                        aux = " colon ";
                        break;
                    case '_':
                        aux = " underscore ";
                        break;
                    case 'º':
                        aux = " grades ";
                        break;
                    case 'ª':
                        aux = " a flied ";
                        break;
                    case '!':
                        aux = " exclamation ";
                        break;
                    case '¿':
                        aux = " opening question ";
                        break;
                    case '?':
                        aux = " question ";
                        break;
                    case '|':
                        aux = " vertical bar ";
                        break;
                    case '¬':
                        aux = " spetial sign ";
                        break;
                    case '\'':
                        aux = " single quote ";
                        break;
                    case '"':
                        aux = " quotation ";
                        break;
                    case '¡':
                        aux = " open exclamation ";
                        break;
                    case '\\':
                        aux = " slash ";
                        break;
                    case '~':
                        aux = " tilde ";
                        break;
                    case '$':
                        aux = " root of ";//"pesos";
                        break;
                    case '%':
                        aux = " percent ";
                        break;
                    case '#':
                        aux = " sharp ";
                        break;
                    case '&':
                        aux = " and ";
                        break;
                    case '/':
                        aux = " divided by "; //"barra inclinada";
                        break;
                    case '@':
                        aux = " at ";
                        break;
                    case 'a':
                    case 'A':
                        aux = "a ";
                        break;
                    case 'b':
                    case 'B':
                        aux = "b ";
                        break;
                    case 'c':
                    case 'C':
                        aux = "c ";
                        break;
                    case 'd':
                    case 'D':
                        aux = "d ";
                        break;
                    case 'e':
                    case 'E':
                        aux = "e ";
                        break;
                    case 'f':
                    case 'F':
                        aux = "f ";
                        break;
                    case 'g':
                    case 'G':
                        aux = "g ";
                        break;
                    case 'h':
                    case 'H':
                        aux = "h ";
                        break;
                    case 'i':
                    case 'I':
                        aux = "i ";
                        break;
                    case 'j':
                    case 'J':
                        aux = "j ";
                        break;
                    case 'k':
                    case 'K':
                        aux = "k ";
                        break;
                    case 'l':
                    case 'L':
                        aux = "l ";
                        break;
                    case 'm':
                    case 'M':
                        aux = "m ";
                        break;
                    case 'n':
                    case 'N':
                        aux = "n ";
                        break;
                    case 'ñ':
                    case 'Ñ':
                        aux = "ñ ";
                        break;
                    case 'o':
                    case 'O':
                        aux = "o ";
                        break;
                    case 'p':
                    case 'P':
                        aux = "p ";
                        break;
                    case 'q':
                    case 'Q':
                        aux = "q ";
                        break;
                    case 'r':
                    case 'R':
                        aux = "r ";
                        break;
                    case 's':
                    case 'S':
                        aux = "s ";
                        break;
                    case 't':
                    case 'T':
                        aux = "t ";
                        break;
                    case 'u':
                    case 'U':
                        aux = "u ";
                        break;
                    case 'v':
                    case 'V':
                        aux = "v ";
                        break;
                    case 'w':
                    case 'W':
                        aux = "w ";
                        break;
                    case 'x':
                    case 'X':
                        aux = "x ";
                        break;
                    case 'y':
                    case 'Y':
                        aux = "y ";
                        break;
                    case 'z':
                    case 'Z':
                        aux = "z ";
                        break;
                }
            }


            return aux;
        }

        private bool esEnMayúscula(char c)
        {
            string aux = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            if (aux.Contains(c))
                return true;

            return false;
        }
    }
}
