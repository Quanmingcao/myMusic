using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myMusic.ViewModel
{
    public class SongViewModel
    {
        public int SongID { get; set; }
        public string Title { get; set; }
        public DateTime? UploadDate { get; set; }
        public int? Views { get; set; }
        public string Username { get; set; }
        public string CoverImage { get; set; }
        public string FilePath { get; set; }
    }
}