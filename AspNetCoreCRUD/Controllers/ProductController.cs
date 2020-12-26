using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreCRUD.Models;
using AspNetCoreCRUD.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreCRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductRepository repository, IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _repository.GetProducts().ToList();
            return View(products);
        }

        public void CategoryDDL(object selectedValue = null)
        {
            List<Category> categories = _repository.GetCategories().ToList();
            ViewBag.DDL = new SelectList(categories, "Id", "CategoryName", selectedValue);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CategoryDDL();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProdutCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UrlImage = "";
                var files = HttpContext.Request.Form.Files;
                foreach (var image in files)
                {
                    if (image != null && image.Length > 0)
                    {
                        var file = image;
                        var uploadFiles = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("_", "") + file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploadFiles, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                UrlImage = fileName;
                            }
                        }
                    }
                }

                var data = new Product()
                {
                    ProductName = model.ProductName,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    UrlImage = UrlImage
                };
                _repository.CreateProduct(data);
                return RedirectToAction(nameof(Index));
            }
            return View();            
        }

        [HttpGet]
        public IActionResult Edit (int id)
        {
            Product product = _repository.GetProductById(id);
            CategoryDDL();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                string imgUrl = "";
                var files = HttpContext.Request.Form.Files;

                foreach(var image in files)
                {
                    if (image != null && image.Length > 0)
                    {
                        var file = image;
                        var uploadFiles = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("_", "") + file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploadFiles, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                imgUrl = fileName;
                            }
                        }
                    }
                    Product produtFromDB = _repository.GetProductById(id);
                    produtFromDB.ProductName = product.ProductName;
                    produtFromDB.Price = product.Price;
                    produtFromDB.CategoryId = product.CategoryId;

                    if (produtFromDB.UrlImage != null)
                    {
                        string fp = Path.Combine(_hostingEnvironment.WebRootPath, "images", produtFromDB.UrlImage);
                        System.IO.File.Delete(fp);
                    }
                    produtFromDB.UrlImage = imgUrl;

                    _repository.UpdateProduct(produtFromDB);
                    return RedirectToAction(nameof(Index));
                }                
            }
            return View();

        }

        public IActionResult Delete(int id)
        {
            _repository.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
