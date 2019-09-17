namespace BoxOfficeCroller
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOXOFFICE_DETAIL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDX { get; set; }

        public int MASTER_IDX { get; set; }

        public int RANK { get; set; }

        public string RANK_FLAG { get; set; }

        public int RANK_INC { get; set; }

        [Required]
        [StringLength(200)]
        public string M_NM { get; set; }

        [StringLength(20)]
        public string M_ID { get; set; }

        [Required]
        [StringLength(10)]
        public string OPEN_DT { get; set; }

        public double SALES { get; set; }

        public double SALES_RATE { get; set; }

        public double SALES_INC { get; set; }

        public double SALES_CUM { get; set; }

        public double ADN { get; set; }

        public double ADN_INC { get; set; }

        public double ADN_CUM { get; set; }

        public double SCREEN_CNT { get; set; }

        public double PLAY_TIMES { get; set; }
    }
}
