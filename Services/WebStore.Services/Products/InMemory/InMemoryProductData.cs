﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Services.Data;
using WebStore.Services.Interfaces;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InMemory
{
	public class InMemoryProductData : IProductData
    {
        public IEnumerable<SectionDTO> GetSections() => TestData.Sections.AsEnumerable().Select(x => x.ToDTO());

        public IEnumerable<BrandDTO> GetBrands() => TestData.Brands.AsEnumerable().Select(x =>x.ToDTO());

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            return query.ToDTO();
        }

        public ProductDTO GetProductById(int id)
		{
            return TestData.Products.FirstOrDefault(p => p.Id == id).ToDTO();
        }
    }
}
