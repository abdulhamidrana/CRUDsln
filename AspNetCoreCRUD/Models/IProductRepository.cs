using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreCRUD.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);

        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

        IEnumerable<Category> GetCategories();
    }
}
