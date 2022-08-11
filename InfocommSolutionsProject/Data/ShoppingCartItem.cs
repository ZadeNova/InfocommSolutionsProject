using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Data
{
    public class ShoppingCartItem
    {
        //public int Quantity { get; set; }

        //private string _ItemID;

        //public string ItemID { get { return _ItemID; } set { _ItemID = value; } }


        //private string _ItemName;

        //public string Product_Name { get { return _ItemName; } set { _ItemName = value; } }

        //private string _ItemImage;

        //public string Product_Image { get { return _ItemImage; } set { _ItemImage = value; } }

        //private string _ItemType;
        //public string Product_Type
        //{
        //    get { return _ItemType; }
        //    set { _ItemType = value; }

        //}

        //private decimal _ItemPrice;
        //public decimal Product_Price
        //{
        //    get { return _ItemPrice; }
        //    set { _ItemPrice = value; }
        //}

        //public decimal TotalPrice
        //{
        //    get { return Product_Price * Quantity;  }
        //}

        //public ShoppingCartItem(string productID)
        //{
        //    this.ItemID = productID;
        //}

        //public ShoppingCartItem(string productID , ProductModel product)
        //{

        //}


        public ProductModel Product { get; set; }

        public int Quantity { get; set; }


       


    }
}
