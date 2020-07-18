using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class DadosUsuEmailModel
    {
        public int usuCodi { get; set; }
        public string usuNome { get; set; }
        public string usuNCPF { get; set; }
        public DateTime usuNasc { get; set; }
        public string usuEmail { get; set; }
        public DateTime usuDtCad { get; set; }
        public string usuSexo { get; set; }
        public int perCodi { get; set; }
        public bool usuValido { get; set; }
    }
}