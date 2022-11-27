using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using randevuProje.Models;

namespace randevuProje
{
    public class KullaniciViewModel
    {
        public Admin admin { get; set; }
        public List<Admin> admins { get; set; }
        public Yetki yetki { get; set; }
        public List<Yetki> yetkis { get; set; }
    }
}