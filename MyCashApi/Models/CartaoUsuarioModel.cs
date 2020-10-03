using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class CartaoUsuarioModel
    {   public int carCodi { get; set; }
        public int bcCodi { get; set; }
        public int ifCodi { get; set; }
        public int usuCodi { get; set; }
        public string carDesc { get; set; }
        public decimal carLimit { get; set; }
        public int carDiaVenc { get; set; }
        public decimal carSald { get; set; }
        public bool carFlAt { get; set; }

        
        public string ifDesc { get; set; }
        public string ifImg { get; set; }
        public string bcDesc { get; set; }
        public string bcImg { get; set; }
    }
}