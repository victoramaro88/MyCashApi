using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class TransCartaoModel
    {
        public int tcCodi { get; set; }
        public int carCodi { get; set; }
        public int usuCodi { get; set; }
        public int ttCodi { get; set; }
        public int tdCodi { get; set; }
        public decimal tcValor { get; set; }
        public int tcNumParc { get; set; }
        public string tcDesc { get; set; }
        public DateTime tcData { get; set; }
        public string tcComprov { get; set; }
        public bool tcStatus { get; set; }
    }
}