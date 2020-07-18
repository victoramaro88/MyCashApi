using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class SaldoCartoesModel
    {
        public int tcCodi { get; set; }
        public int usuCodi { get; set; }
        public int ttCodi { get; set; }
        public decimal tcValor { get; set; }
        public int tcNumPar { get; set; }
        public DateTime tcData { get; set; }
        public string tcDesc { get; set; }
        public int carCodi { get; set; }
        public string carDesc { get; set; }
        public bool tcStatus { get; set; }
        public decimal carLimit { get; set; }
        public int carDiaVenc { get; set; }
        public decimal carSald { get; set; }
        public string tdDesc { get; set; }
    }
}