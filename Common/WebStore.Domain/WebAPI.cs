using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain
{
	public static class WebAPI
	{
		public const string Employees = "api/employees";
		public const string Products = "api/products";
		public const string Orders = "api/orders";

		//public static object Identity { get; set; }
		public static class Identity
		{
			public const string Roles = "api/roles";
			public const string Users = "api/users";
		}
	}
}
