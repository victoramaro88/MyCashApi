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
    public class TipoDespesaController : ApiController
    {
        TipoDespesaRepository _tipoDespesaRepository = new TipoDespesaRepository();

        [HttpGet]
        [Route("api/TipoDespesa/ListarTipoDespesa/{codigoTipoDespesa:int?}")]
        public List<TipoDespesaModel> ListarTipoDespesa(int codigoTipoDespesa = 0)
        {
            var retorno = _tipoDespesaRepository.ListarTipoDespesa(codigoTipoDespesa);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterTipoDespesa(TipoDespesaModel tipoDespesaModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (tipoDespesaModel != null)
            {
                retorno = _tipoDespesaRepository.ManterTipoDespesa(tipoDespesaModel);

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
