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
    }
}