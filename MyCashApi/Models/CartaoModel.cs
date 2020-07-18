using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class CartaoModel
    {
        public int carCodi { get; set; }
        public int bcCodi { get; set; }
        public int ifCodi { get; set; }
        public int usuCodi { get; set; }
        public string carDesc { get; set; }
        public decimal carLimit { get; set; }
        public int carDiaVenc { get; set; }
        public decimal carSald { get; set; }
        public bool carFlAt { get; set; }
    }
}