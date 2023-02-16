using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync();
                Preferences.Set("email", EntEmail.Text);
                Preferences.Set("password", EntPassword.Text);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            var response = await ApiService.Login(EntEmail.Text, EntPassword.Text);
            if (response == true)
            {
                Application.Current.MainPage = new NavigationPage(new Homepage());
            }
            else
            {
                await DisplayAlert("Sorry", "Not logged", "Cancel");
            }
        }
    }
}