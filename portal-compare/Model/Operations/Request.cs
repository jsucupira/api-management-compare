namespace portal_compare.Model.Operations
{
    public class Request
    {
        public string description { get; set; }
        public QueryParameter[] queryParameters { get; set; }
        public object[] headers { get; set; }
        public object[] representations { get; set; }
    }
}