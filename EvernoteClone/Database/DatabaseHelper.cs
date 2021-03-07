using EvernoteClone.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.Database
{
    // This project copyrights is for Ahmed Gamal Abdel Gawad,
    // LinkedIn: https://www.linkedin.com/in/aGaabdelgawad
    // GitHub: https://github.com/aGaabdelgawad
    // Youtube: https://www.youtube.com/AhmedGamalAbdelGawad
    // Facebook: https://www.facebook.com/aGaabdelgawad

    public static class DatabaseHelper
    {
        #region Fields
        private static readonly string dbPath = "Your_Firebase_Database_Link";
        #endregion

        #region Methods
        public static async Task<bool> Insert<T>(T item)
        {
            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var result = await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json", content);

                if (result.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public static async Task<bool> Update<T>(T item) where T : HasId
        {
            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var result = await client.PatchAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);

                if (result.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public static async Task<bool> Delete<T>(T item) where T : HasId
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.DeleteAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");

                if (result.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public async static Task<List<T>> ReadToList<T>() where T : HasId
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");
                var jsonResult = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode && jsonResult != "null")
                {
                    var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);

                    List<T> list = new List<T>();

                    if (objects != null)
                    {

                        foreach (var o in objects)
                        {
                            o.Value.Id = o.Key;
                            list.Add(o.Value);
                        }
                    }
                    return list;
                }
                else
                    return null;
            }
        }
        #endregion
    }
}