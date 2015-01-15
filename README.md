# vestorly-csharp
The Vestorly API and .NET client provides the ability for external developers to synchronize their client data with Vestorly.

## Installation

Install Visual Studio 2013 or Higher

Open the Solution File

    VestorlyAdvisorExample.sln

And then execute the sample:

    VestorlyAdvisorExample


## Basic Setup

To setup the client, you'll need to request an apiKey from success@vestorly.com


### SignIn to Vestorly API

Sign in will return the authentication token on success, and if the sign_in is invalid, it will raise `VestorlyException`

**Example**: Authentication to the API

```cs
  VestorlyClient client = new VestorlyClient(apiKey);

  // logins to an advisor account
  try { 
      client.Login(username, password);
  } catch (VestorlyException e)
  { 
    // handle errors here
  }
```

`VestorlyClient.LoginWithToken` also provides the ability to later login via a token:

```cs
  // Login without a username and password - if token is desired to be saved without password
  client.LoginWithToken(client.AuthToken); 
```

### Using the advisor API object

Once logged in with client object, it can be passed to the `VestorlyClient.PublishingUrl` property to get a valid URL:

**Example**: Obtain the link to Vestorly Publishing Page or Reporting Page

```cs
  Console.WriteLine("Publisher URL: " + client.PublisherUrl);
  Console.WriteLine("Reporting URL: " + client.ReportingUrl);
```


## Dependencies

### Visual Studio 2013

```bash
RestSharp 104.4.0
.NET 4.5
```

### License  

Licensed under [MIT License](https://github.com/Vestorly/vestorly-csharp/blob/master/LICENSE.txt)

