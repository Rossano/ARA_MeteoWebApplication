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
                model._path = "C:\\Test";
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
                        model._graph = graph.GetImage;
                        //byte[] foo = File.ReadAllBytes(graph.GetImage);
                        //model.graphPath = "data:image/png;base64," + Convert.ToBase64String(foo);
                        model.graphPath = GetImg(model._graph);
                        //Passing image data in viewbag to view  
                        ViewBag.ImageData = model.graphPath;
                    }
                }
                return View("DailyDataGraph", model);
            }
            catch
            {
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