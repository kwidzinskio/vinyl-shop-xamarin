using Plugin.Geolocator;
using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public ObservableCollection<ItemMap> ItemMapCollection;
        private readonly Geocoder _geocoder = new Geocoder();

        [Obsolete]
        public MapPage()
        {
            InitializeComponent();
            ItemMapCollection = new ObservableCollection<ItemMap>();
            //UpdateMap();
        }

        [Obsolete]
        protected override async void OnAppearing()
        {

            var items = await ApiService.GetItemMap();
            foreach (var item in items)
            {
                ItemMapCollection.Add(item);
                Pin pin = new Pin()
                {
                    Label = $"{item.Title} {item.Price}$",
                    Position = new Position(item.Latitude, item.Longitude),
                    Type = PinType.Place,
                    Address = item.Location,
                    ClassId = item.Id.ToString(),
                };
                MyMap.Pins.Add(pin);

                pin.Clicked += (object sender, EventArgs e) =>
                {
                    var p = sender as Pin;
                    var vehicleId = p.ClassId;
                    Navigation.PushModalAsync(new ItemDetailPage(Int16.Parse(vehicleId)));
                };
            }

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                         Distance.FromMiles(1)));

        }


    }
}