using Microsoft.AspNetCore.Http;

namespace ShopOnline.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductRepository(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
     
        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            if (httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await dbContext.Products                    
                    .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted);
            }
            else
            {
                product = await dbContext.Products                    
                    .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && p.Visible);
            }

            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await dbContext.Products
                .Where(p => p.Visible && !p.Deleted)
                .ToListAsync()
            };            
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await dbContext.Products
                .Where(p => p.Category.Url.ToLower() == categoryUrl.ToLower() &&
                p.Visible && !p.Deleted)
                .ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchString)
        {
            var products = await GetProductsBySearchStringAsync(searchString);

            List<string> suggestions = new List<string>();
            foreach (var product in products)
            {
                if (product.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    suggestions.Add(product.Name);
                if (product.Description != null)
                {
                    var punctuations = product.Description.Where(char.IsPunctuation).
                        Distinct().ToArray();
                    var words = product.Description.Split().Select(w => w.Trim(punctuations));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            && !suggestions.Contains(word))
                        {
                            suggestions.Add(word);
                        }
                    }
                }

            }

            return new ServiceResponse<List<string>> { Data = suggestions };
        }

        private async Task<List<Product>> GetProductsBySearchStringAsync(string searchString)
        {
            return await dbContext.Products.
                Where(p => p.Visible && !p.Deleted &&
                p.Name.ToLower().Contains(searchString.ToLower()) ||
                p.Description.ToLower().Contains(searchString.ToLower()))              
                .ToListAsync();
        }
        public async Task<ServiceResponse<ProductSearchResultDTO>> SearchProductsAsync(string searchString, int page)
        {
            var countPerPage = 2f;
            var totalPages = Math.Ceiling((await GetProductsBySearchStringAsync(searchString)).Count / countPerPage);

            var products = await dbContext.Products.
                Where(p => p.Visible && !p.Deleted && 
                p.Name.ToLower().Contains(searchString.ToLower()) ||
                p.Description.ToLower().Contains(searchString.ToLower())).
                Skip((page - 1) * (int)countPerPage).
                Take((int)countPerPage).ToListAsync();

            var response = new ServiceResponse<ProductSearchResultDTO>()
            {
               Data = new ProductSearchResultDTO
               {
                   Products = products,
                   CurrentPage = page,
                   TotalPages = (int)totalPages
               }
                
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await dbContext.Products
                     .Where(p => !p.Deleted)                     
                     .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> CreateProductAsync(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<Product>> UpdateProductAsync(Product product)
        {
            var dbProduct = await dbContext.Products               
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Price = product.Price;
            dbProduct.Visible = product.Visible;           

            await dbContext.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int productId)
        {
            var dbProduct = await dbContext.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Deleted = true;

            await dbContext.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }       
    }
}
