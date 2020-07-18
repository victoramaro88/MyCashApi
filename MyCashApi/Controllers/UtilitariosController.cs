using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Net.Mail;

namespace MyCashApi.Controllers
{
    public class UtilitariosController : ApiController
    {
        [NonAction]
        public string GerarHashString(string value)
        {
            byte[] arrBytes = Encoding.ASCII.GetBytes(value);
            byte[] result;
            using (SHA256 shaM = new SHA256Managed())
            {
                result = shaM.ComputeHash(arrBytes);
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in result)
                stringBuilder.AppendFormat("{0:X2}", b);

            string hashString = stringBuilder.ToString();

            return hashString;
        }

        [NonAction]
        public byte[] GerarHashByteArray(string value)
        {
            byte[] arrBytes = Encoding.ASCII.GetBytes(value);
            byte[] result;
            using (SHA256 shaM = new SHA256Managed())
            {
                result = shaM.ComputeHash(arrBytes);
            }
            return result;
        }

        [NonAction]
        public string EnviarEmail(string destinatário, string assunto, string mensagem)
        {
            string resp = "";
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("contatomycash@gmail.com", "ypvsjqcscfffmyeg"),
                    EnableSsl = true
                })
                {
                    client.Send("contatomycash@gmail.com", destinatário, assunto, mensagem);
                }

                resp = "OK";
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            return resp;
        }
    }
}
