using System;
using System.Collections.Generic;
using System.Linq;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Data
{
    public class BestSellerClass
    {
        public string ProductName { get; set; }

        public double Sales { get; set; }

        public int UnitsSold { get; set; }

        //public BestSellerClass(string productName, double sales, int unitsSold)
        //{
        //    ProductName = productName;
        //    Sales = sales;
        //    UnitsSold = unitsSold;
        //}
    }
}
