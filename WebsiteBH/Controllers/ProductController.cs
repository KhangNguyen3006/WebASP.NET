using System.Linq;
using System.Web.Mvc;
using WebsiteBH.Context;

namespace WebsiteBH.Controllers
{
    public class ProductController : Controller
    {
        QLBHEntities objQLBHEntities = new QLBHEntities();
        // GET: Product

        public ActionResult Detail(int id)
        {
            var objProduct = objQLBHEntities.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }
    }
}