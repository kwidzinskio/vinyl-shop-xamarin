using Plugin.Geolocator;
using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetLocationPage : ContentPage
    {
        private readonly Geocoder _geocoder = new Geocoder();
        private int _itemId;
        private ItemLocation _itemLocation;
        public SetLocationPage(int itemId)
        {
            InitializeComponent();
            _itemId = itemId;
            _itemLocation = new ItemLocation();
            UpdateMap();
        }

        private async void UpdateMap()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
        }

        private async void MyMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            MyMap.Pins.Clear();

            //await DisplayAlert("Coordinates", $"Lat: {e.Position.Latitude}, Long: {e.Position.Longitude}", "OK");

            var addresses = await _geocoder.GetAddressesForPositionAsync(e.Position);
            await DisplayAlert("Address set to:", addresses.FirstOrDefault()?.ToString(), "OK");

            Pin pin = new Pin
            {
                Label = "Pickup place",
                Type = PinType.Place,
                Position = new Position(e.Position.Latitude, e.Position.Longitude)
            };
            MyMap.Pins.Add(pin);

            _itemLocation.Latitude = e.Position.Latitude;
            _itemLocation.Longitude = e.Position.Longitude;
            _itemLocation.Location = addresses.FirstOrDefault()?.ToString();
        }

        private async void BtnAddPhotos_Clicked(object sender, EventArgs e)
        {
            var response = await ApiService.UpdateItemMap(_itemLocation, _itemId);

            Navigation.PushAsync(new AddImagePage(_itemId));
            Navigation.RemovePage(this);
        }
    }
}