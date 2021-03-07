using EvernoteClone.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.Database
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public static class FirebaseAuthHelper
    {
        #region Fields
        private static readonly string apiKey = "Your_Firebase_Authentication_API_Key";
        #endregion

        #region Methods
        public static async Task<bool> Register(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}", data);

                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    user.Id = result.localId;
                    user.IdToken = result.idToken;

                    var newBody = new
                    {
                        idToken = user.IdToken,
                        displayName = user.Name,
                        returnSecureToken = true
                    };

                    string newBodyJson = JsonConvert.SerializeObject(newBody);
                    var newData = new StringContent(newBodyJson, Encoding.UTF8, "application/json");

                    var newResponse = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:update?key={apiKey}", newData);

                    if (newResponse.IsSuccessStatusCode)
                    {
                        string newResultJson = await newResponse.Content.ReadAsStringAsync();
                        var newResult = JsonConvert.DeserializeObject<FirebaseResult>(newResultJson);
                        user.Id = newResult.localId;
                        user.IdToken = newResult.idToken;
                    }

                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<Error>(errorJson);
                    MessageBox.Show(error.error.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }

        public static async Task<bool> Login(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}", data);

                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    user.Id = result.localId;
                    user.IdToken = result.idToken;

                    var newBody = new
                    {
                        idToken = user.IdToken
                    };

                    string newBodyJson = JsonConvert.SerializeObject(newBody);
                    var newData = new StringContent(newBodyJson, Encoding.UTF8, "application/json");

                    var newResponse = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={apiKey}", newData);

                    if (newResponse.IsSuccessStatusCode)
                    {
                        string newResultJson = await newResponse.Content.ReadAsStringAsync();
                        var newResult = JsonConvert.DeserializeObject<FirebaseUserData>(newResultJson);
                        user.Id = newResult.Users.FirstOrDefault().localId;
                        user.Name = newResult.Users.FirstOrDefault().displayName;
                    }

                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<Error>(errorJson);
                    MessageBox.Show(error.error.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }
        #endregion
    }

    public class FirebaseResult
    {
        public string kind { get; set; }
        public string idToken { get; set; }
        public string email { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
        public string localId { get; set; }
        public string displayName { get; set; }
    }

    public class FirebaseUserData
    {
        public List<FirebaseResult> Users { get; set; }
    }

    public class ErrorDetails
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class Error
    {
        public ErrorDetails error { get; set; }
    }
}
