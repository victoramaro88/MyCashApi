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
    public class EnderecoController : ApiController
    {
        EnderecoRepository _enderecoRepo = new EnderecoRepository();

        [HttpGet]
        [Route("api/Endereco/ListarEnderecoByIdUsuario/{idUsuario}")]
        public EnderecoModel ListarEnderecoByIdUsuario(int idUsuario)
        {
            EnderecoModel retorno = new EnderecoModel();

            if (idUsuario > 0)
            {
                retorno = _enderecoRepo.ListarEnderecoByIdUsuario(idUsuario);
            }

            return retorno;
        }

        [HttpPost]
        public HttpResponseMessage InserirEndereco(EnderecoModel enderecoModel)
        {
            string retorno = "";
            var response = new HttpResponseMessage();

            if (enderecoModel != null && enderecoModel.usuCodi > 0)
            {
                retorno = _enderecoRepo.InserirEndereco(enderecoModel);

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
