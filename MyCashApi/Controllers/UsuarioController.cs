using MyCashApi.Models;
using MyCashApi.Repository;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
using System;
using System.Net.Http.Headers;

namespace MyCashApi.Controllers
{
    public class UsuarioController : ApiController
    {
        private readonly UsuarioRepository _usuarioRepo = new UsuarioRepository();

        [HttpGet]
        [Authorize]
        [Route("api/Usuario/ListarUsuarios/{idUsuario:int?}")]
        public List<DadosUsuarioModel> ListarUsuarios(int idUsuario = 0)
        {
            var retorno = _usuarioRepo.ListarUsuarios(idUsuario);
            return retorno;
        }

        [HttpGet]
        [Route("api/Usuario/AtivaUsuario/{idUsuario:int?}")]
        public HttpResponseMessage AtivaUsuario(int idUsuario = 0)
        {
            var response = new HttpResponseMessage();

            var retorno = _usuarioRepo.AtivaUsuario(idUsuario);
            if (retorno == "OK")
            {
                response.Content = new StringContent(@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <meta charset='utf-8'>
                            <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <title>SUCESSO!</title>
                            <meta name='description' content='Sucesso ao ativar sua conta.'>
                            <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css' integrity='sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO' crossorigin='anonymous'>
                        </head>
                        <body style='background-color: #171d1b'>
                            <div class='text-center mt-5 text-white'>
                                <h1>MyCash Online</h1>
                                <br><br>
                                <h2>PARABÉNS!</h2>
                                <br>
                                <h3>Sua conta já está ativa, aproveite!</h3>
                                <br><br><br>
                                <a href='http://mycashonline.gearhostpreview.com/' class='btn btn-success btn-lg' role='button' aria-pressed='true'>ACESSAR O SITE!</a>
                            </div>
                        </body>
                    </html>"
                );
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            }
            else
            {
                response.Content = new StringContent(@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <meta charset='utf-8'>
                            <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <title>ERRO!</title>
                            <meta name='description' content='Falha ao ativar sua conta.'>
                            <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css' integrity='sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO' crossorigin='anonymous'>
                        </head>
                        <body style='background-color: #171d1b'>
                            <div class='text-center mt-5 text-white'>
                                <h1>MyCash Online</h1>
                                <br><br>
                                <h2>QUE PENA =( </h2>
                                <br>
                                <h3>Sentimos muito, mas não foi possível ativar sua conta, contate o administrador.</h3>
                                <br><br><br>
                                <a href='mailto:contatomycash@gmail.com' class='btn btn-danger btn-lg' role='button' aria-pressed='true'>ENVIAR E-MAIL</a>
                            </div>
                        </body>
                    </html>"
                );
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            }
            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("api/Usuario/ListarUsuarioByEmail/{emailUsuario}/")]
        public List<DadosUsuEmailModel> ListarUsuarioByEmail(string emailUsuario)
        {
            var retorno = _usuarioRepo.ListarUsuarioByEmail(emailUsuario);
            return retorno;
        }

        [HttpGet]
        [Route("api/Usuario/ConsultaUsuarioByEmailCPF/{emailCPF}/")]
        public bool ConsultaUsuarioByEmailCPF(string emailCPF)
        {
            var retorno = _usuarioRepo.ConsultaUsuarioByEmailCPF(emailCPF);
            return (retorno.Count > 0 ? true : false);
        }

        [HttpPost]
        public HttpResponseMessage InserirUsuario(UsuarioModel usuarioModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (usuarioModel != null && usuarioModel.usuNome.Length > 0)
            {
                retorno = _usuarioRepo.InserirUsuario(usuarioModel);

                if (retorno == "OK")
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StringContent("OK");
                }
                else
                {
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(retorno);
                }
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Campos obrigatórios inválidos");
            }

            return response;
        }

        [HttpPost]
        public string ManterUsuario(DadosUsuarioModel dadosUsuarioModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if ((dadosUsuarioModel.usuarioModel != null && dadosUsuarioModel.usuarioModel.usuNome.Length > 0) &&
                (dadosUsuarioModel.enderecoModel != null && dadosUsuarioModel.enderecoModel.endNCEP.Length > 0) &&
                (dadosUsuarioModel.perfilUsuarioModel != null && dadosUsuarioModel.perfilUsuarioModel.perCodi > 0))
            {
                retorno = _usuarioRepo.ManterUsuario(dadosUsuarioModel.usuarioModel, dadosUsuarioModel.enderecoModel, dadosUsuarioModel.perfilUsuarioModel);

                return retorno;
            }
            else
            {
                return "Campos obrigatórios inválidos"; ;
            }
        }

        [HttpGet]
        [Route("api/Usuario/ReenviarEmail/{emailUsuario}/")]
        public string ReenviarEmail(string emailUsuario)
        {
            List<DadosUsuEmailModel> listaUsuario = new List<DadosUsuEmailModel>();
            listaUsuario = ListarUsuarioByEmail(emailUsuario);

            int idUsuario = 0;
            string nomeUsuario = "";
            if (listaUsuario.Count > 0)
            {
                idUsuario = listaUsuario[0].usuCodi;
                nomeUsuario = listaUsuario[0].usuNome;

                int indice = 0;
                indice = nomeUsuario.IndexOf(" ");
                nomeUsuario = nomeUsuario.Substring(0, indice);
            }


            string resp;
            string destinatário = emailUsuario;
            string assunto = "Ativação de conta MyCash";
            string mensagem = "Olá " + nomeUsuario + ", \n\n Clique no link http://mycashapp.gearhostpreview.com/api/Usuario/AtivaUsuario/" + idUsuario + " para ativar sua conta. \n\n\n Caso não tenha realizado o cadastro no site, desconsidere este e-mail.";

            try
            {
                UtilitariosController utilitariosController = new UtilitariosController();
                resp = utilitariosController.EnviarEmail(destinatário, assunto, mensagem);
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            return resp;
        }
    }
}
