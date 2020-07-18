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
    public class TransCartaoController : ApiController
    {
        TransCartaoRepository _transCartaoRepo = new TransCartaoRepository();

        [HttpGet]
        [Route("api/TransCartao/ListarTransacaoCartao/{transacaoCartaoCodi:int?}")]
        public List<TransCartaoModel> ListarTransacaoCartao(int transacaoCartaoCodi = 0)
        {
            var retorno = _transCartaoRepo.ListarTransacaoCartao(transacaoCartaoCodi);
            return retorno;
        }

        [HttpGet]
        [Route("api/TransCartao/ListarTransacaoCartaoByIdUsuario/{idUsuario}")]
        public List<TransCartaoModel> ListarTransacaoCartaoByIdUsuario(int idUsuario)
        {
            var retorno = _transCartaoRepo.ListarTransacaoCartaoByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpGet]
        [Route("api/TransCartao/RetornaSaldoCartoesByIdUsuario/{idUsuario}")]
        public List<SaldoCartoesModel> RetornaSaldoCartoesByIdUsuario(int idUsuario)
        {
            var retorno = _transCartaoRepo.RetornaSaldoCartoesByIdUsuario(idUsuario);
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterTransacaoCartao(TransCartaoModel transCartaoModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (transCartaoModel != null)
            {
                // ->Dividindo as parcelas da transação
                List<ParcelasTransCartaoModel> parcelas = new List<ParcelasTransCartaoModel>();
                decimal valorParcela = transCartaoModel.tcValor / transCartaoModel.tcNumParc;
                decimal somatorioParcelas = 0;
                for (int i = 0; i < transCartaoModel.tcNumParc; i++)
                {
                    ParcelasTransCartaoModel parcelasTransCartaoModel = new ParcelasTransCartaoModel();

                    parcelasTransCartaoModel.tcCodi = transCartaoModel.tcCodi;

                    if ((transCartaoModel.tcValor % transCartaoModel.tcNumParc) != 0)
                    {
                        if (i + 1 == transCartaoModel.tcNumParc)
                        {
                            parcelasTransCartaoModel.ptcValorParc = transCartaoModel.tcValor - somatorioParcelas;
                        }
                        else
                        {
                            parcelasTransCartaoModel.ptcValorParc = Math.Round(valorParcela, 2);
                        }
                    }
                    else
                    {
                        parcelasTransCartaoModel.ptcValorParc = Math.Round(valorParcela, 2);
                    }
                    parcelasTransCartaoModel.ptcDataVenc = transCartaoModel.tcData;
                    parcelasTransCartaoModel.ptcStatus = true;

                    somatorioParcelas += parcelasTransCartaoModel.ptcValorParc;

                    parcelas.Add(parcelasTransCartaoModel);
                }

                retorno = _transCartaoRepo.ManterTransacaoCartao(transCartaoModel, parcelas);

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
