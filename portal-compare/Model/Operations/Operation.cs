using System;

namespace portal_compare.Model.Operations
{
    public class Operation
    {
        public Operation(string apiId, string operationId)
        {
            id = $"/apis/{apiId}/operations/{operationId}";
        }

        public string id { get; set; }
        public string name { get; set; }
        public string method { get; set; }
        public string urlTemplate { get; set; }
        public TemplateParameter[] templateParameters { get; set; }
        public string description { get; set; }
        public Request request { get; set; }
        public Response[] responses { get; set; }

        public override string ToString()
        {
            return $"{method} -> {name} -> Template: {urlTemplate}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Operation)
                return Equals((Operation) obj);

            return false;
        }

        private bool Equals(Operation other)
        {
            return string.Equals(name, other.name, StringComparison.OrdinalIgnoreCase) && string.Equals(method, other.method, StringComparison.OrdinalIgnoreCase) && string.Equals(urlTemplate, other.urlTemplate, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = name?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (method?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (urlTemplate?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}