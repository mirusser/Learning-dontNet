using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailDemo.Models
{
    public class WelcomeRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
    }
}
