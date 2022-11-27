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
using randevuProje.App_Start;
using System.Net;
using System.IO;
using static randevuProje.Constants;


namespace randevuProje.Controllers
{
    public class AdminController : Controller
    {
        RandevuContext db = new RandevuContext();


        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // kullanıcılardan veya yöneticilerden gelen isteklere (request) isteklerin doğruluğunu Tokenler aracılığıyla anlayıp ona göre cevap verir
        public ActionResult AdminLogin(Admin form)
        {
            ViewModel vw = new ViewModel();

            if (!ModelState.IsValid)  // box lar doğru validation şekilde doldurulmasını kontrol ediyor.
            {

                return View("Login", form);
            }


            using (RandevuContext db = new RandevuContext())
            {

                var AdminVarmi = db.Admin.FirstOrDefault(
                    x => x.adi_soyadi == form.adi_soyadi && x.sifre == form.sifre); //kullanıcı olup olmadığını kontrol ediyor.

                if (AdminVarmi != null)
                {
                    FormsAuthentication.SetAuthCookie(AdminVarmi.adi_soyadi, false); //kullanıcı kayıtlı kalıncaksa true olacak. (cookie)
                    Proje.Aktif.adminData = AdminVarmi;
                    return RedirectToAction("Randevular", "Admin");
                }


                ViewBag.Hata = "Kullanıcı Adı veya Şifre Hatalı"; //hata mesajı
                return View("Login"); // kullanıcı yok ise geri login sayfasına yönlendiriyor.

            }
        } //login post işlemleri ve kontrol..

        public ActionResult Logout()
        {
            Session.Abandon(); // seansı yok eder.
            FormsAuthentication.SignOut(); //cookie yi yok eder.
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Odalar()
        {
            try
            {
                ViewModel vw = new ViewModel()
                {
                    odalars = db.Odalar.ToList()
                };

                return View(vw);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Randevular()
        {

            try
            {
                ViewModel vw = new ViewModel()
                {

                    randevus = db.Randevu.ToList()

                };

                return View(vw);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }



        }

        [HttpPost]
        [Authorize]
        public ActionResult RandevuDuzenle(int randevuID, string ad_soyad, string telefon, bool aktif)
        {


            Randevu randevu = db.Randevu.Where(x => x.randevuID == randevuID).FirstOrDefault();

            randevu.ad_soyad = ad_soyad;
            randevu.telefon = telefon;
            randevu.aktif = aktif;

            db.SaveChanges();
            TempData["randevuguncellendi"] = "randevu Guncellendi";
            return RedirectToAction("Randevular");
        }

        [Authorize]
        public ActionResult Randevusil(int randevuID)
        {
            using (RandevuContext db = new RandevuContext())
            {
                var silinecekrandevu = db.Randevu.Find(randevuID);
                if (silinecekrandevu == null)
                {
                    return HttpNotFound();
                }
                db.Randevu.Remove(silinecekrandevu);
                db.Randevu.Remove(silinecekrandevu);
                db.SaveChanges();
                TempData["RandevuSilindi"] = "Randevular Silindi";
                return RedirectToAction("Randevular");
            }

        }

        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult OdaEkle(string adi, string kişi, string yatak, string ekyatak, string aciklama, decimal fiyat, HttpPostedFileBase kapakresim, bool aktif)
        {


            var etkin = db.Odalar.Where(x => x.adi == adi || x.adi == adi).SingleOrDefault();
            if (etkin != null)
            {
                TempData["TesisMevcut"] = "Tesis Mevcut";
                return RedirectToAction("OdaEkle");
            }

            if (Proje.Aktif != null)
            {

                Odalar b = new Odalar()
                {
                    adi = adi,
                    kişi = kişi,
                    yatak = yatak,
                    ekyatak = ekyatak,
                    aciklama = aciklama,
                    fiyat = fiyat,
                    aktif = aktif,
                    adminID = Proje.Aktif.adminData.adminID

                };
                if (Request.Files.Count > 0)
                {

                    string dosyaadi = Path.GetFileName(kapakresim.FileName);
                    string _filename = DateTime.Now.ToString("yyyyMMd_Hms_fff") + dosyaadi;
                    string yol = "~/Content/images/kapakresim/" + _filename;

                    if (yol == "~/Content/images/kapakresim/")
                    {
                        TempData["ResimHata"] = "ResimHata";
                    }
                    else
                    {
                        kapakresim.SaveAs(Server.MapPath(yol));
                        b.kapakresim = "/Content/images/kapakresim/" + _filename;

                        db.Odalar.Add(b);
                        db.SaveChanges();
                        TempData["TesisEklendi"] = "TesisEklendi";
                        return RedirectToAction("Odalar");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Odalar");
        }

        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult OdaDuzenle(int odaID, string adi, string kişi, string yatak, string ekyatak, string aciklama, decimal fiyat, HttpPostedFileBase kapakresim, bool aktif)
        {

            if (Proje.Aktif != null)
            {
                Odalar odalar = db.Odalar.Where(x => x.odaID == odaID).FirstOrDefault();

                odalar.adi = adi;
                odalar.kişi = kişi;
                odalar.yatak = yatak;
                odalar.ekyatak = ekyatak;
                odalar.aciklama = aciklama;
                odalar.fiyat = fiyat;
                odalar.aktif = aktif;
                odalar.adminID = Proje.Aktif.adminData.adminID;

                if (Request.Files.Count > 0 && kapakresim != null && kapakresim.ToString() != "")
                {

                    string dosyaadi = Path.GetFileName(kapakresim.FileName);
                    string _filename = DateTime.Now.ToString("yyyyMMd_Hms_fff") + dosyaadi;
                    string yol = "~/Content/images/kapakresim/" + _filename;

                    if (yol == "~/Content/images/kapakresim/")
                    {
                        TempData["ResimHata"] = "ResimHata";
                    }
                    else
                    {
                        kapakresim.SaveAs(Server.MapPath(yol));
                        odalar.kapakresim = "/Content/images/kapakresim/" + _filename;
                    }

                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            db.SaveChanges();
            TempData["odaguncellendi"] = "odaguncellendi";
            return RedirectToAction("Odalar");
        }

        [Authorize]
        public ActionResult Odasil(int odaID)
        {
            using (RandevuContext db = new RandevuContext())
            {
                var silinecekoda = db.Odalar.Find(odaID);
                if (silinecekoda == null)
                {
                    return HttpNotFound();
                }
                db.Odalar.Remove(silinecekoda);
                db.Odalar.Remove(silinecekoda);
                db.SaveChanges();
                TempData["KullaniciSilindi"] = "Kullanici Silindi";
                return RedirectToAction("Odalar");
            }

        }

        [Authorize]
        public ActionResult ResimEkle(int id)
        {
            try
            {
                ViewModel vw = new ViewModel();

                vw.resims = db.Resim.Where(x => x.odaID == id).ToList();
                vw.Odalars = db.Odalar.Where(x => x.odaID == id).FirstOrDefault();

                return View(vw);

                //if (vw.resims.Count > 0)
                //{
                //    if (seo.Seo.AdresDuzenle(vw.bungalov.isim) == bisim)
                //    {
                //        return View(vw);
                //    }
                //    else
                //    {
                //        ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                //        return RedirectToAction("Tesisler");
                //    }
                //}
                //else
                //{
                //    return View(vw);
                //}
            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        [HttpPost]
        public ActionResult ResimEkle(HttpPostedFileBase resim1, HttpPostedFileBase resim2, HttpPostedFileBase resim3, int odaID)
        {
            var bg = db.Odalar.Find(odaID);
            if (Proje.Aktif != null)
            {
                Resim resim = new Resim()
                {
                    odaID = odaID
                };

                if (db.Resim.Where(x => x.odaID == odaID).Count() == 0)
                {
                    string dosya = Server.MapPath("~/Content/images/odaresim/");
                    Directory.CreateDirectory(dosya);
                }

                if (Request.Files.Count > 0)
                {
                    if (resim1 != null)
                    {
                        string dosyaadi = Path.GetFileName(resim1.FileName);
                        string _filename = DateTime.Now.ToString("yyyyMMd_Hms_fff") + dosyaadi;
                        string yol = "/Content/images/odaresim/" + _filename;
                        resim.resim1 = yol;
                        db.Resim.Add(resim);
                        db.SaveChanges();
                        resim1.SaveAs(Server.MapPath(yol));
                    }

                    if (resim2 != null)
                    {
                        string dosyaadi1 = Path.GetFileName(resim2.FileName);
                        string _filename1 = DateTime.Now.ToString("yyyyMMd_Hms_fff") + dosyaadi1;
                        string yol1 = "/Content/images/odaresim/" + _filename1;
                        resim.resim1 = yol1;
                        db.Resim.Add(resim);
                        db.SaveChanges();
                        resim2.SaveAs(Server.MapPath(yol1));
                    }

                    if (resim3 != null)
                    {
                        string dosyaadi2 = resim3 != null ? Path.GetFileName(resim3.FileName) : "";
                        string _filename2 = DateTime.Now.ToString("yyyyMMd_Hms_fff") + dosyaadi2;
                        string yol2 = "/Content/images/odaresim/" + _filename2;
                        resim.resim1 = yol2;
                        db.Resim.Add(resim);
                        db.SaveChanges();
                        resim3.SaveAs(Server.MapPath(yol2));
                    }

                    TempData["ResimEklendi"] = "Resim Eklendi";
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("ResimEkle", new { id = bg.odaID, bisim = bg.adi });
        }

        [Authorize]
        public ActionResult ResimSil(int resimID, int odaID)
        {
            using (RandevuContext db = new RandevuContext())
            {
                var silinecekresim = db.Resim.Find(resimID);
                var bg = db.Odalar.Find(odaID);
                if (silinecekresim == null)
                {
                    return HttpNotFound();
                }
                db.Resim.Remove(silinecekresim);
                db.Resim.Remove(silinecekresim);
                db.SaveChanges();
                TempData["ResimSilindi"] = "Resim Silindi";
                return RedirectToAction("ResimEkle", new { id = bg.odaID, bisim = bg.adi });
            }

        }

        [Authorize]
        public ActionResult Kullanicilar()
        {
            if (Proje.Aktif.adminData.Yetki.yetki1 != "Admin")
            {
                return RedirectToAction("Tesisler");
            }
            KullaniciViewModel vw = new KullaniciViewModel();

            vw.admins = db.Admin.ToList();
            vw.yetkis = db.Yetki.ToList();

            return View(vw);
        }

        [HttpPost]
        public ActionResult KullaniciEkle(string adi_soyadi, string sifre, int yetkiID)
        {
            ViewModel vw = new ViewModel();

            var yon = db.Admin.Where(x => x.adi_soyadi == adi_soyadi).SingleOrDefault();
            if (yon != null)
            {
                TempData["KullaniciEklenmedi"] = "KullaniciEklenmedi";
            }
            else
            {
                Admin a = new Admin()
                {
                    adi_soyadi = adi_soyadi,
                    sifre = sifre,
                    yetkiID = yetkiID

                };

                db.Admin.Add(a);
                db.SaveChanges();
                TempData["KullaniciEklendi"] = "KullaniciEklendi";
                return RedirectToAction("Kullanicilar");
            }

            return RedirectToAction("Kullanicilar");

        }

        [HttpPost]
        [Authorize]
        public ActionResult KullaniciDuzenle(int adminID, string adi_soyadi, int yetkiID)
        {


            Admin admin = db.Admin.Where(x => x.adminID == adminID).FirstOrDefault();

            admin.adi_soyadi = adi_soyadi;
            admin.yetkiID = yetkiID;

            db.SaveChanges();
            TempData["kullaniciguncellendi"] = "kullanici Guncellendi";
            return RedirectToAction("Kullanicilar");
        }

        [Authorize]
        public ActionResult KullaniciSil(int adminID)
        {
            using (RandevuContext db = new RandevuContext())
            {
                var silinecekadmin = db.Admin.Find(adminID);
                if (silinecekadmin == null)
                {
                    return HttpNotFound();
                }
                db.Admin.Remove(silinecekadmin);
                db.Admin.Remove(silinecekadmin);
                db.SaveChanges();
                TempData["KullaniciSilindi"] = "Kullanici Silindi";
                return RedirectToAction("Kullanicilar");
            }

        }


        [HttpPost]
        public ActionResult YetkiEkle(string yetki)
        {
            ViewModel vw = new ViewModel();

            var yon = db.Yetki.Where(x => x.yetki1 == yetki).SingleOrDefault();
            if (yon != null)
            {
                TempData["YetkiEklenmedi"] = "YetkiEklenmedi";
            }
            else
            {
                Yetki a = new Yetki()
                {
                    yetki1 = yetki,

                };

                db.Yetki.Add(a);
                db.SaveChanges();
                TempData["YetkiEklendi"] = "YetkiEklendi";
                return RedirectToAction("Kullanicilar");
            }

            return RedirectToAction("Kullanicilar");

        }

        [HttpPost]
        [Authorize]
        public ActionResult YetkiDuzenle(int yetkiID, string yetki)
        {


            Yetki yetki1 = db.Yetki.Where(x => x.yetkiID == yetkiID).FirstOrDefault();

            yetki1.yetki1 = yetki;

            db.SaveChanges();
            TempData["yetkiguncellendi"] = "yetki Guncellendi";
            return RedirectToAction("Kullanicilar");
        }

        [Authorize]
        public ActionResult YetkiSil(int yetkiID)
        {
            using (RandevuContext db = new RandevuContext())
            {
                var silinecekyetki = db.Yetki.Find(yetkiID);
                if (silinecekyetki == null)
                {
                    return HttpNotFound();
                }
                db.Yetki.Remove(silinecekyetki);
                db.Yetki.Remove(silinecekyetki);
                db.SaveChanges();
                TempData["YetkiSilindi"] = "Kullanici Silindi";
                return RedirectToAction("Kullanicilar");
            }

        }

        [Authorize]
        public ActionResult SiteAyar(int id)
        {
            try
            {
                id = 1;
                SiteAyarViewModel model = new SiteAyarViewModel();
                model.siteAyar = db.Siteayar.Where(x => x.siteayarID == id).FirstOrDefault();


                return View(model);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SiteAyarları(int siteayarID, string baslik, string tesisadi, string slider_aciklama, HttpPostedFileBase slider_resim, string secenek, string numara1, string numara2, string konum, string mail1, string mail2, string harita, string instagram, string _abstract, string description, string keywords)
        {
            Siteayar ayar = db.Siteayar.Where(x => x.siteayarID == siteayarID).FirstOrDefault();

            ayar.baslik = baslik;
            ayar.tesisadi = tesisadi;
            ayar.slider_aciklama = slider_aciklama;
            ayar.secenek = secenek;
            ayar.numara1 = numara1;
            ayar.numara2 = numara2;
            ayar.konum = konum;
            ayar.mail1 = mail1;
            ayar.mail2 = mail2;
            ayar.harita = harita;
            ayar.instagram = instagram;
            ayar._abstract = _abstract;
            ayar.description = description;
            ayar.keywords = keywords;

            if (Request.Files.Count > 0 && slider_resim != null)
            {
                string dosyaadi = Path.GetFileName(slider_resim.FileName);
                string _filename = DateTime.Now.ToString("yyyyMMd_Hms_fff") + dosyaadi;
                string yol = "~/Content/images/" + _filename;

                if (yol == "~/Content/images/")
                {
                    TempData["ResimHata"] = "ResimHata";
                }
                else
                {
                    slider_resim.SaveAs(Server.MapPath(yol));
                    ayar.slider_resim = "/Content/images/" + _filename;

                    
                }

            }

            TempData["Kaydedildi"] = "Kaydedildi";
            db.SaveChanges();
            return RedirectToAction("SiteAyar/1");
        }

        [Authorize]
        public ActionResult Ozellik(int id)
        {
            try
            {
                ViewModel vw = new ViewModel();

                vw.ozellik1 = db.ozellik1.Where(x => x.odaID == id).ToList();
                vw.ozellik2 = db.ozellik2.Where(x => x.odaID == id).ToList();
                vw.ozellik3 = db.ozellik3.Where(x => x.odaID == id).ToList();

                vw.Odalars = db.Odalar.Where(x => x.odaID == id).FirstOrDefault();

                return View(vw);

                //if (vw.resims.Count > 0)
                //{
                //    if (seo.Seo.AdresDuzenle(vw.bungalov.isim) == bisim)
                //    {
                //        return View(vw);
                //    }
                //    else
                //    {
                //        ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                //        return RedirectToAction("Tesisler");
                //    }
                //}
                //else
                //{
                //    return View(vw);
                //}
            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        [HttpPost]
        public ActionResult OzellikEkle(string ozellik1, string icon1, bool aktif1,string ozellik2, string icon2, bool aktif2,string ozellik3, string icon3, bool aktif3, int odaID)
        {
            var bg = db.ozellik1.Find(odaID);

            ozellik1 o1 = new ozellik1()
            {
                ozellik = ozellik1,
                icon = icon1,
                aktif = aktif1

            };
            ozellik2 o2 = new ozellik2()
            {
                ozellik = ozellik2,
                icon = icon2,
                aktif = aktif2

            };
            ozellik3 o3 = new ozellik3()
            {
                ozellik = ozellik3,
                icon = icon3,
                aktif = aktif3

            };

            return RedirectToAction("ResimEkle", new { id = bg.odaID, bisim = bg.Odalar.adi });
        }

    }


}