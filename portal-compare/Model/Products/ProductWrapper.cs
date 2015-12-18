using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portal_compare.Model.Products
{

    public class ProductWrapper
    {
        public Product[] value { get; set; }
        public int count { get; set; }
        public string nextLink { get; set; }
    }
}
