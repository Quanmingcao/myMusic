namespace myMusic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reports
    {
        [Key]
        public int ReportID { get; set; }

        public int ReporterID { get; set; }

        public int? ReportedSongID { get; set; }

        public int? ReportedAccountID { get; set; }

        public int? ReportedCollectionID { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual Accounts Accounts { get; set; }

        public virtual Accounts Accounts1 { get; set; }

        public virtual Collections Collections { get; set; }

        public virtual Songs Songs { get; set; }
    }
}
