using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSTInvoiceHelper
{
    public class AuthRequest
    {
        [JsonProperty(PropertyName = "action")]
        public string action { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }
    }
}
