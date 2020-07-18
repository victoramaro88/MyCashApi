using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class TransInstFinancModel
    {
        public int tifCodi { get; set; }
        public int ifuCodi { get; set; }
        public int usuCodi { get; set; }
        public int ttCodi { get; set; }
        public int tdCodi { get; set; }
        public DateTime tifData { get; set; }
        public decimal tifValor { get; set; }
        public string tifComprov { get; set; }
    }
}