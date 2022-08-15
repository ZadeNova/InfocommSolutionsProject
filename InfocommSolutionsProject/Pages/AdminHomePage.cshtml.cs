using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace InfocommSolutionsProject.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        private readonly UserManager<Accounts> _userManager;

        public AdminPageModel(InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int NumberOfProducts { get; set; }

        public int NumberOfSuppliers { get; set; }

        public int NumberOfSupplierOrders { get; set; }

        public int NumberOfProductOrders { get; set; }

        public int NumberOfUsersInWebApplication { get; set; }

        public IEnumerable<DateTime>? UserDateList { get; set; }

        public Dictionary<Accounts, double> TopCustomers { get; set; }

        public List<BestSellerClass> TopProducts_byAmount { get; set; }

        public List<BestSellerClass> TopProducts_byQuantity { get; set; }

        public List<OrdersModel> OrdersList { get; set; }

        public List<ProductModel> ProductList { get; set; }

        public List<OrdersModel> LatestOrders { get; set; }

        //public List<Categories> ProductsCategory { get; set; }

        public Dictionary<string,int> TopCategories { get; set; }

        public Dictionary<string, double> TopCategoriesBySales { get; set; }

        public double TotalSales { get; set; }

        public double AveragePricePerOrder { get; set; }

        // Start of sales over time
        public Dictionary<DateTime,double> SalesOverTime { get; set; }

        public Dictionary<string,double> SalesOverTime_7day { get; set; }

        public Dictionary<string , double> SalesOverTime_28Day { get; set; }

        public Dictionary<string , double> SalesOverTime_Today { get; set; }

        public Dictionary <string , double> SalesOverTime_Yesterday { get; set; }

        public Dictionary<string , double> SalesOverTime_Monthly { get; set; }
        // Start of herbs

        public Dictionary<string, double> Herbs_SalesOverTime { get; set; }

        public Dictionary<string, double> Herbs_SalesOver_Today { get; set; }

        public Dictionary<string, double> Herbs_SalesOver_Yesterday { get; set; }

        public Dictionary<string, double> Herbs_SalesOver_28Day { get; set; }

        public Dictionary<string, double> Herbs_SalesOver_7Day { get; set; }

        public Dictionary<string, double> Herbs_SalesOver_Monthly { get; set; }
        // End of herbs
        // Start of vegetables

        public Dictionary<string, double> Vegetable_SalesOver_Today { get; set; }

        public Dictionary<string, double> Vegetable_SalesOver_Yesterday { get; set; }

        public Dictionary<string, double> Vegetable_SalesOver_28Day { get; set; }

        public Dictionary<string, double> Vegetable_SalesOver_7Day { get; set; }

        public Dictionary<string, double> Vegetable_SalesOver_Monthly { get; set; }
        // End of vegetables
        // Start of Fruits

        public Dictionary<string, double> Fruits_SalesOverTime { get; set; }

        public Dictionary<string, double> Fruits_SalesOver_Today { get; set; }

        public Dictionary<string, double> Fruits_SalesOver_Yesterday { get; set; }

        public Dictionary<string, double> Fruits_SalesOver_28Day { get; set; }

        public Dictionary<string, double> Fruits_SalesOver_7Day { get; set; }

        public Dictionary<string, double> Fruits_SalesOver_Monthly { get; set; }
        // End of fruits
        // End of Sales Over time
        // Start of Quantity over time

        public Dictionary<string, double> Vegetable_Quantity_Over_Today { get; set; }

        public Dictionary<string, double> Vegetable_Quantity_Over_Yesterday { get; set; }

        public Dictionary<string, double> Vegetable_Quantity_Over_28Day { get; set; }

        public Dictionary<string, double> Vegetable_Quantity_Over_7Day { get; set; }

        public Dictionary<string, double> Vegetable_Quantity_Over_Monthly { get; set; }

        // End of vegetable

        public Dictionary<string, double> Herbs_Quantity_Over_Today { get; set; }

        public Dictionary<string, double> Herbs_Quantity_Over_Yesterday { get; set; }

        public Dictionary<string, double> Herbs_Quantity_Over_28Day { get; set; }

        public Dictionary<string, double> Herbs_Quantity_Over_7Day { get; set; }

        public Dictionary<string, double> Herbs_Quantity_Over_Monthly { get; set; }

        // End of herbs

        public Dictionary<string, double> Fruits_Quantity_Over_Today { get; set; }

        public Dictionary<string, double> Fruits_Quantity_Over_Yesterday { get; set; }

        public Dictionary<string, double> Fruits_Quantity_Over_28Day { get; set; }

        public Dictionary<string, double> Fruits_Quantity_Over_7Day { get; set; }

        public Dictionary<string, double> Fruits_Quantity_Over_Monthly { get; set; }

        // End of fruits

        public List<RatingsModel> LatestRatings { get; set; }


        public List<string> Time_Hours { get; set; }

        public List<string> Months { get; set; }

        public List<BestSellerClass> TheTopSellers { get; set; }

        public List<LatestOrders> Top_8_LatestOrders { get; set; }

        

        public void OnGet()
        {
            // Assigning the variables from database
            NumberOfProducts = _context.Products.Count();
            NumberOfSuppliers = _context.Suppliers.Count();
            NumberOfProductOrders = _context.Orders.Count();
            NumberOfSupplierOrders = _context.SupplierOrders.Count();
            NumberOfUsersInWebApplication = _userManager.Users.Count();
            OrdersList = _context.Orders.ToList();
            ProductList = _context.Products.ToList();


            // Get Latest Orders

            LatestOrders = _context.Orders.OrderByDescending(x => x.DateOfOrder).Take(8).ToList();

            //var lol = (from order in _context.Orders
            //          join prod in _context.Products on order.Product.Id equals prod.Id
            //          select new { order.Accounts, order.PriceOfOrder, order.Product, order.DateOfOrder, order.OrderStatus, order.Notes, prod.Category }).OrderByDescending(x => x.DateOfOrder).Take(8);


            Top_8_LatestOrders = (from order in _context.Orders
                       join prod in _context.Products on order.Product.Id equals prod.Id
                       select new LatestOrders { User_Account = order.Accounts , Product = order.Product , Price_OfOrder = order.PriceOfOrder , Order_Status = order.OrderStatus , DateOf_Order = order.DateOfOrder , Notes = order.Notes , Category = prod.Category }).OrderByDescending(x => x.DateOf_Order).Take(8).ToList();



            //foreach (var i in lol) System.Diagnostics.Debug.WriteLine($"{i.TheAccount.Email} {i.Product.Name} {i.Category} {i.DateOf_Order} {i.Notes}");


            LatestRatings = _context.Ratings.OrderByDescending(x => x.CreatedOn).Take(8).ToList();
            TotalSales = Math.Round(_context.Orders.Sum(x => x.PriceOfOrder) , 2);
            AveragePricePerOrder = Math.Round((_context.Orders.Sum(x => x.PriceOfOrder) / _context.Orders.Count()), 2);
            
           // ProductsCategory = _context.Categories.Where(x => x.CategoryFor.ToLower() == "product" || x.CategoryFor.ToLower() == "products").ToList();
            
            var UserList = _userManager.Users.ToList();
            var testvar = from a in _userManager.Users select a.CreatedAt;
            UserDateList = UserList.Select(a => a.CreatedAt);





            SalesOverTime = _context.Orders.GroupBy(x => x.DateOfOrder).Select(g => new
            {
                TheDate = g.Key,
                Revenue = g.Sum(a => a.PriceOfOrder)
            }).ToDictionary( dic => dic.TheDate , dic => dic.Revenue);

            //foreach (var i in SalesOverTime) System.Diagnostics.Debug.WriteLine($"{i.Key} , {i.Value}");

            // Start Of Initialization of data values for LineChart
            Time_Hours = new List<string> {
                "00:00",
                "01:00",
                "02:00",
                "03:00",
                "04:00",
                "05:00",
                "06:00",
                "07:00",
                "08:00",
                "09:00",
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
                "19:00",
                "20:00",
                "21:00",
                "22:00",
                "23:00",
            };

            Months = new List<string>() {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };




            //Dictionary <string,double> SalesOverTime_DateToString 
            Dictionary<string, double> MonthlyDictSales = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDictSales = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_DictSales = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Final = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Final = new Dictionary<string, double>();
            
            // Create Datapoints for dictionary. Fill everything with dates and Zeros.
            for (int i = 0; i < Time_Hours.Count; i++)
            {
                Today_DictSales_Final[Time_Hours[i]] = 0;
                Yesterday_DictSales_Final[Time_Hours[i]] = 0;

            }

            for (int i = 0; i < Months.Count; i++)
            {
                MonthlyDictSales[Months[i]] = 0;
            }

            for (int i = 1; i < 29; i++)
            {
                TwentyEight_DictSales[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
            }


            for (int i = 1; i < 8; i++)
            {
                SevenDayDictSales[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
            }

            // Add data points if date exists.
            foreach (KeyValuePair<DateTime, double> entry in SalesOverTime)
            {
                if (TwentyEight_DictSales.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_DictSales[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value,2);
                    if (SevenDayDictSales.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDictSales[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value,2);
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_DictSales[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    
                    Yesterday_DictSales[entry.Key.TimeOfDay.ToString().Substring(0,2)] = 0;

                }
                if (entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDictSales.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDictSales[entry.Key.ToString("MMMM")] += Math.Round(entry.Value, 2);
                    }
                }
                
                
                
            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, double> entry in SalesOverTime)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_DictSales.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_DictSales[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }             
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_DictSales.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_DictSales[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }



            
            foreach (var key in Today_DictSales_Final)
            {
                if (Today_DictSales.ContainsKey(key.Key.Substring(0,2)))
                {
                    Today_DictSales_Final[key.Key] = Math.Round(Today_DictSales[key.Key.Substring(0, 2)],2);
                }
            }

            foreach (var key in Yesterday_DictSales_Final)
            {
                if (Yesterday_DictSales.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_DictSales_Final[key.Key] = Math.Round(Yesterday_DictSales[key.Key.Substring(0, 2)],2);
                }
            }

            SalesOverTime_7day = SevenDayDictSales;
            SalesOverTime_28Day = TwentyEight_DictSales;
            SalesOverTime_Today = Today_DictSales_Final;
            SalesOverTime_Yesterday = Yesterday_DictSales_Final;
            SalesOverTime_Monthly = MonthlyDictSales;

            //foreach (KeyValuePair<string, double> entry in MonthlyDictSales) System.Diagnostics.Debug.WriteLine($"{entry.Key} {entry.Value}");
            //foreach (KeyValuePair<string, double> entry in TwentyEight_DictSales) System.Diagnostics.Debug.WriteLine($"{entry.Key} {entry.Value}");


            // End of Initialization of Data values for LineChart







            TopCategories = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id select new { cat.Product , prod.Category , cat.quantity}).GroupBy(x => x.Category).Select(y => new
            {
                Category = y.Key,
                Count = y.Sum(x => x.quantity)
            }).ToDictionary(x => x.Category , x => x.Count);

            TopCategoriesBySales = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id select new { cat.Product, prod.Category , cat.PriceOfOrder }).GroupBy(x => x.Category).Select(y => new
            {
                Category = y.Key,
                Count = Math.Round(y.Sum( a => a.PriceOfOrder) , 2)
            }).ToDictionary(x => x.Category, x => x.Count);


           
            //foreach (var i in getNumberOfCategory) System.Diagnostics.Debug.WriteLine($"{i.Category} , {i.Count}");


            



            TopCustomers = OrdersList.GroupBy(x => x.Accounts).Select(g => new
            {
                Account = g.Key,
                Count = Math.Round(g.Sum(x => x.PriceOfOrder) , 2)
            }).OrderByDescending(a => a.Count).Take(5).ToDictionary(x => x.Account, x => x.Count);


            var Top_ProductBy_Sales = OrdersList.GroupBy(x => x.Product).Select(g => new
            {
                Product = g.Key,
                Sales = Math.Round(g.Sum(x => x.PriceOfOrder) , 2)
            }).OrderByDescending(v => v.Sales).Take(5).ToDictionary(m => m.Product , m => m.Sales);

            //TopProducts_byQuantity = OrdersList.GroupBy(x => x.Product).Select(g => new
            //{
            //    Product = g.Key,
            //    Quantity = g.Sum(x => x.quantity)

            //}).OrderByDescending(x => x.Quantity).Take(5).ToDictionary(m => m.Product.Name, m => m.Quantity);

            //Dictionary<Guid, int> Thetestdict = new Dictionary<Guid, int>();


            var Top_ProductBy_Quantity = OrdersList.GroupBy(x => x.Product).Select(g => new
            {
                Product = g.Key,
                Quantity = g.Sum(x => x.quantity)

            }).OrderByDescending(x => x.Quantity).Take(5).ToDictionary(m => m.Product , m => m.Quantity);

            //TheTopSellers = new List<BestSellerClass>();

            TopProducts_byAmount = new List<BestSellerClass>();
            TopProducts_byQuantity = new List<BestSellerClass>();

            foreach(var key in Top_ProductBy_Quantity)
            {
                //System.Diagnostics.Debug.WriteLine($"{key.Key} {key.Value}");
                

                TopProducts_byQuantity.Add(new BestSellerClass { ProductName = key.Key.Name , Sales = Math.Round(_context.Orders.Where(x => x.Product.Id == key.Key.Id).Sum(g => g.PriceOfOrder), 2) , UnitsSold = key.Value });
            }

            foreach (var key in Top_ProductBy_Sales)
            {
                TopProducts_byAmount.Add(new BestSellerClass { ProductName = key.Key.Name, Sales = Math.Round(key.Value,2), UnitsSold = _context.Orders.Where(x => x.Product.Id == key.Key.Id).Sum(g => g.quantity) });

            }


            //foreach (var i in TopProducts_byQuantity) System.Diagnostics.Debug.WriteLine($"{i.Key} {i.Value}");
            //foreach (var b in Prods) System.Diagnostics.Debug.WriteLine($"{b.Key} {b.Value} Sussybaka");

            CategoryData_Sales();
            CategoryData_Quantity();
        }


        public void CategoryData_Sales()
        {
            var Herbs_sales = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id where prod.Category == "Herbs" select new { cat.PriceOfOrder, cat.DateOfOrder }).GroupBy(x => x.DateOfOrder).Select(f => new
            {
                TheDate = f.Key,
                Sales = Math.Round(f.Sum(a => a.PriceOfOrder), 2),

            }).ToDictionary(x => x.TheDate, x => x.Sales);

            var Vegetable_sales = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id where prod.Category == "Vegetable" select new { cat.PriceOfOrder, cat.DateOfOrder }).GroupBy(x => x.DateOfOrder).Select(f => new
            {
                TheDate = f.Key,
                Sales = Math.Round(f.Sum(a => a.PriceOfOrder), 2),

            }).ToDictionary(x => x.TheDate, x => x.Sales);

            var Fruits_sales = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id where prod.Category == "Fruits" select new { cat.PriceOfOrder, cat.DateOfOrder }).GroupBy(x => x.DateOfOrder).Select(f => new
            {
                TheDate = f.Key,
                Sales = Math.Round(f.Sum(a => a.PriceOfOrder), 2),

            }).ToDictionary(x => x.TheDate, x => x.Sales);

            // For Seeds
            Dictionary<string, double> MonthlyDictSales_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDictSales_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_DictSales_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Final_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Final_Herbs = new Dictionary<string, double>();

            //For Vegetable
            Dictionary<string, double> MonthlyDictSales_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDictSales_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_DictSales_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Final_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Final_Vegetable = new Dictionary<string, double>();

            // For Fruits
            Dictionary<string, double> MonthlyDictSales_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDictSales_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_DictSales_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Final_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Final_Fruits = new Dictionary<string, double>();

            for (int i = 0; i < Time_Hours.Count; i++)
            {
                Yesterday_DictSales_Final_Herbs[Time_Hours[i]] = 0;
                Today_DictSales_Final_Herbs[Time_Hours[i]] = 0;
                Yesterday_DictSales_Final_Vegetable[Time_Hours[i]] = 0;
                Today_DictSales_Final_Vegetable[Time_Hours[i]] = 0;
                Yesterday_DictSales_Final_Fruits[Time_Hours[i]] = 0;
                Today_DictSales_Final_Fruits[Time_Hours[i]] = 0;

            }

            for (int i = 0; i < Months.Count; i++)
            {
                MonthlyDictSales_Vegetable[Months[i]] = 0;
                MonthlyDictSales_Herbs[Months[i]] = 0;
                MonthlyDictSales_Fruits[Months[i]] = 0;
            }

            for (int i = 1; i < 29; i++)
            {
                TwentyEight_DictSales_Vegetable[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                TwentyEight_DictSales_Herbs[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                TwentyEight_DictSales_Fruits[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
            }


            for (int i = 1; i < 8; i++)
            {
                SevenDayDictSales_Vegetable[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                SevenDayDictSales_Herbs[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                SevenDayDictSales_Fruits[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
            }


            // For herb first
            foreach (KeyValuePair<DateTime, double> entry in Herbs_sales)
            {
                if (TwentyEight_DictSales_Herbs.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_DictSales_Herbs[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    if (SevenDayDictSales_Herbs.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDictSales_Herbs[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_DictSales_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_DictSales_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }
                if (entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDictSales_Herbs.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDictSales_Herbs[entry.Key.ToString("MMMM")] += Math.Round(entry.Value, 2);
                    }
                }

                


            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, double> entry in Herbs_sales)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_DictSales_Herbs.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_DictSales_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_DictSales_Herbs.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_DictSales_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }
            // This is for herbs
            foreach (var key in Today_DictSales_Final_Herbs)
            {
                if (Today_DictSales_Herbs.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_DictSales_Final_Herbs[key.Key] = Math.Round(Today_DictSales_Herbs[key.Key.Substring(0, 2)], 2);
                }
            }

            foreach (var key in Yesterday_DictSales_Final_Herbs)
            {
                if (Yesterday_DictSales_Herbs.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_DictSales_Final_Herbs[key.Key] = Math.Round(Yesterday_DictSales_Herbs[key.Key.Substring(0, 2)], 2);
                }
            }

            Herbs_SalesOver_28Day = TwentyEight_DictSales_Herbs;
            Herbs_SalesOver_7Day = SevenDayDictSales_Herbs;
            Herbs_SalesOver_Today = Today_DictSales_Final_Herbs;
            Herbs_SalesOver_Yesterday = Yesterday_DictSales_Final_Herbs;
            Herbs_SalesOver_Monthly = MonthlyDictSales_Herbs;

            //End of Seed
            // Start of vegetable
            foreach (KeyValuePair<DateTime, double> entry in Vegetable_sales)
            {
                if (TwentyEight_DictSales_Vegetable.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_DictSales_Vegetable[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    if (SevenDayDictSales_Vegetable.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDictSales_Vegetable[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_DictSales_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_DictSales_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }
                if (entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDictSales_Vegetable.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDictSales_Vegetable[entry.Key.ToString("MMMM")] += Math.Round(entry.Value, 2);
                    }
                }
                


            }
            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, double> entry in Vegetable_sales)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_DictSales_Vegetable.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_DictSales_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_DictSales_Vegetable.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_DictSales_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }
            // This is for vegetable
            foreach (var key in Today_DictSales_Final_Vegetable)
            {
                if (Today_DictSales_Vegetable.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_DictSales_Final_Vegetable[key.Key] = Math.Round(Today_DictSales_Vegetable[key.Key.Substring(0, 2)], 2);
                }
            }

            foreach (var key in Yesterday_DictSales_Final_Vegetable)
            {
                if (Yesterday_DictSales_Vegetable.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_DictSales_Final_Vegetable[key.Key] = Math.Round(Yesterday_DictSales_Vegetable[key.Key.Substring(0, 2)], 2);
                }
            }


            Vegetable_SalesOver_28Day = TwentyEight_DictSales_Vegetable;
            Vegetable_SalesOver_7Day = SevenDayDictSales_Vegetable;
            Vegetable_SalesOver_Today = Today_DictSales_Final_Vegetable;
            Vegetable_SalesOver_Yesterday = Yesterday_DictSales_Final_Vegetable;
            Vegetable_SalesOver_Monthly = MonthlyDictSales_Vegetable;

            //foreach (KeyValuePair<DateTime, double> entry in Vegetable_sales) System.Diagnostics.Debug.WriteLine($"{entry.Key} {entry.Value}");


            foreach (KeyValuePair<DateTime, double> entry in Fruits_sales)
            {
                if (TwentyEight_DictSales_Fruits.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_DictSales_Fruits[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    if (SevenDayDictSales_Fruits.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDictSales_Fruits[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_DictSales_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_DictSales_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }
                if (entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDictSales_Fruits.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDictSales_Fruits[entry.Key.ToString("MMMM")] += Math.Round(entry.Value, 2);
                    }
                }
                


            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, double> entry in Fruits_sales)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_DictSales_Fruits.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_DictSales_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_DictSales_Fruits.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_DictSales_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }
            // This is for herbs
            foreach (var key in Today_DictSales_Final_Fruits)
            {
                if (Today_DictSales_Fruits.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_DictSales_Final_Fruits[key.Key] = Math.Round(Today_DictSales_Fruits[key.Key.Substring(0, 2)], 2);
                }
            }

            foreach (var key in Yesterday_DictSales_Final_Fruits)
            {
                if (Yesterday_DictSales_Fruits.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_DictSales_Final_Fruits[key.Key] = Math.Round(Yesterday_DictSales_Fruits[key.Key.Substring(0, 2)], 2);
                }
            }

            Fruits_SalesOver_28Day = TwentyEight_DictSales_Fruits;
            Fruits_SalesOver_7Day = SevenDayDictSales_Fruits;
            Fruits_SalesOver_Today = Today_DictSales_Final_Fruits;
            Fruits_SalesOver_Yesterday = Yesterday_DictSales_Final_Fruits;
            Fruits_SalesOver_Monthly = MonthlyDictSales_Fruits;






        }

        public void CategoryData_Quantity()
        {
            var Herbs_Quantity = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id where prod.Category == "Herbs" select new { cat.quantity, cat.DateOfOrder }).GroupBy(x => x.DateOfOrder).Select(f => new
            {
                TheDate = f.Key,
                Quantity = f.Sum(a => a.quantity),

            }).ToDictionary(x => x.TheDate, x => x.Quantity);

            var Vegetable_Quantity = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id where prod.Category == "Vegetable" select new { cat.quantity, cat.DateOfOrder }).GroupBy(x => x.DateOfOrder).Select(f => new
            {
                TheDate = f.Key,
                Quantity = f.Sum(a => a.quantity),

            }).ToDictionary(x => x.TheDate, x => x.Quantity);

            var Fruits_Quantity = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id where prod.Category == "Fruits" select new { cat.quantity, cat.DateOfOrder }).GroupBy(x => x.DateOfOrder).Select(f => new
            {
                TheDate = f.Key,
                Quantity = f.Sum(a => a.quantity),

            }).ToDictionary(x => x.TheDate, x => x.Quantity);

            // For Herbs
            Dictionary<string, double> MonthlyDict_Quantity_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDict_Quantity_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_Dict_Quantity_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Today_Dict_Quantity_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Today_Dict_Quantity_Final_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_Dict_Quantity_Herbs = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_Dict_Quantity_Final_Herbs = new Dictionary<string, double>();

            //For Vegetable
            Dictionary<string, double> MonthlyDict_Quantity_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDict_Quantity_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_Dict_Quantity_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Today_Dict_Quantity_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Today_Dict_Quantity_Final_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_Dict_Quantity_Vegetable = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_Dict_Quantity_Final_Vegetable = new Dictionary<string, double>();

            // For Fruits
            Dictionary<string, double> MonthlyDict_Quantity_Fruits= new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDict_Quantity_Fruits= new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_Dict_Quantity_Fruits= new Dictionary<string, double>();
            Dictionary<string, double> Today_Dict_Quantity_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Today_Dict_Quantity_Final_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_Dict_Quantity_Fruits = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_Dict_Quantity_Final_Fruits = new Dictionary<string, double>();

            for (int i = 0; i < Time_Hours.Count; i++)
            {
                Yesterday_Dict_Quantity_Final_Herbs[Time_Hours[i]] = 0;
                Today_Dict_Quantity_Final_Herbs[Time_Hours[i]] = 0;
                Yesterday_Dict_Quantity_Final_Vegetable[Time_Hours[i]] = 0;
                Today_Dict_Quantity_Final_Vegetable[Time_Hours[i]] = 0;
                Today_Dict_Quantity_Final_Fruits[Time_Hours[i]] = 0;
                Yesterday_Dict_Quantity_Final_Fruits[Time_Hours[i]] = 0;

            }

            for (int i = 0; i < Months.Count; i++)
            {
                MonthlyDict_Quantity_Vegetable[Months[i]] = 0;
                MonthlyDict_Quantity_Herbs[Months[i]] = 0;
                MonthlyDict_Quantity_Fruits[Months[i]] = 0;
            }

            for (int i = 1; i < 29; i++)
            {
                TwentyEight_Dict_Quantity_Vegetable[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                TwentyEight_Dict_Quantity_Herbs[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                TwentyEight_Dict_Quantity_Fruits[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
            }


            for (int i = 1; i < 8; i++)
            {
                SevenDayDict_Quantity_Vegetable[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                SevenDayDict_Quantity_Herbs[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
                SevenDayDict_Quantity_Fruits[DateTime.Now.AddDays(-1 * i).ToString("dd/MM/yyyy")] = 0.0;
            }


            // For herbs first
            foreach (KeyValuePair<DateTime, int> entry in Herbs_Quantity)
            {
                if (TwentyEight_Dict_Quantity_Herbs.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_Dict_Quantity_Herbs[entry.Key.ToString("dd/MM/yyyy")] += entry.Value;
                    if (SevenDayDict_Quantity_Herbs.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDict_Quantity_Herbs[entry.Key.ToString("dd/MM/yyyy")] += entry.Value;
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_Dict_Quantity_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_Dict_Quantity_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }
                if (entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDict_Quantity_Herbs.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDict_Quantity_Herbs[entry.Key.ToString("MMMM")] += entry.Value;
                    }
                }
                


            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, int> entry in Herbs_Quantity)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_Dict_Quantity_Herbs.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_Dict_Quantity_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_Dict_Quantity_Herbs.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_Dict_Quantity_Herbs[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }
            // This is for seed 
            foreach (var key in Today_Dict_Quantity_Final_Herbs)
            {
                if (Today_Dict_Quantity_Herbs.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_Dict_Quantity_Final_Herbs[key.Key] = Math.Round(Today_Dict_Quantity_Herbs[key.Key.Substring(0, 2)], 2);
                }
            }

            foreach (var key in Yesterday_Dict_Quantity_Final_Herbs)
            {
                if (Yesterday_Dict_Quantity_Herbs.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_Dict_Quantity_Final_Herbs[key.Key] = Math.Round(Yesterday_Dict_Quantity_Herbs[key.Key.Substring(0, 2)], 2);
                }
            }

            Herbs_Quantity_Over_Today = Today_Dict_Quantity_Final_Herbs;
            Herbs_Quantity_Over_Yesterday = Yesterday_Dict_Quantity_Final_Herbs;
            Herbs_Quantity_Over_7Day = SevenDayDict_Quantity_Herbs;
            Herbs_Quantity_Over_28Day = TwentyEight_Dict_Quantity_Herbs;
            Herbs_Quantity_Over_Monthly = MonthlyDict_Quantity_Herbs;
            // End of seed
            // Start of Vegetable

            foreach (KeyValuePair<DateTime, int> entry in Vegetable_Quantity)
            {
                if (TwentyEight_Dict_Quantity_Vegetable.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_Dict_Quantity_Vegetable[entry.Key.ToString("dd/MM/yyyy")] += entry.Value;
                    if (SevenDayDict_Quantity_Vegetable.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDict_Quantity_Vegetable[entry.Key.ToString("dd/MM/yyyy")] += entry.Value;
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_Dict_Quantity_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_Dict_Quantity_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }
                if (entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDict_Quantity_Vegetable.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDict_Quantity_Vegetable[entry.Key.ToString("MMMM")] += entry.Value;
                    }
                }
                


            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, int> entry in Vegetable_Quantity)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_Dict_Quantity_Vegetable.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_Dict_Quantity_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_Dict_Quantity_Vegetable.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_Dict_Quantity_Vegetable[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }
            // This is for Vegetable
            foreach (var key in Today_Dict_Quantity_Final_Vegetable)
            {
                if (Today_Dict_Quantity_Vegetable.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_Dict_Quantity_Final_Vegetable[key.Key] = Today_Dict_Quantity_Vegetable[key.Key.Substring(0, 2)];
                }
            }

            foreach (var key in Yesterday_Dict_Quantity_Final_Vegetable)
            {
                if (Yesterday_Dict_Quantity_Vegetable.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_Dict_Quantity_Final_Vegetable[key.Key] = Yesterday_Dict_Quantity_Vegetable[key.Key.Substring(0, 2)];
                }
            }





            Vegetable_Quantity_Over_Today = Today_Dict_Quantity_Final_Vegetable;
            Vegetable_Quantity_Over_Yesterday = Yesterday_Dict_Quantity_Final_Vegetable;
            Vegetable_Quantity_Over_7Day = SevenDayDict_Quantity_Vegetable;
            Vegetable_Quantity_Over_28Day = TwentyEight_Dict_Quantity_Vegetable;
            Vegetable_Quantity_Over_Monthly = MonthlyDict_Quantity_Vegetable;
            //foreach (KeyValuePair<DateTime, int> entry in Seed_Quantity) System.Diagnostics.Debug.WriteLine($"{entry.Key} {entry.Value}");

            foreach (KeyValuePair<DateTime, int> entry in Fruits_Quantity)
            {
                if (TwentyEight_Dict_Quantity_Fruits.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_Dict_Quantity_Fruits[entry.Key.ToString("dd/MM/yyyy")] += entry.Value;
                    if (SevenDayDict_Quantity_Fruits.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDict_Quantity_Fruits[entry.Key.ToString("dd/MM/yyyy")] += entry.Value;
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_Dict_Quantity_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_Dict_Quantity_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }

                if(entry.Key.Year == DateTime.Today.Year)
                {
                    if (MonthlyDict_Quantity_Fruits.ContainsKey(entry.Key.ToString("MMMM")))
                    {
                        MonthlyDict_Quantity_Fruits[entry.Key.ToString("MMMM")] += entry.Value;
                    }
                }
                


            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, int> entry in Fruits_Quantity)
            {
                if (entry.Key.Date == DateTime.Today)
                {
                    if (Today_Dict_Quantity_Fruits.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Today_Dict_Quantity_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }


                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {
                    if (Yesterday_Dict_Quantity_Fruits.ContainsKey(entry.Key.TimeOfDay.ToString().Substring(0, 2)))
                    {
                        Yesterday_Dict_Quantity_Fruits[entry.Key.TimeOfDay.ToString().Substring(0, 2)] += entry.Value;
                    }
                }
            }
            // This is for Vegetable
            foreach (var key in Today_Dict_Quantity_Final_Fruits)
            {
                if (Today_Dict_Quantity_Fruits.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_Dict_Quantity_Final_Fruits[key.Key] = Today_Dict_Quantity_Fruits[key.Key.Substring(0, 2)];
                }
            }

            foreach (var key in Yesterday_Dict_Quantity_Final_Fruits)
            {
                if (Yesterday_Dict_Quantity_Fruits.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_Dict_Quantity_Final_Fruits[key.Key] = Yesterday_Dict_Quantity_Fruits[key.Key.Substring(0, 2)];
                }
            }

            Fruits_Quantity_Over_Today = Today_Dict_Quantity_Final_Fruits;
            Fruits_Quantity_Over_Yesterday = Yesterday_Dict_Quantity_Final_Fruits;
            Fruits_Quantity_Over_7Day = SevenDayDict_Quantity_Fruits;
            Fruits_Quantity_Over_28Day = TwentyEight_Dict_Quantity_Fruits;
            Fruits_Quantity_Over_Monthly = MonthlyDict_Quantity_Fruits;




        }
    }
}