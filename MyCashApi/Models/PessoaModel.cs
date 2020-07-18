using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class PessoaModel
    {
        public int idPessoa { get; set; }
        public string nomePessoa { get; set; }
        public DateTime nascimentoPessoa { get; set; }
    }
}