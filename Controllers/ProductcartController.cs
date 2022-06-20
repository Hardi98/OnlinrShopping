using OnlinrShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinrShopping.Controllers
{
    public class ProductCartController : Controller
    {
        Online_ShoppingEntities1 db = new Online_ShoppingEntities1();

        public ActionResult Buy(int id)
        {
            if (Session["cart"] == null)
            {
                List<ProductCartItems> cart = new List<ProductCartItems>();
                cart.Add(new ProductCartItems { Product = db.tblProductItems.Where(a => a.ItemID == id).FirstOrDefault(), Quantity = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<ProductCartItems> cart = (List<ProductCartItems>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ProductCartItems { Product = db.tblProductItems.Where(a => a.ItemID == id).FirstOrDefault(), Quantity = 1 });
                }
                Session["cart"] = cart;
                Session["cartCount"] = cart.Count();
            }
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Remove(int id)
        {
            List<ProductCartItems> cart = (List<ProductCartItems>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            Session["cartCount"] = cart.Count();
            return RedirectToAction("Index", "Product");
        }

        private int isExist(int id)
        {
            List<ProductCartItems> cart = (List<ProductCartItems>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.ItemID.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult IncreaseQty(int id)
        {
            List<ProductCartItems> cart = (List<ProductCartItems>)Session["cart"];
            int index = isExist(id);
            cart[index].Quantity++;
            Session["cart"] = cart;
            Session["cartCount"] = cart.Count();
            return RedirectToAction("Index", "Product");
        }

        public ActionResult DecreaseQty(int id)
        {
            List<ProductCartItems> cart = (List<ProductCartItems>)Session["cart"];
            int index = isExist(id);
            if (cart[index].Quantity == 1)
            {
                cart.RemoveAt(index);
            }
            else
            {
                cart[index].Quantity--;
            }
            Session["cart"] = cart;
            Session["cartCount"] = cart.Count();
            return RedirectToAction("Index", "Product");
        }
    }
}