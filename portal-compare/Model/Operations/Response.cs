namespace portal_compare.Model.Operations
{
    public class Response
    {
        public int statusCode { get; set; }
        public string description { get; set; }
        public object[] representations { get; set; }
        public object[] headers { get; set; }
    }
}