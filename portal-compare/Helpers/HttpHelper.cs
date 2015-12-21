﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using portal_compare.Model;

namespace portal_compare.Helpers
{
    public class HttpHelper
    {
        private readonly string _baseAddress;
        private readonly string _api;
        private readonly string _key;
        private HttpClient _client;
        private string _apiVersion = "2014-02-14-preview";

        public HttpHelper(string serviceName, string api, string key)
        {
            _baseAddress = $"https://{serviceName}.management.azure-api.net";
            _api = api;
            _key = key;
        }

        public T Get<T>(string operation)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, operation + "?api-version=" + _apiVersion);
            HttpResponseMessage response = GetClient().SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
            return default(T);
        }

        private HttpClient GetClient()
        {
            if (_client == null)
            {
                DateTime expiry = DateTime.UtcNow.AddDays(1);
                string sharedAccessSignature = CreateSharedAccessToken(_api, _key, expiry);
                _client = new HttpClient() { BaseAddress = new Uri(_baseAddress) };
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", sharedAccessSignature);
            }
            return _client;
        }

        private static string CreateSharedAccessToken(string id, string key, DateTime expiry)
        {
            using (var encoder = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
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
