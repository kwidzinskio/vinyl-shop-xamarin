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
    public partial class MyAdsPage : ContentPage
    {
        public ObservableCollection<MyAd> MyAdsCollection;
        public MyAdsPage()
        {
            InitializeComponent();
        }

        private async void GetItem()
        {
            var items = await ApiService.GetMyAds();
            foreach (var item in items)
            {
                MyAdsCollection.Add(item);
            }
            LvItems.ItemsSource = MyAdsCollection;
        }

        private void LvItems_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as MyAd;
            if (selectedItem == null) return;
            Navigation.PushModalAsync(new MyItemDetailPage(selectedItem.Id));
            ((ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                MyAdsCollection = new ObservableCollection<MyAd>();
                GetItem();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}