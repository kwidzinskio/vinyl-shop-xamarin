using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellPage : ContentPage
    {
        public ObservableCollection<Category> CategoriesCollection;
        private int categoryId;
        private string condition;

        public SellPage()
        {
            InitializeComponent();
            CategoriesCollection = new ObservableCollection<Category>();
            GetItemCategories();
        }

        private async void GetItemCategories()
        {
            var categories = await ApiService.GetCategories();
            foreach (var category in categories)
            {
                CategoriesCollection.Add(category);
            }
            PickerCategory.ItemsSource = CategoriesCollection;
        }

        private async void BtnSell_Clicked(object sender, EventArgs e)
        {
            bool valid = true;
            var item = new Item();

            try
            {
                item.Price = Convert.ToInt32(EntPrice.Text);
            }
            catch (Exception)
            {
                await DisplayAlert("Sorry", "Provide price as a number", "Cancel");
                valid = false;
            }

            item.Title = EntTitle.Text;
            item.CategoryId = categoryId;
            item.ReleaseYear = EntReleaseYear.Text;
            item.Album = EntAlbum.Text;
            item.Band = EntBand.Text;
            item.Description = EdiDescription.Text;
            item.DatePosted = DateTime.Now;
            item.UserId = Preferences.Get("userId", 0);
            item.Condition = condition;

            if (categoryId == 0) {
                await DisplayAlert("Sorry", "Provide a category", "Cancel");
                valid = false;
            }

            if (string.IsNullOrEmpty(EntTitle.Text))
            {
                await DisplayAlert("Sorry", "Provide a title", "Cancel");
                valid = false;
            }

            if (valid == true)
            {
                var response = await ApiService.AddItem(item);
                if (response == null)
                {
                    return;
                }
                var itemId = response.itemId;
                await Navigation.PushAsync(new SetLocationPage(itemId));

                var child = this.Content as Layout;
                if (child != null)
                {
                    ProcessLayoutChildren(child);
                }
            }
        }

        public void ProcessLayoutChildren(Layout child)
        {
            foreach (var item in child.Children)
            {
                var layout = item as Layout;
                if (layout != null)
                {
                    ProcessLayoutChildren(layout);
                }
                else
                {
                    ClearEntry(item);

                    FrameNew.BackgroundColor = Color.White;
                    LblNew.TextColor = Color.FromHex("#961111");
                    FrameUsed.BackgroundColor = Color.White;
                    LblUsed.TextColor = Color.FromHex("#961111");
                }
            }

            void ClearEntry(Element entryElement)
            {
                var entry = entryElement as Entry;
                if (entry != null)
                {
                    entry.Text = string.Empty;
                }
            }
        }

        private void PickerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (Category)picker.SelectedItem;
            categoryId = selectedCategory.Id;
        }

        private void TapUsed_Tapped(object sender, EventArgs e)
        {
            condition = "Used";
            FrameUsed.BackgroundColor = Color.FromHex("#961111");
            LblUsed.TextColor = Color.White;
            FrameNew.BackgroundColor = Color.White;
            LblNew.TextColor = Color.FromHex("#961111");
        }

        private void TapNew_Tapped(object sender, EventArgs e)
        {
            condition = "New";
            FrameNew.BackgroundColor = Color.FromHex("#961111");
            LblNew.TextColor = Color.White;
            FrameUsed.BackgroundColor = Color.White;
            LblUsed.TextColor = Color.FromHex("#961111");
        }

    }
}