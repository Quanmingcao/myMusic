namespace myMusic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Collections
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Collections()
        {
            CollectionLibrary = new HashSet<CollectionLibrary>();
            CollectionSongs = new HashSet<CollectionSongs>();
            Reports = new HashSet<Reports>();
        }

        [Key]
        public int CollectionID { get; set; }

        public int AccountID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int TypeID { get; set; }

        [StringLength(255)]
        public string CoverImage { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsLocked { get; set; }

        public virtual Accounts Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLibrary> CollectionLibrary { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionSongs> CollectionSongs { get; set; }

        public virtual CollectionTypes CollectionTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reports> Reports { get; set; }
    }
}
