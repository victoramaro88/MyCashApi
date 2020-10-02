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
    public class BandeiraCartaoController : ApiController
    {
        BandeiraCartaoRepository _bandCartRepo = new BandeiraCartaoRepository();

        [HttpGet]
        [Authorize]
        [Route("api/BandeiraCartao/ListarBandeiraCartao/{id:int?}")]
        public List<BandeiraCartaoModel> ListarBandeiraCartao(int id = 0)
        {
            var retorno = _bandCartRepo.ListarBandeiraCartao(id);
            return retorno;
        }

        [HttpGet]
        [Authorize]
        [Route("api/BandeiraCartao/AlteraStatusBandeiraCartao/{codigoBandeiraCartao:int}/{statusNovo:bool}")]
        public string AlteraStatusBandeiraCartao(int codigoBandeiraCartao, bool statusNovo)
        {
            var retorno = _bandCartRepo.AlteraStatusBandeiraCartao(codigoBandeiraCartao, statusNovo);
            return retorno;
        }

        [HttpPost]
        [Authorize]
        public string ManterBandeiraCartao(BandeiraCartaoModel bandeiraCartaoModel)
        {

            if (bandeiraCartaoModel != null)
            {
                string retorno = _bandCartRepo.ManterBandeiraCartao(bandeiraCartaoModel);

                return retorno;
            }
            else
            {
                return "Campos obrigatórios inválidos";
            }
        }
    }
}
