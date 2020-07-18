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
    public class EnumeradoresController : ApiController
    {
        EnumeradoresRepository _enumRepo = new EnumeradoresRepository();

        [HttpGet]
        public EnumeradoresModel ListarEnumeradores()
        {
            var retorno = _enumRepo.ListarEnumeradores();
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterEnumeradores(EnumeradoresModel enumeradoresModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (enumeradoresModel != null)
            {
                retorno = _enumRepo.ManterEnumeradores(enumeradoresModel);

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
