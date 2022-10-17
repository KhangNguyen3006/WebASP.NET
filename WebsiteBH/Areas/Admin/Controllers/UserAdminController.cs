using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using WebsiteBH.Context;
using static WebsiteBH.Common;

namespace WebsiteBH.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
    {
        // GET: Admin/UserAdmin
        QLBHEntities objQLBHEntities = new QLBHEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstUser = new List<User>();
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
                lstUser = objQLBHEntities.Users.Where(n => n.LastName.Contains(SearchString)).ToList();
            }
            else
            {
                lstUser = objQLBHEntities.Users.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstUser = lstUser.OrderByDescending(n => n.id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(User objUser)
        {
            if (ModelState.IsValid)
            {
                var check = objQLBHEntities.Users.FirstOrDefault(s => s.Email == objUser.Email);
                if (check == null)
                {
                    objUser.Password = GetMD5(objUser.Password);
                    objQLBHEntities.Configuration.ValidateOnSaveEnabled = false;
                    objQLBHEntities.Users.Add(objUser);
                    objQLBHEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            this.LoadData();
            objQLBHEntities.Users.Add(objUser);
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
            //return View(objUser);
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }

            [HttpGet]
        public ActionResult Details(int id)
        {
            var objUser = objQLBHEntities.Users.Where(n => n.id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = objQLBHEntities.Users.Where(n => n.id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Delete(User objUserDelete)
        {
            var objUser = objQLBHEntities.Users.Where(n => n.id == objUserDelete.id).FirstOrDefault();
            objQLBHEntities.Users.Remove(objUser);
            objQLBHEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objUser = objQLBHEntities.Users.Where(n => n.id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Edit(User objUser)
        {
            objQLBHEntities.Entry(objUser).State = EntityState.Modified;
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

            var lstUser = objQLBHEntities.Users.ToList();
            DataTable dtUser = converter.ToDataTable(lstUser);
            ViewBag.ListUser = objCommon.ToSelectList(dtUser, "Id", "LastName");

        }
    }
}