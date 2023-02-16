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
    public partial class ItemDetailPage : ContentPage
    {
        public ObservableCollection<Image> ItemImage;
        private int totalImages;
        private string number;
        private string email;
        private int _id;
        public ItemDetailPage(int id)
        {
            InitializeComponent();
            _id = id;
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
            LblAlbum.Text = item.Album;
            LblDescription.Text = item.Description;
            LblBand.Text = item.Band;
            LblReleaseDate.Text = item.ReleaseYear;
            LblUsername.Text = item.Username;
            var images = item.Images;
            totalImages = item.Images.Count;
            number = item.Phone;
            email = item.Email;
            foreach (var image in images)
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
                LblCount.Text = count.ToString() + " of " + totalImages.ToString() ;
               
            }
        }

        private void BtnEmail_Clicked(object sender, EventArgs e)
        {
            var emailMessage = new EmailMessage("Query about product", "Hi! I am interested in your product", email);
            Email.ComposeAsync(emailMessage);
        }

        private async void BtnSms_Clicked(object sender, EventArgs e)

        {
            if (String.IsNullOrEmpty(number))
            {
                await DisplayAlert("Sorry", "User did not provided number", "Ok");
            }
            else
            {
                var smsMessage = new SmsMessage("Hi! I am interested in your product", number);
                Sms.ComposeAsync(smsMessage);
            }
        }

        private async void BtnCall_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(number))
            {
                await DisplayAlert("Sorry", "User did not provided number", "Ok");
            }
            else
            {
                PhoneDialer.Open(number);
            }
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var item = await ApiService.GetItemDetail(_id);
            Navigation.PushModalAsync(new ItemDetailMapPage(item));
        }
    }
}