using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Services.Interfaces;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, WebAPI.Products) { }

        public IEnumerable<SectionDTO> GetSections() => Get<IEnumerable<SectionDTO>>($"{_serviceAddress}/sections");

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{_serviceAddress}/brands");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) =>
            Post(_serviceAddress, Filter ?? new ProductFilter())
               .Content
               .ReadAsAsync<IEnumerable<ProductDTO>>()
               .Result;

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_serviceAddress}/{id}");

        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{_serviceAddress}/sections/{id}");

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{_serviceAddress}/brands/{id}");
    }
}
