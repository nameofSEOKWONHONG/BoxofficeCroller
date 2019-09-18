namespace BoxOfficeCroller
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BoxofficeDbContext : DbContext
    {
        public BoxofficeDbContext()
            : base("name=DbContext")
        {
            Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
        }

        public virtual DbSet<BOXOFFICE_DETAIL> TA_NEO_BOXOFFICE_DETAIL { get; set; }
        public virtual DbSet<BOXOFFICE_MASTER> TA_NEO_BOXOFFICE_MASTER { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BOXOFFICE_DETAIL>()
                .Property(e => e.M_NM)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_DETAIL>()
                .Property(e => e.M_ID)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_DETAIL>()
                .Property(e => e.OPEN_DT)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_MASTER>()
                .Property(e => e.TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_MASTER>()
                .Property(e => e.REG_DT)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_MASTER>()
                .Property(e => e.OPT_1)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_MASTER>()
                .Property(e => e.OPT_2)
                .IsUnicode(false);

            modelBuilder.Entity<BOXOFFICE_MASTER>()
                .Property(e => e.OPT_3)
                .IsUnicode(false);
        }
    }
}
