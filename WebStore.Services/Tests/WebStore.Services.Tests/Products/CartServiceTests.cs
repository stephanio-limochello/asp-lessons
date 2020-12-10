using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Interfaces;
using WebStore.Services.Products;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Products
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;
        private ICartService _cartService;

        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new() { ProductId = 1, Quantity = 1 },
                    new() { ProductId = 2, Quantity = 3 },
                }
            };

            _productDataMock = new Mock<IProductData>();
            _productDataMock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(new List<ProductDTO>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO { Id = 1, Name = "Brand 1" },
                        Section = new SectionDTO { Id = 1, Name = "Section 1"}
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.2m,
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDTO { Id = 2, Name = "Brand 2" },
                        Section = new SectionDTO { Id = 2, Name = "Section 2"}
                    },
                });

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);
            _cartService = new CartService(_productDataMock.Object, _cartStoreMock.Object);
        }

        [TestMethod]
        public void CartClassItemsCountReturnsCorrectQuantity()
        {
            var cart = _cart;
            const int expectedCount = 4;
            var actualCount = cart.ItemsCount;
            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartViewModelReturnsCorrectItemsCount()
        {
            var cartViewModel = new CartViewModel
            {
                Items = new[]
                {
                    ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1 ),
                    ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 1.5m }, 3 ),
                }
            };

            const int expectedCount = 4;
            var actualCount = cartViewModel.ItemsCount;
            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartServiceAddToCartWorkCorrect()
        {
            _cart.Items.Clear();
            const int expected_id = 5;
            const int expectedItemsCount = 1;
            _cartService.AddToCart(expected_id);
            Assert.Equal(expectedItemsCount, _cart.ItemsCount);
            Assert.Single(_cart.Items);
            Assert.Equal(expected_id, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartServiceRemoveFromCartRemoveCorrectItem()
        {
            const int itemId = 1;
            const int expectedProductId = 2;
            _cartService.RemoveFromCart(itemId);
            Assert.Single(_cart.Items);
            Assert.Equal(expectedProductId, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartServiceClearClearCart()
        {
            _cartService.Clear();
            Assert.Empty(_cart.Items);
        }

        [TestMethod]
        public void CartServiceDecrementCorrect()
        {
            const int item_id = 2;
            const int expectedQuantity = 2;
            const int expectesItemsCount = 3;
            const int expectedProductsCount = 2;
            _cartService.DecrementFromCart(item_id);

            Assert.Equal(expectesItemsCount, _cart.ItemsCount);
            Assert.Equal(expectedProductsCount, _cart.Items.Count);
            var items = _cart.Items.ToArray();
            Assert.Equal(item_id, items[1].ProductId);
            Assert.Equal(expectedQuantity, items[1].Quantity);
        }

        [TestMethod]
        public void CartServiceRemoveItemWhenDecrementToZero()
        {
            const int itemId = 1;
            const int expectedItemsCount = 3;
            _cartService.DecrementFromCart(itemId);
            Assert.Equal(expectedItemsCount, _cart.ItemsCount);
            Assert.Single(_cart.Items);
        }

        [TestMethod]
        public void CartServiceTransformFromCartWorkCorrect()
        {
            const int expected_items_count = 4;
            const decimal expected_first_product_price = 1.1m;
            var result = _cartService.TransformFromCart();
            Assert.Equal(expected_items_count, result.ItemsCount);
            Assert.Equal(expected_first_product_price, result.Items.First().Product.Price);
        }
    }
}