﻿using Fiorello_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Fiorello_backend.Areas.Admin.ViewModels.Product
{
    public class ProductEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        public int CategoryId { get; set; }
        public int DiscountId { get; set; }
        public List<IFormFile> NewImages { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
