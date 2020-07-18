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
    public class TransacoesFixasController : ApiController
    {
        TransacoesFixasRepository _transFixaRepo = new TransacoesFixasRepository();

        [HttpGet]
        [Route("api/TransacoesFixas/ListarTransacoesFixas/{transacaoFixaCodi:int?}/{idUsuario:int?}")]
        public List<TransacoesFixasModel> ListarTransacoesFixas(int transacaoFixaCodi = 0, int idUsuario = 0)
        {
            var retorno = _transFixaRepo.ListarTransacoesFixas(transacaoFixaCodi, idUsuario);
            return retorno;
        }

        [HttpGet]
        [Route("api/TransacoesFixas/ListarTransacoesFixasByIdUsuario/{idUsuario}")]
        public List<TransacoesFixasModel> ListarTransacoesFixasByIdUsuario(int idUsuario)
        {
            var retorno = _transFixaRepo.ListarTransacoesFixasByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterTransacoesFixas(TransacoesFixasModel transacoesFixasModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (transacoesFixasModel != null)
            {
                retorno = _transFixaRepo.ManterTransacoesFixas(transacoesFixasModel);

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
