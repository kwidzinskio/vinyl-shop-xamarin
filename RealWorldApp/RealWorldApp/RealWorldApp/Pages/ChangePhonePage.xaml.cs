using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePhonePage : ContentPage
    {
        public ChangePhonePage()
        {
            InitializeComponent();
        }

        private async void BtnAddPhone_Clicked(object sender, EventArgs e)
        {
            var response = await ApiService.EditPhoneNumber(EntPhone.Text);
            if (!response)
            {
                await DisplayAlert("Ooops", "Something went wrong", "Cancel");
            }
            else
            {
                await DisplayAlert("Phone number changed", "Success", "Cancel");
                await Navigation.PopAsync();
            }
        }
    }
}