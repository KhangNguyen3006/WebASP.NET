using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBH.Context;
using static WebsiteBH.Common;

namespace WebsiteBH.Areas.Admin.Controllers
{
    public class OrderAdminController : Controller
    {
        // GET: Admin/Order
        QLBHEntities objQLBHEntities = new QLBHEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstOrder = new List<Order>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstOrder = objQLBHEntities.Orders.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstOrder = objQLBHEntities.Orders.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstOrder = lstOrder.OrderByDescending(n => n.id).ToList();
            return View(lstOrder.ToPagedList(pageNumber, pageSize));
        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Order objOrder)
        {
            this.LoadData();   
                    objQLBHEntities.Orders.Add(objOrder);
                    objQLBHEntities.SaveChanges();
                    return RedirectToAction("Index");
            //return View(objOrder);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objOrder = objQLBHEntities.Orders.Where(n => n.id == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objOrder = objQLBHEntities.Orders.Where(n => n.id == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpPost]
        public ActionResult Delete(Order objOrderDelete)
        {
            var objOrder = objQLBHEntities.Orders.Where(n => n.id == objOrderDelete.id).FirstOrDefault();
            objQLBHEntities.Orders.Remove(objOrder);
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objOrder = objQLBHEntities.Orders.Where(n => n.id == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpPost]
        public ActionResult Edit(Order objOrder)
        {
            objQLBHEntities.Entry(objOrder).State = EntityState.Modified;
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        void LoadData()
        {
            Common objCommon = new Common();

            var lstProduct = objQLBHEntities.Products.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtProduct = converter.ToDataTable(lstProduct);
            ViewBag.ListProduct = objCommon.ToSelectList(dtProduct, "Id", "Name");

            var lstOrder = objQLBHEntities.Orders.ToList();
            DataTable dtOrder = converter.ToDataTable(lstOrder);
            ViewBag.ListOrder = objCommon.ToSelectList(dtOrder, "Id", "Name");
        }
    }
}