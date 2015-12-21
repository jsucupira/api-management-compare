using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace portal_compare.Model.Groups
{
    public class Group
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool builtIn { get; set; }
        public string type { get; set; }
        public string externalId { get; set; }
    }
}