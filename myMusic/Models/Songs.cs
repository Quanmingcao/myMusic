namespace myMusic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Songs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Songs()
        {
            CollectionSongs = new HashSet<CollectionSongs>();
            Likes = new HashSet<Likes>();
            ListeningHistory = new HashSet<ListeningHistory>();
            Reports = new HashSet<Reports>();
        }

        [Key]
        public int SongID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? GenreID { get; set; }

        public int? Duration { get; set; }

        [StringLength(255)]
        public string CoverImage { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        public DateTime? UploadDate { get; set; }

        public int? Views { get; set; }

        public int AccountID { get; set; }

        public virtual Accounts Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionSongs> CollectionSongs { get; set; }

        public virtual Genres Genres { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Likes> Likes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListeningHistory> ListeningHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reports> Reports { get; set; }
    }
}
