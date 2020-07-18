﻿using MyCashApi.Models;
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
        public List<BandeiraCartaoModel> ListarBandeiraCartao()
        {
            var retorno = _bandCartRepo.ListarBandeiraCartao();
            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage ManterBandeiraCartao(BandeiraCartaoModel bandeiraCartaoModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (bandeiraCartaoModel != null)
            {
                retorno = _bandCartRepo.ManterBandeiraCartao(bandeiraCartaoModel);

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