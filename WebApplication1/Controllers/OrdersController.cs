using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public OrdersController(ApplicationDbContext context)
        {
            this.context = context; 
     
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetOrders() 
        {
            int userId = JwtReader.GetUserId(User);
            string role = context.Users.Find(userId)?.Role ?? ""; //JwtReader.GetUserRole(User);
            IQueryable<Order> query = context.Orders.Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product);

            if(role != "admin")
            {
                query = query.Where(o => o.UserId == userId);  
            }
            query = query.OrderByDescending(o => o.Id);


            //read the orders
            var orders = query.ToList();
            foreach(var order in orders) 
            {
                //get rid of the object cycle
                foreach (var item in order.OrderItems)
                {
                    item.Order = null;
                }
                order.User.Password = "";
            }
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id) 
        {
            int userId=JwtReader.GetUserId(User);
            string role = context.Users.Find(userId)?.Role ?? "";
            Order? order = null;
            if(role == "admin") 
            {
                order = context.Orders.Include(o=>o.User)
                    .Include(o=>o.OrderItems).ThenInclude(oi=>oi.Product)
                    .FirstOrDefault(o => o.Id == id);


            }
            else
            {
                order = context.Orders.Include(o => o.User)
                   .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                   .FirstOrDefault(o => o.Id == id && o.UserId== userId);
            }
            if(order == null)
            {
                return NotFound();
            }

            // get rid of the object cycle 
            foreach(var item in order.OrderItems) 
            {
                item.Order = null;
            }
            //hide the user password
            order.User.Password = "";

            return Ok(order);
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(OderDto orderDto) 
        {
            //check if the payment method is valid or not 
            if(OrderHelper.PaymentMethods.ContainsKey(orderDto.PaymentMethod)) 
            {
                ModelState.AddModelError("Payment Method ", "Please select a valide payment method");
                return BadRequest(ModelState);
            }
            int userId = JwtReader.GetUserId(User);
            var user = context.Users.Find(userId);
            if (user == null) 
            {
                ModelState.AddModelError("Order", "Unable to create the order");
                return BadRequest(ModelState);
            }

            var productDictionnary = OrderHelper.GetProductDictionary(orderDto.ProductIndentifiers);

            // create a new order 
            Order order = new Order();
            order.UserId = userId;
            order.CreatedAt = DateTime.Now;
            order.ShippingFee = OrderHelper.ShippingFee;
            order.DeliveryAddress = orderDto.DeliveryAddress;
            order.PaymentMethod = orderDto.PaymentMethod;
            order.PaymentStatus = OrderHelper.PaymentStatuses[0]; //pending
            order.OrderStatus = OrderHelper.OrderStatuses[0]; // created

            foreach (var pair in productDictionnary)
            {
                int productId = pair.Key;
                var product = context.Products.Find(productId);
                if (product == null)
                {
                    ModelState.AddModelError("Product", "Product with id " + productId + " is not available");
                    return BadRequest(ModelState);
                }
                var orderItem = new OrderItem();
                orderItem.ProductId = productId;
                orderItem.Quantity = pair.Value;
                orderItem.UnitPrice = product.Price;


                order.OrderItems.Add(orderItem);
            }

            if (order.OrderItems.Count < 1) 
            {
                ModelState.AddModelError("Order", "Unable to create the order");
                return BadRequest(ModelState);
            }
            //save the order in the db 
            context.Orders.Add(order);
            context.SaveChanges();
            // get rig of the object cycle 
            foreach(var item in order.OrderItems) 
            {
                item.Order = null;
            }
            // hide the user password 
            order.User.Password = "";

            return Ok(order);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, string? paymentStatus, string? orderStatus)
        {
            if (paymentStatus == null && orderStatus == null)
            { // we have nothing to do 
                ModelState.AddModelError("Update Order", "There is nothing to update");
                return BadRequest(ModelState);
            }


            if (paymentStatus != null && !OrderHelper.PaymentStatuses.Contains(paymentStatus))
            {
                //the payment status is not valid
                ModelState.AddModelError("Payment Status", "The Payment Status is not valid");
                    return BadRequest(ModelState);
            }

            if (orderStatus != null && !OrderHelper.OrderStatuses.Contains(orderStatus))
            {
                //the payment status is not valid
                ModelState.AddModelError("Payment Status", "The Payment Status is not valid");
                return BadRequest(ModelState);
            }
            var order = context.Orders.Find(id);
            if (order == null) 
            {
                return NotFound();
            }
            if (paymentStatus != null) 
            {
                order.PaymentStatus = paymentStatus;

            }
            if (orderStatus != null) 
            {
                order.OrderStatus = orderStatus;
            }

            context.SaveChanges();
            return Ok(order);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            context.Orders.Remove(order);
            context.SaveChanges();

            return Ok();
        }


    }
}

