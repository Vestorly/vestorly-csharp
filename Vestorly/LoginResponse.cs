using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace Vestorly
{
    class LoginResponse
    {
        public string advisor_id { get; set; }

        public string advisor_company { get; set; }

        public string advisor_website { get; set; }

        public string message { get; set; }

        [DeserializeAs(Name = "vestorly-auth")]
        public string vestorly_auth { get; set; }

        public Theme theme { get; set; }

        public User user { get; set; }

    }
}
