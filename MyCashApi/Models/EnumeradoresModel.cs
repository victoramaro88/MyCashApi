using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class EnumeradoresModel
    {
        public int enuCod { get; set; }
        public string versao { get; set; }
        public DateTime dataVersao { get; set; }
        public string cttEmai { get; set; }
        public string cttFone { get; set; }
        public string imgApp { get; set; }
        public string siteUrl { get; set; }
    }
}