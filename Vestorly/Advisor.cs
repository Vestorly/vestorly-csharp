using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vestorly
{
    
    /// <summary>
    /// Advisor Object to create on Vestorly
    /// </summary>
    public class Advisor
    {
        /// <summary>
        /// Username must be an email address
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Optional Password can be blank
        /// </summary>
        public string password { get; set; }
        
        /// <summary>
        /// First name of the advisor
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// Last name of the advisor 
        /// </summary>
        public string last_name { get; set; }

        /// <summary>
        /// Full http:// URL of the website that the advisor has 
        /// </summary>
        public string website { get; set; }

        /// <summary>
        /// Firmname or company of the advisor 
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// Describes any summary text about the advisor 
        /// </summary>
        public string about { get; set; }

        /// <summary>
        /// Phone # of the advisor account 
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// City of the Advisor 
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Zip code of the Advisor 
        /// </summary>
        public string zip { get; set; }


        /// <summary>
        /// Additional Service Code information that Vestorly may need to store about your application
        /// </summary>
        public string service_code { get; set; }

        /// <summary>
        /// Activation of the Advisor Account - setting true activtes the account on the Vestorly system when creating, 
        /// otherwise a Vestorly admin needs to activate the advisor's account
        /// </summary>
        public bool active { get; set; }
    }
}
