using Plugin.Geolocator;
using RealWorldApp.Models;
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
    public partial class ItemDetailMapPage : ContentPage
    {
        private ItemDetail _item;
        public ItemDetailMapPage(ItemDetail item)
        {
            InitializeComponent();
            _item = item;
            UpdateMap();
        }

        private async void UpdateMap()
        {
            Pin pin = new Pin()
            {
                Label = $"{_item.Title} {_item.Price}$",
                Position = new Position(_item.Latitude, _item.Longitude),
                Type = PinType.Place,
                Address = _item.Location,
                ClassId = _item.Id.ToString(),
            };
            MyMap.Pins.Add(pin);

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                         Distance.FromMiles(1)));

            pin.Clicked += (object sender, EventArgs e) =>
            {
                var p = sender as Pin;
                var vehicleId = p.ClassId;
                Navigation.PushModalAsync(new ItemDetailPage(Int16.Parse(vehicleId)));
            };
        }
    }
}