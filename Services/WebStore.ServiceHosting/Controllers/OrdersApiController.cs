using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO.Order;
using WebStore.Services.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService OrderService) => _OrderService = OrderService;

        [HttpGet("user/{UserName}")] 
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName)
		{
            return
            await _OrderService.GetUserOrders(UserName);
        }

        [HttpGet("{id}")]
        public async Task<OrderDTO> GetOrderById(int id)
		{
            return await _OrderService.GetOrderById(id);
        }

        [HttpPost("{UserName}")]
        public async Task<OrderDTO> CreateOrder(string UserName, [FromBody] CreateOrderModel OrderModel)
        {
            return await _OrderService.CreateOrder(UserName, OrderModel);
        }            
    }
}
