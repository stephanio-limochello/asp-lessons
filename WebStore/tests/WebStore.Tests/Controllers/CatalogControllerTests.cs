using System.Linq;
using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void DetailsReturnsWithCorrectView()
        {
            const int expectedProductId = 1;
            const decimal expectedPrice = 10m;
            var expectedName = $"Product id {expectedProductId}";
            var expectedBrandName = $"Brand of product {expectedProductId}";
            var productDataMock = new Mock<IProductData>();
            productDataMock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns<int>(id => new ProductDTO
               {
                   Id = id,
                   Name = $"Product id {id}",
                   ImageUrl = $"img{id}.png",
                   Order = 1,
                   Price = expectedPrice,
                   Brand = new BrandDTO
                   {
                       Id = 1,
                       Name = $"Brand of product {id}"
                   },
                   Section = new SectionDTO
                   {
                       Id = 1,
                       Name = $"Section of product {id}"
                   }
               });
            var controller = new CatalogController(productDataMock.Object);
            var result = controller.Details(expectedProductId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);

            Assert.Equal(expectedProductId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedPrice, model.Price);
            Assert.Equal(expectedBrandName, model.Brand);
        }

        [TestMethod]
        public void ShopReturnsCorrectView()
        {
            var products = new[]
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "Product 1",
                    Order = 0,
                    Price = 10m,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDTO
                    {
                        Id = 1,
                        Name = "Brand of product 1"
                    },
                    Section = new SectionDTO
                    {
                        Id = 1,
                        Name = "Section of product 1"
                    }
                },
                new ProductDTO
                {
                    Id = 2,
                    Name = "Product 2",
                    Order = 0,
                    Price = 20m,
                    ImageUrl = "Product2.png",
                    Brand = new BrandDTO
                    {
                        Id = 2,
                        Name = "Brand of product 2"
                    },
                    Section = new SectionDTO
                    {
                        Id = 2,
                        Name = "Section of product 2"
                    }
                },
            };

            var productDataMock = new Mock<IProductData>();
            productDataMock
               .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(products);
            const int expectedSectionId = 1;
            const int expectedBrandId = 5;
            var controller = new CatalogController(productDataMock.Object);
            var result = controller.Shop(expectedBrandId, expectedSectionId);
            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);

            Assert.Equal(products.Length, model.Products.Count());
            Assert.Equal(expectedBrandId, model.BrandId);
            Assert.Equal(expectedSectionId, model.SectionId);
        }
    }
}