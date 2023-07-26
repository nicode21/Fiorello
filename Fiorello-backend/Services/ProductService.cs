using Fiorello_backend.Areas.Admin.ViewModels.Product;
using Fiorello_backend.Data;
using Fiorello_backend.Helpers;
using Fiorello_backend.Models;
using Fiorello_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.Include(m => m.Images).ToListAsync();

        public async Task<List<Product>> GetPaginatedDatasAsync(int page, int take)
        {
            return await _context.Products.Include(m => m.Images)
                                          .Include(m => m.Category)
                                          .Include(m => m.Discount)
                                          .Skip((page-1)*take)
                                          .Take(take)
                                          .ToListAsync();
        }
 

        public async Task<Product> GetByIdAsync(int? id) => await _context.Products.FindAsync(id);

        public async Task<Product> GetByIdWithImagesAsync(int? id) => await _context.Products.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);

        public List<ProductVM> GetMappedDatas(List<Product> products)
        {
            List<ProductVM> list = new();

            foreach (var product in products)
            {
                list.Add(new ProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price.ToString("0.#####") + " ₼",
                    Discount = product.Discount.Name,
                    CategoryName = product.Category.Name,
                    Image = product.Images.Where(m => m.IsMain).FirstOrDefault().Image
                });
            }

            return list;
        }

        public ProductDetailVM GetMappedData(Product product)
        {
        
            return new ProductDetailVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString("0.#####"),
                Discount = product.Discount.Name,
                CategoryName = product.Category.Name,
                CreateDate = product.CreatedDate.ToString("dd-MM-yyyy"),
                Images = product.Images.Select(m => m.Image)
            };
        }

        public async Task<Product> GetWithIncludesAsync(int id)
        {
            return await _context.Products.Where(m => m.Id == id)
                                          .Include(m => m.Images)
                                          .Include(m => m.Category)
                                          .Include(m => m.Discount)
                                          .FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();

        public async Task CreateAsync(ProductCreateVM model)
        {
            List<ProductImage> images = new();

            foreach (var item in model.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                await item.SaveFileAsync(fileName, _env.WebRootPath, "img");
                images.Add(new ProductImage { Image = fileName });
            }

            images.FirstOrDefault().IsMain = true;

            decimal decimalPrice = decimal.Parse(model.Price.Replace(".", ","));

            Product product = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = decimalPrice,
                CategoryId = model.CategoryId,
                DiscountId = model.DiscountId,
                Images = images
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageByIdAsync(int id)
        {
            ProductImage image = await _context.ProductImages.FirstOrDefaultAsync(m => m.Id == id);
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "img", image.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(int productId, ProductEditVM model)
        {
            List<ProductImage> images = new();

            var product = await GetByIdAsync(productId);

            if(model.NewImages != null)
            {
                foreach (var item in model.NewImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    await item.SaveFileAsync(fileName, _env.WebRootPath, "img");
                    images.Add(new ProductImage { Image = fileName, ProductId = productId });
                }

                await _context.ProductImages.AddRangeAsync(images);
            }


            decimal decimalPrice = decimal.Parse(model.Price.Replace(".", ","));

            product.Name = model.Name;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.DiscountId = model.DiscountId;
            product.Price = decimalPrice;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.Include(m=>m.Images).FirstOrDefaultAsync(m=>m.Id == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            foreach (var item in product.Images)
            {
                string path = Path.Combine(_env.WebRootPath, "img", item.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
