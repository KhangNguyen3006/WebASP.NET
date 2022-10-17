using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBH.Context;
using WebsiteBH.Models;

namespace WebsiteBH.Controllers
{
    public class PaymentController : Controller
    {
        QLBHEntities objQLBHEntities = new QLBHEntities();
        // GET: Payment
        public ActionResult Index()
        {
            if(Session["idUser"]==null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var lstCart = (List<CartModel>)Session["cart"];
                Order objOrder = new Order();
                objOrder.Name = "DonHang" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc = DateTime.Now;
                objOrder.Status = 1;
                objQLBHEntities.Orders.Add(objOrder);
                objQLBHEntities.SaveChanges();

                int intOrderId = objOrder.id;

                List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
                foreach(var item in lstCart)
                {
                    OrderDetail obj = new OrderDetail();
                    obj.Quantity = item.Quantity;
                    obj.OrderId= intOrderId;
                    obj.ProductId = item.Product.id;
                    lstOrderDetail.Add(obj);
                }

                objQLBHEntities.OrderDetails.AddRange(lstOrderDetail);
                objQLBHEntities.SaveChanges();

            }
            return View();
        }
    }
}