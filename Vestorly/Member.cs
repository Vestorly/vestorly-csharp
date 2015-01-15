using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace Vestorly
{
    /// <summary>
    /// This is used to create new readers/leads in the Vestorly system as well as read specific demographic data out of Vestorly
    /// </summary>
    public class Member
    {
        [DeserializeAs(Name = "_id")]
        public string id { get; set; }

        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public bool unsubscribed { get; set; }


        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public string zip { get; set; }

        public string picture_url { get; set; }

        public string phone { get; set; }

        public string age { get; set; }

        public string education { get; set; }
        public string family { get; set; }
        
        public string gender { get; set; }
        public string group_id { get; set; }
        public bool has_children { get; set; }

        public string hometown { get; set; }
        public string location { get; set; }
        public string marital_status { get; set; }
        public string occupation { get; set; }

    }
}


