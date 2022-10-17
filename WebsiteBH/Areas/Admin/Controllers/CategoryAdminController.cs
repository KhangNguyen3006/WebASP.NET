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
    public class CategoryAdminController : Controller
    {
        // GET: Admin/CategoryAdmin
        QLBHEntities objQLBHEntities = new QLBHEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstCategory = new List<Category>();
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
                lstCategory = objQLBHEntities.Categories.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstCategory = objQLBHEntities.Categories.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstCategory = lstCategory.OrderByDescending(n => n.id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Category objCategory)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objCategory.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                        fileName = fileName +/* "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) +*/ extension;
                        objCategory.Avatar = fileName;
                        objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
                    }
                    objQLBHEntities.Categories.Add(objCategory);
                    objQLBHEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = objQLBHEntities.Categories.Where(n => n.id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = objQLBHEntities.Categories.Where(n => n.id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objCategoryDelete)
        {
            var objCategory = objQLBHEntities.Categories.Where(n => n.id == objCategoryDelete.id).FirstOrDefault();
            objQLBHEntities.Categories.Remove(objCategory);
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = objQLBHEntities.Categories.Where(n => n.id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category objCategory)
        {
            if (objCategory.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                fileName = fileName/* + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss"))*/ + extension;
                objCategory.Avatar = fileName;
                objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            objQLBHEntities.Entry(objCategory).State = EntityState.Modified;
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