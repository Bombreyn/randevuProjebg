namespace randevuProje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Randevu")]
    public partial class Randevu
    {
        public int randevuID { get; set; }

        public int? odaID { get; set; }

        [StringLength(50)]
        public string ad_soyad { get; set; }

        [StringLength(50)]
        public string mail { get; set; }

        [StringLength(50)]
        public string telefon { get; set; }

        public DateTime giris { get; set; }

        public DateTime cikis { get; set; }

        [Column(TypeName = "money")]
        public decimal? fiyat { get; set; }

        public DateTime zaman { get; set; }

        public bool? aktif { get; set; }

        public virtual Odalar Odalar { get; set; }
    }
}
