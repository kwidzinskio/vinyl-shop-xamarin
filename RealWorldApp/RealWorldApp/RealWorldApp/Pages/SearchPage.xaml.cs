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
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void SearchBarItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = await ApiService.SearchItem(e.NewTextValue);
            LvSearch.ItemsSource = items;
        }

        private void LvSearch_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var currenctSelection = e.SelectedItem as SearchItem;
            //if (currenctSelection == null) return;
            Navigation.PushModalAsync(new ItemDetailPage(currenctSelection.Id));
            //((CollectionView)sender).SelectedItem = null;
        }
    }
}