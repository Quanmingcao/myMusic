using myMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myMusic.ViewModel
{
    public class CollectionDetailsViewModel
    {

        public int CollectionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string Username { get; set; }
        public string TypeName { get; set; }
        public DateTime? CreatedAt { get; set; }

        public List<Songs> Songs { get; set; }
    }
}