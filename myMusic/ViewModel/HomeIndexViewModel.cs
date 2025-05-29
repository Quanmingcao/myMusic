using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using myMusic.Models;


namespace myMusic.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<Songs> TopViewedSongs { get; set; }
        public List<Songs> AllSongs { get; set; }
    }
}