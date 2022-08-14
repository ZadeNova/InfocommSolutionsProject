using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public DetailsModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public ProductModel Product { get; set; } = default!;
        
        public Dictionary<DateTime , double> SalesOverTime_ForProduct { get; set; }

        public List<string> Time_Hours { get; } = new List<string> {
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

        public List<string> Months { get; } = new List<string>
        {
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

        public Dictionary<string , double> ProductSales_Today { get; set; }

        public Dictionary<string, double> ProductSales_Yesterday { get; set; }

        public Dictionary<string, double> ProductSales_7Day { get; set; }

        public Dictionary<string, double> ProductSales_28Day { get; set; }

        public Dictionary<string, double> ProductSales_Month { get; set; }

        public double Total_ProductSales_Today { get; set; }

        public double Total_ProductSales_Yesterday { get; set; }

        public double Total_ProductSales_7Day { get; set; }

        public double Total_ProductSales_28Day { get; set; }

        public double Total_ProductSales_Month { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {   
                Product = product;
                //var test = (from prod in _context.Orders where prod.Id == id select new { TheDate = prod.DateOfOrder , Sales = prod.PriceOfOrder }).ToDictionary( x => x.TheDate , x => x.Sales);
                //foreach (KeyValuePair<DateTime, double> entry in test) System.Diagnostics.Debug.WriteLine($"{entry.Key} {entry.Value} wtf");
                //var lol = _context.Orders.Where(x => x.Product.Id == id).Select(F => new
                //{
                //    TheDate = F.DateOfOrder,
                //    Sales = F.PriceOfOrder
                //}).ToDictionary(x => x.TheDate, x => x.Sales);
                SalesOverTime_ForProduct = _context.Orders.Where(x => x.Product.Id == id).GroupBy(x => x.DateOfOrder).Select(l => new
                {
                    TheDate = l.Key,
                    Sales = l.Sum(p => p.PriceOfOrder)
                }).ToDictionary(m => m.TheDate , m => m.Sales);

                CategorizeToDates_Sales();

                Total_ProductSales_Today = Math.Round(SalesOverTime_ForProduct.Where(x => x.Key.Date == DateTime.Today).Sum(x => x.Value) , 2);
                Total_ProductSales_Yesterday = Math.Round(SalesOverTime_ForProduct.Where(x => x.Key.Date == DateTime.Today.AddDays(-1)).Sum(x => x.Value) , 2);
                Total_ProductSales_7Day = Math.Round(ProductSales_7Day.Sum(x => x.Value) , 2);
                Total_ProductSales_28Day = Math.Round(ProductSales_28Day.Sum(x => x.Value) , 2);
                Total_ProductSales_Month = Math.Round(ProductSales_Month.Sum(x => x.Value),2);
                
                foreach (KeyValuePair<DateTime, double> entry in SalesOverTime_ForProduct) System.Diagnostics.Debug.WriteLine($"{entry.Key} {entry.Value} wtf");
                System.Diagnostics.Debug.WriteLine("OI HELLO THEREa");



            }
            return Page();
        }

        // Categorize all data to the dates
        public void CategorizeToDates_Sales()
        {
            Dictionary<string, double> MonthlyDictSales = new Dictionary<string, double>();
            Dictionary<string, double> SevenDayDictSales = new Dictionary<string, double>();
            Dictionary<string, double> TwentyEight_DictSales = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales = new Dictionary<string, double>();
            Dictionary<string, double> Today_DictSales_Final = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales = new Dictionary<string, double>();
            Dictionary<string, double> Yesterday_DictSales_Final = new Dictionary<string, double>();

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
            foreach (KeyValuePair<DateTime, double> entry in SalesOverTime_ForProduct)
            {
                if (TwentyEight_DictSales.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                {
                    TwentyEight_DictSales[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    if (SevenDayDictSales.ContainsKey(entry.Key.ToString("dd/MM/yyyy")))
                    {
                        SevenDayDictSales[entry.Key.ToString("dd/MM/yyyy")] += Math.Round(entry.Value, 2);
                    }
                }

                if (entry.Key.Date == DateTime.Today)
                {
                    Today_DictSales[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;
                }

                if (entry.Key.Date == DateTime.Today.AddDays(-1))
                {

                    Yesterday_DictSales[entry.Key.TimeOfDay.ToString().Substring(0, 2)] = 0;

                }

                if (MonthlyDictSales.ContainsKey(entry.Key.ToString("MMMM")))
                {
                    MonthlyDictSales[entry.Key.ToString("MMMM")] += Math.Round(entry.Value, 2);
                }


            }

            // For today and Yesterday only
            foreach (KeyValuePair<DateTime, double> entry in SalesOverTime_ForProduct)
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
                if (Today_DictSales.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Today_DictSales_Final[key.Key] = Math.Round(Today_DictSales[key.Key.Substring(0, 2)], 2);
                }
            }

            foreach (var key in Yesterday_DictSales_Final)
            {
                if (Yesterday_DictSales.ContainsKey(key.Key.Substring(0, 2)))
                {
                    Yesterday_DictSales_Final[key.Key] = Math.Round(Yesterday_DictSales[key.Key.Substring(0, 2)], 2);
                }
            }


            ProductSales_Today = Today_DictSales_Final;
            ProductSales_Yesterday = Yesterday_DictSales_Final;
            ProductSales_7Day = SevenDayDictSales;
            ProductSales_28Day = TwentyEight_DictSales;
            ProductSales_Month = MonthlyDictSales;
            foreach (var key in Today_DictSales_Final) System.Diagnostics.Debug.WriteLine($"LOOOL { key.Key } {key.Value}");

        }

    }
}
