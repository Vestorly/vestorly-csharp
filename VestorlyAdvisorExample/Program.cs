using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using Vestorly;

namespace VestorlyAdvisorExample
{

    /// <summary>
    /// Shows how to login using that advisor's credentials or an optional long-running token which can be saved instead of username/password
    /// Launches Vestorly's SSO Link as a final step
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // creates a vestorly client session uses the test server for this
            VestorlyClient client = new VestorlyClient("2eb8b7cdde852ad836d469ff8a351eeeac6c331491dcc265446699591022e666");

            // logins to an advisor account / if this 
            try { 
                client.Login("vpw58dyy@gmail.com", "aszxxc@23:sdas");
                Console.WriteLine("Logged In Advisor");
            } catch (VestorlyException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            // Login without a username and password - if token is desired to be saved without password
            client.LoginWithToken(client.AuthToken); 

            // Publish a post
            var post = new Post
            {
                title = "A Great Article",
                created_at = DateTime.Now,
                post_date = DateTime.Now,
                summary = "an in-dept article about something of interest",
                comment = "I think this is wonderful"                
            };

            // Publish a Post with the following PDF document, optional if there is no PDF
            client.CreatePost(post, Path.GetFullPath("..\\..\\..\\sample.pdf"));

            // Publish a Post 
            //client.CreatePost(post);

            // Create a new member / reader
            //var member = new Member
            //{
            //    first_name = "Josh",
            //    last_name = "Jetson",
            //    email = "josh@jetsons"+Guid.NewGuid().ToString().Substring(0, 8)+".com"
            //};

            //var memberSaved = client.CreateMember(member);

            //memberSaved.age = "23";
            //memberSaved.first_name = "George";

            //var memberSaved2 = client.CreateMember(memberSaved);

            //var members = client.Members;

            //client.UpdateMember(memberSaved);

            Console.WriteLine("Publisher URL: " + client.PublisherUrl);
            Console.WriteLine("Reporting URL: " + client.ReportingUrl);

            if (!String.IsNullOrWhiteSpace(client.PublisherCustomContentUrl)) 
            {
                Process.Start(client.PublisherCustomContentUrl);
            }
            Console.ReadLine();
        }
    }
}
