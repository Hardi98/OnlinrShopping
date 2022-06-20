using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinrShopping.Models
{
    public class ProductCartItems
    {
        public tblProductItem Product { get; set; }
        public int Quantity { get; set; }
    }
}