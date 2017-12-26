using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCGrid.Models;
using MVCGrid.Web;
//using MVCGrid.Web.Data;
//using MVCGrid.Web.Models;
using MeteoWebApplication.Models;
using System.Web.Mvc;

namespace MeteoWebApplication.App_Start
{
    public class MVCGridConfig
    {
        public static void RegsterGrids()
        {
            ColumnDefaults colDefaults = new ColumnDefaults()
            {
                EnableSorting = true
            };
        }
    }
}