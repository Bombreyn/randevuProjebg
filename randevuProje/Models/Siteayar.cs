namespace randevuProje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Siteayar")]
    public partial class Siteayar
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int siteayarID { get; set; }

        [StringLength(50)]
        public string baslik { get; set; }

        [StringLength(50)]
        public string tesisadi { get; set; }

        [Column(TypeName = "text")]
        public string slider_aciklama { get; set; }

        [Column(TypeName = "text")]
        public string slider_resim { get; set; }

        [StringLength(50)]
        public string secenek { get; set; }

        [StringLength(50)]
        public string numara1 { get; set; }

        [StringLength(50)]
        public string numara2 { get; set; }

        [StringLength(50)]
        public string konum { get; set; }

        [StringLength(50)]
        public string mail1 { get; set; }

        [StringLength(50)]
        public string mail2 { get; set; }

        [Column(TypeName = "text")]
        public string harita { get; set; }

        [StringLength(50)]
        public string instagram { get; set; }

        [Column("abstract", TypeName = "text")]
        public string _abstract { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column(TypeName = "text")]
        public string keywords { get; set; }
    }
}
