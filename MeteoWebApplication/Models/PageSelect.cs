using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MeteoWebApplication.Models
{
    public class PageSelect
    {
  //      [Required]
 //       public int _pageID { get; set; }

        [Required]
        [Display(Name ="_page")]
        public string _page { get; set; }

        public IEnumerable<SelectListItem> _pages { get; set; }
 //       public List<SelectListItem> _pages { get; set; }

        //public IEnumerable<SelectListItem> Page { get; set; }
        //public PageSelectEnum Page { get; set; }

/*        public PageSelect() { }
        
        public PageSelect(int index, string str)
        {
            _index = index;
            _page = str;
        } */
    }
/*
    public struct PageNames
    {
        public const string DailyDataGrid = "Griglia Dati Giornalieri";
        public const string DailyDataGraph = "Grafici Dati Giornalieri";
    }

    public enum PageSelectEnum
    {
        DailyDataGrid,
        DailyGraph
    }
    */
}