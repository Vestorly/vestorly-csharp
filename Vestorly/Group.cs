using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace Vestorly
{
    /// <summary>
    /// Content Library
    /// </summary>
    public class Group
    {
        [DeserializeAs(Name = "_id")]
        public string id { get; set; }
        public string name { get; set; }
        public string advisor_id { get; set; }

        public bool autopublish { get; set; }

        public bool is_default { get; set; }

        public string newsletter_id { get; set; }

        public string newsletter_setting_id { get; set; }

        public string newsletter_subject { get; set; }
    }
}


