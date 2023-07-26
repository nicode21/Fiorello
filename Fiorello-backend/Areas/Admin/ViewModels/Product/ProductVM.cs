﻿using Fiorello_backend.Models;

namespace Fiorello_backend.Areas.Admin.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Discount { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
    }
}
