using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI
{ 
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void InvClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Inventory");
        }

        private void ShopClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Shop");
        }
    }

}
