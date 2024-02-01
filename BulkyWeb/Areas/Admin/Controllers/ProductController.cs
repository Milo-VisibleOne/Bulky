using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var objCategoryList = _unitOfWork.productRepository.GetAll(IncludeProperties: "Category").ToList();
            return View(objCategoryList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.categoryRepository.GetAll().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.productRepository.Get(x => x.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string oldPath = Path.Combine(wwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.productRepository.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.productRepository.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.categoryRepository.GetAll().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product objProduct = _unitOfWork.productRepository.Get(u => u.Id == id);
            if (objProduct == null)
            {
                return NotFound();
            }
            return View(objProduct);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.productRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
       
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objCategoryList = _unitOfWork.productRepository.GetAll(IncludeProperties: "Category").ToList();
            return Json(new { data = objCategoryList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.productRepository.Get(u => u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false,message="Error while deleting" });
            }
            string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _unitOfWork.productRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }

        #endregion
    }
}
