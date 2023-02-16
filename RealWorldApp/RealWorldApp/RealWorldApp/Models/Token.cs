using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Token
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
        public int Creation_Time { get; set; }
        public int Expiration_Time { get; set; }
        public int User_id { get; set; }
    }
}
