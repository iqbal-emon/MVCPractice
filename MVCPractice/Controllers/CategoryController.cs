using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCPractice.Models;

namespace MVCPractice.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
           _context = context;
        }
        public IActionResult Index()
        {
          var categoriesList = _context.Categories.ToList();
            ViewBag.Message = TempData["SuccessMessage"];
            return View(categoriesList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Category Added Succesfully";
            return RedirectToAction("Index","Category");
        }
        public IActionResult Edit(int? id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.Categories.Update(category);
            TempData["SuccessMessage"] = "Category Updated Successfully";
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Delete(int? id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Category Deleted Succesfully";

            return RedirectToAction("Index", "Category");
        }


    }
}
