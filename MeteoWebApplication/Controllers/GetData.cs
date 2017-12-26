using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MeteoWebApplication.Models;

namespace MeteoWebApplication.Controllers
{
    public class GetData
    {
        public List<PeriodicAggregateDailyData> GetDailyData(DataTable grid)
        {
            DataRowCollection rows = grid.Rows;
            List<PeriodicAggregateDailyData> data = new List<PeriodicAggregateDailyData>();

            foreach(DataRow dr in rows)
            {               
                PeriodicAggregateDailyData dly = new PeriodicAggregateDailyData();
                dly.measureType = (string)dr.ItemArray[0];
                dly.um = (string)dr.ItemArray[1];
                dly.max = (string)dr.ItemArray[2];
                dly.hourmax = (string)dr.ItemArray[3];
                dly.min = (string)dr.ItemArray[4];
                dly.hourmin = (string)dr.ItemArray[5];
                dly.escursion = (string)dr.ItemArray[6];
                dly.nmeasurement = (string)dr.ItemArray[7];

                data.Add(dly);
            }

            return data;
        }
    }
}