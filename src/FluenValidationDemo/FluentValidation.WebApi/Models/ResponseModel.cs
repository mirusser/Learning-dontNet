using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidation.WebApi.Models
{
    public class ResponseModel
    {
        public bool IsValid { get; set; }
        public List<string> ValidationMessages { get; set; }

        public ResponseModel()
        {
            IsValid = true;
            ValidationMessages = new List<string>();
        }
    }
}
