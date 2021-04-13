using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Requested the Index Page");

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    _logger.LogInformation($"Iteration count: {i}");
                    if (i == 3)
                    {
                        throw new Exception("RandomException");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught");
            }
        }
    }
}
