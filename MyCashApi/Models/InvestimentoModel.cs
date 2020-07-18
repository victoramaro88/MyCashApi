using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class InvestimentoModel
    {
        public int invCodi { get; set; }
        public int usuCodi { get; set; }
        public int ifuCodi { get; set; }
        public string invDesc { get; set; }
        public DateTime invDtIni { get; set; }
        public DateTime invDtFin { get; set; }
        public decimal invVlrIni { get; set; }
        public decimal invTxJur { get; set; }
        public string invObse { get; set; }
        public bool invFlAt { get; set; }
    }
}