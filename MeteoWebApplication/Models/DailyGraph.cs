using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace MeteoWebApplication.Models
{
    public class DailyGraph
    {
        [Required]
        public string _path { get; set; }

        [Required]
        public int _station { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime _date { get; set; }

        public Image _graphTemp { get; set; }
        public Image _graphTempMaxMin { get; set; }
        public Image _graphTempDewP { get; set; }
        public Image _graphTempHeat { get; set; }
        public Image _graphTempWindChill { get; set; }
        public Image _graphWind { get; set; }
        public Image _graphRain { get; set; }
        public string graphPath1 { get; set; }
        public string graphPath2 { get; set; }
        public string graphPath3 { get; set; }
        public string graphPath4 { get; set; }
        public string graphPath5 { get; set; }
        public string graphPath6 { get; set; }
        public string graphPath7 { get; set; }
    }
}