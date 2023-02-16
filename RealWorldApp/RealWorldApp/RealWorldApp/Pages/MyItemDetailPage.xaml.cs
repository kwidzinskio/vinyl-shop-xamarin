using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = RealWorldApp.Models.Image;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace RealWorldApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyItemDetailPage : ContentPage
	{
        public ObservableCollection<Image> ItemImage;
        private int itemId;
        private int totalImages;
        private string number;
        private string email;
        public MyItemDetailPage (int id)
		{
            InitializeComponent();
            itemId = id;
            ItemImage = new ObservableCollection<Image>();
            GetItemDetails(id);
        }

        private async void GetItemDetails(int id)
        {
            var item = await ApiService.GetItemDetail(id);
            LblTitle.Text = item.Title;
            LblLocation.Text = item.Location;
            LblCondition.Text = item.Condition;
            LblPrice.Text = $"{item.Price.ToString()} $";
            LblBand.Text = item.Band;
            LblDescription.Text = item.Description;
            LblAlbum.Text = item.Album;
            LblReleaseYear.Text = item.ReleaseYear;
            var images = item.Images;
            totalImages = item.Images.Count;
            number = item.Phone;
            email = item.Email;
            foreach (Image image in images)
            {
                ItemImage.Add(image);
            }
            CrvImages.ItemsSource = ItemImage;
            ImgUser.Source = item.FullImageUrl;

        }


        private void CrvImages_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.FirstVisibleItemIndex <= totalImages)
            {
                var count = e.FirstVisibleItemIndex + 1;
                LblCount.Text = count.ToString() + "of" + totalImages.ToString();

            }
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            var response = await ApiService.DeleteItem(itemId);
            if (response)
            {
                await DisplayAlert("", "Your offer has been deleted", "Allright");
                Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("", "Something went wrong", "Cancel");
            }
        }

    }
}