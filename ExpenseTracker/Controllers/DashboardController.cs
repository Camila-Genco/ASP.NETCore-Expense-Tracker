using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //Last 30 days
            DateTime StartDate = DateTime.Today.AddDays(-29);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(y => y.Date.Date >= StartDate && y.Date.Date <= EndDate)
                .ToListAsync();

            //Total income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);

            //Pie Chart            
            var transactions = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.Date.Date >= StartDate && t.Date.Date <= EndDate)
            .ToListAsync();

            ViewBag.PieChartData = transactions
                .Where(t => t.Category.Type == "Expense")
                .GroupBy(t => t.Category.CategoryId)
                .Select(group => new
                {
                    categoryName = group.First().Category.Name,
                    totalAmount = group.Sum(t => t.Amount)
                })
                .OrderByDescending(l => l.totalAmount)
                .ToList();


            //Line Chart
            DateTime StartWeekDate = DateTime.Today.AddDays(-6);
            DateTime EndWeekDate = DateTime.Today;

            var expenses = await _context.Transactions
           .Where(t => t.Date.Date >= StartWeekDate && t.Date.Date <= EndWeekDate && t.Category.Type == "Expense")
           .GroupBy(t => t.Date.Date)
           .Select(group => new { Date = group.First().Date.Date, TotalExpense = group.Sum(t => t.Amount) })
           .OrderBy(g => g.Date)
           .ToListAsync();

            var incomes = await _context.Transactions
            .Where(t => t.Date.Date >= StartWeekDate && t.Date.Date <= EndWeekDate && t.Category.Type == "Income")
            .GroupBy(t => t.Date.Date)
            .Select(group => new { Date = group.First().Date.Date, TotalIncome = group.Sum(t => t.Amount) })
            .OrderBy(g => g.Date)
            .ToListAsync();

            var chartData = new List<object[]>();

            foreach (var dateExpenses in expenses)
            {
                var matchingIncome = incomes.FirstOrDefault(i => i.Date.Date == dateExpenses.Date.Date);
                var incomeAmount = matchingIncome?.TotalIncome ?? 0;

                CultureInfo englishCulture = new CultureInfo("en-US");            
                var dayName = dateExpenses.Date.Date.ToString("dddd", englishCulture);

                chartData.Add(new object[] { dayName, dateExpenses.TotalExpense, incomeAmount });
            }

            ViewBag.LineChartData = chartData;

            //Latest transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            //Savings
            ViewBag.TotalSavings = await _context.Savings
                .SumAsync(i => i.Amount);

            return View();
        }
    }
}
