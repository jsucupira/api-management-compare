using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using portal_compare.Model;

namespace portal_compare.Helpers
{
    public class HttpHelper
    {
        private readonly string _baseAddress;
        private readonly string _api;
        private readonly string _key;
        private string _apiVersion = "2014-02-14-preview";

        public HttpHelper(string serviceName, string api, string key)
        {
            _baseAddress = $"https://{serviceName}.management.azure-api.net";
            _api = api;
            _key = key;
            //"test";
        }

        public T Get<T>(string operation)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = new Uri(_baseAddress) })
            {
                DateTime expiry = DateTime.UtcNow.AddDays(1);
                string sharedAccessSignature = CreateSharedAccessToken(_api, _key, expiry);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", sharedAccessSignature);

                if (operation.Contains("?"))
                    operation += "&api-version=" + _apiVersion;
                else
                    operation += "?api-version=" + _apiVersion;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, operation);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    T result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }


                string error = response.Content.ReadAsStringAsync().Result;
                throw new Exception(error);
            }
        }

        public void Put<T>(string operation, T data)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = new Uri(_baseAddress) })
            {
                DateTime expiry = DateTime.UtcNow.AddDays(1);
                string sharedAccessSignature = CreateSharedAccessToken(_api, _key, expiry);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", sharedAccessSignature);

                if (operation.Contains("?"))
                    operation += "&api-version=" + _apiVersion;
                else
                    operation += "?api-version=" + _apiVersion;

                HttpResponseMessage response = client.PutAsJsonAsync(operation, data).Result;
                if (!response.IsSuccessStatusCode)
                {
                    string error = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(error);
                }
            }
        }

        public void Patch<T>(string operation, T data)
        {
            using (HttpClient client = new HttpClient() { BaseAddress = new Uri(_baseAddress) })
            {
                DateTime expiry = DateTime.UtcNow.AddDays(1);
                string sharedAccessSignature = CreateSharedAccessToken(_api, _key, expiry);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", sharedAccessSignature);

                if (operation.Contains("?"))
                    operation += "&api-version=" + _apiVersion;
                else
                    operation += "?api-version=" + _apiVersion;

                using (HttpContent content = new StringContent(JsonConvert.SerializeObject(data)))
                {
                    content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), operation) { Content = content };
                    request.Headers.Add("If-Match", "*");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        throw new Exception(error);
                    }
                }
            }
        }

        private static string CreateSharedAccessToken(string id, string key, DateTime expiry)
        {
            using (HMACSHA512 encoder = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                string dataToSign = id + "\n" + expiry.ToString("O", CultureInfo.InvariantCulture);
                string x = $"{id}\n{expiry.ToString("O", CultureInfo.InvariantCulture)}";
                var hash = encoder.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
                var signature = Convert.ToBase64String(hash);
                string encodedToken = $"uid={id}&ex={expiry:o}&sn={signature}";
                return encodedToken;
            }
        }
    }
}
