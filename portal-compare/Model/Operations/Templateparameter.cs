namespace portal_compare.Model.Operations
{
    public class TemplateParameter
    {
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public object defaultValue { get; set; }
        public bool required { get; set; }
        public object[] values { get; set; }
    }
}