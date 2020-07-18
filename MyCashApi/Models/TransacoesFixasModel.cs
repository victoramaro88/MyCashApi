using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class TransacoesFixasModel
    {
        public int tfCodi { get; set; }
        public int usuCodi { get; set; }
        public int ifuCodi { get; set; }
        public int ttCodi { get; set; }
        public int tdCodi { get; set; }
        public string tfDesc { get; set; }
        public decimal tfValor { get; set; }
        public int tfDiaVenc { get; set; }
        public DateTime tfMesDebit { get; set; }
        public DateTime tfDataIni { get; set; }
        public DateTime tfDataFim { get; set; }
        public bool tfFlAt { get; set; }
        public string tfDoc { get; set; }
    }
}