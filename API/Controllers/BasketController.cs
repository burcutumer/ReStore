using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;
        public BasketController(StoreContext context)
        {
            _context = context;
        }
        [HttpGet(Name="GetBasket")]  //getBasket as the name of this route CLIENTI ILGILENDIRMIYOR     http.../api/basket
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();

            if (basket == null) return NotFound(); 
            return MapBasketDto(basket);  
        }

        

        [HttpPost]     //where func takes parameters    api/basket?productId=3&quantity=2
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();

            if(basket== null) basket = CreateBasket();

            var product = await _context.Products.FindAsync(productId);

            if(product == null) return NotFound();

            basket.AddItem(product,quantity);
           
            var result= await _context.SaveChangesAsync()>0;
            if(result) return CreatedAtRoute("GetBasket", MapBasketDto(basket));  //location headera getBasket Routeunu(/api/basket) ekliyor

            return BadRequest(new ProblemDetails{Title="problem saving item to basket"});
        }

        

        [HttpDelete]    //           PARAMETERS:      api/basket?productId=3&quantity=2
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) return NotFound();
            
            basket.RemoveItem(productId,quantity);

            var result= await _context.SaveChangesAsync()>0;
            if(result) return Ok();
            return BadRequest(new ProblemDetails{Title="problem deleting item"});
        }

        private async Task<Basket> RetrieveBasket()
        {
            return await _context.Baskets
                .Include(i => i.Items)             //nav property olan Items list
                .ThenInclude(p => p.Product)       //nav property olan Product
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
        }
        
        private Basket CreateBasket()
        {
           var buyerId = Guid.NewGuid().ToString();
           var cookieOptions = new CookieOptions{IsEssential= true, Expires= DateTime.Now.AddDays(30)};
           Response.Cookies.Append("buyerId", buyerId, cookieOptions);
           var basket = new Basket{BuyerId = buyerId};
           _context.Baskets.Add(basket);
           return basket;
        }
        
        private BasketDto MapBasketDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}