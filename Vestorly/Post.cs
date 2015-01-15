using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vestorly
{
    public class Post
    {                
        public string title { get; set; }

        public string body { get; set; }

        public string summary { get; set; }

        public string comment { get; set; }

        public string logo_url { get; set; }

        public string external_url { get; set; }

        public string external_url_source { get; set; }

        public string external_url_type { get; set; }

        public string[] group_ids { get; set; }

        public DateTime created_at { get; set; }

        public DateTime post_date { get; set; }

    }
}
