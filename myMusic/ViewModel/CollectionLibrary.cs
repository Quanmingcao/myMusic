using myMusic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace myMusic.ViewModel
{
    public class CollectionLibrary
    {
        [Key]
        public int ID { get; set; }

        public int CollectionID { get; set; }

        public int AccountID { get; set; }

        public virtual Collections Collections { get; set; }

        public virtual Accounts Accounts { get; set; }
    }
}