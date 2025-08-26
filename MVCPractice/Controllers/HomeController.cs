using Microsoft.AspNetCore.Authorization;
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

            return RedirectToAction("Summary","Home");
        }
        [Authorize]
        public IActionResult Summary()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           ShoppingCardVM shoppingCardVMs = new ShoppingCardVM();

            shoppingCardVMs.ShoppingCardList = _context.ShoppingCarts.Where(s => s.UserId == userId).Include(s=>s.Product).ToList();
            foreach (var obj in shoppingCardVMs.ShoppingCardList)
            {
                obj.Total = obj.Product.Price * obj.Count;
                shoppingCardVMs.TotalAmount += (obj.Product.Price*obj.Count);

            }

            return View(shoppingCardVMs);
        }

        public IActionResult Checkout()
        {
            CheckoutVM checkoutVM = new CheckoutVM();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ShoppingCardVM shoppingCardVMs = new ShoppingCardVM();

            shoppingCardVMs.ShoppingCardList = _context.ShoppingCarts.Where(s => s.UserId == userId).Include(s => s.Product).ToList();
            foreach (var obj in shoppingCardVMs.ShoppingCardList)
            {
                obj.Total = obj.Product.Price * obj.Count;
                shoppingCardVMs.TotalAmount += (obj.Product.Price * obj.Count);

            }
            checkoutVM.ShoppingCard = shoppingCardVMs;


            return View(checkoutVM);
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutVM checkoutVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartList = _context.ShoppingCarts.Where(s => s.UserId == userId).Include(s => s.Product).ToList();
            decimal TotalAmount = 0;
            foreach (var obj in cartList)
            {
                obj.Total = obj.Product.Price * obj.Count;
                TotalAmount += (obj.Product.Price * obj.Count);

            }

            OrderHeader orderHeader = new OrderHeader();
            orderHeader = checkoutVM.OrderHeader;
            orderHeader.PaymentStatus = "Pending";
            orderHeader.OrderStatus = "Pending";
            orderHeader.UserId = userId;
            orderHeader.OrderTotal = TotalAmount;
            orderHeader.OrderDate = DateTime.Now;
            _context.OrderHeader.Add(orderHeader);
            _context.SaveChanges();

            List<OrderDetail> orderDetail = new List<OrderDetail>();


            foreach (var Model in cartList)
            {
                OrderDetail newOrder = new OrderDetail
                {
                    OrderId =checkoutVM.OrderHeader.Id,
                    ProductId =Model.ProductId,
                    Count =Model.Count,
                    Price =Model.Product.Price,


                };
                orderDetail.Add(newOrder);
                

            }
            _context.OrderDetails.AddRange(orderDetail);
            _context.SaveChanges();
            // 4. Clear the Cart
            _context.ShoppingCarts.RemoveRange(cartList);
            _context.SaveChanges();






            return RedirectToAction("Index","Home");
        }


        public IActionResult Plus(int id)
        {

           var UpdateData = _context.ShoppingCarts.FirstOrDefault(s=>s.Id==id);
            UpdateData.Count += 1;
            _context.ShoppingCarts.Update(UpdateData);
            _context.SaveChanges();

            return RedirectToAction(nameof(Summary));
        }
        public IActionResult Minus(int id)
        {

            var UpdateData = _context.ShoppingCarts.FirstOrDefault(s => s.Id == id);
            if (UpdateData.Count > 1)
            {
                UpdateData.Count -= 1;
                _context.ShoppingCarts.Update(UpdateData);
                _context.SaveChanges();
            }
            else
            {

                _context.ShoppingCarts.Remove(UpdateData);
                _context.SaveChanges();
            }


                return RedirectToAction(nameof(Summary));
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
