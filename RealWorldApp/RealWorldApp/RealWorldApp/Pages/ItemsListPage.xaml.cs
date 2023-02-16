using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsListPage : ContentPage
    {
        public ObservableCollection<ItemByCategory> ItemsCollection;
        public ItemsListPage(int categoryId)
        {
            InitializeComponent();
            ItemsCollection = new ObservableCollection<ItemByCategory>();
            GetItems(categoryId);
        }

        private async void GetItems(int categoryId)
        {
            var items = await ApiService.GetItemByCategory(categoryId);
            foreach (var item in items)
            {
                ItemsCollection.Add(item);
            }
            LvItems.ItemsSource = ItemsCollection;
        }

        private void LvItems_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as ItemByCategory;
            if (selectedItem == null) return;
            Navigation.PushModalAsync(new ItemDetailPage(selectedItem.Id));
            ((ListView)sender).SelectedItem = null;
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}