using System;
using Newtonsoft.Json;
using portal_compare.Model.Operations;

namespace portal_compare.Model.Apis
{
    public class Api
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string serviceUrl { get; set; }
        public string path { get; set; }
        public string[] protocols { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AuthenticationSettings authenticationSettings { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SubscriptionKeyParameterNames subscriptionKeyParameterNames { get; set; }

        public override string ToString()
        {
            return $"{name} -> Path: {path}";
        }

        public override bool Equals(object obj)
        {
            Api api = obj as Api;
            if (api != null)
                return Equals(api);

            return false;
        }

        private bool Equals(Api other)
        {
            return string.Equals(name, other.name, StringComparison.OrdinalIgnoreCase) && string.Equals(description, other.description, StringComparison.OrdinalIgnoreCase) && string.Equals(path, other.path, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = name?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (description?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (path?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (protocols?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (subscriptionKeyParameterNames?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}