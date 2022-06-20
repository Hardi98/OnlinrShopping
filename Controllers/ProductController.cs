using OnlinrShopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OnlinrShopping.Controllers
{
    public class ProductController : Controller
    {
        Online_ShoppingEntities1 db = new  Online_ShoppingEntities1();

        // GET: Item
        public ActionResult Index()
        {
            ViewBag.products = db.tblProductItems.ToList();
            return View();
        }
    }
}