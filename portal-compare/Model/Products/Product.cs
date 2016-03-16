using System;

namespace portal_compare.Model.Products
{
    public class Product
    {
        public bool? approvalRequired { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public bool subscriptionRequired { get; set; }
        public int? subscriptionsLimit { get; set; }
        public string terms { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Product)
                return Equals(obj as Product);
            return false;
        }

        private bool Equals(Product other)
        {
            return string.Equals(name, other.name, StringComparison.OrdinalIgnoreCase) && subscriptionRequired == other.subscriptionRequired && approvalRequired == other.approvalRequired && subscriptionsLimit == other.subscriptionsLimit && string.Equals(state, other.state, StringComparison.OrdinalIgnoreCase) && string.Equals(description, other.description);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = name?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ subscriptionRequired.GetHashCode();
                hashCode = (hashCode*397) ^ approvalRequired.GetHashCode();
                hashCode = (hashCode*397) ^ subscriptionsLimit.GetHashCode();
                hashCode = (hashCode*397) ^ subscriptionsLimit.GetHashCode();
                hashCode = (hashCode*397) ^ (description?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (state?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{name} -> Subscription Required: {subscriptionRequired} -> State: {state}";
        }
    }
}