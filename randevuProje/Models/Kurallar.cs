namespace randevuProje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kurallar")]
    public partial class Kurallar
    {
        [Key]
        public int kuralID { get; set; }

        [Column(TypeName = "text")]
        public string kural { get; set; }

        public int? odaID { get; set; }

        public virtual Odalar Odalar { get; set; }
    }
}
