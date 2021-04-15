using FileDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDemo.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            var vm = new FileUploadViewModel();

            return View(vm);
        }
    }
}
