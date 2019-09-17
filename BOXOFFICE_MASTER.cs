namespace BoxOfficeCroller
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOXOFFICE_MASTER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDX { get; set; }

        [Required]
        [StringLength(200)]
        public string TITLE { get; set; }

        [Required]
        [StringLength(10)]
        public string REG_DT { get; set; }

        [StringLength(300)]
        public string OPT_1 { get; set; }

        [StringLength(300)]
        public string OPT_2 { get; set; }

        [StringLength(300)]
        public string OPT_3 { get; set; }
    }
}
