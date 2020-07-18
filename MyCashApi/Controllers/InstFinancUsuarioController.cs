using MyCashApi.Models;
using MyCashApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyCashApi.Controllers
{
    public class InstFinancUsuarioController : ApiController
    {
        InstFinancUsuarioRepository _instFiUsuRepo = new InstFinancUsuarioRepository();

        [HttpGet]
        [Route("api/InstFinancUsuario/ListarInstitFinancUsuario/{codInstFinUsu:int?}")]
        public List<InstFinancUsuarioModel> ListarInstitFinancUsuario(int codInstFinUsu = 0)
        {
            var retorno = _instFiUsuRepo.ListarInstitFinancUsuario(codInstFinUsu);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterInstitFinancUsuario(InstFinancUsuarioModel instFinancUsuarioModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (instFinancUsuarioModel != null)
            {
                retorno = _instFiUsuRepo.ManterInstitFinancUsuario(instFinancUsuarioModel);

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
    }
}
