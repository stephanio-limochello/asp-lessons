using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Services.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
	[Route(WebAPI.Products)]
	[ApiController]
	public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _productData;
		private readonly ILogger<ProductsApiController> _logger;

		public ProductsApiController(IProductData productData, ILogger<ProductsApiController> logger)
		{
            _productData = productData;
			_logger = logger;
		}

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections() => _productData.GetSections();

        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => _productData.GetBrands();

        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter Filter = null) =>
            _productData.GetProducts(Filter ?? new ProductFilter());

        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id) => _productData.GetProductById(id);
    }
}
