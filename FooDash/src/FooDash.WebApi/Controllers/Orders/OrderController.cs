using Dawn;
using FooDash.Application.Common.Interfaces.Orders;
using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Dtos.Contracts;
using FooDash.Domain.Entities.Orders;
using FooDash.WebApi.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.Orders
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = Guard.Argument(orderService).NotNull().Value;
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] CreateOrderContract createOrderContract)
        {
            var orderId = await _orderService.CreateOrderFromCart(createOrderContract);

            return Created(orderId.ToString(), orderId);
        }

        [HttpGet]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Order)}")]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<ReadOrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrders();

            return Ok(orders);
        }

        [HttpPatch]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Order)}")]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<ReadOrderDto>>> AssignOrderToCourier(AssignOrderToCourierContract assignOrderToCourierContract)
        {
            await _orderService.AssignOrderToCourier(
                assignOrderToCourierContract.OrderId,
                assignOrderToCourierContract.CourierId);

            return Ok();
        }

        [HttpGet]
        [Route("api/[controller]/{orderId}")]
        public async Task<ActionResult<IEnumerable<ReadOrderDto>>> GetOrder([FromRoute] Guid orderId)
        {
            var order = await _orderService.GetOrder(orderId);

            return Ok(order);
        }

        [HttpPatch]
        [Authorize(Policy = $"{PolicyTypes.CanUpdate}{nameof(Order)}")]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<ReadOrderDto>>> ChangeOrdersStatus(ChangeOrderStatusContract changeOrderStatusContract)
        {
            await _orderService.ChangeOrdersStatus(
                changeOrderStatusContract.OrderId,
                changeOrderStatusContract.OrderStatus);

            return Ok();
        }

        [HttpPatch]
        [Authorize(Policy = $"{PolicyTypes.CanRead}{nameof(Order)}")]
        [Route("api/[controller]/{orderId}")]
        public async Task<ActionResult> CancelOrder([FromRoute] Guid orderId)
        {
            await _orderService.CancelOrder(orderId);

            return Ok();
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{courierId}")]
        public async Task<ActionResult<IEnumerable<ReadOrderDto>>> GetCourierOrders([FromRoute] Guid courierId)
        {
            var orders = await _orderService.GetCourierOrders(courierId);

            return Ok(orders);
        }
    }
}
