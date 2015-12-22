using System;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using portal_compare.Model.Apis;

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

        public override string ToString()
        {
            return $"{name} -> Type: {type}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Group)
                return Equals((Group) obj);

            return false;
        }

        private bool Equals(Group other)
        {
            return string.Equals(name, other.name) && builtIn == other.builtIn;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name?.GetHashCode() ?? 0)*397) ^ builtIn.GetHashCode();
            }
        }
    }
}