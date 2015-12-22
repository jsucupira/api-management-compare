using Newtonsoft.Json;
using portal_compare.Model.Apis;
using portal_compare.Model.Operations;

namespace portal_compare.Model.Products
{
    public class ProductApi
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string serviceUrl { get; set; }
        public string path { get; set; }
        public string[] protocols { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AuthenticationSettings AuthenticationSettings { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SubscriptionKeyParameterNames subscriptionKeyParameterNames { get; set; }

        public override string ToString()
        {
            return $"{name} -> Path: {path}";
        }

        public override bool Equals(object obj)
        {
            if (obj is ProductApi)
                return Equals(obj as ProductApi);
            return false;
        }

        private bool Equals(ProductApi other)
        {
            return string.Equals(name, other.name) && string.Equals(description, other.description) && string.Equals(path, other.path);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = name?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (description?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (path?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}