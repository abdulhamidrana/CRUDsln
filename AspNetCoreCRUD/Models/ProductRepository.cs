using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreCRUD.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Product GetProductById(int id)
        {
            Product product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            var data = _context.Products.Select(p => new Product
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                UrlImage = p.UrlImage,
                CategoryId = p.CategoryId,
                Category = _context.Categories.Where(c => c.Id == p.CategoryId).FirstOrDefault()
            }).ToList();

            return data;
        }

        public void UpdateProduct(Product changeProduct)
        {
            var product1 = _context.Products.Attach(changeProduct);
            product1.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
