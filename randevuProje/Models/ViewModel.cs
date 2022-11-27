using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using randevuProje.Models;

namespace randevuProje.Models
{
    public class ViewModel
    {

        public Siteayar Siteayar { get; set; }
        public List<Odalar> odalars { get; set; }
        public Odalar Odalars { get; set; }
        public List<Resim> resims { get; set; }
        public Resim resim { get; set; }
        public List<Admin> admins { get; set; }
        public List<Randevu> randevus { get; set; }
        public List<ozellik1> ozellik1 { get; set; }
        public List<ozellik2> ozellik2 { get; set; }
        public List<ozellik3> ozellik3 { get; set; }
        public ozellik1 ozelliks1 { get; set; }
        public ozellik2 ozelliks2 { get; set; }
        public ozellik3 ozelliks3 { get; set; }

    }
}