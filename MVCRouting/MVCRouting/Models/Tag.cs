using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCRouting.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        virtual public List<PostTag> PostTags { get; set; }
    }
}
