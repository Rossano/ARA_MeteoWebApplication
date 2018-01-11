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

        public Image _graph { get; set; }
        public string graphPath { get; set; }        
    }
}