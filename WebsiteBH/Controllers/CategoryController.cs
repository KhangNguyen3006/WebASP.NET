using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebsiteBH.Context;

namespace WebsiteBH.Controllers
{
    public class CategoryController : Controller
    {
        QLBHEntities objQLBHEntities = new QLBHEntities();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory = objQLBHEntities.Categories.ToList();
            return View(lstCategory);
        }

        public ActionResult ProductCategory(int id)
        {
            var lstProduct = objQLBHEntities.Products.Where(n => n.CategoryId == id).ToList();
            return View(lstProduct);
        }
    }
}