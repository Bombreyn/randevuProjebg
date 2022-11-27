using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace randevuProje.Models
{
    public class sms
    {
        public string api_id { get; set; }
        public string api_key { get; set; }
        public string sender { get; set; }
        public string message_type { get; set; }
        public string message { get; set; }
        public string[] phones { get; set; }
    }
}