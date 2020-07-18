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
    public class CartaoController : ApiController
    {
        CartaoRepository _cartaoRepo = new CartaoRepository();

        [HttpGet]
        [Route("api/Cartao/ListarCartoesByIdUsuario/{idUsuario}")]
        public List<CartaoModel> ListarCartoesByIdUsuario(int idUsuario)
        {
            var retorno = _cartaoRepo.ListarCartoesByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterCartaoUsuario(CartaoModel cartaoModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (cartaoModel != null)
            {
                retorno = _cartaoRepo.ManterCartaoUsuario(cartaoModel);

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
