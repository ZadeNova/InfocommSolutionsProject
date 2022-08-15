using System;
using System.Linq;
using System.Collections.Generic;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Data
{
    public class LatestOrders
    {
        public Accounts User_Account { get; set; }

        public ProductModel Product { get; set; }

        public double Price_OfOrder { get; set; }
        
        public DateTime DateOf_Order { get; set; }

        public string Order_Status { get; set; }

        public string Notes { get; set; }

        public string Category { get; set; }

    }
}
