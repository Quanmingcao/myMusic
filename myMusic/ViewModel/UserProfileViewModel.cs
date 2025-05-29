using myMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myMusic.ViewModel
{
    public class UserProfileViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int TotalViews { get; set; }
        public List<Songs> Songs { get; set; }

    }

}