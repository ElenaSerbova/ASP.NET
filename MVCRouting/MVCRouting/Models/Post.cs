using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCRouting.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        virtual public List<PostTag> PostTags { get; set; }
    }
}
