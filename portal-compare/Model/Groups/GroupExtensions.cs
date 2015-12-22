using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portal_compare.Model.Groups
{
    static class GroupExtensions
    {
        public static GroupRequest Map(this Group group)
        {
            if (group == null) return null;

            return new GroupRequest(group.name, group.description);
        }
    }
}
