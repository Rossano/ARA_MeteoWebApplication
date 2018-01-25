using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeteoWebApplication.Models;
using Asp.Meteo.Viewer;
using Asp.Meteo.Utility;
using Asp.Meteo.Utility.Graph;


namespace MeteoWebApplication.Controllers
{
    public class DailyGraphController : Controller
    {
        // GET: DailyGraph
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult displayPage(DailyGraph model)
        {
            try
            {
                //model._path = "C:\\Test";
                model._path = System.Configuration.ConfigurationManager.AppSettings["dataRootFolder"];
                
                DateTime date = DateTime.ParseExact(FormAspDateString(model._date), "yyyyMMdd", null);


                string pf = AspPath.ComposePath(model._path, AspUtility.ComposeMeteoDirectoryPath(AspDirectoryDeepType.Day, model._station, date));
                FFile Readfile = new FFile(model._station, pf);
                string fpf = Readfile.Read(AspUtility.LiteralDecodeElaborationType(AspElaborationType.DAY));

                AspRDailyData rdg = new AspRDailyData(fpf);
                //if (rdg.Read())
                //{
                //    model._grid = AspPeriodicDataUtility.PeriodicDailyDataSnapshotPresentation(rdg.Daily);
                //}
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

                    if (graph.Create(info, date))
                    {
                        model._graphTemp = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath1 = GetImg(model._graphTemp);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData1 = model.graphPath1;
                    }

                }
                else
                {
                    throw new Exception("Dati giornalieri della stazione specificati inesistenti");
                }

                // temperatura, temp max e temp min
                AspMeteoParametersType[] p1 = new AspMeteoParametersType[3];
                p1[0] = AspMeteoParametersType.TemperatureMin;
                p1[1] = AspMeteoParametersType.TemperaturaMax;
                p1[2] = AspMeteoParametersType.Temperature;
                AspMeteoParametersDataGroup dp1 = rpars.Read(p1, date);
                if (dp1.Isvalid)
                {
                    AspGraphInfoType info;
                    info.colors = new System.Drawing.Color[3];
                    info.parameters = dp1;
                    info.tollerance = 15;
                    info.type = AspTypeGrapEnum.Line;

                    AspDailyGraph graph = new AspDailyGraph(600, 480);
                    graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                    graph.AxisYLabel = "Gradi Centigradi";
                    graph.PointSize = 5;
                    graph.Title = "Temperatura (Arancione, minima blu, massima rossa)";

                    if (graph.Create(info, date))
                    {
                        model._graphTempMaxMin = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath2 = GetImg(model._graphTempMaxMin);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData2 = model.graphPath2;
                    }
                }

                // temperatura e dewpoint
                AspMeteoParametersType[] p2 = new AspMeteoParametersType[2];
                p2[0] = AspMeteoParametersType.DewPoint;
                p2[1] = AspMeteoParametersType.Temperature;
                AspMeteoParametersDataGroup dp2 = rpars.Read(p2, date);
                if (dp2.Isvalid)
                {
                    AspGraphInfoType info;
                    info.colors = new System.Drawing.Color[2];
                    info.parameters = dp2;
                    info.tollerance = 15;
                    info.type = AspTypeGrapEnum.Line;

                    AspDailyGraph graph = new AspDailyGraph(600, 480);
                    graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                    graph.AxisYLabel = "Gradi Centigradi";
                    graph.PointSize = 5;
                    graph.Title = "Temperatura (arancione) e Dewpoint (blu)";

                    if (graph.Create(info, date))
                    {
                        model._graphTempDewP = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath3 = GetImg(model._graphTempDewP);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData3 = model.graphPath3;
                    }
                }

                // temperatura e heat index
                AspMeteoParametersType[] p3 = new AspMeteoParametersType[2];
                p3[0] = AspMeteoParametersType.HeathIndex;
                p3[1] = AspMeteoParametersType.Temperature;
                AspMeteoParametersDataGroup dp3 = rpars.Read(p3, date);
                if (dp3.Isvalid)
                {
                    AspGraphInfoType info;
                    info.colors = new System.Drawing.Color[2];
                    info.parameters = dp3;
                    info.tollerance = 15;
                    info.type = AspTypeGrapEnum.Line;

                    AspDailyGraph graph = new AspDailyGraph(600, 480);
                    graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                    graph.AxisYLabel = "Gradi Centigradi";
                    graph.PointSize = 5;
                    graph.Title = "Temperatura (arancione) e Heat Index (blu)";

                    if (graph.Create(info, date))
                    {
                        model._graphTempHeat = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath4 = GetImg(model._graphTempHeat);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData4 = model.graphPath4;
                    }
                }

                // temperatura e Wind Chill
                AspMeteoParametersType[] p5 = new AspMeteoParametersType[2];
                p5[0] = AspMeteoParametersType.WindChill;
                p5[1] = AspMeteoParametersType.Temperature;
                AspMeteoParametersDataGroup dp5 = rpars.Read(p5, date);
                if (dp5.Isvalid)
                {
                    AspGraphInfoType info;
                    info.colors = new System.Drawing.Color[2];
                    info.parameters = dp5;
                    info.tollerance = 15;
                    info.type = AspTypeGrapEnum.Line;

                    AspDailyGraph graph = new AspDailyGraph(600, 480);
                    graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                    graph.AxisYLabel = "Gradi Centigradi";
                    graph.PointSize = 5;
                    graph.Title = "Temperatura (arancione) e Wind Chill (blu)";

                    if (graph.Create(info, date))
                    {
                        model._graphTempWindChill = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath6 = GetImg(model._graphTempWindChill);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData5 = model.graphPath6;
                    }
                }

                // Vento  e vento max
                AspMeteoParametersType[] p4 = new AspMeteoParametersType[2];
                p4[0] = AspMeteoParametersType.windmax;
                p4[1] = AspMeteoParametersType.Wind;
                AspMeteoParametersDataGroup dp4 = rpars.Read(p4, date);
                if (dp4.Isvalid)
                {
                    AspGraphInfoType info;
                    info.colors = new System.Drawing.Color[2];
                    info.parameters = dp4;
                    info.tollerance = 15;
                    info.type = AspTypeGrapEnum.Line;

                    AspDailyGraph graph = new AspDailyGraph(600, 480);
                    graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                    graph.AxisYLabel = "Velocità in Km/h";
                    graph.PointSize = 5;
                    graph.Title = "Vel. vento (arancione) e vel. raffiche (porpora)";

                    if (graph.Create(info, date))
                    {
                        model._graphWind = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath5 = GetImg(model._graphWind);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData6 = model.graphPath5;
                    }
                }

                // pioggia
                AspMeteoParametersType[] p6 = new AspMeteoParametersType[1];
                p6[0] = AspMeteoParametersType.Rain;
                AspMeteoParametersDataGroup dp6 = rpars.Read(p6, date);
                if (dp6.Isvalid)
                {
                    AspGraphInfoType info;
                    info.colors = new System.Drawing.Color[1];
                    info.parameters = dp6;
                    info.tollerance = 15;
                    info.type = AspTypeGrapEnum.Line;

                    AspDailyGraph graph = new AspDailyGraph(600, 480);
                    graph.AxisXLabel = string.Format("Orario (data: {0:dd/MM/yyy}", date);
                    graph.AxisYLabel = "millimetri";
                    graph.PointSize = 5;
                    graph.Title = "Pioggia";

                    if (graph.Create(info, date))
                    {
                        model._graphRain = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath7 = GetImg(model._graphRain);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData7 = model.graphPath7;
                    }
                }

                return View("DailyDataGraph", model);
            }
            catch(Exception ex)
            {
                ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", ex.Message, DateTime.Now.ToString());
                return View("Error");
            }
        }

        public string GetImg(System.Drawing.Image img)
        {
            using (var streak = new System.IO.MemoryStream())
            {
                img.Save(streak, System.Drawing.Imaging.ImageFormat.Gif);
                byte[] bytes = streak.ToArray();
                //string res = Convert.ToBase64String(bytes, 0, bytes.Length);
                //return res;
                string imreBase64Data = Convert.ToBase64String(bytes);
                string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                return imgDataURL;
            }
        }

        private string FormAspDateString(DateTime date)
        {
            string foo;
            string month;
            string day;
            foo = string.Format("{0}", date.Year);
            if (date.Month < 10)
            {
                month = string.Format("0{0}", date.Month);
            }
            else
            {
                month = string.Format("{0}", date.Month);
            }
            if (date.Day < 10)
            {
                day = string.Format("0{0}", date.Day);
            }
            else
            {
                day = string.Format("{0}", date.Day);
            }

            return foo + month + day;
        }
    }
}