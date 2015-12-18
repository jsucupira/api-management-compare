using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portal_compare.Model.Operations
{

    public class OperationWrapper
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string serviceUrl { get; set; }
        public string path { get; set; }
        public string[] protocols { get; set; }
        public Operations operations { get; set; }
        public AuthenticationSettings authenticationSettings { get; set; }
        public SubscriptionKeyParameterNames subscriptionKeyParameterNames { get; set; }
        public Schemas schemas { get; set; }
    }
}
