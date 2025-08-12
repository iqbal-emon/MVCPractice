using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MVCPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;


        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var productList = _context.Products.Include(p => p.Category).ToList();
            return View(productList);
        }
        public IActionResult Details(int Id)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == Id);
            ShoppingCart shoppingCart = new ShoppingCart
            {
                ProductId = product.Id,
                Product = product,
                Count = 1
            };
            return View(shoppingCart);
        }

        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            shoppingCart.UserId = userId;
            shoppingCart.Id = 0;

            var dbShoppingCart = _context.ShoppingCarts.FirstOrDefault(s => s.ProductId == shoppingCart.ProductId && s.UserId == shoppingCart.UserId);
            if (dbShoppingCart != null)
            {
                dbShoppingCart.Count += shoppingCart.Count;
                _context.ShoppingCarts.Update(dbShoppingCart);

            }
            else
            {
                _context.ShoppingCarts.Add(shoppingCart);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
