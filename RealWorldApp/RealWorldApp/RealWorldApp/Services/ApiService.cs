using Newtonsoft.Json;
using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using UnixTimeStamp;
using RealWorldApp.Services;

namespace RealWorldApp.Services
{
    public static class ApiService
    {

        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
            var registerModel = new RegisterModel()
            {
                Name = name,
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(registerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/Accounts/Register", content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;

        }


        public static async Task<bool> Login(string email, string password)
        {
            var loginModel = new LoginModel()
            {
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/accounts/login", content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            Preferences.Set("accessToken", result.Access_token);
            Preferences.Set("usedId", result.User_id);
            Preferences.Set("tokenExpirationTime", result.Expiration_Time);
            Preferences.Set("currentTime", (int)UnixTime.GetCurrentTime());
            return true;
        }


        public static async Task<bool> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            await TokenValidator.CheckTokenValidity();
            var changePasswordModel = new ChangePasswordModel()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(changePasswordModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/accounts/changepassword", content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> EditPhoneNumber(string phoneNumber)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = string.Format("{0}{1}PhoneNumber{1}:{1}{2}{1}{3}", "{", "\"", phoneNumber, "}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/accounts/EditPhoneNumber", content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }


        public static async Task<bool> EditUserProfile(byte[] imageArray)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(imageArray);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/accounts/EditUserProfile", content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<UserImageModel> GetUserProfileImage()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/accounts/UserProfileImage");
            return JsonConvert.DeserializeObject<UserImageModel>(response);
        }


        public static async Task<List<Category>> GetCategories()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/categories");
            return JsonConvert.DeserializeObject<List<Category>>(response);
        }


        public static async Task<bool> AddImage(int itemId, byte[] imageArray)
        {
            await TokenValidator.CheckTokenValidity();
            var itemImage = new ItemImage()
            {
                ItemId = itemId,
                ImageArray = imageArray
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(itemImage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/Images", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<ItemDetail> GetItemDetail(int itemId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + $"api/item/itemdetails?id={itemId}");
            return JsonConvert.DeserializeObject<ItemDetail>(response);
        }

        public static async Task<List<ItemByCategory>> GetItemByCategory(int itemId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + $"api/Item?categoryId={itemId}");
            return JsonConvert.DeserializeObject<List<ItemByCategory>>(response);
        }

        public static async Task<List<SearchItem>> SearchItem(string search)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + $"api/Item/SearchItems?search={search}");
            return JsonConvert.DeserializeObject<List<SearchItem>>(response);
        }

        public static async Task<ItemResponse> AddItem(Item item)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/item", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ItemResponse>(jsonResult);
        }

        public static async Task<List<HotAndNewAd>> GetHotAndNewAds()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/item/HotAndNewAds");
            return JsonConvert.DeserializeObject<List<HotAndNewAd>>(response);
        }

        public static async Task<List<MyAd>> GetMyAds()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/item/myads");
            return JsonConvert.DeserializeObject<List<MyAd>>(response);
        }

        public static async Task<bool> DeleteItem(int itemId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + "api/item/" + itemId);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<ItemMap>> GetItemMap()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/Item/GetItemsMap");
            return JsonConvert.DeserializeObject<List<ItemMap>>(response);
        }

        public static async Task<bool> UpdateItemMap(ItemLocation itemLocation, int itemId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(itemLocation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.PutAsync(AppSettings.ApiUrl + "api/item/" + itemId, content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

    }
}

public static class TokenValidator
{
    public async static Task CheckTokenValidity()
    {
        var expirationTime = Preferences.Get("tokenExpirationTime", 0);
        Preferences.Set("currentTime", (int)UnixTime.GetCurrentTime());
        var currentTime = Preferences.Get("currentTime", 0);
        if (expirationTime < currentTime)
        {
            await ApiService.Login(Preferences.Get("email", string.Empty), Preferences.Get("password", string.Empty));
        }
    }
}