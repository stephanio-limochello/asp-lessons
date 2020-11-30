using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebStore.Domain.DTO.Products
{
	[Table("ProductBrand")]
    public class BrandDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
