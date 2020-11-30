using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
	public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands()
		{
            return
            _ProductData.GetBrands()
               .Select(x => x.FromDTO())
               .Select(brand => new BrandViewModel
               {
                   Id = brand.Id,
                   Name = brand.Name,
                   Order = brand.Order
               })
               .OrderBy(brand => brand.Order);
        }
    }
}
