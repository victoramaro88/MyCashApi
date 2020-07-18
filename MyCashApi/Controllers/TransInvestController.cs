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
    public class TransInvestController : ApiController
    {
        TransInvestRepository _transInvestRepo = new TransInvestRepository();

        [HttpGet]
        [Route("api/TransInvest/ListarTransacaoInvestimento/{transacaoInvestCodi:int?}")]
        public List<TransInvestModel> ListarTransacaoInvestimento(int transacaoInvestCodi = 0)
        {
            var retorno = _transInvestRepo.ListarTransacaoInvestimento(transacaoInvestCodi);
            return retorno;
        }

        [HttpGet]
        [Route("api/TransInvest/ListarTransacaoInvestimentoByIdUsuario/{idUsuario}")]
        public List<TransInvestModel> ListarTransacaoInvestimentoByIdUsuario(int idUsuario)
        {
            var retorno = _transInvestRepo.ListarTransacaoInvestimentoByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterTransacaoInvestimento(TransInvestModel transInvestModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (transInvestModel != null)
            {
                retorno = _transInvestRepo.ManterTransacaoInvestimento(transInvestModel);

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
