using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCashApi.Models
{
    public class BandeiraCartaoModel
    {
        public int bcCodi { get; set; }
        public string bcDesc { get; set; }
        public string bcImg { get; set; }
        public bool bcFlAt { get; set; }
    }
}