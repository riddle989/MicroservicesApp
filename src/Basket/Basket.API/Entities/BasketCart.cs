using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }
        public List<BasketCartItem> Items { get; set; } = new List<BasketCartItem>();

        public BasketCart()
        {

        }

        public BasketCart(string userName)
        {
            UserName = userName;
        }


        // The main benifit of this approach is that
        // we dont have to create method to get the price
        // when this variable is called, it will be set automatically
        // and return it
        public decimal TotalPrice 
        { 
            get
            {
                decimal totalPrice = 0;
                foreach(var item in Items)
                {
                    totalPrice += (item.Price * item.Quantity);
                }
                return totalPrice;
            }
        }
    }
}
