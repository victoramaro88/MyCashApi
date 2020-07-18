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
    public class TransInstFinancController : ApiController
    {
        TransInstFinancRepository _transInstFinancRepo = new TransInstFinancRepository();

        [HttpGet]
        [Route("api/TransInstFinanc/ListarTransacaoInstFinanceira/{transacaoInstFinancCodi:int?}")]
        public List<TransInstFinancModel> ListarTransacaoInstFinanceira(int transacaoInstFinancCodi = 0)
        {
            var retorno = _transInstFinancRepo.ListarTransacaoInstFinanceira(transacaoInstFinancCodi);
            return retorno;
        }

        [HttpGet]
        [Route("api/TransInstFinanc/ListarTransacaoInstFinanceiraByIdUsuario/{idUsuario}")]
        public List<TransInstFinancModel> ListarTransacaoInstFinanceiraByIdUsuario(int idUsuario)
        {
            var retorno = _transInstFinancRepo.ListarTransacaoInstFinanceiraByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterTransacaoInstFinanceira(TransInstFinancModel transInstFinancModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (transInstFinancModel != null)
            {
                retorno = _transInstFinancRepo.ManterTransacaoInstFinanceira(transInstFinancModel);

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
