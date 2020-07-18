using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class InstituicaoFinanceiraModel
    {
        public int ifCodi { get; set; }
        public string ifDesc { get; set; }
        public string ifCod { get; set; }
        public string ifImg { get; set; }
        public bool ifFlAt { get; set; }
    }
}