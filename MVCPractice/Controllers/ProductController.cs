using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Models;

namespace MVCPractice.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var productList = _context.Products.Include(p => p.Category).ToList();
            return View(productList);
        }
        public IActionResult Create()
        {
            CategoryProductDto categoryProductDto = new CategoryProductDto();
            categoryProductDto.CategoryList = _context.Categories.ToList();
            categoryProductDto.Product=new Product();
            return View(categoryProductDto);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
        public IActionResult Edit(int? id)
        {
            var products = _context.Products.FirstOrDefault(c => c.Id == id);

            CategoryProductDto categoryProductDto = new CategoryProductDto();
            categoryProductDto.CategoryList = _context.Categories.ToList();
            categoryProductDto.Product = products;
            return View(categoryProductDto);

        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Delete(int? id)
        {
            var product = _context.Products.FirstOrDefault(c => c.Id == id);
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var product = _context.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }


    }
}
