using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portal_compare.Model
{
    public class Credentials
    {
        public string SourceServiceName { get; set; }
        public string SourceId { get; set; }
        public string SourceKey { get; set; }

        public string TargetServiceName { get; set; }
        public string TargetApi { get; set; }
        public string TargetKey { get; set; }
    }
}
