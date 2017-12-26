using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MeteoWebApplication.Models
{
    public class DailyMeteoData
    {
        public DateTime _date { get; set; }
        public DataTable _table { get; set; }
        public DataRow measureType { get; set; }
        public DataRow um { get; set; }
    }
}