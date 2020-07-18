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
    public class InvestimentoController : ApiController
    {
        InvestimentoRepository _investRepo = new InvestimentoRepository();

        [HttpGet]
        [Route("api/Investimento/ListarInstitFinancByIdUsuario/{idUsuario}")]
        public List<InvestimentoModel> ListarInstitFinancByIdUsuario(int idUsuario)
        {
            var retorno = _investRepo.ListarInstitFinancByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterInvestimentoUsuario(InvestimentoModel investimentoModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (investimentoModel != null)
            {
                retorno = _investRepo.ManterInvestimentoUsuario(investimentoModel);

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
