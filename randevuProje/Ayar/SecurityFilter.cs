using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using randevuProje.Models;

namespace randevuProje.Ayar
{
    public class SecurityFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RandevuContext db = new RandevuContext();

            if (HttpContext.Current.Session["Admin"] == null)
                HttpContext.Current.Session["Admin"] = new Aktif();

            string controlAdi = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionAdi = filterContext.ActionDescriptor.ActionName;
            try
            {
                Proje.Aktif.SiteAyar = db.Siteayar.FirstOrDefault();

                if (controlAdi == "Admin" && (actionAdi != "Login" && actionAdi != "AdminLogin"))
                {
                    if (Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Admin/Login");
                        return;
                    }
                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                filterContext.Result = new RedirectResult("/Home/Hata");
            }

        }
    }
}