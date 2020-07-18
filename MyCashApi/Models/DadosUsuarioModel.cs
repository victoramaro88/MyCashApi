using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class DadosUsuarioModel
    {
        public UsuarioModel usuarioModel{ get; set; }
        public EnderecoModel enderecoModel{ get; set; }
        public PerfilUsuarioModel perfilUsuarioModel { get; set; }
    }
}