using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace MeteoWebApplication.Models
{
    public class DailyData
    {
        [Required]
        [Display(Name="_path")]
        public string _path { get; set; }

        [Required]
        [Display(Name ="_station")]
        public int _station { get; set; }

        [Required]
 //       [Display(Name = "_date")]
 //       public string _date { get; set; }
        [DataType(DataType.Date)]
        public DateTime _date { get; set; }

        [Required]
        [Display(Name ="_userAction")]
        public IEnumerable<SelectListItem> _userAction { get; set; }

        //public string actionSelected { get; set; }
        public user_display_selection actionSelected;

        public DataTable _grid { get; set; }

        public Image _graph { get; set; }
        //public byte[] _graph { get; set; }
        public string graphPath { get; set; }
    }

    public enum user_display_selection
    {
        meteo_daily_grid,
        meteo_daily_graph
    }
}