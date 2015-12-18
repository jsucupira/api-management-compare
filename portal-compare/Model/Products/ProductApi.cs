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
        public AuthenticationSettings AuthenticationSettings { get; set; }
        public SubscriptionKeyParameterNames subscriptionKeyParameterNames { get; set; }
    }
}