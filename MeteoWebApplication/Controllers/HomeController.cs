using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeteoWebApplication.Models;

namespace MeteoWebApplication.Controllers
{
    public class HomeController : Controller
    {

 /*       public class PageModel
        {
            public int _pageID { get; set; }
            public string _value { get; set; }
        }

        public IEnumerable<PageModel> PageModelOptions = new List<PageModel>
        {
            new PageModel{ _pageID=0, _value="Visualizza Griglia dati giornalieri"},
            new PageModel{_pageID=1, _value="Visualizza Grafici Dati giornalieri"}
        };

        public string _page { get; set; }

        //private IEnumerable<PageSelect> _pages; //= new PageSelect();
        private List<SelectListItem> _pages;

        private List<SelectListItem> LoadPages()
        {
            List<SelectListItem> _pages = new List<SelectListItem>();
            _pages = new List<SelectListItem>();
            _pages.Add(new SelectListItem { Text = "Griglia Dati Giornalieri", Value = "0" });
            _pages.Add(new SelectListItem { Text = "Grafici Dati GIornalieri", Value = "1" });
            ViewData["pages"] = _pages;
            return _pages;
        }
*/

        public ActionResult Index()
        {
 /*           var pages = GetAllPages();
            var model = new PageSelect();
            model._pages = GetSelectListItems(pages);
   */         //var repo = new PageRepository();
            //var pageList = repo.GetPage();
            //pages._pages = LoadPages();
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(PageSelect model)
        {
            //page._pages = LoadPages();
            /*
            var selectedItem = page._pages.Find(p => p.Value == page._pageID.ToString());
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
                ViewBag.Message = "Selezionato: " + selectedItem.Text;
            }
            return View(page);
            */
//            var pages = GetAllPages();
//            model._pages = GetSelectListItems(pages);
            if (ModelState.IsValid)
            {
                Session["PageSelect"] = model;
                return RedirectToAction("Done");
            }
            return View("Index", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /*public ViewResult SelectedPage(HomeController model, string action)
        {
            return View();
        }*/
/*
        public ActionResult SelectPage(PageSelect page, string sel)
        {
            if (sel.Equals(PageNames.DailyDataGrid))
            {
                return RedirectToAction("DailyData");
            }
            if (sel.Equals(PageNames.DailyDataGraph))
            {
                return RedirectToAction("DailyData");
            }
            else
            {
                return View();
            }
        }

        public RedirectResult SelectedPage(string _page)
        {
            ViewBag.messageString = _page;

            if (_page.Equals("Griglia Dati Giornalieri")) return Redirect("DailyData");
            else return Redirect("Home");
        }

        public ActionResult Done()
        {
            var model = Session["PageSelect"] as PageSelect;
            return View(model);
        }

        private IEnumerable<string> GetAllPages()
        {
            return new List<string>
            {
                PageNames.DailyDataGrid,
                PageNames.DailyDataGraph
            };
        }

        private IEnumerable<SelectListItem>GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach(var el in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = el,
                    Text = el
                });
            }
            return selectList;
        }
        */

        //public class PageRepository
        //{
        //    public IEnumerable<SelectListItem> GetPage()
        //    {
        //        List<SelectListItem> pages = new List<SelectListItem>()
        //        {
        //            new SelectListItem
        //            {
        //                Value=null,
        //                Text=""
        //            }
        //        };
        //        return pages;
        //    }
        //}
    }
}