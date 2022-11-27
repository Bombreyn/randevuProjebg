using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using randevuProje.Models;
using System.Web.Security; //FormsAuthentication kullanmak için
using System.Data.Entity;
using System.Net;
using System.IO;
using randevuProje.App_Start;
using System.Web.Script.Serialization;

namespace randevuProje.Controllers
{
    public class HomeController : Controller
    {

        RandevuContext db = new RandevuContext();


        public ActionResult Index()
        {
            try
            {
                ViewModel model = new ViewModel();
                model.odalars = db.Odalar.ToList();
                model.resims = db.Resim.ToList();


                return View(model);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        public ActionResult Odalar(int odaid, string adi)
        {
            try
            {
                var odalar = db.Odalar.Where(x => x.odaID == odaid).FirstOrDefault();
                if (seo.Seo.AdresDuzenle(odalar.adi) == adi)
                {
                    return View(odalar);
                }
                else
                {
                    ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Randevu(string adi_soyad, string mail, string telefon, string giris, string cikis, int odaID)
        {
            ViewModel vw = new ViewModel();
            var oda = db.Odalar.Find(odaID);


            if (cikis != null && cikis != "" && cikis != " " && giris != null && giris != "" && giris != " ")
            {
                var girisDate = Convert.ToDateTime(giris);
                var cikisDate = Convert.ToDateTime(cikis);
                int NumberOfDays = Convert.ToInt32((cikisDate.Day - girisDate.Day) * oda.fiyat);

                var test = db.Randevu.Where(x => x.odaID == odaID).ToList();
                List<DateTime> dbDateList = new List<DateTime>();

                foreach (var item in test)
                {
                    var gunGiris = item.giris;
                    var gunCikis = item.cikis;
                    while (gunGiris <= gunCikis)
                    {
                        dbDateList.Add(gunGiris);
                        gunGiris = gunGiris.AddDays(1);
                    }
                }

                while (girisDate <= cikisDate)
                {
                    if (dbDateList.FindIndex(x => x == girisDate) > -1)
                    {
                        TempData["RandevuEklenmedi"] = "RandevuEklenmedi";
                        // Yöntem1 : return Redirect("/Odalar/"+ seo.Seo.AdresDuzenle(oda.adi) + "-" + oda.odaID ); Zor durumda kalmadıkça kullanmayın 

                        return RedirectToAction("Odalar", new { odaid = oda.odaID, adi = seo.Seo.AdresDuzenle(oda.adi) });
                    }
                    girisDate = girisDate.AddDays(1);
                }

                Randevu a = new Randevu()
                {

                    ad_soyad = adi_soyad,
                    mail = mail,
                    telefon = telefon,
                    giris = Convert.ToDateTime(giris),
                    cikis = Convert.ToDateTime(cikis),
                    odaID = odaID,
                    fiyat = NumberOfDays,
                    zaman = DateTime.Now
                };


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.vatansms.net/api/v1/1toN");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    sms sms = new sms()
                    {
                        api_id = "6020a3d225bce3cf20fab86d",
                        api_key = "a33cde79443ef1ecd00b733a",
                        sender = "K.SAMET U.",
                        message_type = "turkce",
                        message = a.ad_soyad + " " + giris + " - " + cikis + " tarihleri arasında " + Proje.Aktif.SiteAyar.tesisadi + "  " + "tesisinde randevu oluşturdu." + " " +  "mail: " + a.mail + " " + "telefon: " + a.telefon,
                    };

                    sms.phones = new string[] { "5418674063","5533841291"};

                    var json = new JavaScriptSerializer().Serialize(sms);


                    //string json = @"{""api_id"": ""6020a3d225bce3cf20fab86d"",""api_key"": ""a33cde79443ef1ecd00b733a"",""sender"": ""K.SAMET U."",""message_type"": ""normal"",""message"":""Bungalovcum'a Hoşgeldiniz"",""phones"": [""5418674063""]}";

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                db.Randevu.Add(a);
                db.SaveChanges();
                TempData["RandevuEklendi"] = "RandevuEklendi";

            }
            else
            {
                TempData["tarihhata"] = "tarihhata";

            }

            return RedirectToAction("Odalar", new { odaid = oda.odaID, adi = seo.Seo.AdresDuzenle(oda.adi) });

        }




    }
}