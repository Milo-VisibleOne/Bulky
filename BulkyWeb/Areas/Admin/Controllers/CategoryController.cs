using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var objCategoryList = _unitOfWork.categoryRepository.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display Order cannot exactly match the name");
            }
            //if (obj.Name != null && obj.Name == "test")
            //{
            //    ModelState.AddModelError("", "Test is an invalid input");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.categoryRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category objCategory = _unitOfWork.categoryRepository.Get(u => u.Id == id);
            if (objCategory == null)
            {
                return NotFound();
            }
            return View(objCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.categoryRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category objCategory = _unitOfWork.categoryRepository.Get(u => u.Id == id);
            if (objCategory == null)
            {
                return NotFound();
            }
            return View(objCategory);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category objCategory = _unitOfWork.categoryRepository.Get(u => u.Id == id);
            if (objCategory == null)
            {
                return NotFound();
            }
            _unitOfWork.categoryRepository.Remove(objCategory);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
