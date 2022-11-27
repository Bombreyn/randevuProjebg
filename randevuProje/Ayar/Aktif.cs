using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using randevuProje.Models;

namespace randevuProje
{
    public class Proje
    {
        public static Aktif Aktif
        {
            get
            {
                return (Aktif)HttpContext.Current.Session["Admin"];
            }
        }
    }
    public class Aktif
    {
        public Admin adminData { get; set; }
        public Siteayar SiteAyar { get; set; }
    }
}