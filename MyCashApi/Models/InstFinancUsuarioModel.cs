using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class InstFinancUsuarioModel
    {
        public int ifuCodi { get; set; }
        public int ifCodi { get; set; }
        public int usuCodi { get; set; }
        public string ifuNAgen { get; set; }
        public string ifuNConta { get; set; }
        public decimal ifuLimit { get; set; }
        public decimal ifuSaldo { get; set; }
        public bool ifuFlAt { get; set; }
    }
}