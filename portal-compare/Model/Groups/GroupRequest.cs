using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portal_compare.Model.Groups
{
    public class GroupRequest
    {
        public GroupRequest(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public string name { get; set; }
        public string description { get; set; }

    }
}
