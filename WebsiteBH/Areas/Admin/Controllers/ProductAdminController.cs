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
    public class ProductAdminController : Controller
    {
        QLBHEntities objQLBHEntities = new QLBHEntities();
        // GET: Admin/Product
        public ActionResult Index(string currentFilter ,string SearchString, int? page)
        {

            var lstProduct=new List<Product>();
            if(SearchString!=null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if(!string.IsNullOrEmpty(SearchString))
            {
                lstProduct= objQLBHEntities.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct=objQLBHEntities.Products.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber=(page ?? 1);
            lstProduct=lstProduct.OrderByDescending(n=>n.id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                        fileName = fileName +/* "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) +*/ extension;
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
                    }
                    objQLBHEntities.Products.Add(objProduct);
                    objQLBHEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = objQLBHEntities.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objQLBHEntities.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objProductDelete)
        {
            var objProduct = objQLBHEntities.Products.Where(n => n.id == objProductDelete.id).FirstOrDefault();
            objQLBHEntities.Products.Remove(objProduct);
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = objQLBHEntities.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                fileName = fileName /*+ "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss"))*/ + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            objQLBHEntities.Entry(objProduct).State = EntityState.Modified;
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


            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "id", "Name");
        }

    }
}