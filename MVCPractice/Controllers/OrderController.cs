using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Models;
using System.Security.Claims;

namespace MVCPractice.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = _context.OrderHeader.Where(o =>o.UserId ==userId);

            return View(orders);
        }
        [Authorize]

        public IActionResult OrderDetails(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _context.OrderHeader
          .Where(o => o.Id == id && o.UserId == userId)  
          .Include(o => o.OrderDetails)
              .ThenInclude(od => od.Product)
          .FirstOrDefault();
            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }


    }
}
