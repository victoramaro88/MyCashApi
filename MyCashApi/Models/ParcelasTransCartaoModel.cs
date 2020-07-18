using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class ParcelasTransCartaoModel
    {
        public int ptcCodi { get; set; }
        public int tcCodi { get; set; }
        public decimal ptcValorParc { get; set; }
        public DateTime ptcDataVenc { get; set; }
        public bool ptcStatus { get; set; }
    }
}