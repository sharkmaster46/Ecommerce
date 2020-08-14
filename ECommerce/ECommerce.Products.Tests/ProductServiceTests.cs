using AutoMapper;
using ECommerce.Products.DB;
using ECommerce.Products.Profiles;
using ECommerce.Products.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Products.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var result = await CreateProductProviderInstance(nameof(GetProductsReturnsAllProducts)).GetProductsAsync();
            Assert.True(result.IsSuccess);
            Assert.True(result.Products.Any());
            Assert.Null(result.ErrorMessage);
        }
        [Fact]
        public async Task GetProductsReturnsAllUsingValidId()
        {
            var result = await CreateProductProviderInstance(nameof(GetProductsReturnsAllUsingValidId)).GetProductAsync(1);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Product);
            Assert.True(result.Product.Id == 1);
            Assert.Null(result.ErrorMessage);
        }
        [Fact]
        public async Task GetProductsReturnsAllUsingInvalidId()
        {
            
            var result = await CreateProductProviderInstance(nameof(GetProductsReturnsAllUsingInvalidId)).GetProductAsync(-1);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Product);
            Assert.NotNull(result.ErrorMessage);
        }


        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1 ; i <=10; i++)
            {
                dbContext.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.145)
                });
            }
            dbContext.SaveChanges();
        }
        private ProductsProvider CreateProductProviderInstance(string key)
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(key)
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            return new ProductsProvider(dbContext, null, mapper);
        }
    }
}
