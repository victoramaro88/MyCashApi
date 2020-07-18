using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class UsuarioModel
    {
        public int usuCodi { get; set; }
        public string usuNome { get; set; }
        public string usuNCPF { get; set; }
        public DateTime usuNasc { get; set; }
        public string usuEmail { get; set; }
        public string usuSenha { get; set; }
        public DateTime usuDtCad { get; set; }
        public bool usuFlAt { get; set; }
        public bool usuValido { get; set; }
        public string usuSexo { get; set; }
        public bool usuCttEma { get; set; }
    }
}