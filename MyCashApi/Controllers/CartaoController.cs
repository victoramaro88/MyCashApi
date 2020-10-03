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
        [Authorize]
        [Route("api/Cartao/ListarCartoesByIdUsuario/{idUsuario}")]
        public List<CartaoUsuarioModel> ListarCartoesByIdUsuario(int idUsuario)
        {
            var retorno = _cartaoRepo.ListarCartoesByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpGet]
        [Authorize]
        [Route("api/Cartao/AlteraStatusCartaoUsr/{codigoCartUsr:int}/{statusNovo:bool}")]
        public string AlteraStatusCartaoUsr(int codigoCartUsr, bool statusNovo)
        {
            var retorno = _cartaoRepo.AlteraStatusCartaoUsr(codigoCartUsr, statusNovo);
            return retorno;
        }

        [HttpPost]
        [Authorize]
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
