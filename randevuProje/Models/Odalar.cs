namespace randevuProje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Odalar")]
    public partial class Odalar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Odalar()
        {
            Kurallar = new HashSet<Kurallar>();
            ozellik1 = new HashSet<ozellik1>();
            ozellik2 = new HashSet<ozellik2>();
            ozellik3 = new HashSet<ozellik3>();
            Randevu = new HashSet<Randevu>();
            Resim = new HashSet<Resim>();
        }

        [Key]
        public int odaID { get; set; }

        [StringLength(50)]
        public string adi { get; set; }

        [StringLength(50)]
        public string kişi { get; set; }

        [StringLength(50)]
        public string yatak { get; set; }

        [StringLength(50)]
        public string ekyatak { get; set; }

        [Column(TypeName = "text")]
        public string aciklama { get; set; }

        [Column(TypeName = "money")]
        public decimal? fiyat { get; set; }

        [Column(TypeName = "text")]
        public string kapakresim { get; set; }

        public int? adminID { get; set; }

        public bool? aktif { get; set; }

        public virtual Admin Admin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kurallar> Kurallar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ozellik1> ozellik1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ozellik2> ozellik2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ozellik3> ozellik3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Randevu> Randevu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resim> Resim { get; set; }
    }
}
