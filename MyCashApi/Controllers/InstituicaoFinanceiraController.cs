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
        [Authorize]
        [Route("api/InstituicaoFinanceira/ListarInstitFinanceiras/{idInstFin:int?}")]
        public List<InstituicaoFinanceiraModel> ListarInstitFinanceiras(int idInstFin = 0)
        {
            var retorno = _instFinRepo.ListarInstitFinanceiras(idInstFin);
            return retorno;
        }

        [HttpGet]
        [Authorize]
        [Route("api/InstituicaoFinanceira/ListarInstitFinanceirasAtivas/{idInstFin:int?}")]
        public List<InstituicaoFinanceiraModel> ListarInstitFinanceirasAtivas(int idInstFin = 0)
        {
            var retorno = _instFinRepo.ListarInstitFinanceirasAtivas(idInstFin);
            return retorno;
        }

        [HttpPost]
        [Authorize]
        public string ManterInstitFinanc(InstituicaoFinanceiraModel instituicaoFinanceiraModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (instituicaoFinanceiraModel != null)
            {
                retorno = _instFinRepo.ManterInstitFinanc(instituicaoFinanceiraModel);

                return retorno;

                //if (retorno == "OK")
                //{
                //    response = new HttpResponseMessage(HttpStatusCode.OK);
                //    response.Content = new StringContent("OK");
                //}
                //else
                //{
                //    response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //    response.Content = new StringContent(retorno);
                //}
            }
            else
            {
                return "Campos obrigatórios inválidos";
                //response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //response.Content = new StringContent("Campos obrigatórios inválidos");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/InstituicaoFinanceira/AlteraStatusInstFinanceira/{codigoInstFinanc:int}/{statusNovo:bool}")]
        public string AlteraStatusInstFinanceira(int codigoInstFinanc, bool statusNovo)
        {
            var retorno = _instFinRepo.AlteraStatusInstFinanceira(codigoInstFinanc, statusNovo);
            return retorno;
        }
    }
}
