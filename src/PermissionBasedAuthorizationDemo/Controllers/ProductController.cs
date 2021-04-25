using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationDemo.Controllers
{
    public class ProductController : Controller
    {
        //this is a dummy action, it exists only to demonstrate permissions
        public IActionResult Index()
        {
            return View();
        }
    }
}
