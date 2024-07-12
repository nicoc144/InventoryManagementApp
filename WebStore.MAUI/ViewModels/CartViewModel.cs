using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Models;
using WebStore.Library.Services;

namespace WebStore.MAUI.ViewModels
{
    public class CartViewModel
    {

        public ShoppingCart? ShoppingCart;

        public string ShoppingCartName
        {
            get
            {
                return ShoppingCart?.ShoppingCartName ?? string.Empty;
            }
            set
            {
                if (ShoppingCart != null)
                {
                    ShoppingCart.ShoppingCartName = value;
                }
            }
        }

        public int ShoppingCartID //sets the ID for the item
        {
            get
            {
                return ShoppingCart?.ShoppingCartID ?? 0;
            }
            set
            {
                if (ShoppingCart != null)
                {
                    ShoppingCart.ShoppingCartID = value;
                }
            }
        }

        public decimal ShoppingCartTotal
        {
            get
            {
                return ShoppingCart.ShoppingCartTotal;
            }
        }

        public decimal ShoppingCartTotalAfterTax
        {
            get
            {
                return ShoppingCart.ShoppingCartTotalAfterTax;

            }
        }

        public List<ItemViewModel> Contents
        { 
            get
            {
                return ShoppingCart.Contents.Select(i => new ItemViewModel(i)).ToList() ?? new List<ItemViewModel>();
            }
        }

        public CartViewModel()
        {
            ShoppingCart = new ShoppingCart();
        }

        public CartViewModel(ShoppingCart c)
        {
            ShoppingCart = c;
        }

        public CartViewModel(int ID)
        {
            ShoppingCart = ShoppingCartServiceProxy.Current?.Carts?.FirstOrDefault(c => c.ShoppingCartID == ID);
            if(ShoppingCart == null)
            {
                ShoppingCart = new ShoppingCart();
            }
        }

        public void AddCart() //Adds or updates cart details (like the name, or ID if youre making a new cart)
        {
            ShoppingCartServiceProxy.Current.AddOrUpdateCartDetails(ShoppingCart);
        }


    }
}
