using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebsiteBH.Context;
using static WebsiteBH.Common;

namespace WebsiteBH.Areas.Admin.Controllers
{
    public class BrandAdminController : Controller
    {
        // GET: Admin/BrandAdmin
        QLBHEntities objQLBHEntities = new QLBHEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstBrand = new List<Brand>();
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
                lstBrand = objQLBHEntities.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = objQLBHEntities.Brands.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstBrand = lstBrand.OrderByDescending(n => n.id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Brand objBrand)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                        fileName = fileName +/* "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) +*/ extension;
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
                    }
                    objQLBHEntities.Brands.Add(objBrand);
                    objQLBHEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBrand = objQLBHEntities.Brands.Where(n => n.id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = objQLBHEntities.Brands.Where(n => n.id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objBrandDelete)
        {
            var objBrand = objQLBHEntities.Brands.Where(n => n.id == objBrandDelete.id).FirstOrDefault();
            objQLBHEntities.Brands.Remove(objBrand);
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = objQLBHEntities.Brands.Where(n => n.id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Edit(Brand objBrand)
        {
            if (objBrand.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                fileName = fileName /*+ "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss"))*/ + extension;
                objBrand.Avatar = fileName;
                objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            objQLBHEntities.Entry(objBrand).State = EntityState.Modified;
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        void LoadData()
        {
            Common objCommon = new Common();

            var lstCat = objQLBHEntities.Categories.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");


            var lstBrand = objQLBHEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");
        }
    }
}