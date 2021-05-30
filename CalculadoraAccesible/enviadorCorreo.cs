using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;


namespace CalculadoraAccesible
{
    class enviadorCorreo
    {
        string body;

        public enviadorCorreo(string cuerpoMensaje)
        {
            body = cuerpoMensaje;
        }

        public bool enviarMail()
        {
            try
            {
                MailAddress fromAddress = new MailAddress("guillermoestadisticas@gmail.com", "Calculadora Accesible");
                MailAddress toAddress = new MailAddress("guillermoestadisticas@gmail.com", "Mi creador");
                const string fromPassword = "3sA6qbjqMQO26k0fZHrk";
                const string subject = "Estadística App Calculadora";
                //           const string body = "Body";

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (MailMessage message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}






