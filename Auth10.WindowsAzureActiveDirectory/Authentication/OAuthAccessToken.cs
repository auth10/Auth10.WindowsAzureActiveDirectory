using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth10.WindowsAzureActiveDirectory.Authentication
{
    public class OAuthAccessToken
    {
        public DateTime ExpiresOn { get; set; }
        public string AccessToken { get; set; }
    }
}
