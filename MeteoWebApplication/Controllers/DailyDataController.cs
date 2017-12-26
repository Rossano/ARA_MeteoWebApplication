using MeteoWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Asp.Meteo.Utility;
using Asp.Meteo.Viewer;
using Asp.Meteo.Utility.Graph;
using Asp.Meteo.Utility;


namespace MeteoWebApplication.Controllers
{
    public class DailyDataController : Controller
    {
        // GET: DailyData
        public ActionResult Index()
        {
            //var states = GetAllStates();

            //var model = new DailyData();
            //model._userAction = GetSelectListItems(states);

            //return View(model);
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Griglia Dati Giornalieri", Value = "0" });
            items.Add(new SelectListItem { Text = "Grafici Dati Giornalieri", Value = "1" });

            ViewBag._userAction = items;

            return View();
        }

        [HttpPost]
        public ActionResult Index(DailyData model)
        {
            //var states = GetAllStates();

            //model._userAction = GetSelectListItems(states);
            //if (model._userAction.Equals(GetAllStates().ElementAt(0)))
            // if(model._userAction.ElementAt(0).Selected)
            //if(ModelState.IsValid)
            // {

            //     Session["Display Daily Data Grid"] = model;
            //     return RedirectToAction("DailyDataGrid");
            // }
            // else return View();

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Griglia Dati Giornalieri", Value = "0" });
            items.Add(new SelectListItem { Text = "Grafici Dati Giornalieri", Value = "1" });

            ViewBag._userAction = items;

            return View();
        }

        public ViewResult PageChosen(DailyData model, string actionChosen)
        {
            switch(model.actionSelected)
            {
                case user_display_selection.meteo_daily_graph: return View(); break;
                case user_display_selection.meteo_daily_grid: //return View("DailyDataGrid"); break;
                    try
                    {
                        model._path = "C:\\Test";
                        DateTime date = DateTime.ParseExact(model._date, "yyyyMMdd", null);


                        string pf = AspPath.ComposePath(model._path, AspUtility.ComposeMeteoDirectoryPath(AspDirectoryDeepType.Day, model._station, date));
                        FFile Readfile = new FFile(model._station, pf);
                        string fpf = Readfile.Read(AspUtility.LiteralDecodeElaborationType(AspElaborationType.DAY));

                        AspRDailyData rdg = new AspRDailyData(fpf);
                        if (rdg.Read())
                        {
                            model._grid = AspPeriodicDataUtility.PeriodicDailyDataSnapshotPresentation(rdg.Daily);
                        }

                        /*model._grid.Columns.Add(new DataColumn("Misura", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("U.M.", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("Max", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("H. Max", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("Min", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("H. Min", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("Media", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("Escursione", typeof(string)));
                        model._grid.Columns.Add(new DataColumn("N. Misure", typeof(string)));*/

                        //                Queue<ComboSelectionFileParameters> parlist = new Queue<ComboSelectionFileParameters>();
                        ReadMeteoParametersFiles rpars = new ReadMeteoParametersFiles(model._station, model._path);
                        rpars.DelayDeleteMinutes = 0D;
                        rpars.IsDeleteOldFiles = true;
                        //                rpars.Error += Rpars_Error;

                        // per i colori: http://www.computerhope.com/htmcolor.htm
                        // temperatura
                        AspMeteoParametersType[] p0 = new AspMeteoParametersType[1];
                        p0[0] = AspMeteoParametersType.Temperature;
                        AspMeteoParametersDataGroup dp0 = rpars.Read(p0, date);
                        if (dp0.Isvalid)
                        {
                            /*                   ComboSelectionFileParameters cmbp0 = new ComboSelectionFileParameters();
                                               cmbp0.Text = "Temperatura";
                                               cmbp0.Params = dp0;
                                               cmbp0.color = new Color[1];
                                               cmbp0.color[0] = ColorTranslator.FromHtml("#E56717"); // Papaya - arancione scuro
                                               cmbp0.Title = "Temperatura";
                                               cmbp0.AxisX = string.Format("Orario (data: {0:dd/MM/yyyy})", date);
                                               cmbp0.AxixY = "Gradi centigradi";
                                               cmbp0.TypeGraph = AspTypeGrapEnum.Line;
                                               cmbp0.Delta = 15;
                                               parlist.Enqueue(cmbp0); */
                            AspGraphInfoType info;
                            info.colors = new System.Drawing.Color[1];
                            info.parameters = dp0;
                            info.tollerance = 15;
                            info.type = AspTypeGrapEnum.Line;

                            AspDailyGraph graph = new AspDailyGraph(600, 480);
                            graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                            graph.AxisYLabel = "Gradi Centigradi";
                            graph.PointSize = 5;
                            graph.Title = "Temperatura";

                            if(graph.Create(info, date))
                            {
                                model._graph = graph.GetImage;
                               // byte[] foo = File.ReadAllBytes(graph.GetImage);
                                //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);

                            }
                        }
                        return View("DailyDataGrid", model);
                    }
                    catch (Exception ex)
                    {
                        return View("Error.cshtml");
                    }

            }
            return View();
        }

        // GET:
        public ActionResult DailyDataGrid(DailyData model)
        {
            try
            {
                DateTime date=DateTime.ParseExact(model._date, "yyyyMMdd", null);


                string pf = AspPath.ComposePath(model._path, AspUtility.ComposeMeteoDirectoryPath(AspDirectoryDeepType.Day, model._station, date));
                FFile Readfile = new FFile(model._station, pf);
                string fpf = Readfile.Read(AspUtility.LiteralDecodeElaborationType(AspElaborationType.DAY));

                AspRDailyData rdg = new AspRDailyData(fpf);
                model._grid = AspPeriodicDataUtility.PeriodicDailyDataSnapshotPresentation(rdg.Daily);

                return View(model);
            }
            catch
            {
                return Redirect("Error.cshtml");
            }
            //return RedirectToAction("Index");
        }

        private IEnumerable<string> GetAllStates()
        {
            return new List<string>
            {
                "Griglia Dati Giornalieri",
                "Grafici Dati Giornalieri"
            };
        }
        
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> _list)
        {
            var selectList = new List<SelectListItem>();

            foreach(var el in _list)
            {
                selectList.Add(new SelectListItem
                {
                    Value = el,
                    Text = el
                });
            }
            return selectList;
        }
        
    }

    public class AspPeriodicDataUtility
    {
        public static AspMeteoParametersType[] SequenceSnapshotMeasureData()
        {
            AspMeteoParametersType[] sq = new AspMeteoParametersType[13];

            sq[0] = AspMeteoParametersType.Temperature;
            sq[1] = AspMeteoParametersType.Pressure;
            sq[2] = AspMeteoParametersType.Umidity;
            sq[3] = AspMeteoParametersType.Evapotraspiration;
            sq[4] = AspMeteoParametersType.Wind;
            sq[5] = AspMeteoParametersType.WindAngle;
            sq[6] = AspMeteoParametersType.DewPoint;
            sq[7] = AspMeteoParametersType.HeathIndex;
            sq[8] = AspMeteoParametersType.Thom;
            sq[9] = AspMeteoParametersType.WindChill;
            sq[10] = AspMeteoParametersType.WinterScharlau;
            sq[11] = AspMeteoParametersType.UV;
            sq[12] = AspMeteoParametersType.SolRad;

            return sq;
        }
        //---
        //+--
        public static AspMeteoParametersType[] SequenceHourlyMeasureData()
        {
            AspMeteoParametersType[] sq = new AspMeteoParametersType[13];

            sq[0] = AspMeteoParametersType.Temperature;
            sq[1] = AspMeteoParametersType.Umidity;
            sq[2] = AspMeteoParametersType.Pressure;
            sq[3] = AspMeteoParametersType.DewPoint;
            sq[4] = AspMeteoParametersType.Wind;
            sq[5] = AspMeteoParametersType.WindAngle;
            sq[6] = AspMeteoParametersType.WindChill;
            sq[7] = AspMeteoParametersType.HeathIndex;
            sq[8] = AspMeteoParametersType.Thom;
            sq[9] = AspMeteoParametersType.UV;
            sq[10] = AspMeteoParametersType.Evapotraspiration;
            sq[11] = AspMeteoParametersType.SolRad;
            sq[12] = AspMeteoParametersType.Rain;

            return sq;
        }
        //---
        //+--
        public static DataTable PeriodicDailyDataSnapshotPresentation(AspDataReadRecordType[] records)
        {
            DataTable fdt = new DataTable();
            fdt.Columns.Add(new DataColumn("measuretype", typeof(string)));
            fdt.Columns.Add(new DataColumn("um", typeof(string)));
            fdt.Columns.Add(new DataColumn("max", typeof(string)));
            fdt.Columns.Add(new DataColumn("hourmax", typeof(string)));
            fdt.Columns.Add(new DataColumn("min", typeof(string)));
            fdt.Columns.Add(new DataColumn("hourmin", typeof(string)));
            fdt.Columns.Add(new DataColumn("mean", typeof(string)));
            fdt.Columns.Add(new DataColumn("Escursion", typeof(string)));
            fdt.Columns.Add(new DataColumn("nmisure", typeof(string)));

            for (int i = 0; i < records.Length; i++)
            {
                DataRow nr = fdt.NewRow();
                AspMeteoParametersType type = AspUtility.ReverseDecodeMeteoParametersType(records[i].measuretype);
                AspMeasureDescriptioType dtype = AspUtility.LiteralDecodeMeteoParametersType(type);

                nr["measuretype"] = dtype.Description;
                nr["um"] = dtype.MeasureUnit;
                nr["max"] = float.IsNaN(records[i].max) ? AspUtility.BadDataPresentation : records[i].max.ToString("#0.0");
                nr["hourmax"] = DateTime.MinValue.CompareTo(records[i].datemax) == 0 ? AspUtility.BadDataPresentation : records[i].datemax.ToString("HH:mm");
                nr["min"] = float.IsNaN(records[i].min) ? AspUtility.BadDataPresentation : records[i].min.ToString("#0.0");
                nr["hourmin"] = DateTime.MinValue.CompareTo(records[i].datemin) == 0 ? AspUtility.BadDataPresentation : records[i].datemin.ToString("HH:mm");
                nr["mean"] = float.IsNaN(records[i].mean) ? AspUtility.BadDataPresentation : records[i].mean.ToString("#0.0");
                nr["Escursion"] = float.IsNaN(records[i].excursion) ? AspUtility.BadDataPresentation : records[i].excursion.ToString("#0.0");
                nr["nmisure"] = float.IsNaN(records[i].nmisure) ? AspUtility.BadDataPresentation : records[i].nmisure.ToString("#0");

                fdt.Rows.Add(nr);
            }

            return fdt;
        }
        //---
        //+--
        public static DataTable PeriodicDailyDataHourlyPresentation(AspDataReadSequenceRecordType[] records)
        {
            DataTable fdt = new DataTable();
            fdt.Columns.Add(new DataColumn("hfrom", typeof(string)));
            fdt.Columns.Add(new DataColumn("hto", typeof(string)));
            fdt.Columns.Add(new DataColumn("temp", typeof(string)));
            fdt.Columns.Add(new DataColumn("umid", typeof(string)));
            fdt.Columns.Add(new DataColumn("press", typeof(string)));
            fdt.Columns.Add(new DataColumn("dewpoint", typeof(string)));
            fdt.Columns.Add(new DataColumn("windmax", typeof(string)));
            fdt.Columns.Add(new DataColumn("wind", typeof(string)));
            fdt.Columns.Add(new DataColumn("windangle", typeof(string)));
            fdt.Columns.Add(new DataColumn("heat", typeof(string)));
            fdt.Columns.Add(new DataColumn("thom", typeof(string)));
            fdt.Columns.Add(new DataColumn("windchill", typeof(string)));
            fdt.Columns.Add(new DataColumn("uv", typeof(string)));
            fdt.Columns.Add(new DataColumn("rad", typeof(string)));
            fdt.Columns.Add(new DataColumn("ev", typeof(string)));
            fdt.Columns.Add(new DataColumn("rain", typeof(string)));

            for (int i = 0; i < records.Length; i++)
            {
                DataRow dtr = fdt.NewRow();

                dtr["hfrom"] = AspUtility.MorphHourToString(records[i].hfrom);
                dtr["hto"] = AspUtility.MorphHourToString(records[i].hto);
                dtr["temp"] = float.IsNaN(records[i].temperature) ? AspUtility.BadDataPresentation : records[i].temperature.ToString("#0.0");
                dtr["umid"] = float.IsNaN(records[i].umidity) ? AspUtility.BadDataPresentation : records[i].umidity.ToString("#0.0");
                dtr["press"] = float.IsNaN(records[i].pressure) ? AspUtility.BadDataPresentation : records[i].pressure.ToString("#0.0");
                dtr["dewpoint"] = float.IsNaN(records[i].dewpoint) ? AspUtility.BadDataPresentation : records[i].dewpoint.ToString("#0.0");
                dtr["windmax"] = float.IsNaN(records[i].windmax) ? AspUtility.BadDataPresentation : records[i].windmax.ToString("#0.0");
                dtr["wind"] = float.IsNaN(records[i].wind) ? AspUtility.BadDataPresentation : records[i].wind.ToString("#0.0");
                dtr["windangle"] = float.IsNaN(records[i].windangle) ? AspUtility.BadDataPresentation : records[i].windangle.ToString("#0.0");
                dtr["heat"] = float.IsNaN(records[i].heat) ? AspUtility.BadDataPresentation : records[i].heat.ToString("#0.0");
                dtr["thom"] = float.IsNaN(records[i].thom) ? AspUtility.BadDataPresentation : records[i].thom.ToString("#0.0");
                dtr["windchill"] = float.IsNaN(records[i].windchill) ? AspUtility.BadDataPresentation : records[i].windchill.ToString("#0.0");
                dtr["uv"] = float.IsNaN(records[i].uv) ? AspUtility.BadDataPresentation : records[i].uv.ToString("#0.0");
                dtr["rad"] = float.IsNaN(records[i].radiation) ? AspUtility.BadDataPresentation : records[i].radiation.ToString("#0.0");
                dtr["ev"] = float.IsNaN(records[i].evapotraspiration) ? AspUtility.BadDataPresentation : records[i].evapotraspiration.ToString("#0.0");
                dtr["rain"] = float.IsNaN(records[i].rain) ? AspUtility.BadDataPresentation : records[i].rain.ToString("#0.0");

                fdt.Rows.Add(dtr);
            }

            return fdt;
        }
        //---
        //+--
        public static DataTable PeriodicDailyDataHeatIndexesPresentation(AspDataReadHeatRecordType heat, AspDataReadHeatRecordType thom)
        {
            DataTable fdt = new DataTable();

            fdt.Columns.Add(new DataColumn("Measure", typeof(string)));
            fdt.Columns.Add(new DataColumn("um", typeof(string)));
            fdt.Columns.Add(new DataColumn("Max", typeof(string)));
            fdt.Columns.Add(new DataColumn("HMax", typeof(string)));
            fdt.Columns.Add(new DataColumn("From", typeof(string)));
            fdt.Columns.Add(new DataColumn("To", typeof(string)));
            fdt.Columns.Add(new DataColumn("Mean", typeof(string)));
            fdt.Columns.Add(new DataColumn("Duration", typeof(string)));
            fdt.Columns.Add(new DataColumn("NMeasure", typeof(string)));

            AspMeasureDescriptioType dtype = AspUtility.LiteralDecodeMeteoParametersType(AspMeteoParametersType.HeathIndex);
            DataRow hr = fdt.NewRow();
            hr["Measure"] = dtype.Description;
            hr["um"] = dtype.MeasureUnit;
            hr["Max"] = float.IsNaN(heat.Max) ? AspUtility.BadDataPresentation : heat.Max.ToString("#0.0");
            hr["HMax"] = AspUtility.MorphHourToString(heat.HMax);
            hr["From"] = AspUtility.MorphHourToString(heat.From);
            hr["To"] = AspUtility.MorphHourToString(heat.To);
            hr["Mean"] = float.IsNaN(heat.Mean) ? AspUtility.BadDataPresentation : heat.Mean.ToString("#0.0");
            hr["Duration"] = heat.Duration < 0 ? AspUtility.BadDataPresentation : heat.Duration.ToString();
            hr["NMeasure"] = heat.NMeasure < 0 ? AspUtility.BadDataPresentation : heat.NMeasure.ToString();
            fdt.Rows.Add(hr);

            AspMeasureDescriptioType dtype2 = AspUtility.LiteralDecodeMeteoParametersType(AspMeteoParametersType.Thom);
            DataRow tr = fdt.NewRow();
            tr["Measure"] = dtype2.Description;
            tr["um"] = string.Empty;
            tr["Max"] = float.IsNaN(thom.Max) ? AspUtility.BadDataPresentation : thom.Max.ToString("#0.0");
            tr["HMax"] = AspUtility.MorphHourToString(thom.HMax);
            tr["From"] = AspUtility.MorphHourToString(thom.From);
            tr["To"] = AspUtility.MorphHourToString(thom.To);
            tr["Mean"] = float.IsNaN(thom.Mean) ? AspUtility.BadDataPresentation : thom.Mean.ToString("#0.0");
            tr["Duration"] = thom.Duration < 0 ? AspUtility.BadDataPresentation : thom.Duration.ToString();
            tr["NMeasure"] = thom.NMeasure < 0 ? AspUtility.BadDataPresentation : thom.NMeasure.ToString();
            fdt.Rows.Add(tr);

            return fdt;
        }
        //---
        //+--
        public static DataTable PeriodicDailyRainPresentation(AspDataReadRecordRainType record)
        {
            AspMeasureDescriptioType dtype = AspUtility.LiteralDecodeMeteoParametersType(AspMeteoParametersType.Rain);
            DataTable fdt = new DataTable();

            fdt.Columns.Add(new DataColumn("Measure", typeof(string)));
            fdt.Columns.Add(new DataColumn("um", typeof(string)));
            fdt.Columns.Add(new DataColumn("Total", typeof(string)));
            fdt.Columns.Add(new DataColumn("Rate", typeof(string)));
            fdt.Columns.Add(new DataColumn("NMeasure", typeof(string)));

            DataRow row = fdt.NewRow();
            row["Measure"] = dtype.Description;
            row["um"] = dtype.MeasureUnit;
            row["Total"] = float.IsNaN(record.Total) ? AspUtility.BadDataPresentation : record.Total.ToString("#0.0");
            row["Rate"] = float.IsNaN(record.Rate) ? AspUtility.BadDataPresentation : record.Rate.ToString("#0.0");
            row["NMeasure"] = record.NMisure < 0 ? AspUtility.BadDataPresentation : record.NMisure.ToString();
            fdt.Rows.Add(row);

            return fdt;
        }
        //---
        //+--
        public static DataTable PeriodicMonthlyDataSnapshotPresentation(AspDataReadRecordType[] records)
        {
            DataTable fdt = new DataTable();
            fdt.Columns.Add(new DataColumn("measuretype", typeof(string)));
            fdt.Columns.Add(new DataColumn("um", typeof(string)));
            fdt.Columns.Add(new DataColumn("max", typeof(string)));
            fdt.Columns.Add(new DataColumn("daymax", typeof(string)));
            fdt.Columns.Add(new DataColumn("hourmax", typeof(string)));
            fdt.Columns.Add(new DataColumn("min", typeof(string)));
            fdt.Columns.Add(new DataColumn("daymin", typeof(string)));
            fdt.Columns.Add(new DataColumn("hourmin", typeof(string)));
            fdt.Columns.Add(new DataColumn("mean", typeof(string)));
            fdt.Columns.Add(new DataColumn("Escursion", typeof(string)));
            fdt.Columns.Add(new DataColumn("nmisure", typeof(string)));

            for (int i = 0; i < records.Length; i++)
            {
                DataRow nr = fdt.NewRow();
                AspMeteoParametersType type = AspUtility.ReverseDecodeMeteoParametersType(records[i].measuretype);
                AspMeasureDescriptioType dtype = AspUtility.LiteralDecodeMeteoParametersType(type);

                nr["measuretype"] = dtype.Description;
                nr["um"] = dtype.MeasureUnit;
                nr["max"] = float.IsNaN(records[i].max) ? AspUtility.BadDataPresentation : records[i].max.ToString("#0.0");
                nr["daymax"] = float.IsNaN(records[i].max) ? AspUtility.BadDataPresentation : records[i].datemax.Day.ToString();
                nr["hourmax"] = float.IsNaN(records[i].max) ? AspUtility.BadDataPresentation : string.Format("{0:00}:{1:00}", records[i].datemax.Hour, records[i].datemax.Minute);
                nr["min"] = float.IsNaN(records[i].min) ? AspUtility.BadDataPresentation : records[i].min.ToString("#0.0");
                nr["daymin"] = float.IsNaN(records[i].min) ? AspUtility.BadDataPresentation : records[i].datemin.Day.ToString();
                nr["hourmin"] = float.IsNaN(records[i].min) ? AspUtility.BadDataPresentation : string.Format("{0:00}:{1:00}", records[i].datemin.Hour, records[i].datemin.Minute);
                nr["mean"] = float.IsNaN(records[i].mean) ? AspUtility.BadDataPresentation : records[i].mean.ToString("#0.0");
                nr["Escursion"] = float.IsNaN(records[i].excursion) ? AspUtility.BadDataPresentation : records[i].excursion.ToString("#0.0");
                nr["nmisure"] = float.IsNaN(records[i].nmisure) ? AspUtility.BadDataPresentation : records[i].nmisure.ToString("#0");
                fdt.Rows.Add(nr);
            }

            return fdt;
        }
        //---
        //+--
        public static DataTable PeriodicMontlyDataSequencePresentation(AspDataReadSequenceMontlyRecordType[] records)
        {
            DataTable fdt = new DataTable();
            fdt.Columns.Add(new DataColumn("date", typeof(string)));
            fdt.Columns.Add(new DataColumn("tempmax", typeof(string)));
            fdt.Columns.Add(new DataColumn("tempmin", typeof(string)));
            fdt.Columns.Add(new DataColumn("tempmed", typeof(string)));
            fdt.Columns.Add(new DataColumn("ummed", typeof(string)));
            fdt.Columns.Add(new DataColumn("pressure", typeof(string)));
            fdt.Columns.Add(new DataColumn("dewpoint", typeof(string)));
            fdt.Columns.Add(new DataColumn("wind", typeof(string)));
            fdt.Columns.Add(new DataColumn("windmax", typeof(string)));
            fdt.Columns.Add(new DataColumn("windangle", typeof(string)));
            fdt.Columns.Add(new DataColumn("windchill", typeof(string)));
            fdt.Columns.Add(new DataColumn("thom", typeof(string)));
            fdt.Columns.Add(new DataColumn("heat", typeof(string)));
            fdt.Columns.Add(new DataColumn("uv", typeof(string)));
            fdt.Columns.Add(new DataColumn("evapotraspiration", typeof(string)));
            fdt.Columns.Add(new DataColumn("radiation", typeof(string)));
            fdt.Columns.Add(new DataColumn("rain", typeof(string)));
            fdt.Columns.Add(new DataColumn("nmisure", typeof(string)));

            for (int i = 0; i < records.Length; i++)
            {
                DataRow row = fdt.NewRow();
                row["date"] = records[i].date.ToString("dd/MM/yyyy");
                row["tempmax"] = float.IsNaN(records[i].tempmax) ? AspUtility.BadDataPresentation : records[i].tempmax.ToString("#0.0");
                row["tempmin"] = float.IsNaN(records[i].tempmin) ? AspUtility.BadDataPresentation : records[i].tempmin.ToString("#0.0");
                row["tempmed"] = float.IsNaN(records[i].tempmed) ? AspUtility.BadDataPresentation : records[i].tempmed.ToString("#0.0");
                row["ummed"] = float.IsNaN(records[i].ummed) ? AspUtility.BadDataPresentation : records[i].ummed.ToString("#0.0");
                row["pressure"] = float.IsNaN(records[i].pressure) ? AspUtility.BadDataPresentation : records[i].pressure.ToString("#0.0");
                row["dewpoint"] = float.IsNaN(records[i].dewpoint) ? AspUtility.BadDataPresentation : records[i].dewpoint.ToString("#0.0");
                row["wind"] = float.IsNaN(records[i].wind) ? AspUtility.BadDataPresentation : records[i].wind.ToString("#0.0");
                row["windmax"] = float.IsNaN(records[i].windmax) ? AspUtility.BadDataPresentation : records[i].windmax.ToString("#0.0");
                row["windangle"] = float.IsNaN(records[i].windangle) ? AspUtility.BadDataPresentation : records[i].windangle.ToString("#0.0");
                row["windchill"] = float.IsNaN(records[i].windchill) ? AspUtility.BadDataPresentation : records[i].windchill.ToString("#0.0");
                row["heat"] = float.IsNaN(records[i].heat) ? AspUtility.BadDataPresentation : records[i].heat.ToString("#0.0");
                row["thom"] = float.IsNaN(records[i].thom) ? AspUtility.BadDataPresentation : records[i].thom.ToString("#0.0");
                row["uv"] = float.IsNaN(records[i].uv) ? AspUtility.BadDataPresentation : records[i].uv.ToString("#0.0");
                row["evapotraspiration"] = float.IsNaN(records[i].evapotraspiration) ? AspUtility.BadDataPresentation : records[i].evapotraspiration.ToString("#0.0");
                row["radiation"] = float.IsNaN(records[i].radiation) ? AspUtility.BadDataPresentation : records[i].evapotraspiration.ToString("#0.0");
                row["rain"] = float.IsNaN(records[i].rain) ? AspUtility.BadDataPresentation : records[i].rain.ToString("#0.0");
                row["nmisure"] = records[i].nmisure < 0 ? AspUtility.BadDataPresentation : records[i].rain.ToString("#0");
                fdt.Rows.Add(row);
            }

            return fdt;
        }
        //---
        //+--
        public static DataTable PeriodicMontlyDataRainPresentation(AspDataReadMontlyRecordsRain[] records)
        {
            DataTable fdt = new DataTable();
            fdt.Columns.Add(new DataColumn("Decade", typeof(string)));
            fdt.Columns.Add(new DataColumn("Total", typeof(string)));
            fdt.Columns.Add(new DataColumn("NMisure", typeof(string)));

            for (int i = 0; i < records.Length; i++)
            {
                DataRow row = fdt.NewRow();
                string description;
                switch (records[i].Decade)
                {
                    case 0:
                        description = "Mese";
                        break;
                    case 1:
                        description = "Prima decade";
                        break;
                    case 2:
                        description = "seconda decade";
                        break;
                    case 3:
                        description = "Terza decade";
                        break;
                    default:
                        description = string.Empty;
                        break;
                }

                row["Decade"] = description;
                row["Total"] = float.IsNaN(records[i].Total) ? AspUtility.BadDataPresentation : records[i].Total.ToString("#0.0");
                row["NMisure"] = records[i].NMisure < 0 ? AspUtility.BadDataPresentation : records[i].NMisure.ToString("#0");
                fdt.Rows.Add(row);
            }

            return fdt;
        }
        //---
    }
    //---
    class FFile : AspGenericFilterFiles
    {
        //+--
        public FFile(int station, string pathbase)
            : base(station, pathbase)
        { }
        //---
        //+--
        public string Read(string prefix)
        {
            base.g_isdeleteoldfiles = true;
            base.DelayDeleteMinutes = 0D;
            IReadType rt = base.IRead(prefix, base.g_basedirectory);
            if (rt.Isvalid)
                return rt.Pathfile;
            else
                return string.Empty;
        }
        //---
    }
}