namespace randevuProje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ozellik1
    {
        public int ozellik1ID { get; set; }

        [StringLength(50)]
        public string ozellik { get; set; }

        [StringLength(50)]
        public string icon { get; set; }

        public int? odaID { get; set; }

        public bool? aktif { get; set; }

        public virtual Odalar Odalar { get; set; }
    }
}
