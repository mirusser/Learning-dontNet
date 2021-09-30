using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DbModels
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
    }
}