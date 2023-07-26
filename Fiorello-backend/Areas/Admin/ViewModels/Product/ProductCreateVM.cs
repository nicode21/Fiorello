﻿using Fiorello_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Fiorello_backend.Areas.Admin.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        public int CategoryId { get; set; }
        public int DiscountId { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
