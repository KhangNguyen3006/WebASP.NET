using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBH.Context;
using WebsiteBH.Models;

namespace WebsiteBH.Controllers
{
    public class CartController : Controller
    {
        QLBHEntities objQLBHEntities = new QLBHEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View((List<CartModel>)Session["cart"]);
        }

        public ActionResult AddToCart(int id, int quantity)
        {
            if (Session["cart"]==null)
            {
                List<CartModel> cart = new List<CartModel>();
                cart.Add(new CartModel { Product = objQLBHEntities.Products.Find(id), Quantity=quantity });
                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                List<CartModel> cart = (List < CartModel >)Session["cart"];
                int index = isExist(id);
                if(index !=-1)
                {
                    cart[index].Quantity+=quantity;
                }
                else
                {
                    cart.Add(new CartModel { Product=objQLBHEntities.Products.Find(id), Quantity = quantity });
                    Session["count"]=Convert.ToInt32(Session["count"])+1;
                }
                Session["cart"] = cart;
            }
            return Json(new {Message= "Successfuly", JsonRequestBehavior.AllowGet });
        }

        private int isExist(int id)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for(int i = 0; i < cart.Count; i++)
                if(cart[i].Product.id.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult Remove(int id)
        {

            List<CartModel> li = (List<CartModel>)Session["cart"];
            li.RemoveAll(x => x.Product.id == id);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }
    }
}