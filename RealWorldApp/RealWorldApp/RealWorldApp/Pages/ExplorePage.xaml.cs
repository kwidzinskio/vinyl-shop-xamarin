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
    public partial class ExplorePage : ContentPage
    {
        public ObservableCollection<HotAndNewAd> HotItemCollection;
        public ExplorePage()
        {
            InitializeComponent();
            HotItemCollection = new ObservableCollection<HotAndNewAd>();
            GetHotAndNewItems();
        }

        private async void GetHotAndNewItems()
        {
            var items = await ApiService.GetHotAndNewAds();
            foreach (var item in items)
            {
                HotItemCollection.Add(item);
            }
            CvItems.ItemsSource = HotItemCollection;
        }

        private void CvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currenctSelection = e.CurrentSelection.FirstOrDefault() as HotAndNewAd;
            if (currenctSelection == null) return;
            Navigation.PushModalAsync(new ItemDetailPage(currenctSelection.Id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void TapRock_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ItemsListPage(1));
        }

        private void TapRap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ItemsListPage(2));
        }

        private void TapPop_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ItemsListPage(3));
        }

        private void TapSearch_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SearchPage());
        }
    }
}