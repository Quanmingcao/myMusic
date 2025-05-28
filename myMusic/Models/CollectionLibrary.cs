namespace myMusic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CollectionLibrary")]
    public partial class CollectionLibrary
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CollectionID { get; set; }

        public DateTime? SavedAt { get; set; }

        public virtual Accounts Accounts { get; set; }

        public virtual Collections Collections { get; set; }
    }
}
