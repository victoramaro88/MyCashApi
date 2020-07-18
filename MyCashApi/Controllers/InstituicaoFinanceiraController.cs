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
    public class InstituicaoFinanceiraController : ApiController
    {
        InstituicaoFinanceiraRepository _instFinRepo = new InstituicaoFinanceiraRepository();

        [HttpGet]
        [Route("api/InstituicaoFinanceira/ListarInstitFinanceiras/{idInstFin:int?}")]
        public List<InstituicaoFinanceiraModel> ListarInstitFinanceiras(int idInstFin = 0)
        {
            var retorno = _instFinRepo.ListarInstitFinanceiras(idInstFin);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterInstitFinanc(InstituicaoFinanceiraModel instituicaoFinanceiraModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (instituicaoFinanceiraModel != null)
            {
                retorno = _instFinRepo.ManterInstitFinanc(instituicaoFinanceiraModel);

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
