using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Markup;
using WebStore.Library.DTO;
using WebStore.Library.Models;
using WebStore.Library.Services;

namespace WebStore.MAUI.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged //overall management for the inventory's data
    {
        //implementation below redraws the grid if a change happens in the list
        public event PropertyChangedEventHandler? PropertyChanged;   

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List<ItemViewModel> Items
        {
            get
            {
                //getting the items list, selects each item from the item list and creates a new list
                //need to specify ToList because without it would assume that it's an IEnumerable
                return ItemServiceProxy.Current?.Items?.Select(i => new ItemViewModel(i)).ToList() ?? new List<ItemViewModel>();
            }
        }
        public ItemViewModel SelectedItem {  get; set; } 
        public InventoryManagementViewModel()
        {

        }

        public async Task RefreshItems() 
        {
            await ItemServiceProxy.Current.Get(); //get the newest data from the database via controller and EC 
            NotifyPropertyChanged(nameof(Items)); //display the new items
        }

        public async void UpdateItem()
        {
            if(SelectedItem?.Item == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//Item?itemID={SelectedItem.Item.ID}"); //asynchronously go to the //Item view and
                                                                              //set itemID to ID at SelectedItem.Item.ID
                                                                              //(Sends the item ID to QueryProperty)
            await ItemServiceProxy.Current.AddOrUpdate(SelectedItem.Item); //does add or update passing in the item
        }

        public async void DeleteItem()
        {
            if(SelectedItem?.Item == null)
            {
                return;
            }
            await ItemServiceProxy.Current.Delete(SelectedItem.Item.ID); //deletes passing in the id of the item
            RefreshItems(); //need to add refresh here or page doesnt update
        }

        public async void AddCSV(string CSVFile)
        {
            if (CSVFile == null)
            {
                return;
            }
            using (var reader =  new StreamReader(CSVFile)) //create new StreamReader which reads from the file specified by CSVFile
            {
                while (!reader.EndOfStream) //while the StreamReader hasn't reached the end
                {
                    var line = reader.ReadLine(); //read each line and store it here
                    var values = line.Split(','); //splits the line string into an array of values

                    //
                    //In the .CSV file it's important to follow this format:
                    //

                    // [0] = String Name 
                    // [1] = String Description
                    // [2] = Decimal Price
                    // [3] = Int Quantity

                    ItemDTO ItemFromCSV = new ItemDTO(); //create a new item item dto

                    ItemFromCSV.Name = values[0].Trim('\"');
                    ItemFromCSV.Description = values[1].Trim('\"');
                    String PriceTrimmed = (values[2]).Trim('\"'); //couldnt trim and parse on the same line
                    ItemFromCSV.Price = Decimal.Parse(PriceTrimmed);
                    String QuantityTrimmed = (values[3]).Trim('\"'); //couldnt trim and parse on the same line
                    ItemFromCSV.Quantity = int.Parse(QuantityTrimmed);

                    await ItemServiceProxy.Current.AddOrUpdate(ItemFromCSV);
                }
            }
        }
    }
}
