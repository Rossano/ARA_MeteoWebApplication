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
//        public user_display_selection actionSelected;

        public DataTable _gridDaily { get; set; }
        public DataTable _gridHourlyAgg { get; set; }
        public DataTable _gridPredawn { get; set; }
        public DataTable _gridDailyMorning { get; set; }
        public DataTable _gridDailyNoon { get; set; }
        public DataTable _gridDailyEvening { get; set; }
        public DataTable _gridDailyRain { get; set; }
        public DataTable _gridRainPredawn { get; set; }
        public DataTable _gridRainMorning { get; set; }
        public DataTable _gridRainNoon { get; set; }
        public DataTable _gridRainEvening { get; set; }
        public DataTable _gridHeat { get; set; }

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