namespace portal_compare.Model.Products
{
    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string terms { get; set; }
        public bool subscriptionRequired { get; set; }
        public bool approvalRequired { get; set; }
        public int? subscriptionsLimit { get; set; }
        public string state { get; set; }
    }
}