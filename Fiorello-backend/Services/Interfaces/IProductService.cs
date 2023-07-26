﻿using Fiorello_backend.Areas.Admin.ViewModels.Product;
using Fiorello_backend.Models;

namespace Fiorello_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<List<Product>> GetPaginatedDatasAsync(int page, int take);
        Task<Product> GetByIdAsync(int? id);
        Task<Product> GetByIdWithImagesAsync(int? id);
        List<ProductVM> GetMappedDatas(List<Product> products);
        Task<Product> GetWithIncludesAsync(int id);
        ProductDetailVM GetMappedData(Product product);
        Task<int> GetCountAsync();
        Task CreateAsync(ProductCreateVM model);
        Task DeleteImageByIdAsync(int id);
        Task EditAsync(int productId, ProductEditVM model);
        Task DeleteAsync(int id);
    }
}
