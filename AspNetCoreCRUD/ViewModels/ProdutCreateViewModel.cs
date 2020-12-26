using AspNetCoreCRUD.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreCRUD.ViewModels
{
    public class ProdutCreateViewModel
    {
        [Required(ErrorMessage = "Can't be empty")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public decimal Price { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public IFormFile Photo { get; set; }
    }
}
