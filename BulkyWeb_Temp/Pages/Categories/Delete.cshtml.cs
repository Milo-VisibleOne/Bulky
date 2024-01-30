using BulkyWeb_Temp.Data;
using BulkyWeb_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet(int? Id)
        {
            if (Id != null && Id != 0)
            {
                Category = _db.Categories.Find(Id);
            }
        }

        public IActionResult OnPost()
        {
            Category objCategory = _db.Categories.Find(Category.Id);
            if (objCategory == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(objCategory);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}
