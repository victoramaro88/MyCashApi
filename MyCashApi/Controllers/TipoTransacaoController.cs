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
    public class TipoTransacaoController : ApiController
    {
        private readonly TipoTransacaoRepository _tipoTransacaoRepo = new TipoTransacaoRepository();

        [HttpGet]
        //[Authorize]
        [Route("api/TipoTransacao/ListarTipoTransacao/{idTipoTransacao:int?}")]
        public List<TipoTransacaoModel> ListarTipoTransacao(int idTipoTransacao = 0)
        {
            var retorno = _tipoTransacaoRepo.ListarTipoTransacao(idTipoTransacao);
            return retorno;
        }
    }
}
