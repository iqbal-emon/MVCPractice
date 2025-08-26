using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCPractice.Models;

namespace MVCPractice.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Create(Product product)
        {
            if (product.ImageFile != null)
            {
                // 1. Create folder path
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);

                fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                // Ensure images folder exists
                string imageFolder = Path.Combine(wwwRootPath, "images");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                string path = Path.Combine(imageFolder, fileName);

                // 2. Save the image
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }

                // 3. Save file name to DB
                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Edit(Product product)
        {
            
                var existingProduct = await _context.Products.FindAsync(product.Id);
                if (existingProduct == null)
                    return NotFound();

                // Update other fields
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;

                // Only update image if a new file is uploaded
                if (product.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    fileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";

                    string imageFolder = Path.Combine(wwwRootPath, "images");
                    if (!Directory.Exists(imageFolder))
                        Directory.CreateDirectory(imageFolder);

                    string path = Path.Combine(imageFolder, fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }

                    // Delete old image (optional)
                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, existingProduct.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    existingProduct.ImageUrl = "/images/" + fileName;
                }

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();



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
