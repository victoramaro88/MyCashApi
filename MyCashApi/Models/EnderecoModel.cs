using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class EnderecoModel
    {
        public int endCodi { get; set; }
        public int usuCodi { get; set; }
        public string endNCEP { get; set; }
        public string endLogr { get; set; }
        public string endNume { get; set; }
        public string endCompl { get; set; }
        public string endBairr { get; set; }
        public string endCidad { get; set; }
        public string endEsta { get; set; }
    }
}