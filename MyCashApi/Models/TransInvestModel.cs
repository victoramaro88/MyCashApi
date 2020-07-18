using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class TransInvestModel
    {
        public int tiCodi { get; set; }
        public int invCodi { get; set; }
        public int usuCodi { get; set; }
        public int ttCodi { get; set; }
        public int tdCodi { get; set; }
        public DateTime tiData { get; set; }
        public decimal tiValor { get; set; }
        public string tiComprov { get; set; }
    }
}