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
    public class CartManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void RefreshCarts()
        {
            await ShoppingCartServiceProxy.Current.Get();
            NotifyPropertyChanged(nameof(Carts));
        }

        public List<CartViewModel> Carts //list of carts
        {
            get
            {
                return ShoppingCartServiceProxy.Current?.Carts.Select(c => new CartViewModel(c)).ToList() ?? new List<CartViewModel>();
            }
        }

        public CartViewModel SelectedActiveCart { get; set; }

        public void SetActiveCart()
        {
            if (SelectedActiveCart?.ShoppingCart == null) //if the selected active cart is null, do nothing
            {
                return;
            }
            ShoppingCartServiceProxy.SetCurrentShoppingCart(SelectedActiveCart.ShoppingCartID);
            RefreshCarts();
        }

        public async void UpdateCart()
        {
            if (SelectedActiveCart?.ShoppingCart == null)
            {
                return;
            }

            Shell.Current.GoToAsync($"//CartView?cartID={SelectedActiveCart.ShoppingCart.ShoppingCartID}");

            await ShoppingCartServiceProxy.Current.AddOrUpdateCartDetails(SelectedActiveCart.ShoppingCart);
        }

        public async void DeleteShoppingCart()
        {
            if (SelectedActiveCart?.ShoppingCart == null)
            {
                return;
            }
            await ShoppingCartServiceProxy.Current.DeleteCart(SelectedActiveCart.ShoppingCart.ShoppingCartID); //deletes passing in the id of the item
            RefreshCarts(); //Need to add refresh here or page doesn't update
        }

    }
}
