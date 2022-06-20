using OnlinrShopping;
using OnlinrShopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinrShopping.Controllers
{
    public class ProductItemController : Controller
    {
        Online_ShoppingEntities1 db = new Online_ShoppingEntities1();

        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAllItems()
        {
            return View(GetAllItem());
        }

        IEnumerable<tblProductItem> GetAllItem()
        {
            return db.tblProductItems.ToList<tblProductItem>();
        }

        public ActionResult AddOrEditItem(int id = 0)
        {
            tblProductItem emp = new tblProductItem();
            if (id != 0)
            {
                emp = db.tblProductItems.Where(x => x.ItemID == id).FirstOrDefault<tblProductItem>();
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult AddOrEditItem(tblProductItem item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ItemID == 0)
                    {
                        int tblItemCount = db.tblProductItems.Where(a => a.ItemName == item.ItemName).Count();
                        if (tblItemCount > 0)
                        {
                            return Json(new { success = false, message = "Item with same name is already exist." }, JsonRequestBehavior.AllowGet);
                        }
                        db.tblProductItems.Add(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (item.ImageUpload != null)
                    {
                        string extension = Path.GetExtension(item.ImageUpload.FileName);
                        item.Image = item.ItemID + extension;
                        item.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), item.Image));
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAllItems", GetAllItem()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteItem(int id)
        {
            try
            {
                tblProductItem item = db.tblProductItems.Where(a => a.ItemID == id).FirstOrDefault<tblProductItem>();
                List<ProductCartItems> cart = (List<ProductCartItems>)Session["cart"];
                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Product.ItemID.Equals(id))
                        cart.Remove(cart[i]);
                }
                Session["cart"] = cart;
                Session["cartCount"] = cart.Count();
                db.tblProductItems.Remove(item);
                db.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAllItems", GetAllItem()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}