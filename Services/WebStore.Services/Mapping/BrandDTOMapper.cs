using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandDTOMapper
    {
        public static BrandDTO ToDTO(this Brand Brand)
		{
            return Brand is null ? null : new BrandDTO
            {
                Id = Brand.Id,
                Name = Brand.Name,
                Order = Brand.Order,
                ProductsCount = Brand.Products.Count()
            };
        }

        public static Brand FromDTO(this BrandDTO Brand)
		{
            return Brand is null ? null : new Brand
            {
                Id = Brand.Id,
                Name = Brand.Name,
                Order = Brand.Order,
            };
        }
    }
}
